using InventoryMultiGudang.Models;
using InventoryMultiGudang.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace InventoryMultiGudang.Repository
{

    public class SupplierRepository : ISupplierRepository
    {
        private readonly ApplicationDbContext _context;

        public SupplierRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<SupplierPagedResultViewModel> GetPagedAsync(string search, bool? status, int page, int pageSize)
        {
            var query = _context.Suppliers.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.ToLower();
                query = query.Where(s => s.KodeSupplier.ToLower().Contains(search) ||
                                         s.NamaSupplier.ToLower().Contains(search) ||
                                         s.NamaPic.ToLower().Contains(search));
            }

            if (status.HasValue)
            {
                query = query.Where(s => s.IsAktif == status.Value);
            }

            var totalItems = await query.CountAsync();
            var items = await query.OrderByDescending(s => s.Id)
                                   .Skip((page - 1) * pageSize)
                                   .Take(pageSize)
                                   .Select(s => new SupplierViewModel
                                   {
                                       Id = s.Id,
                                       KodeSupplier = s.KodeSupplier,
                                       NamaSupplier = s.NamaSupplier,
                                       Email = s.Email,
                                       Telepon = s.Telepon,
                                       NamaPic = s.NamaPic,
                                       WhatsappPic = s.WhatsappPic,
                                       Alamat = s.Alamat,
                                       Catatan = s.Catatan,                                       
                                       IsAktif = s.IsAktif
                                   }).ToListAsync();

            return new SupplierPagedResultViewModel
            {
                Items = items,
                TotalItems = totalItems,
                CurrentPage = page,
                PageSize = pageSize
            };
        }

        public async Task<Supplier> GetByIdAsync(int id)
        {
            return await _context.Suppliers.FindAsync(id);
        }

        public async Task<string> GenerateKodeSupplierAsync()
        {
            var count = await _context.Suppliers.CountAsync() + 1;
            return $"SPL-{count:D3}";
        }

        public async Task AddAsync(Supplier supplier)
        {
            await _context.Suppliers.AddAsync(supplier);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Supplier supplier)
        {
            _context.Suppliers.Update(supplier);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var supplier = await _context.Suppliers.FindAsync(id);
            if (supplier != null)
            {
                _context.Suppliers.Remove(supplier);
                await _context.SaveChangesAsync();
            }
        }
    }
}
