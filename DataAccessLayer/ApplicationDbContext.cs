using DataAccessLayer.Entities;
using Microsoft.AspNet.Identity.EntityFramework;

namespace DataAccessLayer
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public System.Data.Entity.DbSet<Entities.ExpensesDomain.Group> Groups { get; set; }

        public System.Data.Entity.DbSet<Entities.ExpensesDomain.Expense> Expenses { get; set; }

        public System.Data.Entity.DbSet<Entities.ExpensesDomain.Transfer> Transfers { get; set; }
    }
}
