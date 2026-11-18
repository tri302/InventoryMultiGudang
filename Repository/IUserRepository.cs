using InventoryMultiGudang.Models;
using Microsoft.AspNetCore.Identity;

namespace InventoryMultiGudang.Repository
{
    public interface IUserRepository
    {
        Task<(List<UserViewModel> Users, int TotalCount)> GetPagedUsersAsync(string search, string role, int page, int pageSize);
        Task<UserViewModel> GetByIdAsync(string id);        
        Task<(bool Succeeded, IEnumerable<string> Errors)> CreateAsync(UserViewModel model);
        Task<(bool Succeeded, IEnumerable<string> Errors)> UpdateAsync(UserViewModel model);    
        Task<bool> DeleteAsync(string id);
        Task<List<string>> GetAllRolesAsync();

        /// <summary>
        /// Mengambil data user berdasarkan alamat email secara asinkron.
        /// </summary>
        Task<ApplicationUser?> GetUserByEmailAsync(string email);

        /// <summary>
        /// Mengambil daftar role dari user tertentu.
        /// </summary>
        Task<IList<string>> GetUserRolesAsync(ApplicationUser user);

        /// <summary>
        /// Melakukan proses otentikasi login menggunakan PasswordSignInAsync.
        /// </summary>
        Task<SignInResult> AuthenticateUserAsync(ApplicationUser user, string password, bool isPersistent, bool lockoutOnFailure);

        /// <summary>
        /// Melakukan proses logout pengguna dari sistem secara asinkron.
        /// </summary>
        Task SignOutAsync();

        Task<IEnumerable<Karyawan>> SearchKaryawansAsync(string? searchTerm, int takeLimit = 5);
        Task<Karyawan?> GetByIdAsync(int id);
    }
}