using Microsoft.EntityFrameworkCore;
using InventoryMultiGudang.ViewModels;
using InventoryMultiGudang.Models;
namespace InventoryMultiGudang.Repository
{
    public class LaporanAssetRepository : ILaporanAssetRepository
    {
        private readonly ApplicationDbContext _context;

        public LaporanAssetRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<(IEnumerable<AssetSerialNumberViewModel> Items, int TotalCount)> GetDaftarSerialNumberAsync(string search, int page, int pageSize)
        {
            // Query dasar dengan relasi ke tabel Produk, Gudang, dan Rak menggunakan EF Core
            var query = from item in _context.ProdukItems
                        join prod in _context.Produks on item.ProdukId equals prod.Id
                        join gudang in _context.Gudangs on item.GudangId equals gudang.Id
                        join rak in _context.Raks on item.RakId equals rak.Id into rakGroup
                        from rak in rakGroup.DefaultIfEmpty()
                        select new AssetSerialNumberViewModel
                        {
                            Id = item.Id,
                            SerialNumber = item.SerialNumber,
                            NamaBarang = prod.NamaProduk,
                            Sku = prod.Sku,
                            Merek = prod.Merek,
                            KodeGudang = gudang.KodeGudang,
                            NamaGudang = gudang.NamaGudang,
                            NamaRak = rak != null ? rak.NamaRak : "-",
                            Status = item.Status
                        };

            // Filter Live Search (berdasarkan Serial Number atau Nama Barang/SKU)
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(x => x.SerialNumber.Contains(search) ||
                                         x.NamaBarang.Contains(search) ||
                                         x.Sku.Contains(search));
            }

            int totalCount = await query.CountAsync();

            // Pagination menggunakan Skip dan Take
            var items = await query
                .OrderBy(x => x.SerialNumber)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        public async Task<(IEnumerable<AssetPeminjamanBelumKembaliViewModel> Items, int TotalCount)> GetPeminjamanBelumKembaliAsync(string search, int page, int pageSize)
        {
            var today = DateTime.Today;

            // Filter peminjaman yang belum memiliki Tanggal Pengembalian (NULL)
            var query = from p in _context.PeminjamanAsets
                        join d in _context.PeminjamanAsetDetails on p.Id equals d.Id
                        join k in _context.Karyawans on p.KaryawanId equals k.Id into kGroup
                        from k in kGroup.DefaultIfEmpty()
                        where p.TanggalPengembalian == null
                        select new AssetPeminjamanBelumKembaliViewModel
                        {
                            Id = p.Id,
                            KodePinjam = p.KodePeminjaman,
                            NamaPeminjam = k != null ? k.NamaLengkap : "-",
                            NikPeminjam = k != null ? k.Nik : "-",
                            EmailPeminjam = k != null ? k.Email : "-",
                            NamaBarang = d.NamaBarang,
                            SerialNumber = d.SerialNumber,
                            TanggalPinjam = p.TanggalPinjam,
                            TanggalJatuhTempo = p.TanggalJatuhTempo,
                            IsOverdue = p.TanggalJatuhTempo < today,
                            StatusPeminjaman = p.TanggalJatuhTempo < today ? "Terlambat (Overdue)" : "Dipinjam"
                        };

            // Ajax Search Filter
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(x => x.KodePinjam.Contains(search) ||
                                         x.NamaPeminjam.Contains(search) ||
                                         x.NikPeminjam.Contains(search) ||
                                         x.NamaBarang.Contains(search) ||
                                         x.SerialNumber.Contains(search));
            }

            int totalCount = await query.CountAsync();

