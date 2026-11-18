using InventoryMultiGudang.Models;
using InventoryMultiGudang.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace InventoryMultiGudang.Repository
{
    public class TransferOrderRepository : ITransferOrderRepository
    {
        private readonly ApplicationDbContext _context;

        public TransferOrderRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResultTransferOrder<TransferOrder>> GetPagedListAsync(TransferOrderFilterViewModel filter)
        {
            var query = _context.TransferOrders
                .Include(t => t.GudangAsal)
                .Include(t => t.GudangTujuan)
                .AsQueryable();

            // Filter Search Term
            if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
            {
                query = query.Where(t => t.NomorDokumen.Contains(filter.SearchTerm));
            }

            // Filter Gudang
            if (filter.GudangId.HasValue && filter.GudangId > 0)
            {
                query = query.Where(t => t.GudangAsalId == filter.GudangId || t.GudangTujuanId == filter.GudangId);
            }

            // Filter Status
            if (!string.IsNullOrWhiteSpace(filter.Status) && Enum.TryParse<StatusTransfer>(filter.Status, true, out var statusEnum))
            {
                query = query.Where(t => t.Status == statusEnum);
            }

            int totalItems = await query.CountAsync();
            var items = await query
                .OrderByDescending(t => t.TanggalDibuat)
                .Skip((filter.Page - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync();

            return new PagedResultTransferOrder<TransferOrder>
            {
                Items = items,
                TotalItems = totalItems,
                CurrentPage = filter.Page,
                PageSize = filter.PageSize,
                TotalPages = (int)Math.Ceiling((double)totalItems / filter.PageSize)
            };
        }

        public async Task<TransferOrderDetailViewModel?> GetByIdAsync(int id)
        {
            var data = await _context.TransferOrders
                .Include(t => t.GudangAsal) // Menarik data dari tabel 'gudang' via GudangAsalId
                .Include(t => t.GudangTujuan) // Menarik data dari tabel 'gudang' via GudangTujuanId
                .Include(t => t.TransferOrdersDetails)
                    .ThenInclude(i => i.Produk) // Menarik data dari tabel 'produk'
                .FirstOrDefaultAsync(t => t.Id == id);

            if (data == null) return null;

            return new TransferOrderDetailViewModel
            {
                Id = data.Id,
                NoReferensi = data.NomorDokumen,
                Status = data.Status,

                // Mengambil kolom NamaGudang, KodeGudang, AlamatGudang dari tabel 'gudang'
                GudangAsalNama = data.GudangAsal?.NamaGudang ?? "-",
                GudangAsalKode = data.GudangAsal?.KodeGudang ?? "-",
                GudangAsalAlamat = data.GudangAsal?.AlamatGudang ?? "-",

                GudangTujuanNama = data.GudangTujuan?.NamaGudang ?? "-",
                GudangTujuanKode = data.GudangTujuan?.KodeGudang ?? "-",
                GudangTujuanAlamat = data.GudangTujuan?.AlamatGudang ?? "-",

                TanggalDibuat = data.TanggalDibuat,
                TanggalDikirim = data.TanggalDikirim,
                TanggalDiterima = data.TanggalDiterima,
                DibuatOleh = data.Pembuat,
                Catatan = data.Catatan,

                Items = data.TransferOrdersDetails.Select(i => new TransferOrderItemViewModel
                {
                    ItemId = i.Id,
                    NamaProduk = i.Produk?.NamaProduk ?? "-",
                    Sku = i.Produk?.Sku ?? "-",
                    SerialNumber = data.NomorDokumen,
                    Satuan = i.Produk?.Satuan ?? "-",
                    QtyPermintaan = i.KuantitasPermintaan,
                    QtyDiterima = i.KuantitasDiterima > 0 ? i.KuantitasDiterima : i.KuantitasPermintaan
                }).ToList()
            };
        }

        public async Task<bool> CreateAsync(TransferOrderViewModel model)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // 1. VALIDASI: Gudang Asal dan Tujuan tidak boleh sama
                if (model.GudangAsalId == model.GudangTujuanId)
                {
                    throw new Exception("Gudang Asal dan Tujuan tidak boleh sama.");
                }

                // Generate Nomor Dokumen Otomatis
                string noRef = await GenerateNoReferensiAsync();

                var transferOrder = new TransferOrder
                {
                    NomorDokumen = noRef,
                    GudangAsalId = model.GudangAsalId,
                    GudangTujuanId = model.GudangTujuanId,
                    Catatan = model.Catatan,
                    Status = StatusTransfer.DalamPerjalanan,
                    TanggalDibuat = DateTime.Now,
                    Pembuat = !string.IsNullOrEmpty(model.DibuatOleh) ? model.DibuatOleh : "System", // <-- Fallback pengaman
                    TransferOrdersDetails = new List<TransferOrderDetail>()
                };

                foreach (var item in model.Details)
                {
                    // Ambil data produk di gudang asal
                    var produkAsal = await _context.Produks
                        .FirstOrDefaultAsync(p => p.Id == item.ProdukId && p.GudangId == model.GudangAsalId);

                    if (produkAsal == null)
                    {
                        var masterProduk = await _context.Produks.FindAsync(item.ProdukId);
                        if (masterProduk != null)
                        {
                            produkAsal = await _context.Produks
                                .FirstOrDefaultAsync(p => p.Sku == masterProduk.Sku && p.GudangId == model.GudangAsalId);
                        }
                    }

                    // Validasi ketersediaan stok di gudang asal
                    if (produkAsal == null || produkAsal.Stok < item.KuantitasPermintaan)
                    {
                        await transaction.RollbackAsync();
                        return false;
                    }

                    // 2. KURANGI STOK DI GUDANG ASAL
                    produkAsal.Stok -= item.KuantitasPermintaan;

                    // 3. CEK / BUAT PRODUK DI GUDANG TUJUAN (Menyalin SEMUA field dari produk asal)
                    var produkTujuan = await _context.Produks
                        .FirstOrDefaultAsync(p => p.Sku == produkAsal.Sku && p.GudangId == model.GudangTujuanId);

                    if (produkTujuan == null)
                    {
                        produkTujuan = new Produk
                        {
                            Sku = produkAsal.Sku,
                            NamaProduk = produkAsal.NamaProduk,
                            Kategori = produkAsal.Kategori,
                            Merek = produkAsal.Merek,
                            Satuan = produkAsal.Satuan,
                            TipeBarang = produkAsal.TipeBarang,
                            HargaBeli = produkAsal.HargaBeli,
                            HargaPerolehan = produkAsal.HargaPerolehan,
                            Deskripsi = produkAsal.Deskripsi,
                            Stok = 0, // Stok akan bertambah saat diterima atau langsung dicatat
                            GudangId = model.GudangTujuanId,
                            RakId = produkAsal.RakId,
                            PelacakanBarang = produkAsal.PelacakanBarang
                        };
                        _context.Produks.Add(produkTujuan);
                        await _context.SaveChangesAsync(); // Simpan untuk mendapatkan ID baru produk tujuan
                    }

                    // 4. UPDATE tblproduk_item JIKA PRODUK BERSTATUS SERIAL NUMBER (SN)
                    if (produkAsal.PelacakanBarang == "Pelacakan Serial Number (SN)")
                    {
                        var itemSerials = await _context.ProdukItems
                            .Where(pi => pi.ProdukId == produkAsal.Id && pi.GudangId == model.GudangAsalId && pi.Status == "In Stock")
                            .Take(item.KuantitasPermintaan)
                            .ToListAsync();

                        foreach (var pi in itemSerials)
                        {
                            // Update GudangId dan ProdukId ke gudang tujuan
                            pi.GudangId = model.GudangTujuanId;
                            pi.ProdukId = produkTujuan.Id;
                        }
                    }

                    // Tambah stok di gudang tujuan (bisa langsung atau saat konfirmasi terima barang)
                    produkTujuan.Stok += item.KuantitasPermintaan;

                    transferOrder.TransferOrdersDetails.Add(new TransferOrderDetail
                    {
                        ProdukId = produkAsal.Id,
                        KuantitasPermintaan = item.KuantitasPermintaan,
                        KuantitasDiterima = item.KuantitasPermintaan
                    });
                }

                _context.TransferOrders.Add(transferOrder);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                Console.WriteLine("Error CreateTO: " + ex.Message);
                return false;
            }
        }

        // PERBAIKAN 2: Filter daftar produk HANYA yang ada di Gudang Asal & stoknya > 0
        public async Task<List<Produk>> GetProdukPagedAsync(string? search, int limit = 20, int? gudangId = null)
        {
            var query = _context.Produks.AsQueryable();

            if (gudangId.HasValue && gudangId > 0)
            {
                query = query.Where(p => p.GudangId == gudangId.Value);
            }

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(p => p.NamaProduk.Contains(search) || p.Sku.Contains(search));
            }

            return await query.Where(p => p.Stok > 0).Take(limit).ToListAsync();
        }
        public async Task<bool> UpdateAsync(TransferOrderViewModel model)
        {
            // Implementasi update bila status masih draft/pending jika dibutuhkan
            return await Task.FromResult(true);
        }
        public async Task<bool> DeleteAsync(int id)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var data = await _context.TransferOrders
                    .Include(t => t.TransferOrdersDetails)
                        .ThenInclude(d => d.Produk)
                    .FirstOrDefaultAsync(t => t.Id == id);

                if (data == null) return false;

                foreach (var item in data.TransferOrdersDetails)
                {
                    var produkDetail = item.Produk;
                    string skuTarget = produkDetail?.Sku ?? "";

                    // 1. KEMBALIKAN STOK KE GUDANG ASAL
                    // Cari produk di gudang asal berdasarkan SKU/Id
                    var stokAsal = await _context.Produks
                        .FirstOrDefaultAsync(s => s.GudangId == data.GudangAsalId && (s.Id == item.ProdukId || s.Sku == skuTarget));

                    if (stokAsal != null)
                    {
                        // Kembalikan stok sesuai kuantitas permintaan/penerimaan
                        int qtyToRestore = item.KuantitasDiterima > 0 ? item.KuantitasDiterima : item.KuantitasPermintaan;
                        stokAsal.Stok += qtyToRestore;
                    }

                    // 2. TANGANI STOK DI GUDANG TUJUAN (Jika status sudah DITERIMA)
                    if (data.Status == StatusTransfer.Diterima && item.KuantitasDiterima > 0)
                    {
                        var stokTujuan = await _context.Produks
                            .FirstOrDefaultAsync(s => s.GudangId == data.GudangTujuanId && s.Sku == skuTarget);

                        if (stokTujuan != null)
                        {
                            // Kurangi stok di gudang tujuan
                            stokTujuan.Stok -= item.KuantitasDiterima;

                            // Jika stok di gudang tujuan menjadi 0 atau kurang, HAPUS produk dari gudang tujuan
                            if (stokTujuan.Stok <= 0)
                            {
                                _context.Produks.Remove(stokTujuan);
                            }
                        }
                    }
                }

                // 3. HAPUS DOKUMEN TRANSFER ORDER
                _context.TransferOrders.Remove(data);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                System.Diagnostics.Debug.WriteLine("Error DeleteTO: " + ex.Message);
                return false;
            }
        }

        public async Task<bool> ReceiveTransferAsync(int id, Dictionary<int, int> receivedQtyMap)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var transferOrder = await _context.TransferOrders
                    .Include(t => t.TransferOrdersDetails)
                        .ThenInclude(d => d.Produk)
                    .FirstOrDefaultAsync(t => t.Id == id);

                if (transferOrder == null) return false;
                if (transferOrder.Status == StatusTransfer.Diterima) return false;

                foreach (var detail in transferOrder.TransferOrdersDetails)
                {
                    // Cek apakah ID detail cocok dengan key dari JS
                    if (receivedQtyMap.TryGetValue(detail.Id, out int qtyDiterima))
                    {
                        detail.KuantitasDiterima = qtyDiterima;

                        if (qtyDiterima > 0)
                        {
                            // Cari produk di Gudang Tujuan berdasarkan SKU atau NamaProduk
                            var produkAsal = detail.Produk;
                            var skuTarget = produkAsal != null ? produkAsal.Sku : "";

                            var produkTujuan = await _context.Produks
                                .FirstOrDefaultAsync(p => p.GudangId == transferOrder.GudangTujuanId && p.Sku == skuTarget);

                            if (produkTujuan != null)
                            {
                                // Tambah stok jika produk sudah ada di gudang tujuan
                                produkTujuan.Stok += qtyDiterima;
                            }
                            else if (produkAsal != null)
                            {
                                // Buat entri produk baru di gudang tujuan jika belum pernah ada
                                var newProduk = new Produk
                                {
                                    GudangId = transferOrder.GudangTujuanId,
                                    NamaProduk = produkAsal.NamaProduk,
                                    Sku = produkAsal.Sku,
                                    Satuan = produkAsal.Satuan,
                                    Stok = qtyDiterima,
                                    HargaBeli = produkAsal.HargaBeli,
                                    HargaPerolehan = produkAsal.HargaPerolehan,
                                    Kategori = produkAsal.Kategori,
                                    TipeBarang = produkAsal.TipeBarang,
                                    RakId = produkAsal.RakId
                                };
                                _context.Produks.Add(newProduk);
                            }
                        }
                    }
                }

                transferOrder.Status = StatusTransfer.Diterima;
                transferOrder.TanggalDiterima = DateTime.Now;

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                // Log pesan error agar tahu penyebab pasti crash
                System.Diagnostics.Debug.WriteLine("Error ReceiveTransferAsync: " + ex.Message);
                return false;
            }
        }

        // Search Ajax Dynamic max 20 item
        public async Task<List<Gudang>> GetGudangPagedAsync(string? search, int limit = 20)
        {
            var query = _context.Gudangs.AsQueryable();
            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(g => g.NamaGudang.Contains(search) || g.KodeGudang.Contains(search));
            }
            return await query.Take(limit).ToListAsync();
        }

        public async Task<List<Produk>> GetProdukPagedAsync(string? search, int limit = 20)
        {
            var query = _context.Produks.AsQueryable();
            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(p => p.NamaProduk.Contains(search) || p.Sku.Contains(search));
            }
            return await query.Take(limit).ToListAsync();
        }
        private async Task<string> GenerateNoReferensiAsync()
        {
            string todayPrefix = $"TO/{DateTime.Now:yyyyMMdd}/";

            // Cari dokumen terakhir untuk hari ini
            var lastDoc = await _context.TransferOrders
                .Where(t => t.NomorDokumen.StartsWith(todayPrefix))
                .OrderByDescending(t => t.Id) // Urutkan berdasarkan Primary Key terakhir
                .Select(t => t.NomorDokumen)
                .FirstOrDefaultAsync();

            int nextSequence = 1;

            if (!string.IsNullOrEmpty(lastDoc) && lastDoc.Length >= todayPrefix.Length + 4)
            {
                string lastSeqStr = lastDoc.Substring(todayPrefix.Length);
                if (int.TryParse(lastSeqStr, out int lastSeq))
                {
                    nextSequence = lastSeq + 1;
                }
            }

            return $"{todayPrefix}{nextSequence:D4}";
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

        public async Task<TransferOrderDetailViewModel?> GetByIdAsync(int id, string penyerah)
        {
            // 1. Ambil data dasar menggunakan method GetByIdAsync(id) yang sudah ada
            var result = await GetByIdAsync(id);
            if (result == null)
            {
                return null;
            }
            // 2. Timpa atau set bagian informasi penyerah/pembuat sesuai parameter yang dikirimkan dari Controller (Print)
            if (!string.IsNullOrEmpty(penyerah))
            {
                result.DibuatOleh = penyerah;
            }

            return result;
        }

    }
}
