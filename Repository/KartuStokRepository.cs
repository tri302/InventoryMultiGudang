using InventoryMultiGudang.Models;
using InventoryMultiGudang.ViewModels;
using Microsoft.EntityFrameworkCore;
namespace InventoryMultiGudang.Repository
{
    public class KartuStokRepository : IKartuStokRepository
    {
        private readonly ApplicationDbContext _context;
        public KartuStokRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<KartuStokDetailViewModel?> GetDetailsAsync(
            KartuStokFilterViewModel filter,
            CancellationToken cancellationToken = default)
        {
            if (filter.GudangId <= 0 || filter.ProdukId <= 0)
                return null;
            if (filter.Page <= 0)
                filter.Page = 1;
            if (filter.PageSize <= 0 || filter.PageSize > 100)
                filter.PageSize = 10;
            return await BuildKartuStokAsync(
                filter.GudangId,
                filter.ProdukId,
                cancellationToken);
        }

        public async Task<KartuStokDetailViewModel?> BuildKartuStokAsync(
            int gudangId,
            int produkId,
            CancellationToken cancellationToken)
        {
            // Ambil Informasi Gudang & Produk menggunakan EF Core
            var gudang = await _context.Gudangs.AsNoTracking().FirstOrDefaultAsync(x => x.Id == gudangId, cancellationToken);
            var produk = await _context.Produks.AsNoTracking().FirstOrDefaultAsync(x => x.Id == produkId, cancellationToken);
            if (gudang == null || produk == null)
                return null;

            // 1. Ambil transaksi stok regular
            var transaksiReguler = await (
                from detail in _context.TransaksiStokDetails.AsNoTracking()
                join header in _context.TransaksiStoks.AsNoTracking() on detail.TransaksiStokId equals header.Id
                join rak in _context.Raks.AsNoTracking() on detail.RakId equals rak.Id into rakGroup
                from rak in rakGroup.DefaultIfEmpty()
                where header.GudangId == gudangId && detail.ProdukId == produkId
                select new TransactionProjection
                {
                    TanggalPosting = header.TanggalPosting,
                    TipeTransaksi = header.TipeTransaksi,
                    Jumlah = detail.Jumlah,
                    NomorReferensi = header.NomorReferensi,
                    Pembuat = header.Pembuat,
                    Catatan = header.Catatan,
                    KodeLokasi = rak != null ? rak.KodeLokasi : string.Empty
                }).ToListAsync(cancellationToken);

            // 2. Ambil transaksi dari Peminjaman Aset
            var transaksiPinjaman = await (
                from detail in _context.PeminjamanAsetDetails.AsNoTracking()
                join header in _context.PeminjamanAsets.AsNoTracking() on detail.PeminjamanAsetID equals header.Id
                join karyawan in _context.Karyawans.AsNoTracking() on header.KaryawanId equals karyawan.Id into karyawanGroup
                from karyawan in karyawanGroup.DefaultIfEmpty()
                where header.GudangId == gudangId && detail.ProdukItemId == produkId
                select new TransactionProjection
                {
                    TanggalPosting = header.TanggalPinjam,
                    TipeTransaksi = "PEMINJAMAN",
                    Jumlah = detail.Kuantitas,
                    NomorReferensi = header.KodePeminjaman,
                    Pembuat = header.KaryawanId != null && karyawan != null ? karyawan.NamaLengkap : "System",
                    Catatan = "Dipinjamkan ke: " + ((karyawan != null ? karyawan.NamaLengkap : "Karyawan")),
                    KodeLokasi = detail.LokasiAwal ?? string.Empty
                }).ToListAsync(cancellationToken);

            // 3. Ambil transaksi dari Penugasan Aset
            var penugasanDetailRaw = await _context.PenugasanAsetDetails.AsNoTracking().ToListAsync(cancellationToken);
            var penugasanHeaderRaw = await _context.PenugasanAsets.AsNoTracking().ToListAsync(cancellationToken);
            var produkItemRaw = await _context.ProdukItems.AsNoTracking().ToListAsync(cancellationToken);

            var transaksiPenugasan = (
                from detail in penugasanDetailRaw
                join header in penugasanHeaderRaw on detail.PenugasanAsetId equals header.Id
                join produkItem in produkItemRaw on detail.SerialNumber equals produkItem.SerialNumber into produkItemGrp
                from produkItem in produkItemGrp.DefaultIfEmpty()
                where produkItem != null && produkItem.GudangId == gudangId && produkItem.ProdukId == produkId
                select new TransactionProjection
                {
                    TanggalPosting = header.TanggalSerahTerima,
                    TipeTransaksi = "PENUGASAN",
                    Jumlah = -detail.Kuantitas,
                    NomorReferensi = header.KodePenugasan,
                    Pembuat = "System",
                    Catatan = header.Keterangan ?? ("Penugasan aset: " + detail.NamaBarang),
                    KodeLokasi = detail.LokasiAwal ?? string.Empty
                }).ToList();

            // 4. Ambil transaksi dari Transfer Order
            var transaksiTransfer = await (
                from detail in _context.TransferOrderDetails.AsNoTracking()
                join header in _context.TransferOrders.AsNoTracking() on detail.TransferOrderId equals header.Id
                where (header.GudangAsalId == gudangId || header.GudangTujuanId == gudangId)
                      && detail.ProdukId == produkId
                select new TransactionProjection
                {
                    TanggalPosting = header.TanggalDikirim ?? header.TanggalDibuat,
                    TipeTransaksi = header.GudangAsalId == gudangId ? "TRANSFER OUT" : "TRANSFER IN",
                    Jumlah = detail.KuantitasDiterima > 0 ? detail.KuantitasDiterima : detail.KuantitasPermintaan,
                    NomorReferensi = header.NomorDokumen,
                    Pembuat = header.Pembuat ?? "System",
                    Catatan = header.Catatan ?? (header.GudangAsalId == gudangId ? "Transfer Keluar Gudang" : "Transfer Masuk Gudang"),
                    KodeLokasi = string.Empty
                }).ToListAsync(cancellationToken);

            // 5. Ambil transaksi dari Stock Opname
            var transaksiStockOpname = await (
                from detail in _context.StockOpnameDetails.AsNoTracking()
                join header in _context.StockOpnames.AsNoTracking() on detail.StockOpnameId equals header.Id
                where header.GudangId == gudangId && detail.ProdukId == produkId && header.Status == StatusOpname.Selesai
                select new TransactionProjection
                {
                    TanggalPosting = header.TanggalSelesai ?? header.TanggalMulai,
                    TipeTransaksi = "STOCK OPNAME",
                    Jumlah = detail.Selisih,
                    NomorReferensi = header.NoReferensi,
                    Pembuat = header.Auditor ?? "System",
                    Catatan = header.Catatan ?? "Penyesuaian Stock Opname",
                    KodeLokasi = string.Empty
                }).ToListAsync(cancellationToken);

            // Gabungkan dan urutkan secara kronologis
            var allTransactions = transaksiReguler
                .Concat(transaksiPinjaman)
                .Concat(transaksiPenugasan)
                .Concat(transaksiTransfer)
                .Concat(transaksiStockOpname)
                .OrderBy(x => x.TanggalPosting)
                .ToList();

            // Hitung total net mutasi dari semua transaksi yang tercatat
            decimal totalNetTransactions = 0;
            foreach (var tx in allTransactions)
            {
                decimal qtyIn = 0;
                decimal qtyOut = 0;
                if (tx.TipeTransaksi == "STOK OPNAME")
                {
                    if (tx.Jumlah > 0) qtyIn = tx.Jumlah;
                    else qtyOut = Math.Abs(tx.Jumlah);
                }
                else if (IsStockIn(tx.TipeTransaksi))
                {
                    qtyIn = Math.Abs(tx.Jumlah);
                }
                else if (IsStockOut(tx.TipeTransaksi))
                {
                    qtyOut = Math.Abs(tx.Jumlah);
                }
                else
                {
                    if (tx.Jumlah >= 0) qtyIn = tx.Jumlah;
                    else qtyOut = Math.Abs(tx.Jumlah);
                }
                totalNetTransactions += (qtyIn - qtyOut);
            }

            // Tentukan Stok Awal (Saldo Awal) akurat berdasarkan stok produk saat ini dikurangi total net mutasi
            decimal initialStock = produk.Stok - totalNetTransactions;
            decimal runningBalance = initialStock;

            var evaluatedRows = new List<KartuStokRowViewModel>();
            foreach (var tx in allTransactions)
            {
                decimal qtyIn = 0;
                decimal qtyOut = 0;
                if (tx.TipeTransaksi == "STOK OPNAME")
                {
                    if (tx.Jumlah > 0) qtyIn = tx.Jumlah;
                    else qtyOut = Math.Abs(tx.Jumlah);
                }
                else if (IsStockIn(tx.TipeTransaksi))
                {
                    qtyIn = Math.Abs(tx.Jumlah);
                }
                else if (IsStockOut(tx.TipeTransaksi))
                {
                    qtyOut = Math.Abs(tx.Jumlah);
                }
                else
                {
                    if (tx.Jumlah >= 0) qtyIn = tx.Jumlah;
                    else qtyOut = Math.Abs(tx.Jumlah);
                }

                decimal stokSebelum = runningBalance;
                runningBalance = runningBalance + qtyIn - qtyOut;
                decimal stokSesudah = runningBalance;

                evaluatedRows.Add(new KartuStokRowViewModel
                {
                    TanggalPosting = tx.TanggalPosting,
                    TipeTransaksi = tx.TipeTransaksi,
                    NomorReferensi = tx.NomorReferensi,
                    Jumlah = (int)(qtyIn > 0 ? qtyIn : -qtyOut),
                    StokSebelum = (int)stokSebelum,
                    StokSesudah = (int)stokSesudah,
                    Pembuat = tx.Pembuat,
                    Catatan = tx.Catatan,
                    KodeLokasi = tx.KodeLokasi,
                    BadgeClass = GetBadgeClass(tx.TipeTransaksi),
                    BadgeIcon = GetBadgeIcon(tx.TipeTransaksi)
                });
            }

            decimal totalMasuk = evaluatedRows.Where(x => x.Jumlah > 0).Sum(x => (decimal)x.Jumlah);
            decimal totalKeluar = evaluatedRows.Where(x => x.Jumlah < 0).Sum(x => Math.Abs((decimal)x.Jumlah));
            decimal stokAwal = initialStock;
            decimal stokAkhir = runningBalance;

            return new KartuStokDetailViewModel
            {
                GudangId = gudang.Id,
                KodeGudang = gudang.KodeGudang,
                NamaGudang = gudang.NamaGudang,
                ProdukId = produk.Id,
                Sku = produk.Sku,
                NamaProduk = produk.NamaProduk,
                Satuan = produk.Satuan ?? "Pcs",
                Summary = new KartuStokSummaryViewModel
                {
                    StokAwal = (int)stokAwal,
                    TotalMasuk = (int)totalMasuk,
                    TotalKeluar = (int)totalKeluar,
                    StokAkhir = (int)stokAkhir
                },
                Rows = evaluatedRows
            };
        }

