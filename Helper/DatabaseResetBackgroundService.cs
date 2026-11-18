using MySqlConnector;

public class DatabaseResetBackgroundService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<DatabaseResetBackgroundService> _logger;
    private readonly string _connectionString;

    public DatabaseResetBackgroundService(
        IServiceProvider serviceProvider,
        ILogger<DatabaseResetBackgroundService> logger,
        IConfiguration configuration)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
        _connectionString = configuration.GetConnectionString("DefaultConnection")
                            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Database Reset Background Service sedang berjalan.");

        while (!stoppingToken.IsCancellationRequested)
        {
            var now = DateTime.Now;

            // Hitung target waktu berikutnya yaitu pukul 00:00:00 (Jam 12 Malam) hari berikutnya
            var nextMidnight = DateTime.Today.AddDays(1);
            var timeToWait = nextMidnight - now;

            _logger.LogInformation("Reset database berikutnya dijadwalkan pada: {NextMidnight}", nextMidnight);

            try
            {
                // Tunggu sampai jam 12 malam tiba atau service di-stop
                await Task.Delay(timeToWait, stoppingToken);
            }
            catch (TaskCanceledException)
            {
                // Task dibatalkan saat aplikasi berhenti (shutdown)
                break;
            }

            // Jika pembatalan tidak terjadi, jalankan proses reset
            if (!stoppingToken.IsCancellationRequested)
            {
                await ResetDatabaseAsync();

                // Beri jeda 1 menit agar tidak terjadi eksekusi berulang di menit yang sama
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }
    }

    private async Task ResetDatabaseAsync()
    {
        try
        {
            _logger.LogInformation("Memulai proses reset database otomatis jam 12 malam...");

            // Tentukan jalur file SQL seed
            string scriptPath = Path.Combine(AppContext.BaseDirectory, "Data", "Seed", "initial_data.sql"); // Sesuaikan nama file jika pakai initial_data_2.sql

            if (!File.Exists(scriptPath))
            {
                _logger.LogWarning("File skrip seed tidak ditemukan di path: {Path}", scriptPath);
                return;
            }

            string sqlScript = await File.ReadAllTextAsync(scriptPath);

            using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();

            // Pecah script berdasarkan titik koma (;) untuk menghindari error MySqlScript yang tidak ditemukan
            var commands = sqlScript.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

            using var transaction = await connection.BeginTransactionAsync();
            try
            {
                foreach (var query in commands)
                {
                    var trimmedQuery = query.Trim();
                    if (string.IsNullOrWhiteSpace(trimmedQuery)) continue;

                    using var cmd = new MySqlCommand(trimmedQuery, connection, transaction);
                    await cmd.ExecuteNonQueryAsync();
                }

                await transaction.CommitAsync();
                _logger.LogInformation("Database berhasil di-reset kembali ke data awal dummy.");
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Terjadi kesalahan fatal saat melakukan reset database otomatis.");
        }
    }
}


// untuk testing DatabaseResetBackgroundService secara cepat per 5 detik.
// using MySqlConnector;

// public class DatabaseResetBackgroundService : BackgroundService
// {
//     private readonly IServiceProvider _serviceProvider;
//     private readonly ILogger<DatabaseResetBackgroundService> _logger;
//     private readonly string _connectionString;

//     public DatabaseResetBackgroundService(
//         IServiceProvider serviceProvider, 
//         ILogger<DatabaseResetBackgroundService> logger, 
//         IConfiguration configuration)
//     {
//         _serviceProvider = serviceProvider;
//         _logger = logger;
//         _connectionString = configuration.GetConnectionString("DefaultConnection") 
//                             ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
//     }

//     protected override async Task ExecuteAsync(CancellationToken stoppingToken)
//     {
//         // 1. Log saat service pertama kali menyala/start
//         _logger.LogInformation("==================================================");
//         _logger.LogInformation("[BG-SERVICE] Database Reset Background Service BERHASIL START.");
//         _logger.LogInformation("==================================================");

//         // --- MODE TEST 5 DETIK (Untuk pengujian) ---
//         _logger.LogInformation("[BG-SERVICE] MODE TEST: Menunggu 5 detik sebelum eksekusi reset pertama...");
//         try
//         {
//             await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);

//         }
//         catch (TaskCanceledException)
//         {
//             _logger.LogWarning("[BG-SERVICE] Task dibatalkan saat masa tunggu 5 detik.");
//             return;
//         }

//         // Panggil fungsi reset untuk test
//         await ResetDatabaseAsync();
//         // ---------------------------------------------

//         // Siklus utama berikutnya (Kembali ke penjadwalan normal jam 12 malam)
//         while (!stoppingToken.IsCancellationRequested)
//         {
//             var now = DateTime.Now;
//             var nextMidnight = DateTime.Today.AddDays(1);
//             var timeToWait = nextMidnight - now;

//             _logger.LogInformation("[BG-SERVICE] Reset database otomatis berikutnya dijadwalkan pada: {NextMidnight}", nextMidnight);

//             try
//             {
//                 await Task.Delay(timeToWait, stoppingToken);
//             }
//             catch (TaskCanceledException)
//             {
//                 break;
//             }

//             if (!stoppingToken.IsCancellationRequested)
//             {
//                 await ResetDatabaseAsync();
//                 await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
//             }
//         }
//     }

//     private async Task ResetDatabaseAsync()
//     {
//         try
//         {
//             _logger.LogInformation("[BG-SERVICE] >>> MEMULAI PROSES RESET DATABASE KEMBALI KE DUMMY AWAL <<<");

//             // Pastikan nama file sesuai (initial_data.sql atau initial_data_2.sql)
//             string scriptPath = Path.Combine(AppContext.BaseDirectory, "Data", "Seed", "initial_data.sql");
            
//             if (!File.Exists(scriptPath))
//             {
//                 _logger.LogError("[BG-SERVICE] GAGAL: File skrip seed tidak ditemukan di path: {Path}", scriptPath);
//                 return;
//             }

//             string sqlScript = await File.ReadAllTextAsync(scriptPath);

//             using var connection = new MySqlConnection(_connectionString);
//             await connection.OpenAsync();
//             _logger.LogInformation("[BG-SERVICE] Koneksi database MySQL berhasil dibuka untuk reset.");

//             // Pecah script berdasarkan titik koma (;)
//             var commands = sqlScript.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

//             using var transaction = await connection.BeginTransactionAsync();
//             try
//             {
//                 int queryCount = 0;
//                 foreach (var query in commands)
//                 {
//                     var trimmedQuery = query.Trim();
//                     if (string.IsNullOrWhiteSpace(trimmedQuery)) continue;

//                     using var cmd = new MySqlCommand(trimmedQuery, connection, transaction);
//                     await cmd.ExecuteNonQueryAsync();
//                     queryCount++;
//                 }

//                 await transaction.CommitAsync();
//                 _logger.LogInformation("[BG-SERVICE] >>> SUKSES: Database berhasil di-reset! Total {Count} perintah SQL dieksekusi. <<<", queryCount);
//             }
//             catch (Exception exTransaction)
//             {
//                 await transaction.RollbackAsync();
//                 _logger.LogError(exTransaction, "[BG-SERVICE] ERROR SQL Transaction: Rollback dilakukan karena ada query yang gagal.");
//                 throw;
//             }
//         }
//         catch (Exception ex)
//         {
//             _logger.LogError(ex, "[BG-SERVICE] TERJADI KESALAHAN FATAL saat melakukan reset database otomatis.");
//         }
//     }
// }