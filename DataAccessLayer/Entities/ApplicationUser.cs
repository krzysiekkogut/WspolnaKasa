using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using DataAccessLayer.Entities.ExpensesDomain;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authenticationType)
        {
            return await manager.CreateIdentityAsync(this, authenticationType);
        }

        public virtual List<Group> Groups { get; set; }

        [InverseProperty("UserPaying")]
        public virtual List<Expense> ExpensesPaid { get; set; }

        public virtual List<Expense> ExpensesParticipated { get; set; }

        [InverseProperty("Sender")]
        public virtual List<Transfer> TransfersSent { get; set; }

        [InverseProperty("Receiver")]
        public virtual List<Transfer> TransfersReceived { get; set; }

        public string DisplayName { get; set; }
    }
}