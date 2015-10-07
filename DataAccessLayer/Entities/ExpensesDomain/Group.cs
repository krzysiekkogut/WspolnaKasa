using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.Entities.ExpensesDomain
{
    public class Group
    {
        public Group()
        {
            Members = new List<ApplicationUser>();
            Transfers = new List<Transfer>();
        }

        [Key]
        public int GroupId { get; set; }

        [Required]
        [StringLength(100)]
        [Index(IsUnique = true)]
        public string Name { get; set; }

        [Required]
        public string Secret { get; set; }

        public virtual List<ApplicationUser> Members { get; set; }

        public virtual List<Transfer> Transfers { get; set; }

        public virtual List<Expense> Expenses { get; set; }

        public override bool Equals(object obj)
        {
            var group = obj as Group;
            if (group == null) return false;
            if (ReferenceEquals(this, obj)) return true;
            return GroupId == group.GroupId && Name == group.Name && Secret == group.Secret;
        }

        public override int GetHashCode()
        {
            return GroupId;
        }
    }
}