            var items = await query
                .OrderByDescending(x => x.TanggalJatuhTempo)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        public async Task<(IEnumerable<AssetPerCustomerViewModel> Items, int TotalCount)> GetAssetPerCustomerAsync(string search, int page, int pageSize)
        {
            // Berdasarkan tabel customers 
            var query = from c in _context.Customers
                        select new AssetPerCustomerViewModel
                        {
                            CustomerId = c.Id,
                            KodeCustomer = c.KodeCustomer,
                            NamaCustomer = c.NamaCustomer,
                            Email = c.Email,
                            NoTelepon = c.NoTelepon,
                            // Menghitung jumlah aset aktif yang ditugaskan ke customer (TipePenerima = Customer, misal ID 4 atau sesuai skema penugasan)
                            JumlahAsetAktif = _context.PenugasanAsets
                                .Where(pa => pa.TargetPenerimaId == c.Id && pa.TanggalPengembalian == null)
                                .Join(_context.PenugasanAsetDetails, pa => pa.Id, pad => pad.PenugasanAsetId, (pa, pad) => pad)
                                .Count()
                        };

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(x => x.KodeCustomer.Contains(search) ||
                                         x.NamaCustomer.Contains(search) ||
                                         x.Email.Contains(search));
            }

            int totalCount = await query.CountAsync();

            var items = await query
                .OrderBy(x => x.KodeCustomer)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        public async Task<IEnumerable<CustomerAssetDetailViewModel>> GetDetailAssetByCustomerAsync(int customerId)
        {
            var query = from pa in _context.PenugasanAsets
                        join pad in _context.PenugasanAsetDetails on pa.Id equals pad.PenugasanAsetId
                        where pa.TargetPenerimaId == customerId && pa.TanggalPengembalian == null
                        select new CustomerAssetDetailViewModel
                        {
                            NamaBarang = pad.NamaBarang,
                            SerialNumber = pad.SerialNumber,
                            Kondisi = pad.KondisiSerah,
                            LokasiAwal = pad.LokasiAwal,
                            TanggalDiserahkan = pa.TanggalSerahTerima.ToString("yyyy-MM-dd")
                        };

            var list = await query.ToListAsync();

            return list.Select(x => new CustomerAssetDetailViewModel
            {
                NamaBarang = x.NamaBarang,
                SerialNumber = x.SerialNumber,
                Kondisi = x.Kondisi,
                RakBin = "-",
                TanggalDiserahkan = x.TanggalDiserahkan
            });
        }

        public async Task<(IEnumerable<AssetPerDepartemenViewModel> Items, int TotalCount)> GetAssetPerDepartemenAsync(string search, int page, int pageSize)
        {
            // Berdasarkan tabel departments dan karyawans
            var query = from dept in _context.Departments
                        select new AssetPerDepartemenViewModel
                        {
                            DepartmentId = dept.Id,
                            KodeDept = dept.Code,
                            NamaDepartemen = dept.Name,
                            Deskripsi = dept.Description,
                            // Menghitung jumlah aset aktif yang dipegang oleh karyawan dalam departemen tersebut
                            JumlahAsetAktif = (from k in _context.Karyawans
                                               join pa in _context.PenugasanAsets on k.Id equals pa.TargetPenerimaId
                                               join pad in _context.PenugasanAsetDetails on pa.Id equals pad.PenugasanAsetId
                                               where k.DepartmentId == dept.Id && pa.TanggalPengembalian == null && pa.TipePenerima == TipePenerima.Karyawan
                                               select pad).Count()
                        };

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(x => x.KodeDept.Contains(search) ||
                                         x.NamaDepartemen.Contains(search) ||
                                         x.Deskripsi.Contains(search));
            }

            int totalCount = await query.CountAsync();