        public Task<KartuStokDetailViewModel?> GetReportAsync(
            int gudangId,
            int produkId,
            CancellationToken cancellationToken)
        {
            return BuildKartuStokAsync(gudangId, produkId, cancellationToken);
        }

        public async Task<KartuStokReportViewModel?> GetReportKartuStokAsync(int gudangId, int produkId, string? penyerah = null)
        {
            var detail = await BuildKartuStokAsync(gudangId, produkId, CancellationToken.None);
            if (detail == null) return null;

            return new KartuStokReportViewModel
            {
                GudangId = detail.GudangId,
                ProdukId = detail.ProdukId,
                KodeGudang = detail.KodeGudang,
                NamaGudang = detail.NamaGudang,
                Sku = detail.Sku,
                NamaProduk = detail.NamaProduk,
                Satuan = detail.Satuan,
                NomorReport = GenerateReportNumber(gudangId, produkId),
                TanggalCetak = DateTime.Now,
                StokAwal = detail.Summary.StokAwal,
                TotalMasuk = detail.Summary.TotalMasuk,
                TotalKeluar = detail.Summary.TotalKeluar,
                StokAkhir = detail.Summary.StokAkhir,
                Rows = detail.Rows.Select(x => new KartuStokReportRowViewModel
                {
                    TanggalPosting = x.TanggalPosting,
                    TipeTransaksi = x.TipeTransaksi,
                    Jumlah = x.Jumlah,
                    StokSebelum = x.StokSebelum,
                    StokSesudah = x.StokSesudah,
                    NomorReferensi = x.NomorReferensi,
                    KodeLokasi = x.KodeLokasi,
                    Pembuat = x.Pembuat,
                    Catatan = x.Catatan,
                    BadgeClass = x.BadgeClass
                }).ToList()
            };
        }

