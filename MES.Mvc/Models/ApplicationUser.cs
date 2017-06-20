using System.ComponentModel;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace MES.Mvc.Models
{
    public class ApplicationUser :IdentityUser
    {
        [DisplayName("Employee Register Number")]
        public string EmployeeRegisterNumber { get; set; }
        public string Name { get; set; }
        public int? Level { get; set; }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(
            UserManager<ApplicationUser> manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);

            // Add custom user claims here
            return userIdentity;
        }

    }
}
