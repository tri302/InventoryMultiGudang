using InventoryMultiGudang.Models;
using InventoryMultiGudang.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace InventoryMultiGudang.Repository
{
    public class PenugasanAsetRepository : IPenugasanAsetRepository
    {
        private readonly ApplicationDbContext _db;

        public PenugasanAsetRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<PagedResultPenugasanAset<PenugasanAsetIndexViewModel>> GetPagedAsync(string? search, int? status, int pageIndex, int pageSize)
        {
            var query = _db.PenugasanAsets.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim().ToLower();
                query = query.Where(p => p.KodePenugasan.ToLower().Contains(search) ||
                                         (p.Keterangan != null && p.Keterangan.ToLower().Contains(search)));
            }

            if (status.HasValue && status.Value > 0)
            {
                query = query.Where(p => (int)p.Status == status.Value);
            }

            int totalItems = await query.CountAsync();

            var rawData = await query.OrderByDescending(p => p.Id)
                                     .Skip((pageIndex - 1) * pageSize)
                                     .Take(pageSize)
                                     .ToListAsync();

            var items = new List<PenugasanAsetIndexViewModel>();

            foreach (var item in rawData)
            {
                string targetNama = "-";
                if (item.TipePenerima == TipePenerima.Karyawan)
                    targetNama = await _db.Karyawans.Where(k => k.Id == item.TargetPenerimaId).Select(k => k.NamaLengkap).FirstOrDefaultAsync() ?? "-";
                else if (item.TipePenerima == TipePenerima.Departemen)
                    targetNama = await _db.Departments.Where(d => d.Id == item.TargetPenerimaId).Select(d => d.Name).FirstOrDefaultAsync() ?? "-";
                else if (item.TipePenerima == TipePenerima.Customer)
                    targetNama = await _db.Customers.Where(c => c.Id == item.TargetPenerimaId).Select(c => c.NamaCustomer).FirstOrDefaultAsync() ?? "-";

                // Deteksi jika merupakan hasil Transfer Aset untuk info siklus A -> B
                string cycleInfo = string.Empty;
                if (item.Status == StatusPenugasan.TransferAsetLangsung && !string.IsNullOrEmpty(item.Keterangan) && item.Keterangan.Contains("[Transfer]"))
                {
                    cycleInfo = item.Keterangan;
                }

                items.Add(new PenugasanAsetIndexViewModel
                {
                    Id = item.Id,
                    KodePenugasan = item.KodePenugasan,
                    NamaTarget = targetNama,
                    TipeTargetStr = item.TipePenerima.ToString(),
                    TanggalSerahTerima = item.TanggalSerahTerima,
                    TanggalPengembalian = item.TanggalPengembalian,
                    Status = item.Status,
                    Keterangan = cycleInfo
                });
            }

            return new PagedResultPenugasanAset<PenugasanAsetIndexViewModel>
            {
                Items = items,
                TotalItems = totalItems,
                PageIndex = pageIndex,
                PageSize = pageSize
            };
        }

        public async Task<PenugasanAset?> GetByIdAsync(int id)
        {
            return await _db.PenugasanAsets.Include(p => p.Details).FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<PenugasanAsetFormViewModel?> GetViewModelByIdAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity == null) return null;

            return new PenugasanAsetFormViewModel
            {
                Id = entity.Id,
                KodePenugasan = entity.KodePenugasan,
                TipePenerima = entity.TipePenerima,
                TargetPenerimaId = entity.TargetPenerimaId,
                TanggalSerahTerima = entity.TanggalSerahTerima,
                Keterangan = entity.Keterangan,
                Details = entity.Details.Select(d => new PenugasanAsetDetailViewModel
                {
                    Id = d.Id,
                    NamaBarang = d.NamaBarang,
                    SerialNumber = d.SerialNumber,
                    LokasiAwal = d.LokasiAwal,
                    Kuantitas = d.Kuantitas,
                    KondisiSerah = d.KondisiSerah,
                    KondisiKembali = d.KondisiKembali
                }).ToList()
            };
        }

        public async Task CreateAsync(PenugasanAset model)
        {
            // Status awal penugasan adalah MenungguPersetujuan (1)
            model.Status = StatusPenugasan.MenungguPersetujuan;
            _db.PenugasanAsets.Add(model);

            // Sinkronisasi status dan stok
            foreach (var detail in model.Details)
            {
                // A. Jika menggunakan Serial Number (SN)
                if (!string.IsNullOrEmpty(detail.SerialNumber) && detail.SerialNumber != "NON-SN" && detail.SerialNumber != "-")
                {
                    var item = await _db.ProdukItems.FirstOrDefaultAsync(p => p.SerialNumber == detail.SerialNumber);
                    if (item != null)
                    {
                        item.Status = "Assigned";
                    }
                }
                // B. Jika Non-SN / Bulk (Kurangi stok utama produk)
                else
                {
                    var produk = await _db.Produks.FirstOrDefaultAsync(p => p.NamaProduk == detail.NamaBarang);
                    if (produk != null)
                    {
                        // Validasi agar stok tidak minus
                        produk.Stok = Math.Max(0, produk.Stok - detail.Kuantitas);
                    }
                }
            }

            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(PenugasanAset model)
        {
            _db.PenugasanAsets.Update(model);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _db.PenugasanAsets.FindAsync(id);
            if (entity != null)
            {
                _db.PenugasanAsets.Remove(entity);
                await _db.SaveChangesAsync();
            }
        }

        public async Task<string> GenerateKodePenugasanAsync()
        {
            var count = await _db.PenugasanAsets.CountAsync() + 1;
            return $"AS-{DateTime.Now:yyyyMM}-{count:D4}";
        }

        public async Task ProcessPengembalianAsync(int id, DateTime tanggalKembali, List<PenugasanAsetDetailViewModel> updatedDetails)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                // Blokir pengembalian jika penugasan ini berisi barang Consumable (Habis Pakai)
                foreach (var detail in entity.Details)
                {
                    var produkCek = await _db.Produks.FirstOrDefaultAsync(p => p.NamaProduk == detail.NamaBarang);
                    if (produkCek != null && produkCek.TipeBarang != null && produkCek.TipeBarang.ToLower().Contains("consumable"))
                    {
                        throw new Exception($"Barang '{detail.NamaBarang}' adalah barang Consumable (Habis Pakai) dan tidak boleh dikembalikan ke inventaris.");
                    }
                }

                entity.TanggalPengembalian = tanggalKembali;
                entity.Status = StatusPenugasan.Dikembalikan;

                foreach (var detail in updatedDetails)
                {
                    var targetDetail = entity.Details.FirstOrDefault(d => d.Id == detail.Id);
                    if (targetDetail != null)
                    {
                        targetDetail.KondisiKembali = detail.KondisiKembali;

                        // ==========================================
                        // 1. KONDISI UNTUK ASET BER-SERIAL NUMBER (SN)
                        // ==========================================
                        bool isHasSerial = !string.IsNullOrEmpty(targetDetail.SerialNumber) &&
                                           targetDetail.SerialNumber != "NON-SN" &&
                                           targetDetail.SerialNumber != "-";

                        if (isHasSerial)
                        {
                            var prodItem = await _db.ProdukItems.FirstOrDefaultAsync(p => p.SerialNumber == targetDetail.SerialNumber);
                            if (prodItem != null)
                            {
                                if (detail.KondisiKembali == "Lost")
                                {
                                    prodItem.Status = "Lost";
                                }
                                else
                                {
                                    prodItem.Status = "In Stock";
                                }
                            }
                        }
                        // ==========================================
                        // 2. KONDISI UNTUK BARANG NON-SN (BULK / KUANTITAS)
                        // ==========================================
                        else
                        {
                            var produk = await _db.Produks.FirstOrDefaultAsync(p => p.NamaProduk == targetDetail.NamaBarang);
                            if (produk != null)
                            {
                                if (detail.KondisiKembali == "Good")
                                {
                                    // Karena saat CreateAsync stok sudah dipotong, sekarang aman dikembalikan
                                    produk.Stok += targetDetail.Kuantitas;
                                }
                                else if (detail.KondisiKembali == "Lost")
                                {
                                    // Barang hilang permanen, stok tidak dikembalikan (sudah dipotong saat create)
                                    // Jika ingin dicatat sebagai susut/write-off, biarkan tetap berkurang
                                }
                                // Untuk 'Broken', stok layak pakai tidak ditambah (dianggap rusak total)
                            }
                        }
                    }
                }

                await _db.SaveChangesAsync();
            }
        }

        public async Task<List<PenerimaSelectOptionDto>> GetPenerimaOptionsAsync(TipePenerima tipe, string? search)
        {
            search = search?.Trim().ToLower();

            switch (tipe)
            {
                case TipePenerima.Karyawan:
                    var queryKaryawan = _db.Karyawans.AsQueryable();
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

                case TipePenerima.Departemen:
                    var queryDept = _db.Departments.AsQueryable();
                    if (!string.IsNullOrEmpty(search))
                    {
                        queryDept = queryDept.Where(d => d.Name.ToLower().Contains(search) || d.Code.ToLower().Contains(search));
                    }
                    return await queryDept
                        .OrderBy(d => d.Name)
                        .Take(5)
                        .Select(d => new PenerimaSelectOptionDto
                        {
                            Id = d.Id,
                            Text = string.IsNullOrEmpty(d.Code) ? d.Name : $"{d.Name} ({d.Code})"
                        })
                        .ToListAsync();

                case TipePenerima.Customer:
                    var queryCust = _db.Customers.Where(c => c.IsActive);
                    if (!string.IsNullOrEmpty(search))
                    {
                        queryCust = queryCust.Where(c => c.NamaCustomer.ToLower().Contains(search) || c.KodeCustomer.ToLower().Contains(search));
                    }
                    return await queryCust
                        .OrderBy(c => c.NamaCustomer)
                        .Take(5)
                        .Select(c => new PenerimaSelectOptionDto
                        {
                            Id = c.Id,
                            Text = string.IsNullOrEmpty(c.KodeCustomer) ? c.NamaCustomer : $"{c.NamaCustomer} ({c.KodeCustomer})"
                        })
                        .ToListAsync();

                default:
                    return new List<PenerimaSelectOptionDto>();
            }
        }

        public async Task<List<AsetAvailableDto>> GetAvailableAssetsAsync(string? search, string? type)
        {
            var listResult = new List<AsetAvailableDto>();
            search = search?.Trim().ToLower();
            string filterType = type?.ToLower() ?? "semua";

            // 1. AMBIL DATA BER-SN (Dari ProdukItems)
            if (filterType == "semua" || filterType == "asset" || filterType == "inventory")
            {
                var queryItem = from item in _db.ProdukItems
                                join prod in _db.Produks on item.ProdukId equals prod.Id
                                join gdg in _db.Gudangs on item.GudangId equals gdg.Id into gdgGroup
                                from g in gdgGroup.DefaultIfEmpty()
                                join rak in _db.Raks on item.RakId equals rak.Id into rakGroup
                                from r in rakGroup.DefaultIfEmpty()
                                where item.Status == "In Stock" || item.Status == "InStock"
                                select new { item, prod, GudangNama = g.NamaGudang, RakNama = r.NamaRak };

                // Filter spesifik berdasarkan tab yang dipilih
                if (filterType == "asset")
                {
                    queryItem = queryItem.Where(x => x.prod.TipeBarang != null && x.prod.TipeBarang.ToLower().Contains("asset"));
                }
                else if (filterType == "inventory")
                {
                    queryItem = queryItem.Where(x => x.prod.TipeBarang != null && x.prod.TipeBarang.ToLower().Contains("inventory"));
                }

                if (!string.IsNullOrWhiteSpace(search))
                {
                    queryItem = queryItem.Where(x => x.prod.NamaProduk.ToLower().Contains(search)
                                                  || (x.item.SerialNumber != null && x.item.SerialNumber.ToLower().Contains(search))
                                                  || x.prod.Sku.ToLower().Contains(search));
                }

                var assetItems = await queryItem.Select(x => new AsetAvailableDto
                {
                    Id = x.item.Id, // ID ProdukItem
                    NamaBarang = x.prod.NamaProduk,
                    Sku = x.prod.Sku,
                    SerialNumber = x.item.SerialNumber ?? "-",
                    LokasiAwal = x.GudangNama ?? "Gudang Utama JKT",
                    Rak = x.RakNama ?? "-",
                    Tipe = x.prod.TipeBarang ?? "Asset",
                    Kuantitas = 1,
                    KondisiSerah = "Good"
                }).Take(30).ToListAsync();

                listResult.AddRange(assetItems);
            }

            // 2. AMBIL DATA NON-SN / BULK STOK (Dari Produk langsung untuk Consumable / Non-SN)            
            if (filterType == "semua" || filterType == "consumable" || (filterType == "inventory" && listResult.Count == 0))
            {
                var queryProd = from prod in _db.Produks
                                join gdg in _db.Gudangs on prod.GudangId equals gdg.Id into gdgGroup
                                from g in gdgGroup.DefaultIfEmpty()
                                join rak in _db.Raks on prod.RakId equals rak.Id into rakGroup
                                from r in rakGroup.DefaultIfEmpty()
                                    //Tambahkan pengecualian !TipeBarang.Contains("consumable")
                                where prod.Stok > 0
                                   && prod.TipeBarang != null
                                   && !prod.TipeBarang.ToLower().Contains("consumable")
                                   && prod.PelacakanBarang.Contains("Tanpa Serial Number")
                                select new { prod, GudangNama = g.NamaGudang, RakNama = r.NamaRak };

                if (filterType == "consumable")
                {
                    queryProd = queryProd.Where(x => x.prod.TipeBarang != null && x.prod.TipeBarang.ToLower().Contains("consumable"));
                }

                if (!string.IsNullOrWhiteSpace(search))
                {
                    queryProd = queryProd.Where(x => x.prod.NamaProduk.ToLower().Contains(search)
                                                  || x.prod.Sku.ToLower().Contains(search));
                }

                var inventoryItems = await queryProd.Select(x => new AsetAvailableDto
                {
                    Id = x.prod.Id, // ID Produk (Non-SN)
                    NamaBarang = x.prod.NamaProduk,
                    Sku = x.prod.Sku,
                    SerialNumber = "NON-SN",
                    LokasiAwal = x.GudangNama ?? "Gudang Utama JKT",
                    Rak = x.RakNama ?? "-",
                    Tipe = x.prod.TipeBarang ?? "Inventory",
                    Kuantitas = x.prod.Stok,
                    KondisiSerah = "Good"
                }).Take(30).ToListAsync();

                listResult.AddRange(inventoryItems);
            }

            return listResult.Take(30).ToList();
        }

        public async Task<AsetAvailableDto?> GetAssetBySerialAsync(string serialNumber)
        {
            if (string.IsNullOrWhiteSpace(serialNumber))
                return null;

            serialNumber = serialNumber.Trim().ToLower();

            var data = await (from item in _db.ProdukItems
                              join prod in _db.Produks on item.ProdukId equals prod.Id
                              join gdg in _db.Gudangs on item.GudangId equals gdg.Id into gdgGroup
                              from g in gdgGroup.DefaultIfEmpty()
                              join rak in _db.Raks on item.RakId equals rak.Id into rakGroup
                              from r in rakGroup.DefaultIfEmpty()
                              where item.SerialNumber.ToLower() == serialNumber
                                 && item.Status == "In Stock"
                              select new AsetAvailableDto
                              {
                                  Id = item.Id,
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

        // ==========================================
        // FITUR TRANSFER ASET LANGSUNG IMPLEMENTASI
        // ==========================================
        public async Task<List<AsetAssignedOptionDto>> GetAssignedAssetsAsync(string? search)
        {
            search = search?.Trim().ToLower();

            var query = from item in _db.ProdukItems
                        join prod in _db.Produks on item.ProdukId equals prod.Id
                        where item.Status == "Dalam Penugasan" || item.Status == "DalamPenugasan"
                        select new { item.Id, item.SerialNumber, prod.NamaProduk };

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(x => x.SerialNumber.ToLower().Contains(search) || x.NamaProduk.ToLower().Contains(search));
            }

            var activeAssignments = await query.Take(10).ToListAsync();
            var list = new List<AsetAssignedOptionDto>();

            foreach (var row in activeAssignments)
            {
                string holderName = "-";
                string sn = row.SerialNumber?.Trim().ToLower() ?? string.Empty;

                // Pencarian penugasan terakhir secara case-insensitive
                var lastPenugasan = await (from d in _db.PenugasanAsetDetails
                                           join p in _db.PenugasanAsets on d.PenugasanAsetId equals p.Id
                                           where d.SerialNumber != null && d.SerialNumber.Trim().ToLower() == sn
                                              && p.Status != StatusPenugasan.Ditolak
                                              && p.Status != StatusPenugasan.Dikembalikan
                                           orderby p.Id descending
                                           select p).FirstOrDefaultAsync();

                if (lastPenugasan != null)
                {
                    if (lastPenugasan.TipePenerima == TipePenerima.Karyawan)
                    {
                        holderName = await _db.Karyawans.Where(k => k.Id == lastPenugasan.TargetPenerimaId).Select(k => k.NamaLengkap).FirstOrDefaultAsync() ?? "-";
                    }
                    else if (lastPenugasan.TipePenerima == TipePenerima.Departemen)
                    {
                        holderName = await _db.Departments.Where(d => d.Id == lastPenugasan.TargetPenerimaId).Select(d => d.Name).FirstOrDefaultAsync() ?? "-";
                    }
                    else if (lastPenugasan.TipePenerima == TipePenerima.Customer)
                    {
                        holderName = await _db.Customers.Where(c => c.Id == lastPenugasan.TargetPenerimaId).Select(c => c.NamaCustomer).FirstOrDefaultAsync() ?? "-";
                    }
                }

                list.Add(new AsetAssignedOptionDto
                {
                    ProdukItemId = row.Id,
                    DisplayText = $"{row.SerialNumber} - {row.NamaProduk} (Saat ini di: {holderName})"
                });
            }

            return list;
        }

        public async Task<AsetAssignedDetailDto?> GetAssignedAssetDetailAsync(int produkItemId)
        {
            var item = await (from pi in _db.ProdukItems
                              join p in _db.Produks on pi.ProdukId equals p.Id
                              join g in _db.Gudangs on pi.GudangId equals g.Id into gGroup
                              from gdg in gGroup.DefaultIfEmpty()
                              where pi.Id == produkItemId
                              select new { pi, p, GudangNama = gdg.NamaGudang }).FirstOrDefaultAsync();

            if (item == null) return null;

            string sn = item.pi.SerialNumber?.Trim().ToLower() ?? string.Empty;

            // Cari penugasan aktif dengan pencocokan Serial Number yang aman (case-insensitive & trimmed)
            var lastPenugasan = await (from d in _db.PenugasanAsetDetails
                                       join p in _db.PenugasanAsets on d.PenugasanAsetId equals p.Id
                                       where d.SerialNumber != null && d.SerialNumber.Trim().ToLower() == sn
                                          && p.Status != StatusPenugasan.Ditolak
                                          && p.Status != StatusPenugasan.Dikembalikan
                                       orderby p.Id descending
                                       select p).FirstOrDefaultAsync();

            string holderName = "-";
            string holderTypeStr = "Unknown";
            int tipeVal = 1;

            if (lastPenugasan != null)
            {
                tipeVal = (int)lastPenugasan.TipePenerima;
                holderTypeStr = lastPenugasan.TipePenerima.ToString();

                if (lastPenugasan.TipePenerima == TipePenerima.Karyawan)
                    holderName = await _db.Karyawans.Where(k => k.Id == lastPenugasan.TargetPenerimaId).Select(k => k.NamaLengkap).FirstOrDefaultAsync() ?? "-";
                else if (lastPenugasan.TipePenerima == TipePenerima.Departemen)
                    holderName = await _db.Departments.Where(d => d.Id == lastPenugasan.TargetPenerimaId).Select(d => d.Name).FirstOrDefaultAsync() ?? "-";
                else if (lastPenugasan.TipePenerima == TipePenerima.Customer)
                    holderName = await _db.Customers.Where(c => c.Id == lastPenugasan.TargetPenerimaId).Select(c => c.NamaCustomer).FirstOrDefaultAsync() ?? "-";
            }

            return new AsetAssignedDetailDto
            {
                ProdukItemId = item.pi.Id,
                NamaBarang = item.p.NamaProduk,
                Sku = item.p.Sku,
                Brand = item.p.Merek ?? "—",
                GudangAsal = item.GudangNama ?? "Gudang Utama Jakarta",
                SerialNumber = item.pi.SerialNumber,
                PemegangSaatIni = holderName,
                TipePemegangStr = holderTypeStr,
                TipePemegang = tipeVal,
                StatusAsetStr = lastPenugasan != null ? (lastPenugasan.Status switch
                {
                    StatusPenugasan.MenungguPersetujuan => "Menunggu Persetujuan",
                    StatusPenugasan.DalamPenugasan => "Dalam Penugasan",
                    StatusPenugasan.Dikembalikan => "Dikembalikan",
                    StatusPenugasan.Ditolak => "Ditolak",
                    StatusPenugasan.TransferAsetLangsung => "Transfer Aset Langsung",
                    _ => lastPenugasan.Status.ToString()
                }) : "Dalam Penugasan"
            };
        }

        public async Task ProcessTransferAssetAsync(TransferAssetRequestDto dto)
        {
            var prodItem = await _db.ProdukItems.FirstOrDefaultAsync(p => p.Id == dto.ProdukItemId);
            if (prodItem == null) throw new Exception("Produk tidak ditemukan.");

            var prod = await _db.Produks.FirstOrDefaultAsync(p => p.Id == prodItem.ProdukId);

            // 1. Cari pemegang sebelumnya untuk siklus A -> B
            var prevPenugasan = await (from d in _db.PenugasanAsetDetails
                                       join p in _db.PenugasanAsets on d.PenugasanAsetId equals p.Id
                                       where d.SerialNumber == prodItem.SerialNumber && p.Status != StatusPenugasan.Dikembalikan
                                       orderby p.Id descending
                                       select p).FirstOrDefaultAsync();

            string prevHolder = "-";
            if (prevPenugasan != null)
            {
                if (prevPenugasan.TipePenerima == TipePenerima.Karyawan)
                    prevHolder = await _db.Karyawans.Where(k => k.Id == prevPenugasan.TargetPenerimaId).Select(k => k.NamaLengkap).FirstOrDefaultAsync() ?? "-";
                else if (prevPenugasan.TipePenerima == TipePenerima.Departemen)
                    prevHolder = await _db.Departments.Where(d => d.Id == prevPenugasan.TargetPenerimaId).Select(d => d.Name).FirstOrDefaultAsync() ?? "-";
                else if (prevPenugasan.TipePenerima == TipePenerima.Customer)
                    prevHolder = await _db.Customers.Where(c => c.Id == prevPenugasan.TargetPenerimaId).Select(c => c.NamaCustomer).FirstOrDefaultAsync() ?? "-";

                // Tandai penugasan lama sebagai Dikembalikan/Di-transfer
                prevPenugasan.Status = StatusPenugasan.Dikembalikan;
                prevPenugasan.TanggalPengembalian = dto.TanggalTransfer;
            }

            // 2. Ambil Nama Penerima Baru
            string newHolder = "-";
            var targetTipe = (TipePenerima)dto.TipePenerima;
            if (targetTipe == TipePenerima.Karyawan)
                newHolder = await _db.Karyawans.Where(k => k.Id == dto.TargetPenerimaId).Select(k => k.NamaLengkap).FirstOrDefaultAsync() ?? "-";
            else if (targetTipe == TipePenerima.Departemen)
                newHolder = await _db.Departments.Where(d => d.Id == dto.TargetPenerimaId).Select(d => d.Name).FirstOrDefaultAsync() ?? "-";
            else if (targetTipe == TipePenerima.Customer)
                newHolder = await _db.Customers.Where(c => c.Id == dto.TargetPenerimaId).Select(c => c.NamaCustomer).FirstOrDefaultAsync() ?? "-";

            // 3. Buat Penugasan Baru dengan Status TransferAsetLangsung (5)
            string kode = await GenerateKodePenugasanAsync();
            string cycleLog = $"[Transfer] Siklus: Dari '{prevHolder}' ke '{newHolder}'. {dto.Catatan}";

            var newAssignment = new PenugasanAset
            {
                KodePenugasan = kode,
                TipePenerima = targetTipe,
                TargetPenerimaId = dto.TargetPenerimaId,
                TanggalSerahTerima = dto.TanggalTransfer,
                Keterangan = cycleLog,
                Status = StatusPenugasan.TransferAsetLangsung, // Status = 5
                Details = new List<PenugasanAsetDetail>
                {
                    new PenugasanAsetDetail
                    {
                        NamaBarang = prod?.NamaProduk ?? "Asset",
                        SerialNumber = prodItem.SerialNumber,
                        LokasiAwal = "Transfer Langsung",
                        Kuantitas = 1,
                        KondisiSerah = "Good"
                    }
                }
            };

            _db.PenugasanAsets.Add(newAssignment);

            // 4. Update status produk_item menjadi Dalam Penugasan
            prodItem.Status = "Dalam Penugasan";

            await _db.SaveChangesAsync();
        }

        public async Task ApprovePenugasanAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null && entity.Status == StatusPenugasan.MenungguPersetujuan)
            {
                entity.Status = StatusPenugasan.DalamPenugasan;

                foreach (var detail in entity.Details)
                {
                    if (!string.IsNullOrEmpty(detail.SerialNumber))
                    {
                        var item = await _db.ProdukItems.FirstOrDefaultAsync(p => p.SerialNumber == detail.SerialNumber);
                        if (item != null)
                        {
                            item.Status = "Assigned"; // Update ke Assigned
                        }
                    }
                }

                await _db.SaveChangesAsync();
            }
        }

        public async Task RejectPenugasanAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null && entity.Status == StatusPenugasan.MenungguPersetujuan)
            {
                entity.Status = StatusPenugasan.Ditolak;

                foreach (var detail in entity.Details)
                {
                    if (!string.IsNullOrEmpty(detail.SerialNumber))
                    {
                        var item = await _db.ProdukItems.FirstOrDefaultAsync(p => p.SerialNumber == detail.SerialNumber);
                        if (item != null)
                        {
                            item.Status = "In Stock"; // Memastikan status kembali In Stock
                        }
                    }
                }

                await _db.SaveChangesAsync();
            }
        }

        public async Task<PenugasanAsetPrintViewModel?> GetPrintDepartmentViewModelAsync(int id, string? loggedInUser)
        {
            var data = await _db.PenugasanAsets
                .Include(p => p.Details)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (data == null) return null;

            // 1. Ambil Header dari tbl systemsettings
            var setting = await _db.SystemSettings.FirstOrDefaultAsync();

            // 2. Ambil Info Departemen (Penerima)
            string deptNama = "-";
            string deptKode = "-";
            string deptKet = "-";

            if (data.TipePenerima == TipePenerima.Departemen)
            {
                var dept = await _db.Departments.FirstOrDefaultAsync(d => d.Id == data.TargetPenerimaId);
                if (dept != null)
                {
                    deptNama = dept.Name ?? "-";
                    deptKode = dept.Code ?? "DPT-001";
                    deptKet = dept.Description ?? "Fasilitas operasional departemen perusahaan.";
                }
            }

            // Menggunakan helper untuk mendapatkan Nama Lengkap Karyawan
            string penyerah = await GetNamaPenyerahAsync(loggedInUser);
            var userKaryawan = await _db.Karyawans.FirstOrDefaultAsync(k => k.Email == loggedInUser || k.Nik == loggedInUser);
            if (userKaryawan != null)
            {
                penyerah = userKaryawan.NamaLengkap;
            }

            // 4. Mapping Details Aset
            var detailsList = new List<PenugasanAsetPrintDetailViewModel>();
            foreach (var d in data.Details)
            {
                string sku = "-";
                string merek = "-";
                string gudangNama = "Gudang Utama JKT";
                string lokasiRak = "-";

                if (!string.IsNullOrEmpty(d.SerialNumber))
                {
                    var prodItem = await (from pi in _db.ProdukItems
                                          join pr in _db.Produks on pi.ProdukId equals pr.Id
                                          join g in _db.Gudangs on pi.GudangId equals g.Id into gGrp
                                          from gdg in gGrp.DefaultIfEmpty()
                                          join r in _db.Raks on pi.RakId equals r.Id into rGrp
                                          from rak in rGrp.DefaultIfEmpty()
                                          where pi.SerialNumber == d.SerialNumber
                                          select new { pr.Sku, pr.Merek, Gudang = gdg.NamaGudang, Rak = rak.NamaRak }).FirstOrDefaultAsync();

                    if (prodItem != null)
                    {
                        sku = prodItem.Sku ?? "-";
                        merek = prodItem.Merek ?? "-";
                        gudangNama = prodItem.Gudang ?? "Gudang Utama JKT";
                        lokasiRak = prodItem.Rak ?? "-";
                    }
                }

                detailsList.Add(new PenugasanAsetPrintDetailViewModel
                {
                    Sku = sku,
                    Merek = merek,
                    NamaBarang = d.NamaBarang,
                    GudangNama = gudangNama,
                    LokasiRak = lokasiRak,
                    SerialNumber = d.SerialNumber ?? "-",
                    KondisiSerah = d.KondisiSerah ?? "Good"
                });
            }

            return new PenugasanAsetPrintViewModel
            {
                Id = data.Id,
                KodePenugasan = data.KodePenugasan,
                TanggalSerahTerima = data.TanggalSerahTerima,
                Status = data.Status,
                Keterangan = data.Keterangan,
                PerusahaanNama = setting?.SiteName ?? "PT. CONTOH MAJU JAYA",
                PerusahaanSlogan = setting?.SiteDescription ?? "Inventory Multi-Gudang Modern.",
                PerusahaanAlamat = setting?.CompanyAddress ?? "Komp. Perkantoran Indah Blok B-12, Jakarta Selatan",
                PerusahaanTelp = setting?.CompanyPhone ?? "(021) 555-9830",
                PerusahaanEmail = setting?.CompanyEmail ?? "support@ptcontohmajujaya.com",
                AdminPenyerah = penyerah,
                DepartemenNama = deptNama,
                DepartemenKode = deptKode,
                DepartemenKeterangan = deptKet,
                Details = detailsList
            };
        }

        public async Task<PenugasanAsetPrintKaryawanViewModel?> GetPrintKaryawanViewModelAsync(int id, string? loggedInUser)
        {
            var data = await _db.PenugasanAsets
                .Include(p => p.Details)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (data == null) return null;

            // 1. Ambil Header dari tbl systemsettings
            var setting = await _db.SystemSettings.FirstOrDefaultAsync();

            // 2. Ambil Info Karyawan (Penerima) beserta relasi Department-nya
            string karyawanNama = "-";
            string karyawanNik = "-";
            string karyawanDivisi = "-";
            string karyawanEmail = "-";
            string karyawanTelp = "-";

            if (data.TipePenerima == TipePenerima.Karyawan)
            {
                var karyawan = await _db.Karyawans
                    .Include(k => k.Department)
                    .FirstOrDefaultAsync(k => k.Id == data.TargetPenerimaId);

                if (karyawan != null)
                {
                    karyawanNama = karyawan.NamaLengkap ?? "-";
                    karyawanNik = karyawan.Nik ?? "EMP-001";
                    karyawanDivisi = karyawan.Department?.Name ?? "-";
                    karyawanEmail = karyawan.Email ?? "-";
                    karyawanTelp = karyawan.NoTelepon ?? "-";
                }
            }

            // 3. Pihak Penyerah (Issuer) dari user aktif login atau default
            string penyerah = await GetNamaPenyerahAsync(loggedInUser);
            var userKaryawan = await _db.Karyawans.FirstOrDefaultAsync(k => k.Email == loggedInUser || k.Nik == loggedInUser);
            if (userKaryawan != null)
            {
                penyerah = userKaryawan.NamaLengkap;
            }

            // 4. Mapping Details Aset
            var detailsList = new List<PenugasanAsetPrintDetailViewModel>();
            foreach (var d in data.Details)
            {
                string sku = "-";
                string merek = "-";
                string gudangNama = "Gudang Utama JKT";
                string lokasiRak = "-";

                if (!string.IsNullOrEmpty(d.SerialNumber))
                {
                    var prodItem = await (from pi in _db.ProdukItems
                                          join pr in _db.Produks on pi.ProdukId equals pr.Id
                                          join g in _db.Gudangs on pi.GudangId equals g.Id into gGrp
                                          from gdg in gGrp.DefaultIfEmpty()
                                          join r in _db.Raks on pi.RakId equals r.Id into rGrp
                                          from rak in rGrp.DefaultIfEmpty()
                                          where pi.SerialNumber == d.SerialNumber
                                          select new { pr.Sku, pr.Merek, Gudang = gdg.NamaGudang, Rak = rak.NamaRak }).FirstOrDefaultAsync();

                    if (prodItem != null)
                    {
                        sku = prodItem.Sku ?? "-";
                        merek = prodItem.Merek ?? "-";
                        gudangNama = prodItem.Gudang ?? "Gudang Utama JKT";
                        lokasiRak = prodItem.Rak ?? "-";
                    }
                }

                detailsList.Add(new PenugasanAsetPrintDetailViewModel
                {
                    Sku = sku,
                    Merek = merek,
                    NamaBarang = d.NamaBarang,
                    GudangNama = gudangNama,
                    LokasiRak = lokasiRak,
                    SerialNumber = d.SerialNumber ?? "-",
                    KondisiSerah = d.KondisiSerah ?? "Good"
                });
            }

            return new PenugasanAsetPrintKaryawanViewModel
            {
                Id = data.Id,
                KodePenugasan = data.KodePenugasan,
                TanggalSerahTerima = data.TanggalSerahTerima,
                Status = data.Status,
                Keterangan = data.Keterangan,
                PerusahaanNama = setting?.SiteName ?? "PT. CONTOH MAJU JAYA",
                PerusahaanSlogan = setting?.SiteDescription ?? "Inventory Multi-Gudang Modern.",
                PerusahaanAlamat = setting?.CompanyAddress ?? "Komp. Perkantoran Indah Blok B-12, Jakarta Selatan",
                PerusahaanTelp = setting?.CompanyPhone ?? "(021) 555-9830",
                PerusahaanEmail = setting?.CompanyEmail ?? "support@ptcontohmajujaya.com",
                AdminPenyerah = penyerah,
                KaryawanNama = karyawanNama,
                KaryawanNik = karyawanNik,
                KaryawanDivisi = karyawanDivisi,
                KaryawanEmail = karyawanEmail,
                KaryawanTelp = karyawanTelp,
                Details = detailsList
            };
        }

        public async Task<PenugasanAsetPrintCustomerViewModel?> GetPrintCustomerViewModelAsync(int id, string? loggedInUser)
        {
            var data = await _db.PenugasanAsets
                .Include(p => p.Details)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (data == null) return null;

            // 1. Ambil Header dari tbl systemsettings
            var setting = await _db.SystemSettings.FirstOrDefaultAsync();

            // 2. Ambil Info Customer (Penerima)
            string custNama = "-";
            string custKode = "-";
            string custAlamat = "-";
            string custEmail = "-";
            string custTelp = "-";

            if (data.TipePenerima == TipePenerima.Customer)
            {
                var cust = await _db.Customers.FirstOrDefaultAsync(c => c.Id == data.TargetPenerimaId);
                if (cust != null)
                {
                    custNama = cust.NamaCustomer ?? "-";
                    custKode = cust.KodeCustomer ?? "CUST-001";
                    custAlamat = cust.Alamat ?? "-";
                    custEmail = cust.Email ?? "-";
                    custTelp = cust.NoTelepon ?? "-";
                }
            }

            // 3. Pihak Penyerah (Issuer) dari user login aktif atau default
            string penyerah = await GetNamaPenyerahAsync(loggedInUser);
            var userKaryawan = await _db.Karyawans.FirstOrDefaultAsync(k => k.Email == loggedInUser || k.Nik == loggedInUser);
            if (userKaryawan != null)
            {
                penyerah = userKaryawan.NamaLengkap;
            }

            // 4. Mapping Details Aset
            var detailsList = new List<PenugasanAsetPrintDetailViewModel>();
            foreach (var d in data.Details)
            {
                string sku = "-";
                string merek = "-";
                string gudangNama = "Gudang Utama JKT";
                string lokasiRak = "-";

                if (!string.IsNullOrEmpty(d.SerialNumber))
                {
                    var prodItem = await (from pi in _db.ProdukItems
                                          join pr in _db.Produks on pi.ProdukId equals pr.Id
                                          join g in _db.Gudangs on pi.GudangId equals g.Id into gGrp
                                          from gdg in gGrp.DefaultIfEmpty()
                                          join r in _db.Raks on pi.RakId equals r.Id into rGrp
                                          from rak in rGrp.DefaultIfEmpty()
                                          where pi.SerialNumber == d.SerialNumber
                                          select new { pr.Sku, pr.Merek, Gudang = gdg.NamaGudang, Rak = rak.NamaRak }).FirstOrDefaultAsync();

                    if (prodItem != null)
                    {
                        sku = prodItem.Sku ?? "-";
                        merek = prodItem.Merek ?? "-";
                        gudangNama = prodItem.Gudang ?? "Gudang Utama JKT";
                        lokasiRak = prodItem.Rak ?? "-";
                    }
                }

                detailsList.Add(new PenugasanAsetPrintDetailViewModel
                {
                    Sku = sku,
                    Merek = merek,
                    NamaBarang = d.NamaBarang,
                    GudangNama = gudangNama,
                    LokasiRak = lokasiRak,
                    SerialNumber = d.SerialNumber ?? "-",
                    KondisiSerah = d.KondisiSerah ?? "Good"
                });
            }

            return new PenugasanAsetPrintCustomerViewModel
            {
                Id = data.Id,
                KodePenugasan = data.KodePenugasan,
                TanggalSerahTerima = data.TanggalSerahTerima,
                Status = data.Status,
                Keterangan = data.Keterangan,
                PerusahaanNama = setting?.SiteName ?? "PT. CONTOH MAJU JAYA",
                PerusahaanSlogan = setting?.SiteDescription ?? "Inventory Multi-Gudang Modern.",
                PerusahaanAlamat = setting?.CompanyAddress ?? "Komp. Perkantoran Indah Blok B-12, Jakarta Selatan",
                PerusahaanTelp = setting?.CompanyPhone ?? "(021) 555-9830",
                PerusahaanEmail = setting?.CompanyEmail ?? "support@ptcontohmajujaya.com",
                AdminPenyerah = penyerah,
                CustomerNama = custNama,
                CustomerKode = custKode,
                CustomerAlamat = custAlamat,
                CustomerEmail = custEmail,
                CustomerTelp = custTelp,
                Details = detailsList
            };
        }

        // Fungsi Untuk mengambil nama karyawan dari db.
        public async Task<string> GetNamaPenyerahAsync(string? loggedInUser)
        {
            if (string.IsNullOrEmpty(loggedInUser))
                return "Staff Logistik";

            // 1. Cek relasi dari tabel AspNetUsers (KaryawanId) ke tabel Karyawans
            var namaKaryawan = await (from u in _db.Users
                                      join k in _db.Karyawans on u.KaryawanId equals k.Id
                                      where u.UserName == loggedInUser || u.Email == loggedInUser
                                      select k.NamaLengkap).FirstOrDefaultAsync();

            if (!string.IsNullOrEmpty(namaKaryawan))
            {
                return namaKaryawan;
            }

            // 2. Fallback: Cek langsung di tabel Karyawans berdasarkan Email atau NIK
            var karyawanByEmail = await _db.Karyawans
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