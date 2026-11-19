using System.Collections.Generic;
using System.Threading.Tasks;
using InventoryMultiGudang.Models;

namespace InventoryMultiGudang.Repository
{
    public interface ISystemSettingRepository
    {
        Task<IEnumerable<SystemSetting>> GetAllAsync();
        Task<SystemSetting> GetByIdAsync(int id);
        Task AddAsync(SystemSetting setting);
        Task UpdateAsync(SystemSetting setting);
        Task DeleteAsync(int id);
    }
}