using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace InventoryMultiGudang.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }
        public DbSet<UnitOfMeasure> MasterUoms { get; set; }
        public DbSet<Gudang> Gudangs { get; set; }
        public DbSet<Rak> Raks { get; set; }
        public DbSet<Produk> Produks { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Karyawan> Karyawans { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<PenugasanAset> PenugasanAsets { get; set; }
        public DbSet<PenugasanAsetDetail> PenugasanAsetDetails { get; set; }
        public DbSet<TransaksiStok> TransaksiStoks { get; set; }
        public DbSet<TransaksiStokDetail> TransaksiStokDetails { get; set; }
        public DbSet<Kategori> Kategoris { get; set; }
        public DbSet<TransferOrder> TransferOrders { get; set; }
        public DbSet<TransferOrderDetail> TransferOrderDetails { get; set; }
        public DbSet<StockOpname> StockOpnames { get; set; }
        public DbSet<StockOpnameDetail> StockOpnameDetails { get; set; }
        public DbSet<ProdukItem> ProdukItems { get; set; }
        public DbSet<PeminjamanAset> PeminjamanAsets { get; set; }
        public DbSet<PeminjamanAsetDetail> PeminjamanAsetDetails { get; set; }
        public DbSet<ProdukSetting> ProdukSettings { get; set; }
        public DbSet<ReparasiAsset> ReparasiAssets { get; set; }
        public DbSet<SystemSetting> SystemSettings { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<MenuModel> Menus { get; set; }
        public DbSet<ActivityLog> ActivityLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<PenugasanAsetDetail>()
                .ToTable("penugasanasetdetail");

            modelBuilder.Entity<PeminjamanAsetDetail>()
                .ToTable("peminjamanasetdetail")
                .HasOne(d => d.PeminjamanAset)
                .WithMany(p => p.Details)
                .HasForeignKey(d => d.PeminjamanAsetID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PeminjamanAset>()
                .HasOne(p => p.Karyawan)
                .WithMany()
                .HasForeignKey(p => p.KaryawanId);

            // Indeks Unik Kode UOM
            modelBuilder.Entity<UnitOfMeasure>()
                .HasIndex(u => u.Code)
                .IsUnique();
        }
    }
}

