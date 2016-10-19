using Domain.Entities;
using Microsoft.AspNet.Identity.EntityFramework;

namespace DataAccessLayer
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public System.Data.Entity.DbSet<Group> Groups { get; set; }

        public System.Data.Entity.DbSet<Expense> Expenses { get; set; }

        public System.Data.Entity.DbSet<Transfer> Transfers { get; set; }
    }
}
