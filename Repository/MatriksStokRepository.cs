using InventoryMultiGudang.Models;
using InventoryMultiGudang.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace InventoryMultiGudang.Repository
{
    public class MatriksStokRepository : IMatriksStokRepository
    {
        private readonly ApplicationDbContext _context;

        public MatriksStokRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Implementasi pencarian gudang dinamis untuk Select2 AJAX live search.
        /// </summary>
        public async Task<IEnumerable<WarehouseHeaderDto>> GetAllWarehousesAsync(string searchTerm = null, int limit = 10)
        {
            var query = _context.Gudangs.Where(g => g.IsActive);

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(g => g.NamaGudang.Contains(searchTerm) || g.KodeGudang.Contains(searchTerm));
            }

            return await query
                .Take(limit)
                .Select(g => new WarehouseHeaderDto
                {
                    Id = g.Id,
                    KodeGudang = g.KodeGudang,
                    NamaGudang = g.NamaGudang
                })
                .ToListAsync();
        }

        public async Task<MatriksStokReportVm> GetLaporanMatriksStokAsync(string? penyerah)
        {
            // 1. Ambil seluruh data gudang aktif dari tabel gudang
            var daftarGudangDb = await _context.Gudangs
                .Where(g => g.IsActive == true)
                .OrderBy(g => g.Id)
                .ToListAsync();

            var gudangHeaders = daftarGudangDb.Select(g => new GudangHeaderVm
            {
                GudangId = g.Id,
                KodeGudang = g.KodeGudang,
                NamaGudang = g.NamaGudang,
                TipeGudang = g.TipeGudang
            }).ToList();

            // 2. Ambil seluruh data produk dan setting safety stock dari DB
            var produkDb = await _context.Produks.ToListAsync();
            var settingsDb = await _context.ProdukSettings.ToListAsync();

            // 3. Kelompokkan produk secara unik berdasarkan NamaProduk dan Satuan (seperti pada Index)
            var groupedProduk = produkDb
                .GroupBy(p => new { p.NamaProduk, p.Satuan })
                .ToList();

            var rows = new List<MatriksStokRowVm>();
            int totalKritis = 0;

            foreach (var group in groupedProduk)
            {
                // Ambil SKU representatif dari produk pertama dalam group
                var firstItem = group.First();
                var allProductIdsInGroup = group.Select(p => p.Id).ToList();

                var row = new MatriksStokRowVm
                {
                    Sku = firstItem.Sku,
                    NamaProduk = group.Key.NamaProduk,
                    Satuan = group.Key.Satuan
                };

                bool isProdukKritis = false;

                foreach (var g in gudangHeaders)
                {
                    // Ambil stok dari item produk di group ini yang memiliki GudangId yang cocok
                    var stokItem = group.Where(p => p.GudangId == g.GudangId).Sum(p => p.Stok);
                    row.StokPerGudang[g.GudangId] = stokItem;

                    // Ambil safety stock yang benar-benar terikat pada produk-produk di group ini DAN GudangId yang bersangkutan
                    var safetyStockVal = settingsDb
                        .Where(s => allProductIdsInGroup.Contains(s.ProdukId) && s.GudangId == g.GudangId)
                        .Sum(s => s.SafetyStock);

                    row.SafetyStockPerGudang[g.GudangId] = safetyStockVal;

                    // Cek kondisi kritis (stok < safety stock apabila safety stock di-set > 0)
                    if (safetyStockVal > 0 && stokItem < safetyStockVal)
                    {
                        isProdukKritis = true;
                    }
                }

                if (isProdukKritis)
                {
                    totalKritis++;
                }

                rows.Add(row);
            }
            var systemsettings = await _context.SystemSettings.FirstOrDefaultAsync();
            return new MatriksStokReportVm
            {
                RefNo = "SR-MX-" + DateTime.Now.ToString("yyyyMMdd"),
                TanggalCetak = DateTime.Now,
                OperatorName = penyerah ?? "Staff Logistik",
                DaftarGudang = gudangHeaders,
                TotalVarianProduk = rows.Count,
                TotalGudangTerpantau = gudangHeaders.Count,
                TotalProdukKritis = totalKritis,
                Rows = rows,
                SiteName = systemsettings?.SiteName ?? "Inventory Multi-Gudang Modern",
                SiteDescription = systemsettings?.SiteDescription ?? "Sistem Manajemen Stok Multi-Gudang",
                CompanyAddress = systemsettings?.CompanyAddress ?? "Jl. Contoh Alamat No. 123, Kota Contoh",
                CompanyPhone = systemsettings?.CompanyPhone ?? "+62 123 4567 890",
                CompanyEmail = systemsettings?.CompanyEmail ?? "info@contoh.com"
            };
        }

        public async Task<(IEnumerable<ProductStockRowDto> Products, int TotalCount)> GetMatriksStokDataAsync(int? gudangId, int pageNumber, int pageSize)
        {
            // 1. Ambil master produk unik berdasarkan NamaProduk dan Satuan
            var baseProductQuery = _context.Produks
                .GroupBy(p => new { p.NamaProduk, p.Satuan })
                .Select(g => new
                {
                    ProductId = g.Min(p => p.Id),
                    Sku = g.Min(p => p.Sku),
                    NamaProduk = g.Key.NamaProduk,
                    Satuan = g.Key.Satuan,
                    AllProductIds = g.Select(p => p.Id).ToList()
                });

            int totalCount = await baseProductQuery.CountAsync();

            var pagedProducts = await baseProductQuery
                .OrderBy(p => p.NamaProduk)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var allProductIdsFlat = pagedProducts.SelectMany(p => p.AllProductIds).ToList();

            // Ambil setting safety stock
            var settings = await _context.ProdukSettings
                          .Where(ps => allProductIdsFlat.Contains(ps.ProdukId))
                          .ToListAsync();

            // 2. Ambil data langsung dari tabel `Produk` karena kolom GudangId & Stok ada di tabel Produk
            var stockQuery = _context.Produks
                .Where(p => allProductIdsFlat.Contains(p.Id));

            if (gudangId.HasValue && gudangId.Value > 0)
            {
                stockQuery = stockQuery.Where(p => p.GudangId == gudangId.Value);
            }

            var stockData = await stockQuery
                .Select(p => new
                {
                    ProdukId = p.Id,
                    p.GudangId,
                    Qty = p.Stok
                })
                .ToListAsync();

            // 3. Mapping ke DTO ViewModel untuk Cross-Tab Grid
            var productRows = new List<ProductStockRowDto>();

            foreach (var p in pagedProducts)
            {
                var row = new ProductStockRowDto
                {
                    ProductId = p.ProductId,
                    Sku = p.Sku,
                    NamaProduk = p.NamaProduk,
                    Satuan = p.Satuan,
                    safety_stock = settings.Where(s => p.AllProductIds.Contains(s.ProdukId)).Sum(s => s.SafetyStock)
                };

                foreach (var prodId in p.AllProductIds)
                {
                    var prodSettings = settings.Where(s => s.ProdukId == prodId);
                    foreach (var ps in prodSettings)
                    {
                        row.SafetyStockWarehouse[ps.GudangId] = ps.SafetyStock;
                    }

                    var warehouseStocks = stockData.Where(s => s.ProdukId == prodId);
                    foreach (var ws in warehouseStocks)
                    {
                        if (!row.WarehouseStocks.ContainsKey(ws.GudangId))
                        {
                            row.WarehouseStocks[ws.GudangId] = 0;
                        }
                        row.WarehouseStocks[ws.GudangId] += ws.Qty;
                    }
                }

                productRows.Add(row);
            }

            return (productRows, totalCount);
        }

        public async Task<bool> UpdateSafetyStockAsync(int productId, int warehouseId, int newSafetyStock)
        {
            try
            {
                // 1. Ambil SKU atau Nama Produk dari productId yang dikirim
                var targetProduct = await _context.Produks.FindAsync(productId);
                if (targetProduct == null) return false;

                // 2. Cari semua ID produk yang memiliki SKU atau Nama Produk yang sama (serumpun)
                var relatedProductIds = await _context.Produks
                    .Where(p => p.NamaProduk == targetProduct.NamaProduk && p.Satuan == targetProduct.Satuan)
                    .Select(p => p.Id)
                    .ToListAsync();

                // 3. Cek apakah sudah ada setting untuk gudang ini di antara produk serumpun tersebut
                var setting = await _context.ProdukSettings
                    .FirstOrDefaultAsync(ps => relatedProductIds.Contains(ps.ProdukId) && ps.GudangId == warehouseId);

                if (setting != null)
                {
                    // Jika sudah ada, UPDATE nilainya
                    setting.SafetyStock = newSafetyStock;
                }
                else
                {
                    // Jika belum ada sama sekali untuk gudang ini, buat record baru menggunakan productId yang aktif
                    _context.ProdukSettings.Add(new ProdukSetting
                    {
                        ProdukId = productId,
                        GudangId = warehouseId,
                        SafetyStock = newSafetyStock
                    });
                }

                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<string> GetNamaPenyerahAsync(string? loggedInUser)
        {
            if (string.IsNullOrEmpty(loggedInUser))
                return "Staff Logistik";

            // 1. Cek relasi dari tabel AspNetUsers (KaryawanId) ke tabel Karyawans
            var namaKaryawan = await (from u in _context.Users
                                      join k in _context.Karyawans on u.KaryawanId equals k.Id
                                      where u.UserName == loggedInUser || u.Email == loggedInUser
                                      select k.NamaLengkap).FirstOrDefaultAsync();

            if (!string.IsNullOrEmpty(namaKaryawan))
            {
                return namaKaryawan;
            }

            // 2. Fallback: Cek langsung di tabel Karyawans berdasarkan Email atau NIK
            var karyawanByEmail = await _context.Karyawans
                .Where(k => k.Email == loggedInUser || k.Nik == loggedInUser || k.NamaLengkap == loggedInUser)
                .Select(k => k.NamaLengkap)
                .FirstOrDefaultAsync();

            if (!string.IsNullOrEmpty(karyawanByEmail))
            {
                return karyawanByEmail;
            }
            // 3. Jika tidak ditemukan padanan karyawan, kembalikan username asli atau default
            return loggedInUser;
        }
    }
}