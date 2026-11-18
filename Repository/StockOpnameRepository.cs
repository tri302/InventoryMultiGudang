using InventoryMultiGudang.Models;
using InventoryMultiGudang.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace InventoryMultiGudang.Repository
{

    public class StockOpnameRepository : IStockOpnameRepository
    {
        private readonly ApplicationDbContext _context;

        public StockOpnameRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<(IEnumerable<StockOpnameListViewModel> Data, int TotalCount)> GetPagedAsync(string? search, int? gudangId, string? status, int page, int pageSize)
        {
            var query = _context.StockOpnames.Include(s => s.Gudang).AsQueryable();

            // Pencarian Live Search
            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim().ToLower();
                query = query.Where(x => x.NoReferensi.ToLower().Contains(search) ||
                                         x.Gudang!.NamaGudang.ToLower().Contains(search));
            }

            // Filter Gudang
            if (gudangId.HasValue && gudangId.Value > 0)
            {
                query = query.Where(x => x.GudangId == gudangId.Value);
            }

            // Filter Status
            if (!string.IsNullOrWhiteSpace(status) && status != "Semua Status")
            {
                if (Enum.TryParse<StatusOpname>(status, true, out var parsedStatus))
                {
                    query = query.Where(x => x.Status == parsedStatus);
                }
            }

            int totalCount = await query.CountAsync();

            var data = await query
                .OrderByDescending(x => x.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new StockOpnameListViewModel
                {
                    Id = x.Id,
                    NoReferensi = x.NoReferensi,
                    NamaGudang = x.Gudang!.NamaGudang,
                    KodeGudang = x.Gudang.KodeGudang,
                    Auditor = x.Auditor,
                    Status = x.Status.ToString(),
                    Catatan = x.Catatan,
                    TanggalMulai = x.TanggalMulai,
                    TanggalSelesai = x.TanggalSelesai,
                    AlasanSelisih = x.AlasanSelisih,
                    Keterangan = x.Keterangan
                })
                .ToListAsync();

            return (data, totalCount);
        }

        public async Task<StockOpname?> GetByIdAsync(int id)
        {
            return await _context.StockOpnames
                .Include(s => s.Gudang)
                .Include(s => s.Details)
                    .ThenInclude(d => d.Produk) // Include produk utama
                .Include(s => s.Details)
                    .ThenInclude(d => d.ProdukItem) // Include item SN spesifik
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<IEnumerable<Gudang>> GetAllGudangAsync()
        {
            return await _context.Gudangs.Where(x => x.IsActive == true && x.IsAudit == false).AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<Produk>> GetAllProdukAsync()
        {
            return await _context.Produks.AsNoTracking().ToListAsync();
        }

        public async Task<StockOpname> CreateDraftAsync(StockOpname entity)
        {
            entity.Status = StatusOpname.Draft;
            entity.TanggalMulai = DateTime.Now;
            _context.StockOpnames.Add(entity);

            // UPDATE ISAUDIT MENJADI TRUE PADA TABEL GUDANG SAAT DRAF DIBUAT
            var gudang = await _context.Gudangs.FindAsync(entity.GudangId);
            if (gudang != null)
            {
                gudang.IsAudit = true;
                _context.Gudangs.Update(gudang);
            }

            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateDraftAsync(StockOpname entity)
        {
            _context.StockOpnames.Update(entity);

            // PASTIKAN ISAUDIT TETAP TRUE PADA SAAT UPDATE DRAF (JIKA GUDANG BERUBAH/DISIMPAN)
            var gudang = await _context.Gudangs.FindAsync(entity.GudangId);
            if (gudang != null)
            {
                gudang.IsAudit = true;
                _context.Gudangs.Update(gudang);
            }

            await _context.SaveChangesAsync();
        }

        // Selesaikan Audit: Update status item SN, update stok fisik riil gudang, dan ubah status audit menjadi Selesai
        public async Task<bool> SelesaikanAuditAsync(int id)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var audit = await _context.StockOpnames
                    .Include(s => s.Details)
                    .Include(s => s.Gudang)
                    .FirstOrDefaultAsync(s => s.Id == id);

                if (audit == null || audit.Status != StatusOpname.Draft)
                    return false;

                audit.Status = StatusOpname.Selesai;
                audit.TanggalSelesai = DateTime.Now;

                // Ambil daftar produk unik yang di-audit dalam dokumen ini
                var produkIds = audit.Details.Select(d => d.ProdukId).Distinct();

                foreach (var produkId in produkIds)
                {
                    var productDetails = audit.Details.Where(d => d.ProdukId == produkId).ToList();

                    if (productDetails.Any())
                    {
                        int finalStokFisik = 0;
                        bool isSnTracked = productDetails.Any(d => d.ProdukItemId > 0 || !string.IsNullOrEmpty(d.SerialNumber));

                        if (isSnTracked)
                        {
                            // 1. Perbarui status pada tbl produk_item HANYA jika kondisinya "Hilang (Lost)"
                            foreach (var detail in productDetails.Where(d => d.ProdukItemId.HasValue && d.ProdukItemId > 0))
                            {
                                var produkItemDb = await _context.ProdukItems.FindAsync(detail.ProdukItemId);
                                if (produkItemDb != null)
                                {
                                    // Cek secara spesifik apakah kondisi hasil opname adalah "Hilang (Lost)"
                                    if (detail.Kondisi == "Hilang (Lost)")
                                    {
                                        produkItemDb.Status = "Lost"; // Atau "Hilang (Lost)" sesuai penamaan status di database Anda
                                        _context.ProdukItems.Update(produkItemDb);
                                    }
                                    // Kondisi selain "Dihapuskan (Disposed)" akan diskip (tidak mengubah status di tbl produk_item)
                                    // Cek secara spesifik apakah kondisi hasil opname adalah "Dihapuskan (Disposed)"
                                    if (detail.Kondisi == "Dihapuskan (Disposed)")
                                    {
                                        produkItemDb.Status = "Disposed"; // Atau "Dihapuskan (Disposed)" sesuai penamaan status di database Anda
                                        _context.ProdukItems.Update(produkItemDb);
                                    }

                                    if (detail.Kondisi == "Rusak (Damage)")
                                    {
                                        produkItemDb.Status = "Damaged"; // Atau "Rusak (Damage)" sesuai penamaan status di database Anda
                                        _context.ProdukItems.Update(produkItemDb);
                                    }
                                }
                            }

                            // 2. Hitung stok fisik akhir: Item yang berstatus In Stock, Assigned, Loaned, atau Repair tetap dihitung sebagai stok sah.
                            // Yang TIDAK dihitung sebagai stok fisik aktif hanyalah yang berkondisi/berstatus "Hilang (Lost)" atau "Dihapuskan (Disposed)".
                            finalStokFisik = productDetails.Count(d => d.Kondisi != "Hilang (Lost)" && d.Kondisi != "Dihapuskan (Disposed)");
                        }
                        else
                        {
                            // Jika non-SN, ambil nilai StokFisik dari input manual form
                            finalStokFisik = productDetails.First().StokFisik;
                        }

                        // 3. Update total stok pada tabel Produk utama
                        var produk = await _context.Produks.FindAsync(produkId);
                        if (produk != null)
                        {
                            produk.Stok = finalStokFisik;
                            _context.Produks.Update(produk);
                        }
                    }
                }

                // 4. Update status IsAudit pada tabel Gudang kembali menjadi false (gudang tidak lagi terkunci audit)
                var gudangToUpdate = await _context.Gudangs.FindAsync(audit.GudangId);
                if (gudangToUpdate != null)
                {
                    gudangToUpdate.IsAudit = false;
                    _context.Entry(gudangToUpdate).State = EntityState.Modified;
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        // Batalkan Audit: Mengunci status tanpa merubah persediaan stok
        public async Task<bool> BatalkanAuditAsync(int id)
        {
            var audit = await _context.StockOpnames.FindAsync(id);
            if (audit == null || audit.Status != StatusOpname.Draft)
                return false;
            //UPDATE FIELD ISAUDIT MENJADI false PADA TABEL GUDANG                
            var gudangToUpdate = await _context.Gudangs.FindAsync(audit.GudangId);
            if (gudangToUpdate != null)
            {
                gudangToUpdate.IsAudit = false;
                _context.Entry(gudangToUpdate).State = EntityState.Modified;
            }
            audit.Status = StatusOpname.Dibatalkan;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var audit = await _context.StockOpnames.FindAsync(id);
            if (audit == null) return false;

            _context.StockOpnames.Remove(audit);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<string> GenerateNoReferensiAsync()
        {
            string prefix = $"SO-{DateTime.Now:yyyyMM}-";
            var lastOpname = await _context.StockOpnames
                .Where(x => x.NoReferensi.StartsWith(prefix))
                .OrderByDescending(x => x.Id)
                .FirstOrDefaultAsync();

            int nextNum = 1;
            if (lastOpname != null && lastOpname.NoReferensi.Length >= 13)
            {
                string numPart = lastOpname.NoReferensi.Substring(lastOpname.NoReferensi.Length - 4);
                if (int.TryParse(numPart, out int parsedNum))
                {
                    nextNum = parsedNum + 1;
                }
            }

            return $"{prefix}{nextNum:D4}";
        }
        public async Task<IEnumerable<Produk>> GetProdukByGudangIdAsync(int gudangId)
        {
            // Ambil ID produk yang memiliki ProdukItem di gudang ini, 
            // atau produk yang memang terdaftar di GudangId tersebut
            var produkIdsDariItem = await _context.ProdukItems
                .Where(pi => pi.GudangId == gudangId)
                .Select(pi => pi.ProdukId)
                .Distinct()
                .ToListAsync();

            return await _context.Produks
                .Where(p => p.GudangId == gudangId || produkIdsDariItem.Contains(p.Id))
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Produk> GetProdukByIdAsync(int produkId)
        {
            return await _context.Produks.FindAsync(produkId);
        }

        public async Task<IEnumerable<ProdukItem>> GetProdukItemsInStockByGudangAsync(int produkId, int gudangId)
        {
            return await _context.ProdukItems
                .Where(pi => pi.ProdukId == produkId && pi.GudangId == gudangId && pi.Status == "In Stock")
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<StockOpname?> GetByIdAsync(int id, string penyerah)
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
                result.Auditor = penyerah;
            }

            return result;
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