            var items = await query
                .OrderBy(x => x.KodeDept)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        public async Task<IEnumerable<DepartemenAssetDetailViewModel>> GetDetailAssetByDepartemenAsync(int departmentId)
        {
            var query = from k in _context.Karyawans
                        join pa in _context.PenugasanAsets on k.Id equals pa.TargetPenerimaId
                        join pad in _context.PenugasanAsetDetails on pa.Id equals pad.PenugasanAsetId
                        // Tambahkan join ke tabel ProdukItems dan Produks untuk mendapatkan SKU yang valid
                        join pi in _context.ProdukItems on pad.SerialNumber equals pi.SerialNumber into piGroup
                        from pi in piGroup.DefaultIfEmpty()
                        join prod in _context.Produks on pi.ProdukId equals prod.Id into prodGroup
                        from prod in prodGroup.DefaultIfEmpty()
                        where k.DepartmentId == departmentId && pa.TanggalPengembalian == null && pa.TipePenerima == TipePenerima.Karyawan
                        select new DepartemenAssetDetailViewModel
                        {
                            NamaBarang = pad.NamaBarang,
                            Sku = prod != null ? prod.Sku : (pad.Sku ?? "-"), // Ambil SKU dari Produk, fallback ke detail atau "-"
                            SerialNumber = pad.SerialNumber,
                            Kondisi = pad.KondisiSerah,
                            RakBin = pad.LokasiAwal, // atau sesuaikan dengan sumber rak
                            TanggalDiserahkan = pa.TanggalSerahTerima.ToString("yyyy-MM-dd")
                        };

            var list = await query.ToListAsync();

            return list.Select(x => new DepartemenAssetDetailViewModel
            {
                NamaBarang = x.NamaBarang,
                Sku = !string.IsNullOrEmpty(x.Sku) ? x.Sku : "-", // Mencegah nilai null/kosong
                SerialNumber = x.SerialNumber,
                Kondisi = x.Kondisi ?? "Good",
                RakBin = !string.IsNullOrEmpty(x.RakBin) ? x.RakBin : "C-01-01",
                TanggalDiserahkan = x.TanggalDiserahkan
            });
        }

        public async Task<(IEnumerable<AssetPerKaryawanViewModel> Items, int TotalCount)> GetAssetPerKaryawanAsync(string search, int page, int pageSize)
        {
            // Berdasarkan tabel karyawans
            var query = from k in _context.Karyawans
                        select new AssetPerKaryawanViewModel
                        {
                            KaryawanId = k.Id,
                            Nik = k.Nik,
                            NamaKaryawan = k.NamaLengkap,
                            Email = k.Email,
                            NoTelepon = k.NoTelepon,
                            // Menghitung jumlah aset aktif yang ditugaskan ke karyawan (TipePenerima = Karyawan)
                            JumlahAsetAktif = (from pa in _context.PenugasanAsets
                                               join pad in _context.PenugasanAsetDetails on pa.Id equals pad.PenugasanAsetId
                                               where pa.TargetPenerimaId == k.Id
                                                     && pa.TanggalPengembalian == null
                                                     && pa.TipePenerima == TipePenerima.Karyawan
                                               select pad).Count()
                        };

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(x => x.Nik.Contains(search) ||
                                         x.NamaKaryawan.Contains(search) ||
                                         x.Email.Contains(search));
            }

            int totalCount = await query.CountAsync();

            var items = await query
                .OrderBy(x => x.Nik)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        public async Task<IEnumerable<KaryawanAssetDetailViewModel>> GetDetailAssetByKaryawanAsync(int karyawanId)
        {
            var query = from pa in _context.PenugasanAsets
                        join pad in _context.PenugasanAsetDetails on pa.Id equals pad.PenugasanAsetId
                        where pa.TargetPenerimaId == karyawanId
                              && pa.TanggalPengembalian == null
                              && pa.TipePenerima == TipePenerima.Karyawan
                        select new KaryawanAssetDetailViewModel
                        {
                            NamaBarang = pad.NamaBarang,
                            SerialNumber = pad.SerialNumber,
                            Kondisi = pad.KondisiSerah,
                            TanggalDiserahkan = pa.TanggalSerahTerima.ToString("yyyy-MM-dd")
                        };

            var list = await query.ToListAsync();

            return list.Select(x => new KaryawanAssetDetailViewModel
            {
                NamaBarang = x.NamaBarang,
                Sku = x.Sku,
                SerialNumber = x.SerialNumber,
                Kondisi = x.Kondisi ?? "Good",
                RakBin = x.RakBin ?? "C-01-01",
                TanggalDiserahkan = x.TanggalDiserahkan
            });
        }

