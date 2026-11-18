using Microsoft.EntityFrameworkCore;
using InventoryMultiGudang.ViewModels;
using InventoryMultiGudang.Models;

namespace InventoryMultiGudang.Repository
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly ApplicationDbContext _context;

        public DashboardRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Menghitung total valuasi stok finansial (HargaBeli * Stok) dengan EF Core.
        /// </summary>
        public async Task<decimal> GetTotalValuasiFinansialAsync(int? gudangId)
        {
            var query = _context.Produks.AsQueryable();
            if (gudangId.HasValue && gudangId.Value > 0)
            {
                query = query.Where(p => p.GudangId == gudangId.Value);
            }
            return await query.SumAsync(p => p.HargaBeli * p.Stok);
        }

        /// <summary>
        /// Menghitung total unit aset dengan Serial Number yang berstatus In Stock.
        /// </summary>
        public async Task<int> GetTotalUnitAsetSnAsync(int? gudangId)
        {
            var query = _context.ProdukItems.AsQueryable();
            if (gudangId.HasValue && gudangId.Value > 0)
            {
                query = query.Where(pi => pi.GudangId == gudangId.Value);
            }
            return await query.CountAsync(pi => pi.Status == "In Stock");
        }

        /// <summary>
        /// Menghitung total varian SKU produk.
        /// </summary>
        public async Task<int> GetTotalVarianSkuAsync(int? gudangId)
        {
            var query = _context.Produks.AsQueryable();
            if (gudangId.HasValue && gudangId.Value > 0)
            {
                query = query.Where(p => p.GudangId == gudangId.Value);
            }
            return await query.CountAsync();
        }

        /// <summary>
        /// Menghitung total akumulasi stok fisik seluruh produk.
        /// </summary>
        public async Task<int> GetTotalStokFisikAsync(int? gudangId)
        {
            var query = _context.Produks.AsQueryable();
            if (gudangId.HasValue && gudangId.Value > 0)
            {
                query = query.Where(p => p.GudangId == gudangId.Value);
            }
            return await query.SumAsync(p => p.Stok);
        }

        /// <summary>
        /// Menghitung jumlah gudang aktif.
        /// </summary>
        public async Task<int> GetTotalGudangAktifAsync(int? gudangId)
        {
            if (gudangId.HasValue && gudangId.Value > 0)
            {
                return await Task.FromResult(1);
            }
            return await _context.Gudangs.CountAsync(g => g.IsActive);
        }

        /// <summary>
        /// Mengambil daftar gudang aktif untuk dropdown filter.
        /// </summary>
        public async Task<List<GudangDropdownViewModel>> GetListGudangAsync()
        {
            return await _context.Gudangs
                .Where(g => g.IsActive)
                .Select(g => new GudangDropdownViewModel
                {
                    Id = g.Id,
                    KodeGudang = g.KodeGudang,
                    NamaGudang = g.NamaGudang
                })
                .ToListAsync();
        }

        /// <summary>
        /// Mengambil log aktivitas mutasi stok terbaru (jurnal kartu stok).
        /// </summary>
        public async Task<List<AktivitasMutasiViewModel>> GetListAktivitasMutasiAsync(int? gudangId, int takeCount = 10)
        {
            var query = _context.TransaksiStoks
                .Include(t => t.Gudang)
                .Include(t => t.Details)
                .ThenInclude(d => d.Produk)
                .AsQueryable();

            if (gudangId.HasValue && gudangId.Value > 0)
            {
                query = query.Where(t => t.GudangId == gudangId.Value);
            }

            return await query
                .OrderByDescending(t => t.TanggalPosting)
                .Take(takeCount)
                .SelectMany(t => t.Details, (t, d) => new AktivitasMutasiViewModel
                {
                    NamaProduk = d.Produk.NamaProduk,
                    Sku = d.Produk.Sku,
                    NamaGudang = t.Gudang.NamaGudang,
                    TipeTransaksi = t.TipeTransaksi,
                    QtyMutasi = d.Jumlah,
                    SaldoStok = d.Produk.Stok.ToString(),
                    NomorReferensi = t.NomorReferensi,
                    WaktuPencatatan = t.TanggalPosting.ToString("dd MMM yyyy, HH:mm")
                })
                .ToListAsync();
        }

        /// <summary>
        /// Menghitung total varian SKU produk yang stoknya habis (Stok == 0).
        /// </summary>
        public async Task<int> GetTotalVarianHabisAsync(int? gudangId)
        {
            var query = _context.Produks.AsQueryable();
            if (gudangId.HasValue && gudangId.Value > 0)
            {
                query = query.Where(p => p.GudangId == gudangId.Value);
            }
            return await query.CountAsync(p => p.Stok <= 0);
        }

        /// <summary>
        /// Menghitung total data aktivitas mutasi untuk keperluan pagination.
        /// </summary>
        public async Task<int> GetTotalAktivitasMutasiAsync(int? gudangId)
        {
            var query = _context.TransaksiStoks.AsQueryable();
            if (gudangId.HasValue && gudangId.Value > 0)
            {
                query = query.Where(t => t.GudangId == gudangId.Value);
            }
            return await query.CountAsync();
        }

        /// <summary>
        /// Mengambil data aktivitas mutasi dengan pagination (Skip & Take).
        /// </summary>
        public async Task<List<AktivitasMutasiViewModel>> GetPagedAktivitasMutasiAsync(int? gudangId, int pageNumber, int pageSize)
        {
            var query = _context.TransaksiStoks
                .Include(t => t.Gudang)
                .Include(t => t.Details)
                .ThenInclude(d => d.Produk)
                .AsQueryable();

            if (gudangId.HasValue && gudangId.Value > 0)
            {
                query = query.Where(t => t.GudangId == gudangId.Value);
            }

            var listData = await query
                .OrderByDescending(t => t.TanggalPosting)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(); // Eksekusi ke memory untuk menghindari mismatch kolom SQL langsung jika ada perbedaan mapping

            // Mapping manual yang aman dari error missing column
            var result = listData.SelectMany(t => t.Details, (t, d) => new AktivitasMutasiViewModel
            {
                NamaProduk = d.Produk != null ? d.Produk.NamaProduk : "-",
                Sku = d.Produk != null ? d.Produk.Sku : "-",
                NamaGudang = t.Gudang != null ? t.Gudang.NamaGudang : "-",
                TipeTransaksi = t.TipeTransaksi ?? "-",
                QtyMutasi = d.Jumlah,
                SaldoStok = d.Produk != null ? d.Produk.Stok.ToString() : "0",
                NomorReferensi = t.NomorReferensi ?? "-", // Pastikan properti model C# sesuai dengan database
                WaktuPencatatan = t.TanggalPosting.ToString("dd MMM yyyy, HH:mm")
            }).ToList();

            return result;
        }

        /// <summary>
        /// Mengambil tren agregasi mutasi harian (Stock In, Stock Out, Transfer, Opname) berdasarkan rentang tanggal.
        /// </summary>
        public async Task<GrafikMutasiHarianViewModel> GetGrafikMutasiHarianAsync(int? gudangId, DateTime tglMulai, DateTime tglAkhir)
        {
            var result = new GrafikMutasiHarianViewModel();

            // Ambil daftar transaksi stok sesuai tanggal dan gudang
            var queryTrx = _context.TransaksiStoks
                .Include(t => t.Details)
                .Where(t => t.TanggalPosting.Date >= tglMulai.Date && t.TanggalPosting.Date <= tglAkhir.Date);

            if (gudangId.HasValue && gudangId.Value > 0)
            {
                queryTrx = queryTrx.Where(t => t.GudangId == gudangId.Value);
            }

            var listTrx = await queryTrx.ToListAsync();

            // Loop per hari dari tglMulai sampai tglAkhir
            for (var date = tglMulai.Date; date <= tglAkhir.Date; date = date.AddDays(1))
            {
                result.Dates.Add(date.ToString("dd MMM"));

                var trxHariIni = listTrx.Where(t => t.TanggalPosting.Date == date).ToList();

                result.StockIn.Add(trxHariIni.Where(t => t.TipeTransaksi == "Stock In").Sum(t => t.Details.Sum(d => d.Jumlah)));
                result.StockOut.Add(trxHariIni.Where(t => t.TipeTransaksi == "Stock Out").Sum(t => t.Details.Sum(d => d.Jumlah)));
                result.Transfer.Add(trxHariIni.Where(t => t.TipeTransaksi == "Transfer" || t.TipeTransaksi == "Transfer Out" || t.TipeTransaksi == "Transfer In").Sum(t => t.Details.Sum(d => d.Jumlah)));
                result.Opname.Add(trxHariIni.Where(t => t.TipeTransaksi == "Stock Opname" || t.TipeTransaksi == "Penyesuaian").Sum(t => t.Details.Sum(d => d.Jumlah)));
            }

            return result;
        }

        /// <summary>
        /// Mengambil rincian status kepemilikan dan kondisi fisik aset secara akurat berdasarkan tabel produk_item, peminjaman, penugasan, reparasi, dan stockopnamedetail.
        /// </summary>
        public async Task<GrafikStatusAsetViewModel> GetGrafikStatusAsetAsync(int? gudangId)
        {
            var queryItem = _context.ProdukItems.AsQueryable();
            if (gudangId.HasValue && gudangId.Value > 0)
            {
                queryItem = queryItem.Where(pi => pi.GudangId == gudangId.Value);
            }
            var totalItems = await queryItem.ToListAsync();
            if (!totalItems.Any())
            {
                return new GrafikStatusAsetViewModel();
            }

            int readyCount = totalItems.Count(pi => pi.Status == "In Stock" || pi.Status == "Ready");

            // Hitung berdasarkan jumlah dokumen/transaksi penugasan aktif yang unik 
            // (jika ingin menghitung jumlah penugasan, gunakan .Select(d => d.PenugasanAsetId).Distinct().CountAsync())
            var queryPenugasanAktif = _context.PenugasanAsetDetails
                .Include(d => d.PenugasanAset)
                .Where(d => d.PenugasanAset != null &&
                            d.PenugasanAset.TanggalPengembalian == null &&
                            d.PenugasanAset.Status == StatusPenugasan.DalamPenugasan);

            if (gudangId.HasValue && gudangId.Value > 0)
            {
                queryPenugasanAktif = queryPenugasanAktif.Where(d => d.PenugasanAset.GudangId == gudangId.Value);
            }

            // Mengubah SumAsync menjadi Distinct Count berdasarkan PenugasanAsetId 
            // agar dihitung sebagai 2 transaksi penugasan aktif (bukan akumulasi kuantitas fisik item).
            int assignedCount = await queryPenugasanAktif
                .Select(d => d.PenugasanAsetId) // Sesuaikan dengan nama FK di model PenugasanAsetDetail Anda (misal: PenugasanAsetId)
                .Distinct()
                .CountAsync();

            int onLoanCount = totalItems.Count(pi => pi.Status == "Dipinjam" || pi.Status == "On Loan");
            int underRepairCount = totalItems.Count(pi => pi.Status == "Reparasi" || pi.Status == "Under Repair" || pi.Status == "Service");
            int lostCount = totalItems.Count(pi => pi.Status == "Lost" || pi.Status == "Hilang");

            var queryTrxJual = _context.TransaksiStoks
                .Include(t => t.Details)
                .Where(t => t.TipeTransaksi == "Stock Out" && t.IsPenjualan);

            if (gudangId.HasValue && gudangId.Value > 0)
            {
                queryTrxJual = queryTrxJual.Where(t => t.GudangId == gudangId.Value);
            }
            var listTrxJual = await queryTrxJual.ToListAsync();
            int soldCount = listTrxJual.Sum(t => t.Details.Sum(d => d.Jumlah));

            int itemSoldDisposed = totalItems.Count(pi => pi.Status == "Sold" || pi.Status == "Disposed");
            if (soldCount == 0) soldCount = itemSoldDisposed;

            return new GrafikStatusAsetViewModel
            {
                ReadyGudang = readyCount < 0 ? 0 : readyCount,
                AssignedCustody = assignedCount,
                OnLoan = onLoanCount,
                UnderRepair = underRepairCount,
                Lost = lostCount,
                SoldDisposal = soldCount
            };
        }

        /// <summary>
        /// Mengambil akumulasi nilai finansial aset ter-stok di masing-masing gudang.
        /// </summary>
        public async Task<List<ValuasiGudangViewModel>> GetValuasiPerGudangAsync(int? gudangId)
        {
            var query = _context.Gudangs.Where(g => g.IsActive).AsQueryable();
            if (gudangId.HasValue && gudangId.Value > 0)
            {
                query = query.Where(g => g.Id == gudangId.Value);
            }

            var gudangs = await query.ToListAsync();
            var result = new List<ValuasiGudangViewModel>();

            foreach (var g in gudangs)
            {
                var valuasi = await _context.Produks
                    .Where(p => p.GudangId == g.Id)
                    .SumAsync(p => (decimal?)(p.HargaBeli * p.Stok)) ?? 0;

                result.Add(new ValuasiGudangViewModel
                {
                    NamaGudang = g.NamaGudang,
                    Valuasi = valuasi
                });
            }

            return result;
        }

        /// <summary>
        /// Mengambil perbandingan jumlah varian SKU produk vs volume unit aset fisik per kategori.
        /// </summary>
        public async Task<List<DistribusiKategoriViewModel>> GetDistribusiKategoriAsync(int? gudangId)
        {
            var query = _context.Produks.AsQueryable();
            if (gudangId.HasValue && gudangId.Value > 0)
            {
                query = query.Where(p => p.GudangId == gudangId.Value);
            }

            var produks = await query.ToListAsync();

            return produks
                .GroupBy(p => string.IsNullOrEmpty(p.Kategori) ? "Lainnya" : p.Kategori)
                .Select(g => new DistribusiKategoriViewModel
                {
                    KategoriName = g.Key,
                    VarianSku = g.Count(),
                    JumlahAset = g.Sum(p => p.Stok)
                })
                .ToList();
        }

        /// <summary>
        /// Mengambil pembagian jumlah unit aset yang sedang ditugaskan (assigned custody) ke divisi/departemen.
        /// </summary>
        public async Task<List<PenguasaanDepartemenViewModel>> GetPenguasaanPerDepartemenAsync(int? gudangId)
        {
            // Selaraskan kondisi filter penugasan aktif (TanggalPengembalian == null & Status DalamPenugasan)
            var queryPenugasan = _context.PenugasanAsetDetails
                .Include(d => d.PenugasanAset)
                .Where(d => d.PenugasanAset != null &&
                            d.PenugasanAset.TanggalPengembalian == null &&
                            d.PenugasanAset.Status == StatusPenugasan.DalamPenugasan)
                .AsQueryable();

            var listPenugasan = await queryPenugasan.ToListAsync();
            var result = new List<PenguasaanDepartemenViewModel>();

            var targetKaryawanIds = listPenugasan
                .Where(p => p.PenugasanAset.TipePenerima == TipePenerima.Karyawan)
                .Select(p => p.PenugasanAset.TargetPenerimaId)
                .Distinct()
                .ToList();

            var karyawans = await _context.Karyawans
                .Include(k => k.Department)
                .Where(k => targetKaryawanIds.Contains(k.Id))
                .ToListAsync();

            var departments = await _context.Departments.ToListAsync();
            var deptCountMap = new Dictionary<string, int>();

            foreach (var item in listPenugasan)
            {
                string deptName = "Tanpa Departemen";

                if (item.PenugasanAset.TipePenerima == TipePenerima.Karyawan)
                {
                    var kar = karyawans.FirstOrDefault(k => k.Id == item.PenugasanAset.TargetPenerimaId);
                    if (kar != null && kar.Department != null)
                    {
                        deptName = kar.Department.Name;
                    }
                }
                else if (item.PenugasanAset.TipePenerima == TipePenerima.Departemen)
                {
                    var dept = departments.FirstOrDefault(d => d.Id == item.PenugasanAset.TargetPenerimaId);
                    if (dept != null)
                    {
                        deptName = dept.Name;
                    }
                }

                int qtyQty = item.Kuantitas > 0 ? item.Kuantitas : 1;
                if (deptCountMap.ContainsKey(deptName))
                    deptCountMap[deptName] += qtyQty;
                else
                    deptCountMap[deptName] = qtyQty;
            }

            return deptCountMap.Select(kvp => new PenguasaanDepartemenViewModel
            {
                NamaDepartemen = kvp.Key,
                JumlahAset = kvp.Value
            }).OrderByDescending(x => x.JumlahAset).ToList();
        }

        /// <summary>
        /// Mengambil perbandingan nilai beli (valuasi) stok antara Asset, Consumables, dan Persediaan (Inventory).
        /// </summary>
        public async Task<KomposisiValuasiTipeViewModel> GetKomposisiValuasiTipeAsync(int? gudangId)
        {
            var query = _context.Produks.AsQueryable();
            if (gudangId.HasValue && gudangId.Value > 0)
            {
                query = query.Where(p => p.GudangId == gudangId.Value);
            }

            var listProduk = await query.ToListAsync();

            decimal valAsset = listProduk
                .Where(p => p.TipeBarang != null && (p.TipeBarang.Contains("Asset") || p.TipeBarang.Contains("Aset")))
                .Sum(p => p.HargaBeli * p.Stok);

            decimal valConsumable = listProduk
                .Where(p => p.TipeBarang != null && (p.TipeBarang.Contains("Consumable") || p.TipeBarang.Contains("Habis Pakai")))
                .Sum(p => p.HargaBeli * p.Stok);

            decimal valInventory = listProduk
                .Where(p => p.TipeBarang != null && (p.TipeBarang.Contains("Inventory") || p.TipeBarang.Contains("Persediaan") || p.TipeBarang.Contains("Komersial")))
                .Sum(p => p.HargaBeli * p.Stok);

            return new KomposisiValuasiTipeViewModel
            {
                ValuasiAsset = valAsset,
                ValuasiConsumable = valConsumable,
                ValuasiInventory = valInventory
            };
        }

        /// <summary>
        /// Mengambil daftar peminjaman aset yang melewati batas waktu (overdue) dan belum dikembalikan.
        /// </summary>
        public async Task<List<OverdueLoanViewModel>> GetOverdueLoansAsync(int? gudangId)
        {
            var today = DateTime.Now.Date;

            var query = _context.PeminjamanAsets
                .Include(p => p.Karyawan)
                .Include(p => p.Details)
                .ThenInclude(d => d.ProdukItem)
                .Where(p => p.TanggalJatuhTempo.Date < today && p.TanggalPengembalian == null);

            if (gudangId.HasValue && gudangId.Value > 0)
            {
                query = query.Where(p => p.GudangId == gudangId.Value);
            }

            var listData = await query.ToListAsync();
            var result = new List<OverdueLoanViewModel>();

            foreach (var item in listData)
            {
                string peminjam = !string.IsNullOrEmpty(item.Karyawan?.NamaLengkap) ? item.Karyawan.NamaLengkap : "-";
                int selisihHari = (today - item.TanggalJatuhTempo.Date).Days;

                foreach (var detail in item.Details)
                {
                    result.Add(new OverdueLoanViewModel
                    {
                        NomorDokumen = item.KodePeminjaman,
                        NamaPeminjam = peminjam,
                        NamaBarang = detail.NamaBarang,
                        SerialNumber = detail.SerialNumber ?? "-",
                        BatasKembali = item.TanggalJatuhTempo.ToString("dd MMM yyyy"),
                        HariKeterlambatan = selisihHari > 0 ? selisihHari : 1
                    });
                }
            }

            return result;
        }

        /// <summary>
        /// Mengambil daftar stok produk yang berada di bawah batas minimum (Safety Stock) berdasarkan tabel produk_settings.
        /// </summary>
        public async Task<List<StokKritisViewModel>> GetStokKritisListAsync(int? gudangId)
        {
            var querySettings = _context.ProdukSettings
                .Include(s => s.Produk)
                .Include(s => s.Gudang)
                .AsQueryable();

            if (gudangId.HasValue && gudangId.Value > 0)
            {
                querySettings = querySettings.Where(s => s.GudangId == gudangId.Value);
            }

            var settings = await querySettings.ToListAsync();
            var result = new List<StokKritisViewModel>();

            foreach (var s in settings)
            {
                if (s.Produk != null)
                {
                    // Cek apakah stok produk di gudang tersebut kurang dari safety_stock
                    int stokFisik = s.Produk.Stok; // atau ambil spesifik stok per gudang jika ada
                    if (stokFisik <= s.SafetyStock)
                    {
                        int percent = s.SafetyStock > 0 ? (int)((double)stokFisik / s.SafetyStock * 100) : 0;
                        if (percent > 100) percent = 100;

                        result.Add(new StokKritisViewModel
                        {
                            NamaProduk = s.Produk.NamaProduk,
                            Sku = s.Produk.Sku,
                            NamaGudang = s.Gudang != null ? s.Gudang.NamaGudang : "-",
                            StokSaatIni = stokFisik,
                            SafetyStock = s.SafetyStock,
                            Persentase = percent
                        });
                    }
                }
            }

            return result;
        }

    }
}