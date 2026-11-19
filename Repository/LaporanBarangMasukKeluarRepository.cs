using System.Globalization;
using InventoryMultiGudang.Models;
using InventoryMultiGudang.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace InventoryMultiGudang.Repository
{
    public class LaporanBarangMasukKeluarRepository : ILaporanBarangMasukKeluarRepository
    {
        private readonly ApplicationDbContext _context;

        public LaporanBarangMasukKeluarRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // Mengambil data utama laporan & metrik kartu ringkasan secara dinamis dari database
        public async Task<LaporanBarangMasukKeluarIndexViewModel> GetIndexDataAsync(string tanggalMulai, string tanggalSelesai, int? gudangId, string tipeTransaksi, int? produkId)
        {
            var viewModel = new LaporanBarangMasukKeluarIndexViewModel();
            var query = _context.TransaksiStoks
                .Include(t => t.Gudang)
                .Include(t => t.Supplier)
                .Include(t => t.Details)
                    .ThenInclude(d => d.Produk)
                .Include(t => t.Details)
                    .ThenInclude(d => d.Rak)
                .AsQueryable();
            var culture = new CultureInfo("id-ID");
            if (!string.IsNullOrEmpty(tanggalMulai) && DateTime.TryParseExact(tanggalMulai, "dd-MM-yyyy", culture, DateTimeStyles.None, out var dtMulai))
            {
                query = query.Where(t => t.TanggalPosting >= dtMulai);
            }
            if (!string.IsNullOrEmpty(tanggalSelesai) && DateTime.TryParseExact(tanggalSelesai, "dd-MM-yyyy", culture, DateTimeStyles.None, out var dtSelesai))
            {
                var dtSelesaiEnd = dtSelesai.Date.AddDays(1).AddTicks(-1);
                query = query.Where(t => t.TanggalPosting <= dtSelesaiEnd);
            }
            if (gudangId.HasValue && gudangId.Value > 0)
            {
                query = query.Where(t => t.GudangId == gudangId.Value);
            }
            if (!string.IsNullOrEmpty(tipeTransaksi))
            {
                query = query.Where(t => t.TipeTransaksi == tipeTransaksi);
            }
            if (produkId.HasValue && produkId.Value > 0)
            {
                query = query.Where(t => t.Details.Any(d => d.ProdukId == produkId.Value));
            }
            var rawData = await query.OrderByDescending(t => t.TanggalPosting).ToListAsync();
            viewModel.ListTransaksi = rawData.Select(t => new LaporanBarangItemViewModel
            {
                Id = t.Id,
                TanggalWaktu = t.TanggalPosting.ToString("dd MMM yyyy, HH.mm"),
                NoReferensi = t.NoReferensi ?? t.NomorReferensi,
                NamaGudang = t.Gudang != null ? t.Gudang.NamaGudang : "-",
                TipeTransaksi = t.TipeTransaksi, // Menyimpan nilai asli dari database ("Stock In" / "Stock Out")
                SupplierMitra = t.Supplier != null ? t.Supplier.NamaSupplier : "—",
                JumlahSku = t.Details != null ? t.Details.Select(d => d.ProdukId).Distinct().Count() : 0,
                TotalUnit = t.Details != null ? t.Details.Sum(d => d.Jumlah) : 0, // Menggunakan kolom 'Jumlah'
                Pencatat = t.Pembuat ?? "System",
                Catatan = t.Catatan,
                Details = t.Details != null ? t.Details.Select(d => new LaporanBarangDetailItemViewModel
                {
                    Sku = d.Produk != null ? d.Produk.Sku : "-",
                    NamaProduk = d.Produk != null ? d.Produk.NamaProduk : "-",
                    Kuantitas = d.Jumlah,
                    LokasiRak = d.Rak != null ? $"{d.Rak.Id} - {d.Rak.NamaRak}" : "-",
                    //Cek apakah produk melacak SN, lalu tampilkan nomor serinya atau keterangan statusnya
                    SerialNumbers = !string.IsNullOrEmpty(d.SerialNumbers) ? d.SerialNumbers : (d.Produk != null && d.Produk.PelacakanBarang == "Pelacakan Serial Number (SN)" ? "Tanpa Serial Number (Bulk)" : "Non-Serial Number")
                }).ToList() : new List<LaporanBarangDetailItemViewModel>()
            }).ToList();
            // Kalkulasi metrik berdasarkan pengecekan kata "in" atau "stock in"
            viewModel.TotalUnitMasuk = rawData
                .Where(t => t.TipeTransaksi.ToLower().Contains("in"))
                .SelectMany(t => t.Details)
                .Sum(d => d.Jumlah);
            viewModel.TotalUnitKeluar = rawData
                .Where(t => t.TipeTransaksi.ToLower().Contains("out"))
                .SelectMany(t => t.Details)
                .Sum(d => d.Jumlah);
            viewModel.MutasiBersih = viewModel.TotalUnitMasuk - viewModel.TotalUnitKeluar;
            return viewModel;
        }

        // Mengambil dropdown Gudang secara dinamis dengan batasan 5 data awal + Live Search AJAX
        public async Task<List<DropdownItemViewModel>> GetGudangDropdownAsync(string searchTerm)
        {
            var query = _context.Gudangs.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(g => g.NamaGudang.Contains(searchTerm) || g.KodeGudang.Contains(searchTerm));
            }

            return await query
                .Take(5) // Batasi 5 data awal sesuai ketentuan
                .Select(g => new DropdownItemViewModel
                {
                    Id = g.Id,
                    Text = $"{g.KodeGudang} - {g.NamaGudang}"
                })
                .ToListAsync();
        }

        // Mengambil dropdown Produk secara dinamis dengan batasan 5 data awal + Live Search AJAX        
        public async Task<List<DropdownItemViewModel>> GetProdukDropdownAsync(string searchTerm, int? gudangId)
        {
            // Ambil produk berdasarkan relasi transaksi stok pada gudang tertentu
            var query = _context.Produks.AsQueryable();

            if (gudangId.HasValue && gudangId.Value > 0)
            {
                // Filter produk yang hanya ada di gudang tersebut melalui TransaksiStok & Details
                var produkIdsInGudang = await _context.TransaksiStokDetails
                    .Where(d => d.TransaksiStok.GudangId == gudangId.Value)
                    .Select(d => d.ProdukId)
                    .Distinct()
                    .ToListAsync();

                query = query.Where(p => produkIdsInGudang.Contains(p.Id));
            }

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(p => p.NamaProduk.Contains(searchTerm) || p.Sku.Contains(searchTerm));
            }

            return await query
                .Take(5) // Batasi 5 data awal
                .Select(p => new DropdownItemViewModel
                {
                    Id = p.Id,
                    Text = $"{p.Sku} - {p.NamaProduk}"
                })
                .ToListAsync();
        }

        public async Task<LaporanBarangMasukKeluarIndexViewModel> GetReportDataAsync(string tanggalMulai, string tanggalSelesai, int? gudangId, string tipeTransaksi, int? produkId)
        {
            var viewModel = new LaporanBarangMasukKeluarIndexViewModel();
            var query = _context.TransaksiStoks
                .Include(t => t.Gudang)
                .Include(t => t.Supplier)
                .Include(t => t.Details)
                    .ThenInclude(d => d.Produk)
                .Include(t => t.Details)
                    .ThenInclude(d => d.Rak)
                .AsQueryable();
            var culture = new CultureInfo("id-ID");
            if (!string.IsNullOrEmpty(tanggalMulai) && DateTime.TryParseExact(tanggalMulai, "dd-MM-yyyy", culture, DateTimeStyles.None, out var dtMulai))
            {
                query = query.Where(t => t.TanggalPosting >= dtMulai);
            }
            if (!string.IsNullOrEmpty(tanggalSelesai) && DateTime.TryParseExact(tanggalSelesai, "dd-MM-yyyy", culture, DateTimeStyles.None, out var dtSelesai))
            {
                var dtSelesaiEnd = dtSelesai.Date.AddDays(1).AddTicks(-1);
                query = query.Where(t => t.TanggalPosting <= dtSelesaiEnd);
            }
            // Filter Gudang
            if (gudangId.HasValue && gudangId.Value > 0)
            {
                query = query.Where(t => t.GudangId == gudangId.Value);
            }
            // Filter Tipe Transaksi (Abaikan jika kosong atau bernilai "Semua Tipe")
            if (!string.IsNullOrEmpty(tipeTransaksi) && tipeTransaksi != "Semua Tipe")
            {
                query = query.Where(t => t.TipeTransaksi.ToLower().Contains(tipeTransaksi.ToLower()));
            }

            // Filter Produk
            if (produkId.HasValue && produkId.Value > 0)
            {
                query = query.Where(t => t.Details.Any(d => d.ProdukId == produkId.Value));
            }

            var rawData = await query.OrderByDescending(t => t.TanggalPosting).ToListAsync();

            viewModel.ListTransaksi = rawData.Select(t => new LaporanBarangItemViewModel
            {
                Id = t.Id,
                TanggalWaktu = t.TanggalPosting.ToString("dd MMM yyyy, HH.mm"),
                NoReferensi = t.NoReferensi ?? t.NomorReferensi,
                NamaGudang = t.Gudang != null ? t.Gudang.NamaGudang : "-",
                TipeTransaksi = t.TipeTransaksi,
                SupplierMitra = t.Supplier != null ? t.Supplier.NamaSupplier : "—",
                JumlahSku = t.Details != null ? t.Details.Select(d => d.ProdukId).Distinct().Count() : 0,
                TotalUnit = t.Details != null ? t.Details.Sum(d => d.Jumlah) : 0,
                Pencatat = t.Pembuat ?? "System",
                Catatan = t.Catatan,
                Details = t.Details != null ? t.Details.Select(d => new LaporanBarangDetailItemViewModel
                {
                    Sku = d.Produk != null ? d.Produk.Sku : "-",
                    NamaProduk = d.Produk != null ? d.Produk.NamaProduk : "-",
                    Kuantitas = d.Jumlah,
                    LokasiRak = d.Rak != null ? $"{d.Rak.Id} - {d.Rak.NamaRak}" : "-",
                    SerialNumbers = !string.IsNullOrEmpty(d.SerialNumbers) ? d.SerialNumbers : "Non-Serial Number"
                }).ToList() : new List<LaporanBarangDetailItemViewModel>()
            }).ToList();

            return viewModel;
        }

    }
}