        //Untuk menampilkan data ringkasan di halaman Index.cshtml
        public async Task<LaporanAssetIndexViewModel> GetIndexSummaryAsync()
        {
            int totalAset = await _context.ProdukItems.CountAsync();
            int asetDipinjamkan = await _context.PeminjamanAsets.Where(p => p.TanggalPengembalian == null).CountAsync();
            int asetDitugaskan = await _context.PenugasanAsets.Where(p => p.TanggalPengembalian == null).CountAsync();
            int asetTersedia = await _context.ProdukItems.Where(x => x.Status == "In Stock" || x.Status == "Tersedia").CountAsync();
            int dalamReparasi = await _context.ReparasiAssets.Where(x => x.KondisiBarang == "Rusak").CountAsync();
            int asetHilang = await _context.StockOpnameDetails.Where(x => x.Kondisi == "Hilang (Lost)").CountAsync();
            int dihapuskan = await _context.ProdukItems.Where(x => x.Status != null && (x.Status.ToLower().Contains("disposed") ||x.Status.ToLower().Contains("dihapuskan") ||x.Status.ToLower().Contains("write-off") ||x.Status.ToLower().Contains("pensiun"))).CountAsync();
            int sold = await _context.ProdukItems.Where(x => x.Status == "Sold" || x.Status == "Terjual").CountAsync();
            double rasioUtilitas = totalAset > 0 ? ((double)(asetDipinjamkan + asetDitugaskan) / totalAset) * 100 : 0;
            return new LaporanAssetIndexViewModel
            {
                TotalAsetBerseri = totalAset,
                AsetTersedia = asetTersedia > 0 ? asetTersedia : totalAset - (asetDipinjamkan + asetDitugaskan),
                AsetDipinjamkan = asetDipinjamkan,
                AsetDitugaskan = asetDitugaskan,
                DalamReparasi = dalamReparasi,
                AsetHilang = asetHilang,
                Sold = sold,
                Dihapuskan = dihapuskan,
                RasioUtilitas = Math.Round(rasioUtilitas, 2)
            };
        }

