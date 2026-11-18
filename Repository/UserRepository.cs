using InventoryMultiGudang.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace InventoryMultiGudang.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _dbContext;

        public UserRepository(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            SignInManager<ApplicationUser> signInManager,
            ApplicationDbContext dbContext
            )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _dbContext = dbContext;
        }

        public async Task<(List<UserViewModel> Users, int TotalCount)> GetPagedUsersAsync(string search, string role, int page, int pageSize)
        {
            var query = _userManager.Users.AsQueryable();
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(u => u.UserName.Contains(search) || u.Email.Contains(search));
            }
            var totalCount = await query.CountAsync();
            var users = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            var result = new List<UserViewModel>();
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);

                // Jika filter role aktif
                if (!string.IsNullOrEmpty(role) && role != "All Roles" && !roles.Contains(role))
                    continue;

                result.Add(new UserViewModel
                {
                    Id = user.Id,
                    FullName = user.FullName,
                    Email = user.Email,
                    CreatedAt = user.CreatedAt,
                    Roles = roles.ToList()
                });
            }

            return (result, totalCount);
        }

        public async Task<UserViewModel> GetByIdAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return null;

            var roles = await _userManager.GetRolesAsync(user);

            // Ambil data karyawan terkait jika KaryawanId tidak null
            string karyawanText = string.Empty;
            if (user.KaryawanId.HasValue)
            {
                var karyawan = await _dbContext.Karyawans.FindAsync(user.KaryawanId.Value);
                if (karyawan != null)
                {
                    karyawanText = $"{karyawan.Nik} - {karyawan.NamaLengkap} ({karyawan.Jabatan ?? karyawan.Email})";
                }
            }

            return new UserViewModel
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                CreatedAt = user.CreatedAt,
                KaryawanId = user.KaryawanId, 
                KaryawanText = karyawanText, 
                Roles = roles.ToList()
            };
        }

        public async Task<(bool Succeeded, IEnumerable<string> Errors)> CreateAsync(UserViewModel model)
        {
            // 1. Gunakan Email sebagai UserName agar lolos validasi default Identity (tanpa spasi/karakter khusus)
            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                FullName = model.FullName,
                CreatedAt = DateTime.Now,
                KaryawanId = model.KaryawanId,
                EmailConfirmed = true
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                // 2. Setelah user berhasil dibuat, ubah UserName menjadi FullName (atau biarkan jika ingin tetap pakai Email/Full Name)
                // Jika FullName mengandung spasi dan memicu error update, Anda bisa menyimpannya ke field lain atau mengaturnya.
                // Tapi untuk amannya, kita masukkan role terlebih dahulu menggunakan instance 'user' yang valid:

                if (model.Roles != null && model.Roles.Any())
                {
                    foreach (var roleName in model.Roles)
                    {
                        if (!await _roleManager.RoleExistsAsync(roleName))
                        {
                            await _roleManager.CreateAsync(new IdentityRole(roleName));
                        }
                    }

                    var roleResult = await _userManager.AddToRolesAsync(user, model.Roles);
                    if (!roleResult.Succeeded)
                    {
                        return (false, roleResult.Errors.Select(e => e.Description));
                    }
                }

                return (true, Enumerable.Empty<string>());
            }

            return (false, result.Errors.Select(e => e.Description));
        }

        public async Task<(bool Succeeded, IEnumerable<string> Errors)> UpdateAsync(UserViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);
            if (user == null) return (false, new[] { "User tidak ditemukan." });

            user.UserName = model.FullName;
            user.Email = model.Email;
            user.FullName = model.FullName;
            user.KaryawanId = model.KaryawanId;
            user.CreatedAt = model.CreatedAt;
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                if (!string.IsNullOrWhiteSpace(model.Password))
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var passResult = await _userManager.ResetPasswordAsync(user, token, model.Password);
                    if (!passResult.Succeeded)
                    {
                        return (false, passResult.Errors.Select(e => e.Description));
                    }
                }

                // Hapus role lama dan masukkan role baru
                var currentRoles = await _userManager.GetRolesAsync(user);
                if (currentRoles.Any())
                {
                    await _userManager.RemoveFromRolesAsync(user, currentRoles);
                }

                if (model.Roles != null && model.Roles.Any())
                {
                    foreach (var roleName in model.Roles)
                    {
                        if (!await _roleManager.RoleExistsAsync(roleName))
                        {
                            await _roleManager.CreateAsync(new IdentityRole(roleName));
                        }
                    }

                    await _userManager.AddToRolesAsync(user, model.Roles);
                }

                return (true, Enumerable.Empty<string>());
            }

            return (false, result.Errors.Select(e => e.Description));
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return false;
            var result = await _userManager.DeleteAsync(user);
            return result.Succeeded;
        }

        public async Task<List<string>> GetAllRolesAsync()
        {
            return await _roleManager.Roles
                .Select(r => r.Name!)
                .Where(r => r != null)
                .ToListAsync();
        }

        /// <summary>
        /// Mencari data ApplicationUser berdasarkan Email tanpa mengaktifkan Change Tracker EF Core (AsNoTracking).
        /// </summary>
        public async Task<ApplicationUser?> GetUserByEmailAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return null;

            string normalizedEmail = email.ToUpperInvariant();

            // Query efisien & robust menggunakan index NormalizedEmail
            return await _dbContext.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.NormalizedEmail == normalizedEmail);
        }

        /// <summary>
        /// Mendapatkan peran/role dari pengguna secara asinkron.
        /// </summary>
        public async Task<IList<string>> GetUserRolesAsync(ApplicationUser user)
        {
            // Identity user internal query
            return await _userManager.GetRolesAsync(user);
        }

        /// <summary>
        /// Memverifikasi kata sandi pengguna dan mengautentikasi sesi cookie.
        /// Memiliki proteksi lockout bawaan dari ASP.NET Core Identity.
        /// </summary>
        public async Task<SignInResult> AuthenticateUserAsync(ApplicationUser user, string password, bool isPersistent, bool lockoutOnFailure)
        {
            // Karena user diambil AsNoTracking sebelumnya, kita otentikasi via username/user object
            return await _signInManager.PasswordSignInAsync(
                user.UserName ?? user.Email!,
                password,
                isPersistent,
                lockoutOnFailure: lockoutOnFailure
            );
        }

        /// <summary>
        /// Mengakhiri sesi akun pengguna.
        /// </summary>
        public async Task SignOutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<IEnumerable<Karyawan>> SearchKaryawansAsync(string? searchTerm, int takeLimit = 5)
        {
            var query = _dbContext.Karyawans.AsQueryable();
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(k => k.NamaLengkap.Contains(searchTerm) ||
                                         k.Nik.Contains(searchTerm) ||
                                         k.Email.Contains(searchTerm));
            }

            // Ambil data sesuai limit (misal 5 data pertama untuk lazy loading awal)
            return await query.Take(takeLimit).ToListAsync();
        }

        public async Task<Karyawan?> GetByIdAsync(int id)
        {
            return await _dbContext.Karyawans.FindAsync(id);
        }

    }
}