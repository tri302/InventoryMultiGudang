using InventoryMultiGudang.Models;
using InventoryMultiGudang.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace InventoryMultiGudang.Repository
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly ApplicationDbContext _context;

        public CustomerRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PaginatedListCustomer<Customer>> GetPagedAsync(CustomerDataTableRequest request)
        {
            var query = _context.Customers.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                var search = request.Search.Trim().ToLower();
                query = query.Where(c => c.NamaCustomer.ToLower().Contains(search)
                                      || c.KodeCustomer.ToLower().Contains(search)
                                      || c.Email.ToLower().Contains(search));
            }

            if (!string.IsNullOrWhiteSpace(request.Status) && request.Status != "Semua Status")
            {
                bool isActive = request.Status == "Aktif";
                query = query.Where(c => c.IsActive == isActive);
            }

            int totalItems = await query.CountAsync();
            var items = await query.OrderByDescending(c => c.Id)
                                   .Skip((request.Page - 1) * request.PageSize)
                                   .Take(request.PageSize)
                                   .ToListAsync();

            return new PaginatedListCustomer<Customer>
            {
                Items = items,
                TotalItems = totalItems,
                PageIndex = request.Page,
                PageSize = request.PageSize
            };
        }

        public async Task<Customer?> GetByIdAsync(int id)
        {
            return await _context.Customers.FindAsync(id);
        }

        public async Task<Customer> AddAsync(Customer customer)
        {
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
            return customer;
        }

        public async Task<bool> UpdateAsync(Customer customer)
        {
            _context.Customers.Update(customer);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var item = await _context.Customers.FindAsync(id);
            if (item == null) return false;

            _context.Customers.Remove(item);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<string> GenerateKodeCustomerAsync()
        {
            var lastCustomer = await _context.Customers.OrderByDescending(c => c.Id).FirstOrDefaultAsync();
            int nextNumber = (lastCustomer?.Id ?? 0) + 1;
            return $"CUST-{nextNumber:D3}";
        }
    }
}