        public async Task<AssetTimelineViewModel> GetAssetTimelineBySerialNumberAsync(string serialNumber)
        {
            // 1. Mengambil data produk item berdasarkan SerialNumber
            var item = await (from pi in _context.ProdukItems
                              join p in _context.Produks on pi.ProdukId equals p.Id
                              join g in _context.Gudangs on pi.GudangId equals g.Id
                              join r in _context.Raks on pi.RakId equals r.Id into rGroup
                              from r in rGroup.DefaultIfEmpty()
                              where pi.SerialNumber == serialNumber
                              select new AssetTimelineViewModel
                              {
                                  ProdukItemId = pi.Id,
                                  SerialNumber = pi.SerialNumber,
                                  NamaBarang = p.NamaProduk,
                                  Sku = p.Sku,
                                  Merek = p.Merek,
                                  StatusAktif = pi.Status ?? "Tersedia di Gudang",
                                  NamaGudang = g.NamaGudang,
                                  NamaRak = r != null ? r.NamaRak : "-",
                                  QrCodePath = pi.QrCodePath,
                                  HargaPerolehan = p.HargaPerolehan,
                                  TanggalPerolehan = pi.CreatedAt.ToString("yyyy-MM-dd")
                              }).FirstOrDefaultAsync();

            if (item == null) return null;

            // 2. Mengambil informasi supplier dari transaksi masuk (Stock In)
            var supplierInfo = await (from tsd in _context.TransaksiStokDetails
                                      join ts in _context.TransaksiStoks on tsd.TransaksiStokId equals ts.Id
                                      join s in _context.Suppliers on ts.SupplierId equals s.Id into sGroup
                                      from s in sGroup.DefaultIfEmpty()
                                      where tsd.SerialNumbers.Contains(serialNumber)
                                      select s != null ? s.NamaSupplier : "PT. Sumber Makmur ATK").FirstOrDefaultAsync();

            item.NamaSupplier = supplierInfo ?? "PT. Sumber Makmur ATK";

            // 3. Mengambil riwayat penugasan dengan JOIN dinamis ke Karyawan, Departemen, dan Customer
            var penugasanRaw = await (from pa in _context.PenugasanAsets
                                      join pad in _context.PenugasanAsetDetails on pa.Id equals pad.PenugasanAsetId
                                      // Left Join ke Karyawan
                                      join k in _context.Karyawans on pa.TargetPenerimaId equals k.Id into kGroup
                                      from k in kGroup.DefaultIfEmpty()
                                          // Left Join ke Departemen
                                      join dept in _context.Departments on pa.TargetPenerimaId equals dept.Id into deptGroup
                                      from dept in deptGroup.DefaultIfEmpty()
                                          // Left Join ke Customer
                                      join cust in _context.Customers on pa.TargetPenerimaId equals cust.Id into custGroup
                                      from cust in custGroup.DefaultIfEmpty()
                                      where pad.SerialNumber == serialNumber
                                      select new
                                      {
                                          pa.KodePenugasan,
                                          pa.TanggalSerahTerima,
                                          pad.LokasiAwal,
                                          pa.TipePenerima,
                                          NamaKaryawan = k != null ? k.NamaLengkap : null,
                                          NikKaryawan = k != null ? k.Nik : null,
                                          NamaDepartemen = dept != null ? dept.Name : null,
                                          KodeDepartemen = dept != null ? dept.Code : null,
                                          NamaCustomer = cust != null ? cust.NamaCustomer : null,
                                          KodeCustomer = cust != null ? cust.KodeCustomer : null,
                                          pa.Keterangan
                                      }).ToListAsync();

            // 4. Mapping data penerima secara dinamis ke ViewModel Timeline
            var timelineEvents = penugasanRaw.Select(pa =>
            {
                string penerimaInfo = "-";
                string tipeStr = pa.TipePenerima.ToString();

                // Deteksi tipe penerima dan susun detail informasinya
                if (tipeStr.Contains("Karyawan") || tipeStr == "1")
                {
                    penerimaInfo = $"Karyawan: **{pa.NamaKaryawan ?? "-"}** (NIK: {pa.NikKaryawan ?? "-"})";
                }
                else if (tipeStr.Contains("Departemen") || tipeStr == "2")
                {
                    penerimaInfo = $"Departemen: **{pa.NamaDepartemen ?? "-"}** ({pa.KodeDepartemen ?? "-"})";
                }
                else if (tipeStr.Contains("Customer") || tipeStr == "3")
                {
                    penerimaInfo = $"Customer: **{pa.NamaCustomer ?? "-"}** ({pa.KodeCustomer ?? "-"})";
                }
                else
                {
                    // Fallback otomatis berdasarkan data relasi yang terisi
                    if (!string.IsNullOrEmpty(pa.NamaKaryawan))
                        penerimaInfo = $"Karyawan: **{pa.NamaKaryawan}** ({pa.NikKaryawan})";
                    else if (!string.IsNullOrEmpty(pa.NamaDepartemen))
                        penerimaInfo = $"Departemen: **{pa.NamaDepartemen}**";
                    else if (!string.IsNullOrEmpty(pa.NamaCustomer))
                        penerimaInfo = $"Customer: **{pa.NamaCustomer}**";
                }

                return new AssetTimelineItemViewModel
                {
                    StatusEvent = "DITUGASKAN KEPADA PENERIMA",
                    Keterangan = $"Penugasan aset (Kode: {pa.KodePenugasan}) kepada {penerimaInfo}" + (!string.IsNullOrEmpty(pa.Keterangan) ? $" | Catatan: {pa.Keterangan}" : ""),
                    Tanggal = pa.TanggalSerahTerima.ToString("yyyy-MM-dd"),
                    Lokasi = pa.LokasiAwal
                };
            }).ToList();

            // Jika belum ada log penugasan, tampilkan status default gudang
            if (!timelineEvents.Any())
            {
                timelineEvents.Add(new AssetTimelineItemViewModel
                {
                    StatusEvent = "TERSEDIA DI GUDANG",
                    Keterangan = $"Unit tersimpan di {item.NamaGudang}",
                    Tanggal = item.TanggalPerolehan,
                    Lokasi = item.NamaRak
                });
            }

            item.TimelineEvents = timelineEvents;

            return item;
        }

    }
}