        private static bool IsStockOut(string? type)
        {
            var normalized = type?.Trim().ToUpperInvariant() ?? string.Empty;
            return normalized.Contains("STOCK OUT")
                || normalized.Contains("STOK OUT")
                || normalized.Contains("PEMUSNAHAN")
                || normalized.Contains("DISPOSAL")
                || normalized.Contains("PENUGASAN")
                || normalized.Contains("SALE")
                || normalized.Contains("PEMINJAMAN")
                || normalized.Contains("ISSUE")
                || normalized.Contains("TRANSFER OUT")
                || normalized.Contains("RETUR PEMBELIAN");
        }

        private static bool IsStockIn(string? type)
        {
            var normalized = type?.Trim().ToUpperInvariant() ?? string.Empty;
            return normalized.Contains("STOCK IN")
                || normalized.Contains("STOK IN")
                || normalized.Contains("RECEIVE")
                || normalized.Contains("PURCHASE");
        }

        private static string GetBadgeClass(string? type)
        {
            if (IsStockOut(type)) return "badge-stock-out";
            if (IsStockIn(type)) return "badge-stock-in";
            return "badge-proses";
        }

        private static string GetBadgeIcon(string? type)
        {
            if (IsStockOut(type)) return "bi-dash-circle-fill";
            if (IsStockIn(type)) return "bi-plus-circle-fill";
            return "bi-arrow-left-right";
        }

