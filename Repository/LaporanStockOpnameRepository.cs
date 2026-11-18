using Microsoft.EntityFrameworkCore;
using InventoryMultiGudang.Models;
using InventoryMultiGudang.ViewModels;

public class LaporanStockOpnameRepository : ILaporanStockOpnameRepository
{
    private readonly ApplicationDbContext _context;

    public LaporanStockOpnameRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<StockOpname>> GetFilteredOpnameListAsync(DateTime? startDate, DateTime? endDate, int? gudangId, int? status)
    {
       var query = _context.StockOpnames
        .Include(s => s.Gudang)
        .Include(s => s.Details)
            .ThenInclude(d => d.Produk) 
        .AsQueryable();
        // Jika filter Tanggal Mulai diisi, terapkan filter. Jika kosong, abaikan agar semua data tampil.
        if (startDate.HasValue && startDate.Value != default)
        {
            var startDateTime = startDate.Value.Date; // 00:00:00
            query = query.Where(s => s.TanggalMulai >= startDateTime);
        }
        // Jika filter Tanggal Selesai diisi, terapkan filter.
        if (endDate.HasValue && endDate.Value != default)
        {
            var endDateTime = endDate.Value.Date.AddDays(1).AddTicks(-1); // 23:59:59.999
            query = query.Where(s => s.TanggalMulai <= endDateTime);
        }
        // Filter Gudang jika dipilih dan bukan nilai kosong/0
        if (gudangId.HasValue && gudangId.Value > 0)
        {
            query = query.Where(s => s.GudangId == gudangId.Value);
        }
        // Filter Status jika dipilih dan valid
        if (status.HasValue && status.Value >= 0)
        {
            var statusEnum = (StatusOpname)status.Value;
            query = query.Where(s => s.Status == statusEnum);
        }
        return await query.ToListAsync();
    }
    public async Task<StockOpnameSummaryViewModel> GetSummaryReportDataAsync(DateTime? startDate, DateTime? endDate, int? gudangId, int? status)
    {
        var dataList = await GetFilteredOpnameListAsync(startDate, endDate, gudangId, status);
        var opnameSelesai = dataList.Count(s => s.Status == StatusOpname.Selesai);
        var allDetails = dataList.SelectMany(s => s.Details ?? new List<StockOpnameDetail>()).ToList();
        var itemDiperiksa = allDetails.Select(d => d.Sku).Distinct().Count();
        var totalSelisihLebih = allDetails.Where(d => d.Selisih > 0).Sum(d => (decimal)d.Selisih);
        var totalSelisihKurang = allDetails.Where(d => d.Selisih < 0).Sum(d => (decimal)d.Selisih);
        return new StockOpnameSummaryViewModel
        {
            OpnameSelesai = opnameSelesai,
            ItemDiperiksa = itemDiperiksa,
            TotalSelisihLebih = totalSelisihLebih,
            TotalSelisihKurang = totalSelisihKurang
        };
    }

    public async Task<IEnumerable<StockOpnameListDto>> GetDaftarSesiOpnameAsync(DateTime? startDate, DateTime? endDate, int? gudangId, int? status)
    {
        var dataList = await GetFilteredOpnameListAsync(startDate, endDate, gudangId, status);

        var result = dataList.Select(s => new StockOpnameListDto
        {
            Id = s.Id,
            TanggalPosting = s.TanggalSelesai ?? s.TanggalMulai,
            NoReferensi = s.NoReferensi,
            Gudang = s.Gudang != null ? s.Gudang.NamaGudang : "Gudang Utama",
            JumlahSku = s.Details != null ? s.Details.Select(d => d.Sku).Distinct().Count() : 0,
            SelisihLebih = s.Details != null ? s.Details.Where(d => d.Selisih > 0).Sum(d => (decimal)d.Selisih) : 0,
            SelisihKurang = s.Details != null ? s.Details.Where(d => d.Selisih < 0).Sum(d => (decimal)d.Selisih) : 0,
            SelisihBersih = s.Details != null ? s.Details.Sum(d => (decimal)d.Selisih) : 0,
            Pencatat = s.Auditor ?? "Super Admin",
            Status = (int)s.Status
        }).ToList();

        return result;
    }

    public async Task<IEnumerable<RekapitulasiBarangDto>> GetRekapitulasiBarangAsync(DateTime? startDate, DateTime? endDate, int? gudangId, int? status)
    {
        var dataList = await GetFilteredOpnameListAsync(startDate, endDate, gudangId, status);

        // Ambil detail beserta relasi produknya (pastikan entity StockOpnameDetail memiliki navigation property ke Produk)
        // Jika menggunakan .Include(s => s.Details.ThenInclude(d => d.Produk)) di GetFilteredOpnameListAsync:
        var allDetails = dataList.SelectMany(s => s.Details ?? new List<StockOpnameDetail>()).ToList();

        var grouped = allDetails
            .GroupBy(d => new
            {
                // Ambil data dari tabel Produk yang berelasi (d.Produk)
                Sku = d.Produk != null ? d.Produk.Sku : (d.SerialNumber ?? "SKU-UNKNOWN"),
                NamaProduk = d.Produk != null ? d.Produk.NamaProduk : "Produk Item #" + d.ProdukId,
                Kategori = d.Produk != null ? d.Produk.Kategori : "Umum"
            })
            .Select(g => new RekapitulasiBarangDto
            {
                Sku = g.Key.Sku,
                NamaProduk = g.Key.NamaProduk,
                Kategori = g.Key.Kategori,
                TotalStokSistem = g.Sum(x => (decimal)x.StokSistem),
                TotalStokFisik = g.Sum(x => (decimal)x.StokFisik),
                SelisihAkumulasi = g.Sum(x => (decimal)(x.StokFisik - x.StokSistem)),
                FrekuensiSelisih = g.Count(x => (x.StokFisik - x.StokSistem) != 0)
            })
            .ToList();

        return grouped;
    }
}