using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace InventoryMultiGudang.Authorization
{
    public class PermissionPolicyProvider : IAuthorizationPolicyProvider
    {
        public DefaultAuthorizationPolicyProvider FallbackPolicyProvider { get; }

        public PermissionPolicyProvider(IOptions<AuthorizationOptions> options)
        {
            // Fallback ke policy provider bawaan ASP.NET Core untuk policy standar
            FallbackPolicyProvider = new DefaultAuthorizationPolicyProvider(options);
        }

        public Task<AuthorizationPolicy> GetDefaultPolicyAsync() =>
            FallbackPolicyProvider.GetDefaultPolicyAsync();

        public Task<AuthorizationPolicy?> GetFallbackPolicyAsync() =>
            FallbackPolicyProvider.GetFallbackPolicyAsync();

        // Di sinilah keajaibannya: setiap kali [Authorize(Policy = "nama-permission")] dipanggil
        // dan tidak ditemukan secara manual, method ini akan terpanggil secara otomatis.
        public Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
        {
            // Cek apakah policy name diasumsikan sebagai permission 
            // (Anda bisa menambahkan prefix khusus misal "Permission:" atau langsung 
            // menerima nama permission apa adanya). Di sini kita langsung tangkap sebagai permission.
            
            var policy = new AuthorizationPolicyBuilder();
            policy.RequireAssertion(context =>
            {
                // Jika Super Admin, selalu izinkan
                if (context.User != null && context.User.IsInRole("Super Admin"))
                {
                    return true;
                }

                // Cek apakah user memiliki klaim bertipe "Permission" dengan nilai yang sesuai
                return context.User != null && context.User.HasClaim(c => c.Type == "Permission" && c.Value == policyName);
            });

            return Task.FromResult<AuthorizationPolicy?>(policy.Build());
        }
    }
}