        async Task<IReadOnlyList<SelectOptionViewModel>> IKartuStokRepository.SearchGudangAsync(
            string? keyword, CancellationToken cancellationToken)
        {
            return await SearchGudangAsync(keyword ?? string.Empty, 5);
        }

        private static string GenerateReportNumber(int gudangId, int produkId)
        {
            return $"SR-{gudangId:D3}-{produkId:D6}-{DateTime.Now:yyyyMMddHHmmss}";
        }

        public async Task<List<SelectOptionViewModel>> GetInitialGudangAsync()
        {
            return await _context.Gudangs
                .AsNoTracking()
                .Where(x => x.IsActive == true && x.IsAudit == false)
                .OrderBy(x => x.NamaGudang)
                .Take(5)
                .Select(x => new SelectOptionViewModel
                {
                    Id = x.Id,
                    Text = x.KodeGudang + " - " + x.NamaGudang
                })
                .ToListAsync();
        }

        public async Task<List<SelectOptionViewModel>> SearchGudangAsync(string term, int limit = 5)
        {
            term = term?.Trim() ?? string.Empty;
            limit = Math.Clamp(limit, 1, 5);
            var query = _context.Gudangs
                .AsNoTracking()
                .Where(x => x.IsActive == true && x.IsAudit == false);
            if (!string.IsNullOrWhiteSpace(term))
            {
                query = query.Where(x => x.KodeGudang.Contains(term) || x.NamaGudang.Contains(term));
            }
            return await query
                .OrderBy(x => x.NamaGudang)
                .Take(limit)
                .Select(x => new SelectOptionViewModel
                {
                    Id = x.Id,
                    Text = x.KodeGudang + " - " + x.NamaGudang
                })
                .ToListAsync();
        }

        public async Task<List<SelectOptionViewModel>> GetInitialProdukAsync()
        {
            return await _context.Produks
                .AsNoTracking()
                .OrderBy(x => x.NamaProduk)
                .Take(5)
                .Select(x => new SelectOptionViewModel
                {
                    Id = x.Id,
                    Text = x.Sku + " - " + x.NamaProduk
                })
                .ToListAsync();
        }

        public async Task<List<SelectOptionViewModel>> SearchProdukAsync(string term, int limit = 5, int gudangId = 0)
        {
            term = term?.Trim() ?? string.Empty;
            limit = Math.Clamp(limit, 1, 5);
            var query = _context.Produks.AsNoTracking();
            if (gudangId > 0)
            {
                query = query.Where(p =>
                    p.GudangId == gudangId ||
                    _context.ProdukItems.Any(i => i.ProdukId == p.Id && i.GudangId == gudangId) ||
                    _context.TransaksiStokDetails.Any(d =>
                        d.ProdukId == p.Id &&
                        d.TransaksiStok != null &&
                        d.TransaksiStok.GudangId == gudangId));
            }
            if (!string.IsNullOrWhiteSpace(term))
            {
                query = query.Where(x => x.Sku.Contains(term) || x.NamaProduk.Contains(term));
            }
            return await query
                .OrderBy(x => x.NamaProduk)
                .Take(limit)
                .Select(x => new SelectOptionViewModel
                {
                    Id = x.Id,
                    Text = x.Sku + " - " + x.NamaProduk
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

        private sealed class TransactionProjection
        {
            public int Id { get; set; }
            public DateTime TanggalPosting { get; set; }
            public string TipeTransaksi { get; set; } = string.Empty;
            public int Jumlah { get; set; }
            public string NomorReferensi { get; set; } = string.Empty;
            public string Pembuat { get; set; } = string.Empty;
            public string? Catatan { get; set; }
            public string KodeLokasi { get; set; } = string.Empty;
            public string? Supplier { get; set; }
        }
    }
}