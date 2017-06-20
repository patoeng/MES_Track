using System.Security.Principal;
using MES.Mvc.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace MES.Mvc.Helpers
{
    public static class UserControl
    {
        public static bool IsAdminUser(IPrincipal userPrincipal)
        {
            if (userPrincipal == null) return false;
            if (userPrincipal.Identity.IsAuthenticated)
            {
                var user = userPrincipal.Identity;
                ApplicationDbContext context = new ApplicationDbContext();
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                var s = userManager.GetRoles(user.GetUserId());
                return s[0] == "Admin";
            }
            return false;
        }
    }
}