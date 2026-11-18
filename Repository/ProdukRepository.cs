using InventoryMultiGudang.Models;
using InventoryMultiGudang.ViewModels;
using Microsoft.EntityFrameworkCore;
using QRCoder;

namespace InventoryMultiGudang.Repository
{
    public class ProdukRepository : IProdukRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;
        public ProdukRepository(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<PagedResultProduk<ProdukViewModel>> GetPagedAsync(string? search, string? kategori, int page, int pageSize)
        {
            var query = _context.Produks.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(p => p.NamaProduk.Contains(search) || p.Sku.Contains(search));
            }

            if (!string.IsNullOrWhiteSpace(kategori) && kategori != "Semua Kategori")
            {
                query = query.Where(p => p.Kategori == kategori);
            }

            int totalItems = await query.CountAsync();

            var items = await query
                .OrderByDescending(p => p.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(p => new ProdukViewModel
                {
                    Id = p.Id,
                    Sku = p.Sku,
                    NamaProduk = p.NamaProduk,
                    Kategori = p.Kategori,
                    Merek = p.Merek,
                    Satuan = p.Satuan,
                    TipeBarang = p.TipeBarang,
                    HargaBeli = p.HargaBeli,
                    HargaPerolehan = p.HargaPerolehan,
                    Deskripsi = p.Deskripsi,
                    PelacakanBarang = p.PelacakanBarang,
                    Stok = p.Stok
                }).ToListAsync();

            return new PagedResultProduk<ProdukViewModel>
            {
                Items = items,
                TotalItems = totalItems,
                PageNumber = page,
                PageSize = pageSize
            };
        }

        public async Task<ProdukViewModel?> GetByIdAsync(int id)
        {
            var produk = await _context.Produks
                .Include(p => p.Gudang)
                .Include(p => p.Rak)
                .Include(p => p.MasterUom)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (produk == null) return null;

            return new ProdukViewModel
            {
                Id = produk.Id,
                Sku = produk.Sku,
                NamaProduk = produk.NamaProduk,

                // Map Gudang & Rak
                GudangId = produk.GudangId,
                NamaGudang = produk.Gudang?.NamaGudang,
                RakId = produk.RakId,
                NamaRak = produk.Rak?.NamaRak,

                Kategori = produk.Kategori,
                Merek = produk.Merek,
                Satuan = produk.Satuan,

                UomId = produk.MasterUomId,
                NamaUom = produk.MasterUom?.Name,

                TipeBarang = produk.TipeBarang,
                HargaBeli = produk.HargaBeli,
                HargaPerolehan = produk.HargaPerolehan,
                Deskripsi = produk.Deskripsi,
                PelacakanBarang = produk.PelacakanBarang,
                Stok = produk.Stok
            };
        }
        
        public async Task<bool> CreateAsync(ProdukViewModel vm)
        {
            var gudang = await _context.Gudangs.FindAsync(vm.GudangId);
            bool isVirtual = gudang != null && gudang.NamaGudang.Contains("Gudang Transit Virtual");

            // Ambil MasterUom dari database berdasarkan UomId yang dipilih
            string namaSatuan = vm.Satuan;
            if (vm.UomId.HasValue && vm.UomId > 0)
            {
                var uom = await _context.MasterUoms.FindAsync(vm.UomId.Value);
                if (uom != null)
                {
                    namaSatuan = uom.Name; // Atau uom.Code tergantung kebutuhan (misal: "Unit")
                }
            }

            var entity = new Produk
            {
                Sku = vm.Sku,
                NamaProduk = vm.NamaProduk,
                GudangId = vm.GudangId,
                RakId = isVirtual ? null : vm.RakId,
                Kategori = vm.Kategori,
                Merek = vm.Merek,

                // Set UOM ID dan Nama Satuan
                MasterUomId = vm.UomId,
                Satuan = !string.IsNullOrEmpty(namaSatuan) ? namaSatuan : "Pcs",

                TipeBarang = vm.TipeBarang,
                HargaBeli = vm.HargaBeli,
                HargaPerolehan = vm.HargaPerolehan,
                Deskripsi = vm.Deskripsi,
                PelacakanBarang = vm.PelacakanBarang,
                Stok = vm.Stok
            };

            _context.Produks.Add(entity);
            var saved = await _context.SaveChangesAsync() > 0;

            if (saved && entity.Stok > 0 && entity.GudangId > 0 && !isVirtual && entity.RakId.HasValue &&
                entity.PelacakanBarang == "Pelacakan Serial Number (SN)")
            {
                await AutoGenerateProductItemsAsync(
                    entity.Id,
                    entity.GudangId,
                    entity.RakId.Value,
                    entity.Stok,
                    _env.WebRootPath
                );
            }

            return saved;
        }
        
        public async Task<bool> UpdateAsync(ProdukViewModel vm)
        {
            var entity = await _context.Produks.FindAsync(vm.Id);
            if (entity == null) return false;

            var gudang = await _context.Gudangs.FindAsync(vm.GudangId);
            bool isVirtual = gudang != null && gudang.NamaGudang.Contains("Gudang Transit Virtual");
            int oldStok = entity.Stok;

            // Ambil MasterUom dari database berdasarkan UomId yang di-select
            string namaSatuan = vm.Satuan;
            if (vm.UomId.HasValue && vm.UomId > 0)
            {
                var uom = await _context.MasterUoms.FindAsync(vm.UomId.Value);
                if (uom != null)
                {
                    namaSatuan = uom.Name;
                }
            }

            entity.NamaProduk = vm.NamaProduk;
            entity.GudangId = vm.GudangId;
            entity.RakId = isVirtual ? null : vm.RakId;
            entity.Kategori = vm.Kategori;
            entity.Merek = vm.Merek;

            // Update UOM ID & Text Satuan
            entity.MasterUomId = vm.UomId;
            if (!string.IsNullOrEmpty(namaSatuan))
            {
                entity.Satuan = namaSatuan;
            }

            entity.TipeBarang = vm.TipeBarang;
            entity.HargaBeli = vm.HargaBeli;
            entity.HargaPerolehan = vm.HargaPerolehan;
            entity.Deskripsi = vm.Deskripsi;
            entity.PelacakanBarang = vm.PelacakanBarang;
            entity.Stok = vm.Stok;

            _context.Produks.Update(entity);
            var updated = await _context.SaveChangesAsync() > 0;

            if (updated && vm.Stok > oldStok && entity.GudangId > 0 && !isVirtual && entity.RakId.HasValue &&
                entity.PelacakanBarang == "Pelacakan Serial Number (SN)")
            {
                int selisihStok = vm.Stok - oldStok;
                await AutoGenerateProductItemsAsync(
                    entity.Id,
                    entity.GudangId,
                    entity.RakId.Value,
                    selisihStok,
                    _env.WebRootPath
                );
            }

            return updated;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.Produks.FindAsync(id);
            if (entity == null) return false;

            _context.Produks.Remove(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> IsSkuExistsAsync(string sku, int excludeId = 0)
        {
            return await _context.Produks.AnyAsync(p => p.Sku == sku && p.Id != excludeId);
        }
        public async Task<List<DropdownItemDto>> GetDropdownAsync(string? q)
        {
            var query = _context.Gudangs.Where(x => x.IsAudit == false).AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(q))
            {
                query = query.Where(g => g.NamaGudang.Contains(q));
            }

            return await query
                .Select(g => new DropdownItemDto
                {
                    id = g.Id,
                    text = g.NamaGudang
                })
                .ToListAsync();
        }
        public async Task<object> GetDropdownByGudangAsync(int gudangId, string? q)
        {
            var query = _context.Raks
                .AsNoTracking()
                .Where(r => r.GudangId == gudangId && r.IsActive == true);

            if (!string.IsNullOrWhiteSpace(q))
            {
                query = query.Where(r => r.KodeLokasi.Contains(q) ||
                                         r.NamaRak.Contains(q) ||
                                         r.Area.Contains(q) ||
                                         r.BinNo.Contains(q) ||
                                         r.Deskripsi.Contains(q));
            }

            var results = await query
                .Select(r => new
                {
                    id = r.Id,
                    text = $"[{r.KodeLokasi}] {r.NamaRak}",
                    kode = r.KodeLokasi,
                    namaRak = r.NamaRak,
                    area = r.Area,
                    binNo = r.BinNo,
                    deskripsi = r.Deskripsi
                })
                .ToListAsync();

            return results;
        }

        public async Task<ProdukSerialNumbersResultDto?> GetSerialNumbersByProdukIdAsync(int produkId)
        {
            var produk = await _context.Produks.AsNoTracking().FirstOrDefaultAsync(p => p.Id == produkId);
            if (produk == null) return null;

            var items = await _context.ProdukItems
                .AsNoTracking()
                .Include(pi => pi.Gudang)
                .Include(pi => pi.Rak)
                .Where(pi => pi.ProdukId == produkId)
                .OrderBy(pi => pi.Id)
                .Select(pi => new ProdukSerialNumberDto
                {
                    Id = pi.Id,
                    SerialNumber = pi.SerialNumber,
                    Status = pi.Status,
                    NamaGudang = pi.Gudang != null ? pi.Gudang.NamaGudang : "-",
                    NamaRak = pi.Rak != null ? pi.Rak.NamaRak : "-"
                })
                .ToListAsync();

            return new ProdukSerialNumbersResultDto
            {
                NamaProduk = produk.NamaProduk,
                Sku = produk.Sku,
                Items = items
            };
        }

        public async Task AutoGenerateProductItemsAsync(int produkId, int gudangId, int rakId, int quantity, string webRootPath)
        {
            var produk = await _context.Produks.FindAsync(produkId);
            var gudang = await _context.Gudangs.FindAsync(gudangId);

            if (produk == null || gudang == null) return;

            string baseSku = produk.Sku.Replace("SKU-", "");
            int currentCount = await _context.ProdukItems.CountAsync(p => p.ProdukId == produkId);

            // Path folder penyimpanan QR Code (misal: wwwroot/qrcodes)
            string qrFolder = Path.Combine(webRootPath, "qrcodes");
            if (!Directory.Exists(qrFolder))
            {
                Directory.CreateDirectory(qrFolder);
            }

            var newItems = new List<ProdukItem>();

            using (var qrGenerator = new QRCodeGenerator())
            {
                for (int i = 1; i <= quantity; i++)
                {
                    int nextSeq = currentCount + i;
                    string serialNumber = $"{baseSku}-{gudang.KodeGudang}-{nextSeq:D4}";

                    // 1. Generate QR Code Image
                    string fileName = $"{serialNumber}.png";
                    string fullPath = Path.Combine(qrFolder, fileName);
                    string relativePath = $"/qrcodes/{fileName}";

                    QRCodeData qrCodeData = qrGenerator.CreateQrCode(serialNumber, QRCodeGenerator.ECCLevel.Q);
                    PngByteQRCode qrCode = new PngByteQRCode(qrCodeData);
                    byte[] qrCodeAsPngByteArr = qrCode.GetGraphic(20);

                    // 2. Simpan file gambar ke wwwroot/qrcodes/
                    await File.WriteAllBytesAsync(fullPath, qrCodeAsPngByteArr);

                    // 3. Tambahkan entity
                    newItems.Add(new ProdukItem
                    {
                        ProdukId = produkId,
                        GudangId = gudangId,
                        RakId = rakId,
                        SerialNumber = serialNumber,
                        Status = "In Stock",
                        QrCodePath = relativePath
                    });
                }
            }

            _context.ProdukItems.AddRange(newItems);
            await _context.SaveChangesAsync();
        }

        public async Task<ProdukItemPrintDto?> GetProdukItemForPrintAsync(int produkItemId)
        {
            var item = await _context.ProdukItems
                .AsNoTracking()
                .Include(pi => pi.Produk)
                .Include(pi => pi.Gudang)
                .Include(pi => pi.Rak)
                .FirstOrDefaultAsync(pi => pi.Id == produkItemId);

            if (item == null) return null;

            return new ProdukItemPrintDto
            {
                Id = item.Id,
                SerialNumber = item.SerialNumber,
                QrCodePath = item.QrCodePath ?? string.Empty,
                NamaProduk = item.Produk?.NamaProduk ?? "-",
                KodeGudang = item.Gudang?.KodeGudang ?? "-",
                KodeRak = item.Rak?.KodeLokasi ?? "-"
            };
        }

        public async Task<string> GenerateNextSkuAsync(string? kategori)
        {
            // 1. Tentukan Prefix berdasarkan Kategori
            string prefixCode = "GEN";

            if (!string.IsNullOrWhiteSpace(kategori))
            {
                // Ambil 3 karakter pertama dari nama kategori (hanya huruf/angka), lalu uppercase
                string cleanKategori = new string(kategori.Where(char.IsLetterOrDigit).ToArray());
                if (cleanKategori.Length >= 3)
                {
                    prefixCode = cleanKategori.Substring(0, 3).ToUpper();
                }
                else if (cleanKategori.Length > 0)
                {
                    prefixCode = cleanKategori.ToUpper().PadRight(3, 'X');
                }
            }

            string fullPrefix = $"SKU-{prefixCode}-";

            // 2. Cari SKU terakhir di database yang menggunakan prefix tersebut
            var lastSku = await _context.Produks
                .AsNoTracking()
                .Where(p => p.Sku.StartsWith(fullPrefix))
                .OrderByDescending(p => p.Id)
                .Select(p => p.Sku)
                .FirstOrDefaultAsync();

            int nextSeq = 1;

            if (!string.IsNullOrEmpty(lastSku))
            {
                // 3. Ekstrak bagian nomor di akhir (misal "SKU-ITE-0004" -> 4)
                string numberPart = lastSku.Replace(fullPrefix, "");
                if (int.TryParse(numberPart, out int currentNumber))
                {
                    nextSeq = currentNumber + 1;
                }
            }

            // 4. Format urutan dengan 4 digit pad (misal: SKU-ITE-0001)
            return $"{fullPrefix}{nextSeq:D4}";
        }

        // Additional methods for dropdowns
        public async Task<object> GetDropdownKategoriAsync(string? q)
        {
            var query = _context.Kategoris.AsNoTracking().AsQueryable();
            if (!string.IsNullOrWhiteSpace(q))
            {
                query = query.Where(k => k.NamaKategori.Contains(q));
            }
            return await query
                .Take(2)
                .Select(k => new
                {
                    id = k.NamaKategori,
                    text = k.NamaKategori
                })
                .ToListAsync();
        }
    }
}
