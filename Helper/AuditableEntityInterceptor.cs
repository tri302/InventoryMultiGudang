using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using InventoryMultiGudang.Models;

namespace InventoryMultiGudang.Helper
{
    public class AuditableEntityInterceptor : SaveChangesInterceptor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuditableEntityInterceptor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData eventData,
            InterceptionResult<int> result,
            CancellationToken cancellationToken = default)
        {
            var dbContext = eventData.Context;
            if (dbContext == null)
            {
                Console.WriteLine("[AuditInterceptor] DbContext bernilai null.");
                return await base.SavingChangesAsync(eventData, result, cancellationToken);
            }

            // Debug: Cek apakah method SavingChangesAsync terpanggil
            Console.WriteLine("[AuditInterceptor] SavingChangesAsync dipanggil.");

            string currentUsername = _httpContextAccessor.HttpContext?.User?.Identity?.Name;
            string currentCauser = "System User";

            if (!string.IsNullOrEmpty(currentUsername))
            {
                try
                {
                    var user = await dbContext.Set<ApplicationUser>()
                        .Include(u => u.Karyawan)
                        .FirstOrDefaultAsync(u => u.UserName == currentUsername || u.Email == currentUsername, cancellationToken);

                    if (user != null)
                    {
                        if (user.Karyawan != null && !string.IsNullOrEmpty(user.Karyawan.NamaLengkap))
                        {
                            currentCauser = user.Karyawan.NamaLengkap;
                        }
                        else if (!string.IsNullOrEmpty(user.FullName))
                        {
                            currentCauser = user.FullName;
                        }
                        else
                        {
                            currentCauser = user.UserName;
                        }
                    }
                    else
                    {
                        currentCauser = currentUsername;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[AuditInterceptor] Gagal mengambil nama user: {ex.Message}");
                    currentCauser = currentUsername;
                }
            }

            // Daftar tabel yang dipantau (Pastikan namanya sesuai huruf kecil di database Anda)            
            var trackedTables = new[]
            {
                "transaksistok",
                "penugasanasets",
                "peminjamanaset",
                "stockopname",
                "transferorders",
                "aspnetusers",
                "reparasi_aset",
                "produk",
                "customers",
                "departments",
                "gudang",
                "karyawans",
                "kategoris",
                "master_uom",
                "peminjamanasetdetail",
                "penugasanasetdetail",
                "produk_item",
                "produk_settings",
                "rak",
                "stockopnamedetail",
                "suppliers",
                "systemsettings",
                "transaksistokdetail",
                "transferorderdetails"
            };

            var entries = dbContext.ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added ||
                            e.State == EntityState.Modified ||
                            e.State == EntityState.Deleted)
                .ToList();

            Console.WriteLine($"[AuditInterceptor] Jumlah entri yang berubah: {entries.Count}");

            var logsToAdd = new List<ActivityLog>();

            foreach (var entry in entries)
            {
                var tableName = entry.Metadata.GetTableName()?.ToLower();
                Console.WriteLine($"[AuditInterceptor] Memeriksa tabel: {tableName}, State: {entry.State}");

                if (tableName != null && trackedTables.Contains(tableName))
                {
                    string eventType = entry.State switch
                    {
                        EntityState.Added => "CREATE",
                        EntityState.Modified => "UPDATE",
                        EntityState.Deleted => "DELETE",
                        _ => "UNKNOWN"
                    };

                    var primaryKeyName = entry.Metadata.FindPrimaryKey()?.Properties.FirstOrDefault()?.Name;
                    var primaryKeyValue = primaryKeyName != null ? entry.Property(primaryKeyName).CurrentValue : "N/A";

                    string description = $"Melakukan aksi {eventType} pada data {tableName} (ID/Ref: {primaryKeyValue})";

                    logsToAdd.Add(new ActivityLog
                    {
                        Causer = currentCauser,
                        Event = eventType,
                        Description = description,
                        Subject = tableName.ToUpper(),
                        Timestamp = DateTime.Now
                    });

                    Console.WriteLine($"[AuditInterceptor] Log berhasil dibuat untuk tabel: {tableName} dengan aksi {eventType}");
                }
            }

            if (logsToAdd.Count > 0)
            {
                await dbContext.Set<ActivityLog>().AddRangeAsync(logsToAdd, cancellationToken);
                Console.WriteLine($"[AuditInterceptor] Menambahkan {logsToAdd.Count} log ke database.");
            }
            else
            {
                Console.WriteLine("[AuditInterceptor] Tidak ada log yang ditambahkan (tidak ada tabel terlacak yang berubah).");
            }

            return await base.SavingChangesAsync(eventData, result, cancellationToken);
        }
    }
}