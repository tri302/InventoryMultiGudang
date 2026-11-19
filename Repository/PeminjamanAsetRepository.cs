using InventoryMultiGudang.Models;
using InventoryMultiGudang.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace InventoryMultiGudang.Repository
{
    public class PeminjamanAsetRepository : IPeminjamanAsetRepository
    {
        private readonly ApplicationDbContext _context;        

        public PeminjamanAsetRepository(ApplicationDbContext context)
        {
            _context = context;            
        }

        // Mengambil data dengan pagination, search, dan filter status menggunakan EF Core
        public async Task<(IEnumerable<PeminjamanAsetFormViewModel> Items, int TotalCount)> GetPagedAsync(string search, StatusPeminjaman? status, int pageIndex, int pageSize, int? gudangId)
        {
            // Jalankan update status terlambat terlebih dahulu sebelum query data
            await UpdateStatusOtomatisTerlambatAsync();

            var query = _context.PeminjamanAsets
                .Include(p => p.Details)
                .Include(p => p.Karyawan) // Tambahkan Include agar Karyawan tidak null
                .AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(x => x.KodePeminjaman.Contains(search));
            }

            // Filter berdasarkan gudangId jika dipilih
            if (gudangId.HasValue && gudangId.Value > 0)
            {
                query = query.Where(x => x.GudangId == gudangId.Value);
            }

            // Filter menggunakan tipe StatusPeminjaman secara langsung
            if (status.HasValue)
            {
                query = query.Where(x => x.Status == status.Value);
            }

            int totalCount = await query.CountAsync();

            var itemsQuery = await query
                .OrderByDescending(x => x.Id)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new PeminjamanAsetFormViewModel
                {
                    Id = x.Id,
                    GudangId = x.GudangId,
                    KaryawanNik = x.Karyawan != null ? x.Karyawan.Nik : null,
                    KaryawanNama = x.Karyawan.NamaLengkap,
                    KodePeminjaman = x.KodePeminjaman,                    

                    // Konversi DateTime dari database ke string berformat "dd/MM/yyyy" untuk ViewModel
                    TanggalPinjam = x.TanggalPinjam.ToString("dd/MM/yyyy"),
                    TanggalJatuhTempo = x.TanggalJatuhTempo.ToString("dd/MM/yyyy"),
                    TanggalPengembalian = x.TanggalPengembalian.HasValue ? x.TanggalPengembalian.Value.ToString("dd/MM/yyyy") : null,

                    // Simpan juga tanggal asli dalam bentuk DateTime sementara untuk perhitungan selisih hari di bawah
                    // (Pastikan properti ini ada di ViewModel Anda, atau simpan ke variabel lokal)
                    _RawTanggalPinjam = x.TanggalPinjam,
                    _RawTanggalJatuhTempo = x.TanggalJatuhTempo,
                    _RawTanggalPengembalian = x.TanggalPengembalian,

                    Status = x.Status,
                    JumlahHariPinjam = x.JumlahHariPinjam,
                    JumlahHariTelatPinjam = x.JumlahHariTelatPinjam,
                    Details = x.Details.Select(d => new PeminjamanAsetDetailViewModel
                    {
                        Id = d.Id,
                        ProdukItemId = d.ProdukItemId,
                        NamaBarang = d.NamaBarang,
                        SerialNumber = d.SerialNumber
                    }).ToList()
                })
                .ToListAsync();

            // Lakukan perhitungan hari telat di memori C# menggunakan data asli DateTime (_Raw...)
            var items = itemsQuery.Select(x =>
            {
                if (x._RawTanggalPengembalian.HasValue && x._RawTanggalPengembalian.Value.Date > x._RawTanggalJatuhTempo.Date)
                {
                    x.JumlahHariTelatPinjam = (x._RawTanggalPengembalian.Value.Date - x._RawTanggalJatuhTempo.Date).Days;
                }
                else if (x.Status == StatusPeminjaman.Terlambat)
                {
                    x.JumlahHariTelatPinjam = (DateTime.Today - x._RawTanggalJatuhTempo.Date).Days;
                }
                else
                {
                    x.JumlahHariTelatPinjam = 0;
                }
                return x;
            }).ToList();

            return (items, totalCount);
        }

        public async Task<PeminjamanAsetFormViewModel> GetByIdAsync(int id, string currentUserName = "Admin")
        {
            var header = await _context.PeminjamanAsets
        .Include(x => x.Details)
            .ThenInclude(d => d.ProdukItem)
                .ThenInclude(pi => pi.Produk)
        .Include(x => x.Details)
            .ThenInclude(d => d.ProdukItem)
                .ThenInclude(pi => pi.Gudang)
        .Include(x => x.Details)
            .ThenInclude(d => d.ProdukItem)
                .ThenInclude(pi => pi.Rak)
        .Include(x => x.Karyawan)
            .ThenInclude(k => k.Department)
        .FirstOrDefaultAsync(x => x.Id == id);

            if (header == null) return null;

            // Ambil pengaturan sistem dari tabel systemsettings (mengambil baris pertama ID = 1)
            var settings = await _context.SystemSettings.FirstOrDefaultAsync() ?? new SystemSetting();

            return new PeminjamanAsetFormViewModel
            {
                Id = header.Id,
                KodePeminjaman = header.KodePeminjaman,

                // Dinamis dari SystemSettings (tbl systemsettings)
                PerusahaanNama = settings.SiteName ?? "PT. CONTOH MAJU JAYA",
                PerusahaanSlogan = settings.SiteDescription ?? "Inventory Multi-Gudang Modern",
                PerusahaanAlamat = settings.CompanyAddress ?? "Jakarta",
                PerusahaanTelp = settings.CompanyPhone ?? "-",
                PerusahaanEmail = settings.CompanyEmail ?? "-",

                // Pihak Penyerah dinamis dari User yang login saat mencetak
                AdminPenyerah = !string.IsNullOrEmpty(currentUserName) ? currentUserName : "Staff Inventory",

                // Pihak Penerima dinamis dari Database Relasi Karyawan
                KaryawanNik = header.Karyawan?.Nik ?? "-",
                KaryawanNama = header.Karyawan?.NamaLengkap ?? "-",
                KaryawanDivisi = header.Karyawan?.Department?.Name ?? "Umum / Tamu",
                KaryawanEmail = header.Karyawan?.Email ?? "-",
                KaryawanTelp = header.Karyawan?.NoTelepon ?? "-",

                GudangId = header.GudangId,                
                _RawTanggalPinjam = header.TanggalPinjam,
                _RawTanggalJatuhTempo = header.TanggalJatuhTempo,
                _RawTanggalPengembalian = header.TanggalPengembalian,
                TujuanPeminjaman = header.TujuanPeminjaman,
                Status = (StatusPeminjaman)header.Status,

                Details = header.Details.Select(d => new PeminjamanAsetDetailViewModel
                {
                    Id = d.Id,
                    ProdukItemId = d.ProdukItemId,
                    NamaBarang = d.NamaBarang,
                    SerialNumber = d.SerialNumber,
                    Kuantitas = d.Kuantitas,
                    Sku = d.ProdukItem != null && d.ProdukItem.Produk != null ? d.ProdukItem.Produk.Sku : (d.Sku ?? "-"),
                    Merek = d.ProdukItem != null && d.ProdukItem.Produk != null ? d.ProdukItem.Produk.Merek : "-",
                    LokasiAwal = d.ProdukItem != null && d.ProdukItem.Gudang != null ? d.ProdukItem.Gudang.NamaGudang : (d.LokasiAwal ?? "Gudang Utama"),
                    LokasiRak = d.ProdukItem != null && d.ProdukItem.Rak != null ? d.ProdukItem.Rak.NamaRak : "-",
                    KondisiSerah = d.KondisiSerah,
                    KondisiKembali = d.KondisiKembali
                }).ToList()
            };
        }

        // Insert data peminjaman & update status produk_item jadi 'Dipinjam' via EF Core Change Tracking
        public async Task<bool> InsertAsync(PeminjamanAsetFormViewModel model)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // 1. Ambil tahun & bulan saat ini (contoh: 202608)
                var tahunBulan = DateTime.Now.ToString("yyyyMM");

                // 2. Cari nomor urut terakhir di database berdasarkan bulan yang sama
                var lastPeminjaman = _context.PeminjamanAsets
                    .Where(p => p.KodePeminjaman.StartsWith($"LN-{tahunBulan}-"))
                    .OrderByDescending(p => p.Id)
                    .FirstOrDefault();

                int nextNumber = 1;
                if (lastPeminjaman != null)
                {
                    // Ambil 4 digit terakhir dari kode sebelumnya, lalu ubah ke integer dan tambahkan 1
                    string lastSequenceStr = lastPeminjaman.KodePeminjaman.Substring(lastPeminjaman.KodePeminjaman.Length - 4);
                    if (int.TryParse(lastSequenceStr, out int lastSeq))
                    {
                        nextNumber = lastSeq + 1;
                    }
                }

                // 3. Format kode baru dengan nomor urut 4 digit (0001, 0002, dst)
                model.KodePeminjaman = $"LN-{tahunBulan}-{nextNumber:D4}";

                // 4. Konversi string tanggal dari Form (dd/MM/yyyy) ke DateTime dengan benar
                DateTime parsedTanggalPinjam = DateTime.ParseExact(model.TanggalPinjam, "dd/MM/yyyy", null);
                DateTime parsedTanggalJatuhTempo = DateTime.ParseExact(model.TanggalJatuhTempo, "dd/MM/yyyy", null);

                // 5. Mapping viewmodel ke model database & Simpan
                var entity = new PeminjamanAset
                {
                    KodePeminjaman = model.KodePeminjaman,
                    KaryawanId = model.KaryawanId,
                    GudangId = model.GudangId,                    
                    TanggalPinjam = parsedTanggalPinjam,
                    TanggalJatuhTempo = parsedTanggalJatuhTempo,
                    JumlahHariPinjam = (parsedTanggalJatuhTempo.Date - parsedTanggalPinjam.Date).Days,
                    JumlahHariTelatPinjam = 0, // Karena baru dipinjam, hari terlambat otomatis 0
                    TujuanPeminjaman = model.TujuanPeminjaman,
                    Status = StatusPeminjaman.Dipinjam,
                    Details = model.Details.Select(d => new PeminjamanAsetDetail
                    {
                        ProdukItemId = d.ProdukItemId,
                        NamaBarang = d.NamaBarang,
                        SerialNumber = d.SerialNumber,
                        LokasiAwal = d.LokasiAwal,
                        Kuantitas = d.Kuantitas,
                        KondisiSerah = d.KondisiSerah
                    }).ToList()
                };

                _context.PeminjamanAsets.Add(entity);

                // Sinkronisasi status item ke tabel produk_item menjadi 'Dipinjam'
                foreach (var detailModel in model.Details)
                {
                    var produkItem = await _context.ProdukItems.FindAsync(detailModel.ProdukItemId);
                    if (produkItem != null)
                    {
                        produkItem.Status = "Dipinjam";
                        _context.ProdukItems.Update(produkItem);
                    }
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                return false;
            }
        }

        // Proses pengembalian aset dan sinkronisasi status item kembali ke 'In Stock'
        public async Task<bool> ProcessReturnAsync(int id, DateTime tglKembali, List<PeminjamanAsetDetailViewModel> updatedDetails)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var header = await _context.PeminjamanAsets
                    .Include(x => x.Details)
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (header == null) return false;

                // Cek apakah TanggalPengembalian <= TanggalJatuhTempo
                if (tglKembali.Date <= header.TanggalJatuhTempo.Date)
                {
                    header.Status = StatusPeminjaman.Kembali; // Badge Status warna hijau
                }
                else
                {
                    header.Status = StatusPeminjaman.Terlambat; // Badge Status warna danger
                }

                header.TanggalPengembalian = tglKembali;
                foreach (var detailModel in updatedDetails)
                {
                    var detailEntity = header.Details.FirstOrDefault(d => d.Id == detailModel.Id);
                    if (detailEntity != null)
                    {
                        detailEntity.KondisiKembali = detailModel.KondisiKembali;

                        var produkItem = await _context.ProdukItems.FindAsync(detailEntity.ProdukItemId);
                        if (produkItem != null)
                        {
                            produkItem.Status = "In Stock";
                            _context.ProdukItems.Update(produkItem);
                        }
                    }
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                return false;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.PeminjamanAsets.Include(x => x.Details).FirstOrDefaultAsync(x => x.Id == id);
            if (entity == null) return false;

            _context.PeminjamanAsets.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        // Update otomatis status menjadi 'Terlambat' jika melewati TanggalJatuhTempo
        public async Task UpdateStatusOtomatisTerlambatAsync()
        {
            var today = DateTime.Today;
            var listTerlambat = await _context.PeminjamanAsets
                .Where(x => x.Status == StatusPeminjaman.Dipinjam && x.TanggalJatuhTempo < today)
                .ToListAsync();

            if (listTerlambat.Any())
            {
                foreach (var item in listTerlambat)
                {
                    item.Status = StatusPeminjaman.Terlambat; // Menggunakan Enum StatusPeminjaman.Terlambat
                }
                await _context.SaveChangesAsync();
            }
        }

        // Mengambil data gudang dibatasi 5 data awal atau berdasarkan pencarian AJAX
        public async Task<IEnumerable<GudangDropdownViewModel>> GetGudangListAsync(string search = null)
        {
            var query = _context.Gudangs.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(g => g.NamaGudang.Contains(search));
            }

            return await query
                .Take(5) // Batasi hanya 5 data yang dikirim ke dropdown
                .Select(g => new GudangDropdownViewModel
                {
                    Id = g.Id,
                    NamaGudang = g.NamaGudang
                })
                .ToListAsync();
        }

        public async Task<List<AsetAvailableDto>> GetAvailableAssetsAsync(string? search, string? type)
        {
            var query = from item in _context.ProdukItems
                        join prod in _context.Produks on item.ProdukId equals prod.Id
                        join gdg in _context.Gudangs on item.GudangId equals gdg.Id into gdgGroup
                        from g in gdgGroup.DefaultIfEmpty()
                        join rak in _context.Raks on item.RakId equals rak.Id into rakGroup
                        from r in rakGroup.DefaultIfEmpty()
                        where item.Status == "In Stock"
                        select new
                        {
                            item,
                            prod,
                            GudangId = item.GudangId,
                            GudangNama = g != null ? g.NamaGudang : null,
                            RakNama = r != null ? r.NamaRak : null
                        };

            // Filter berdasarkan Tipe Tab (Semua, Asset, Inventory)
            if (!string.IsNullOrWhiteSpace(type) && type.ToLower() != "semua")
            {
                if (type.ToLower() == "asset")
                {
                    query = query.Where(x => x.prod.TipeBarang != null && x.prod.TipeBarang.ToLower().Contains("asset"));
                }
                else if (type.ToLower() == "inventory")
                {
                    query = query.Where(x => x.prod.TipeBarang != null && x.prod.TipeBarang.ToLower().Contains("inventory"));
                }
            }

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim().ToLower();
                query = query.Where(x => x.prod.NamaProduk.ToLower().Contains(search)
                                    || x.item.SerialNumber.ToLower().Contains(search)
                                    || x.prod.Sku.ToLower().Contains(search));
            }

            return await query.Select(x => new AsetAvailableDto
            {
                Id = x.item.Id,
                GudangId = x.GudangId,
                NamaBarang = x.prod.NamaProduk,
                Sku = x.prod.Sku,
                SerialNumber = x.item.SerialNumber,
                LokasiAwal = x.GudangNama ?? "Gudang Utama JKT",
                Rak = x.RakNama ?? "-",
                Tipe = x.prod.TipeBarang ?? "Asset",
                Kuantitas = 1,
                KondisiSerah = "Good"
            }).Take(30).ToListAsync();
        }

        public async Task<AsetAvailableDto?> GetAssetBySerialAsync(string serialNumber)
        {
            if (string.IsNullOrWhiteSpace(serialNumber))
                return null;

            serialNumber = serialNumber.Trim().ToLower();

            var data = await (from item in _context.ProdukItems
                              join prod in _context.Produks on item.ProdukId equals prod.Id
                              join gdg in _context.Gudangs on item.GudangId equals gdg.Id into gdgGroup
                              from g in gdgGroup.DefaultIfEmpty()
                              join rak in _context.Raks on item.RakId equals rak.Id into rakGroup
                              from r in rakGroup.DefaultIfEmpty()
                              where item.SerialNumber.ToLower() == serialNumber
                                 && item.Status == "In Stock"
                              select new AsetAvailableDto
                              {
                                  Id = item.Id,
                                  GudangId = item.GudangId,
                                  NamaBarang = prod.NamaProduk,
                                  Sku = prod.Sku,
                                  SerialNumber = item.SerialNumber,
                                  LokasiAwal = g.NamaGudang ?? "Gudang Utama JKT",
                                  Rak = r.NamaRak ?? "-",
                                  Tipe = prod.TipeBarang ?? "Asset",
                                  Kuantitas = 1,
                                  KondisiSerah = "Good"
                              }).FirstOrDefaultAsync();

            return data;
        }

        public async Task<List<PenerimaSelectOptionDto>> GetPenerimaOptionsAsync(string? search)
        {
            search = search?.Trim().ToLower();
            var queryKaryawan = _context.Karyawans.AsQueryable();
            if (!string.IsNullOrEmpty(search))
            {
                queryKaryawan = queryKaryawan.Where(k => k.NamaLengkap.ToLower().Contains(search) || k.Nik.ToLower().Contains(search));
            }
            return await queryKaryawan
                .OrderBy(k => k.NamaLengkap)
                .Take(5)
                .Select(k => new PenerimaSelectOptionDto
                {
                    Id = k.Id,
                    Text = string.IsNullOrEmpty(k.Nik) ? k.NamaLengkap : $"{k.NamaLengkap} ({k.Nik})"
                })
                .ToListAsync();
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