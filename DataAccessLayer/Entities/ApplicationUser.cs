using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using DataAccessLayer.Entities.ExpensesDomain;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace DataAccessLayer.Entities
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here
            return userIdentity;
        }

        public virtual List<Group> Groups { get; set; }

        public virtual List<Expense> Expenses { get; set; }

        public virtual List<Transfer> TransfersSent { get; set; }

        public string DisplayName { get; set; }
    }
}