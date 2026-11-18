using InventoryMultiGudang.Models;
using InventoryMultiGudang.ViewModels;

namespace InventoryMultiGudang.Repository
{
    public interface ICustomerRepository
    {
        Task<PaginatedListCustomer<Customer>> GetPagedAsync(CustomerDataTableRequest request);
        Task<Customer?> GetByIdAsync(int id);
        Task<Customer> AddAsync(Customer customer);
        Task<bool> UpdateAsync(Customer customer);
        Task<bool> DeleteAsync(int id);
        Task<string> GenerateKodeCustomerAsync();
    }
}
