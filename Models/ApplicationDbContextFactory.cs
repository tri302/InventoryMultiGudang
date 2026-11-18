using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace InventoryMultiGudang.Models 
{
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            // Membaca file appsettings.json untuk mengambil ConnectionString
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            // Sesuaikan dengan database yang Anda gunakan (contoh: UseMySql)
            builder.UseMySql(
                connectionString, 
                ServerVersion.AutoDetect(connectionString)
            );

            return new ApplicationDbContext(builder.Options);
        }
    }
}