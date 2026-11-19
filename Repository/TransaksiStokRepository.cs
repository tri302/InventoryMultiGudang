using InventoryMultiGudang.Models;
using InventoryMultiGudang.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace InventoryMultiGudang.Repository
{
    public class TransaksiStokRepository : ITransaksiStokRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        public TransaksiStokRepository(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<(List<TransaksiStok> Data, int TotalCount)> GetPagedAsync(string? search, int? gudangId, string? tipe, int page, int pageSize)
        {
            var query = _context.TransaksiStoks
                .Include(t => t.Gudang)
                .Include(t => t.Supplier)
                .Include(t => t.Details)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(x => x.NomorReferensi.Contains(search)
                    || (x.Catatan != null && x.Catatan.Contains(search))
                    || x.Details.Any(d => d.Produk!.NamaProduk.Contains(search)));
            }

            if (gudangId.HasValue && gudangId.Value > 0)
            {
                query = query.Where(x => x.GudangId == gudangId.Value);
            }

            if (!string.IsNullOrWhiteSpace(tipe) && tipe != "Semua Tipe")
            {
                query = query.Where(x => x.TipeTransaksi == tipe);
            }

            int total = await query.CountAsync();
            var data = await query.OrderByDescending(x => x.TanggalPosting)
                                  .Skip((page - 1) * pageSize)
                                  .Take(pageSize)
                                  .ToListAsync();

            return (data, total);
        }

        public async Task<TransaksiStok?> GetByIdAsync(int id)
        {
            return await _context.TransaksiStoks
                .Include(t => t.Gudang)
                .Include(t => t.Supplier)
                .Include(t => t.Details)
                    .ThenInclude(d => d.Produk)
                .Include(t => t.Details)
                    .ThenInclude(d => d.Rak)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        // CREATE: Update Stok Produk & ProdukItem (+ untuk Stock In, - untuk Stock Out)
        public async Task<bool> CreateAsync(TransaksiStok entity)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var detailsList = entity.Details.ToList();
                entity.Details = new List<TransaksiStokDetail>();

                await _context.TransaksiStoks.AddAsync(entity);
                await _context.SaveChangesAsync(); // Simpan header dulu agar entity.Id terisi

                foreach (var detail in detailsList)
                {
                    detail.TransaksiStokId = entity.Id;
                    await _context.TransaksiStokDetails.AddAsync(detail);
                    await _context.SaveChangesAsync();

                    var produk = await _context.Produks.FindAsync(detail.ProdukId);
                    if (produk != null)
                    {
                        bool isBerSN = produk.PelacakanBarang == "Pelacakan Serial Number (SN)";
                        if (entity.TipeTransaksi == "Stock In")
                        {
                            produk.Stok += detail.Jumlah;

                            if (isBerSN)
                            {
                                await AutoGenerateProductItemsAsync(
                                    detail.SerialNumbers,
                                    detail.ProdukId,
                                    entity.GudangId,
                                    detail.RakId,
                                    detail.Jumlah,
                                    _env.WebRootPath
                                );
                            }
                        }
                        else if (entity.TipeTransaksi == "Stock Out")
                        {
                            produk.Stok -= detail.Jumlah;

                            if (isBerSN)
                            {
                                List<string> snList = new List<string>();
                                if (!string.IsNullOrWhiteSpace(detail.SerialNumbers))
                                {
                                    snList = detail.SerialNumbers
                                        .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                        .Select(s => s.Trim())
                                        .ToList();
                                }

                                // Validasi ketat: Hanya ambil item yang benar-benar berstatus "In Stock"
                                var itemsToUpdate = await _context.ProdukItems
                                    .Where(pi => pi.ProdukId == detail.ProdukId && pi.GudangId == entity.GudangId && pi.Status == "In Stock" && (snList.Count == 0 || snList.Contains(pi.SerialNumber)))
                                    .Take(detail.Jumlah)
                                    .ToListAsync();

                                if (itemsToUpdate.Count < detail.Jumlah)
                                {
                                    throw new Exception($"Stok 'In Stock' untuk produk {produk.NamaProduk} tidak mencukupi atau sebagian barang sedang dipinjam/status bukan In Stock.");
                                }

                                // Tentukan status: Sold jika Penjualan, Out jika bukan
                                string targetStatus = entity.IsPenjualan ? "Sold" : "Out";
                                foreach (var item in itemsToUpdate)
                                {
                                    item.Status = targetStatus;
                                }
                                await _context.SaveChangesAsync();
                            }
                        }
                    }
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception(ex.InnerException != null ? ex.InnerException.Message : ex.Message);
            }
        }

        // UPDATE: Revert stok lama, kemudian apply stok baru
        public async Task<bool> UpdateAsync(TransaksiStok entity)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var existingEntity = await _context.TransaksiStoks
                    .Include(t => t.Details)
                    .FirstOrDefaultAsync(t => t.Id == entity.Id);

                if (existingEntity == null) return false;

                // 1. Revert Efek Transaksi Lama
                foreach (var oldDetail in existingEntity.Details)
                {
                    var produk = await _context.Produks.FindAsync(oldDetail.ProdukId);
                    if (produk != null)
                    {
                        bool isBerSN = produk.PelacakanBarang == "Pelacakan Serial Number (SN)";

                        if (existingEntity.TipeTransaksi == "Stock In")
                        {
                            produk.Stok -= oldDetail.Jumlah;
                            if (isBerSN)
                            {
                                List<string> oldSnList = !string.IsNullOrWhiteSpace(oldDetail.SerialNumbers)
                                    ? oldDetail.SerialNumbers.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToList()
                                    : new List<string>();

                                var itemsToRemove = await _context.ProdukItems
                                    .Where(pi => pi.ProdukId == oldDetail.ProdukId && pi.GudangId == existingEntity.GudangId && (oldSnList.Count == 0 || oldSnList.Contains(pi.SerialNumber)))
                                    .Take(oldDetail.Jumlah)
                                    .ToListAsync();
                                _context.ProdukItems.RemoveRange(itemsToRemove);
                            }
                        }
                        else if (existingEntity.TipeTransaksi == "Stock Out")
                        {
                            produk.Stok += oldDetail.Jumlah;
                            if (isBerSN)
                            {
                                List<string> oldSnList = !string.IsNullOrWhiteSpace(oldDetail.SerialNumbers)
                                    ? oldDetail.SerialNumbers.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToList()
                                    : new List<string>();

                                // Kembalikan status item yang tadinya Sold/Out menjadi In Stock
                                var itemsToRevert = await _context.ProdukItems
                                    .Where(pi => pi.ProdukId == oldDetail.ProdukId && pi.GudangId == existingEntity.GudangId && (pi.Status == "Sold" || pi.Status == "Out") && (oldSnList.Count == 0 || oldSnList.Contains(pi.SerialNumber)))
                                    .Take(oldDetail.Jumlah)
                                    .ToListAsync();

                                foreach (var item in itemsToRevert)
                                {
                                    item.Status = "In Stock";
                                }
                            }
                        }
                    }
                }

                // Update Properties Transaksi Utama
                existingEntity.GudangId = entity.GudangId;
                existingEntity.SupplierId = entity.SupplierId;
                existingEntity.NomorReferensi = entity.NomorReferensi;
                existingEntity.Catatan = entity.Catatan;
                existingEntity.TipeTransaksi = entity.TipeTransaksi;
                existingEntity.IsPenjualan = entity.IsPenjualan;

                _context.TransaksiStokDetails.RemoveRange(existingEntity.Details);
                existingEntity.Details = entity.Details;

                // 2. Terapkan Transaksi Baru
                foreach (var newDetail in entity.Details)
                {
                    newDetail.TransaksiStokId = existingEntity.Id;
                    await _context.TransaksiStokDetails.AddAsync(newDetail);

                    var produk = await _context.Produks.FindAsync(newDetail.ProdukId);
                    if (produk != null)
                    {
                        bool isBerSN = produk.PelacakanBarang == "Pelacakan Serial Number (SN)";

                        if (entity.TipeTransaksi == "Stock In")
                        {
                            produk.Stok += newDetail.Jumlah;
                            if (isBerSN)
                            {
                                await AutoGenerateProductItemsAsync(newDetail.SerialNumbers, newDetail.ProdukId, entity.GudangId, newDetail.RakId, newDetail.Jumlah, _env.WebRootPath);
                            }
                        }
                        else if (entity.TipeTransaksi == "Stock Out")
                        {
                            produk.Stok -= newDetail.Jumlah;
                            if (isBerSN)
                            {
                                List<string> newSnList = !string.IsNullOrWhiteSpace(newDetail.SerialNumbers)
                                    ? newDetail.SerialNumbers.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToList()
                                    : new List<string>();

                                var itemsToUpdate = await _context.ProdukItems
                                    .Where(pi => pi.ProdukId == newDetail.ProdukId && pi.GudangId == entity.GudangId && pi.Status == "In Stock" && (newSnList.Count == 0 || newSnList.Contains(pi.SerialNumber)))
                                    .Take(newDetail.Jumlah)
                                    .ToListAsync();

                                if (itemsToUpdate.Count < newDetail.Jumlah)
                                {
                                    throw new Exception($"Stok 'In Stock' untuk produk {produk.NamaProduk} tidak mencukupi.");
                                }

                                string targetStatus = entity.IsPenjualan ? "Sold" : "Out";
                                foreach (var item in itemsToUpdate)
                                {
                                    item.Status = targetStatus;
                                }
                            }
                        }
                    }
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception(ex.InnerException != null ? ex.InnerException.Message : ex.Message);
            }
        }

        // DELETE: Revert Stok & ProdukItem Saat Transaksi Dihapus
        public async Task<bool> DeleteAsync(int id)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var item = await _context.TransaksiStoks
                    .Include(t => t.Details)
                    .FirstOrDefaultAsync(t => t.Id == id);

                if (item == null) return false;

                foreach (var detail in item.Details)
                {
                    var produk = await _context.Produks.FindAsync(detail.ProdukId);
                    if (produk != null)
                    {
                        bool isBerSN = produk.PelacakanBarang == "Pelacakan Serial Number (SN)";

                        if (item.TipeTransaksi == "Stock In")
                        {
                            produk.Stok -= detail.Jumlah;
                            if (isBerSN)
                            {
                                List<string> snList = !string.IsNullOrWhiteSpace(detail.SerialNumbers)
                                    ? detail.SerialNumbers.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToList()
                                    : new List<string>();

                                var itemsToRemove = await _context.ProdukItems
                                    .Where(pi => pi.ProdukId == detail.ProdukId && pi.GudangId == item.GudangId && (snList.Count == 0 || snList.Contains(pi.SerialNumber)))
                                    .Take(detail.Jumlah)
                                    .ToListAsync();
                                _context.ProdukItems.RemoveRange(itemsToRemove);
                            }
                        }
                        else if (item.TipeTransaksi == "Stock Out")
                        {
                            produk.Stok += detail.Jumlah;
                            if (isBerSN)
                            {
                                List<string> snList = !string.IsNullOrWhiteSpace(detail.SerialNumbers)
                                    ? detail.SerialNumbers.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToList()
                                    : new List<string>();

                                var itemsToRevert = await _context.ProdukItems
                                    .Where(pi => pi.ProdukId == detail.ProdukId && pi.GudangId == item.GudangId && (pi.Status == "Sold" || pi.Status == "Out") && (snList.Count == 0 || snList.Contains(pi.SerialNumber)))
                                    .Take(detail.Jumlah)
                                    .ToListAsync();

                                foreach (var itemToRev in itemsToRevert)
                                {
                                    itemToRev.Status = "In Stock";
                                }
                            }
                        }
                    }
                }

                _context.TransaksiStoks.Remove(item);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting transaction: {ex.Message}");
                await transaction.RollbackAsync();
                return false;
            }
        }

        public async Task AutoGenerateProductItemsAsync(string? inputSerialNumbers, int produkId, int gudangId, int rakId, int quantity, string webRootPath)
        {
            var produk = await _context.Produks.AsNoTracking().FirstOrDefaultAsync(p => p.Id == produkId);
            var gudang = await _context.Gudangs.AsNoTracking().FirstOrDefaultAsync(g => g.Id == gudangId);

            if (produk == null || gudang == null) return;

            string baseSku = produk.Sku.Replace("SKU-", "");
            int currentCount = await _context.ProdukItems.AsNoTracking().CountAsync(p => p.ProdukId == produkId);

            string qrFolder = Path.Combine(webRootPath, "qrcodes");
            if (!Directory.Exists(qrFolder))
            {
                Directory.CreateDirectory(qrFolder);
            }

            var newItems = new List<ProdukItem>();
            List<string> providedSnList = new List<string>();
            if (!string.IsNullOrWhiteSpace(inputSerialNumbers))
            {
                providedSnList = inputSerialNumbers
                    .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(s => s.Trim())
                    .ToList();
            }

            using (var qrGenerator = new QRCoder.QRCodeGenerator())
            {
                for (int i = 0; i < quantity; i++)
                {
                    string serialNumber = i < providedSnList.Count ? providedSnList[i] : $"{baseSku}-{gudang.KodeGudang}-{currentCount + i + 1:D4}";

                    string fileName = $"{serialNumber.Replace("/", "-")}.png";
                    string fullPath = Path.Combine(qrFolder, fileName);
                    string relativePath = $"/qrcodes/{fileName}";

                    var qrCodeData = qrGenerator.CreateQrCode(serialNumber, QRCoder.QRCodeGenerator.ECCLevel.Q);
                    var qrCode = new QRCoder.PngByteQRCode(qrCodeData);
                    byte[] qrCodeAsPngByteArr = qrCode.GetGraphic(20);

                    await File.WriteAllBytesAsync(fullPath, qrCodeAsPngByteArr);

                    newItems.Add(new ProdukItem
                    {
                        ProdukId = produkId,
                        GudangId = gudangId,
                        RakId = rakId,
                        SerialNumber = serialNumber,
                        Status = "In Stock",
                        QrCodePath = relativePath,
                        CreatedAt = DateTime.Now
                    });
                }
            }

            await _context.ProdukItems.AddRangeAsync(newItems);
            await _context.SaveChangesAsync();
        }

        public async Task<List<DropdownItemDto>> GetDropdownGudangAsync(string? q, int page = 1, int pageSize = 20)
        {
            var query = _context.Gudangs.AsNoTracking().AsQueryable();
            if (!string.IsNullOrWhiteSpace(q))
            {
                query = query.Where(g => g.NamaGudang.Contains(q) || g.KodeGudang.Contains(q));
            }
            return await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(g => new DropdownItemDto { id = g.Id, text = $"{g.NamaGudang} ({g.KodeGudang})" })
                .ToListAsync();
        }

        public async Task<List<DropdownItemDto>> GetDropdownSupplierAsync(string? q, int page = 1, int pageSize = 20)
        {
            var query = _context.Suppliers.AsNoTracking().AsQueryable();
            if (!string.IsNullOrWhiteSpace(q))
            {
                query = query.Where(s => s.NamaSupplier.Contains(q));
            }
            return await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(s => new DropdownItemDto { id = s.Id, text = s.NamaSupplier })
                .ToListAsync();
        }

        public async Task<List<DropdownItemDto>> GetDropdownProdukAsync(string? q, int page = 1, int pageSize = 20)
        {
            var query = _context.Produks.AsNoTracking().AsQueryable();
            if (!string.IsNullOrWhiteSpace(q))
            {
                query = query.Where(p => p.NamaProduk.Contains(q) || p.Sku.Contains(q));
            }
            return await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(p => new DropdownItemDto { id = p.Id, text = $"[{p.Sku}] {p.NamaProduk} (Stok: {p.Stok})" })
                .ToListAsync();
        }

        public async Task<object> GetDropdownRakByGudangAsync(int gudangId, string? q, int page = 1, int pageSize = 20)
        {
            var query = _context.Raks.AsNoTracking().Where(r => r.GudangId == gudangId && r.IsActive == true);
            if (!string.IsNullOrWhiteSpace(q))
            {
                query = query.Where(r => r.KodeLokasi.Contains(q) || r.NamaRak.Contains(q));
            }
            return await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(r => new { id = r.Id, text = $"[{r.KodeLokasi}] {r.NamaRak}" })
                .ToListAsync();
        }

        public async Task<object?> GetDetailKartuStokAsync(int id)
        {
            var transaksi = await GetByIdAsync(id);
            if (transaksi == null) return null;
            return new { transaksi.Id, transaksi.NomorReferensi };
        }

        public async Task<List<DropdownItemDto>> GetDropdownProdukByGudangAsync(int gudangId, string? q, int page = 1, int pageSize = 20)
        {
            var query = _context.Produks
                .AsNoTracking()
                .Where(p => p.GudangId == gudangId)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(q))
            {
                query = query.Where(p => p.NamaProduk.Contains(q) || p.Sku.Contains(q));
            }

            var produkList = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var result = new List<DropdownItemDto>();

            foreach (var p in produkList)
            {
                int realStock = 0;
                bool isBerSN = p.PelacakanBarang == "Pelacakan Serial Number (SN)";

                if (isBerSN)
                {
                    // Hitung hanya yang statusnya "In Stock" untuk produk ber-SN
                    realStock = await _context.ProdukItems
                        .CountAsync(pi => pi.ProdukId == p.Id && pi.GudangId == gudangId && pi.Status == "In Stock");
                }
                else
                {
                    // Jika produk non-SN / bulk, gunakan nilai stok dari tabel Produk
                    realStock = p.Stok;
                }

                // Opsional: Jika ingin menyembunyikan produk yang stok In Stock-nya 0 di form Stock Out
                if (realStock > 0)
                {
                    result.Add(new DropdownItemDto
                    {
                        id = p.Id,
                        text = $"[{p.Sku}] {p.NamaProduk} (Stok: {realStock})"
                    });
                }
            }

            return result;
        }

        public async Task<object?> GetProdukDetailForTransactionAsync(int produkId, int gudangId, string tipeTransaksi, int jumlah = 1)
        {
            var produk = await _context.Produks
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == produkId);

            if (produk == null) return null;

            bool isBerSN = produk.PelacakanBarang == "Pelacakan Serial Number (SN)";
            List<string> availableSerialNumbers = new List<string>();

            if (isBerSN && tipeTransaksi == "Stock Out")
            {
                availableSerialNumbers = await _context.ProdukItems
                    .AsNoTracking()
                    .Where(pi => pi.ProdukId == produkId && pi.GudangId == gudangId && pi.Status == "In Stock")
                    .Take(jumlah)
                    .Select(pi => pi.SerialNumber)
                    .ToListAsync();
            }

            return new
            {
                produk.Id,
                produk.Sku,
                produk.NamaProduk,
                IsBerSN = isBerSN,
                AvailableSerialNumbers = availableSerialNumbers
            };
        }

        // Implementasi Overload GetByIdAsync dengan parameter penyerah
        public async Task<TransaksiStok?> GetByIdAsync(int id, string penyerah)
        {
            var transaksi = await GetByIdAsync(id);
            if (transaksi != null)
            {
                // Menyimpan nama penyerah ke properti Pembuat untuk ditampilkan pada dokumen cetak
                transaksi.Pembuat = penyerah;
            }
            return transaksi;
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