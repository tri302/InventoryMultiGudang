using InventoryMultiGudang.Models;
using InventoryMultiGudang.Repository;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using InventoryMultiGudang.Helper;
using Serilog;
using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using InventoryMultiGudang.Middleware;
using Microsoft.AspNetCore.Authorization;
using InventoryMultiGudang.Authorization;
using InventoryMultiGudang;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHostedService<DatabaseKeepAliveService>();
builder.Services.AddScoped<IGudangRepository, GudangRepository>();
builder.Services.AddScoped<IRakRepository, RakRepository>();
builder.Services.AddScoped<IProdukRepository, ProdukRepository>();
builder.Services.AddScoped<ISupplierRepository, SupplierRepository>();
builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
builder.Services.AddScoped<IKaryawanRepository, KaryawanRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IPenugasanAsetRepository, PenugasanAsetRepository>();
builder.Services.AddScoped<ITransaksiStokRepository, TransaksiStokRepository>();
builder.Services.AddScoped<IKategoriRepository, KategoriRepository>();
builder.Services.AddScoped<ITransferOrderRepository, TransferOrderRepository>();
builder.Services.AddScoped<IStockOpnameRepository, StockOpnameRepository>();
builder.Services.AddScoped<IPeminjamanAsetRepository, PeminjamanAsetRepository>();
builder.Services.AddScoped<IMatriksStokRepository, MatriksStokRepository>();
builder.Services.AddScoped<IKartuStokRepository, KartuStokRepository>();
builder.Services.AddScoped<ILaporanBarangMasukKeluarRepository, LaporanBarangMasukKeluarRepository>();
builder.Services.AddScoped<IActivityLogRepository, ActivityLogRepository>();
builder.Services.AddScoped<IDashboardRepository, DashboardRepository>();
builder.Services.AddScoped<ILaporanAssetRepository, LaporanAssetRepository>();
builder.Services.AddScoped<ILaporanStockOpnameRepository, LaporanStockOpnameRepository>();
builder.Services.AddScoped<IMenuManagerRepository<MenuModel>, MenuManagerRepository<MenuModel>>();
builder.Services.AddScoped<IReparasiAsetsRepository, ReparasiAsetsRepository>();
builder.Services.AddScoped<IRolePermissionRepository, RolePermissionRepository>();
builder.Services.AddScoped<ISystemSettingRepository, SystemSettingRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IMasterUomRepository, MasterUomRepository>();
builder.Services.AddScoped<IMenuService, MenuService>();
builder.Services.AddScoped<AuditableEntityInterceptor>();

// Pendaftaran Background Service untuk Reset DB Jam 12 Malam
builder.Services.AddHostedService<DatabaseResetBackgroundService>();

// 1. Daftarkan Policy dan Policy Provider Kustom
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("DynamicPermission", policy =>
    {
        policy.RequireAssertion(context => context.User != null && context.User.IsInRole("Super Admin"));
    });
});

// Tambahkan baris ini untuk mengganti/menambahkan Dynamic Policy Provider
builder.Services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();

// Tambahkan HttpContextAccessor untuk mengakses HttpContext di dalam service/repository
builder.Services.AddHttpContextAccessor();

// Paksa aplikasi menyimpan kunci enkripsi di folder proyek, bukan di sistem OS hosting
builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(Path.Combine(builder.Environment.ContentRootPath, "PersistedKeys")));

//Konfigurasi logger
// handle global error handling pakai Logger
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .WriteTo.File(
        "Logs/log-.txt",
        rollingInterval: RollingInterval.Day)
    .CreateLogger();
builder.Host.UseSerilog();


// Add services to the container.
builder.Services.AddControllersWithViews()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    });

// Konfigurasi db
builder.Services.AddDbContext<ApplicationDbContext>((sp, options) =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseMySql(
        connectionString,
        ServerVersion.AutoDetect(connectionString),
        mySqlOptions => mySqlOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)
    );
    // Ambil instance interceptor dari service provider dan tambahkan ke EF Core
    var auditableInterceptor = sp.GetRequiredService<AuditableEntityInterceptor>();
    options.AddInterceptors(auditableInterceptor);
});

// Konfigurasi session
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(20);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// konfigurasi Identity dan atur password policy menjadi false/longgar di sini:
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    // Pengaturan Password dibuat longgar (false semua)
    options.Password.RequireDigit = false;            // Tidak wajib angka
    options.Password.RequireLowercase = false;        // Tidak wajib huruf kecil
    options.Password.RequireUppercase = false;        // Tidak wajib huruf besar
    options.Password.RequireNonAlphanumeric = false;  // Tidak wajib simbol/karakter khusus (!@#$%^&*)
    options.Password.RequiredLength = 3;              // Minimal panjang password (bisa diatur sesuai keinginan, misal 3 atau 6)
    options.Password.RequiredUniqueChars = 1;         // Karakter unik minimal
    options.User.RequireUniqueEmail = true;
    options.SignIn.RequireConfirmedEmail = false;
}).AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

//Gunakan PostConfigure untuk mengonfigurasi Cookie yang dibuat oleh Identity
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Error/Forbidden";
    options.LogoutPath = "/Account/Logout";
    options.SlidingExpiration = true;
    options.ExpireTimeSpan = TimeSpan.FromHours(1);
    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
    options.Cookie.SameSite = SameSiteMode.Lax;
});

builder.Services.AddMemoryCache();

//Tambahkan ini hanya jika pakai login
builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
// GLOBAL ERROR HANDLER
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    //app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/Error/{0}"); // handle global error handling
app.UseExceptionHandler("/Error/ServerError"); // handle global error handling
app.UseMiddleware<GlobalExceptionMiddleware>(); // handle global error handling
app.UseMiddleware<MaintenanceMiddleware>(); // handle global error handling

app.UseHttpsRedirection();
app.UseRouting();
app.UseStaticFiles();
app.UseSession();
app.MapStaticAssets();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}")
    .WithStaticAssets();


using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        await SeedRolesAsync.SeederRolesAsync(services);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Terjadi kesalahan saat melakukan seeding data Role.");
    }
}

app.Run();
