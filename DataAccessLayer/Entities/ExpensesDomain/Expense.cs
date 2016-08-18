using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataAccessLayer.Entities.ExpensesDomain
{
    public class Expense
    {
        public Expense()
        {
            Participants = new List<ApplicationUser>();
        }

        [Key]
        public int ExpenseId { get; set; }

        [Required]
        public string UserPayingId { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public virtual List<ApplicationUser> Participants { get; set; }

        [Range(0.01, double.MaxValue)]
        [Required]
        public double Amount { get; set; }

        [Required]
        public int GroupId { get; set; }
        public virtual Group Group { get; set; }

        public override int GetHashCode()
        {
            return ExpenseId;
        }

        public override bool Equals(object obj)
        {
            var expense = obj as Expense;
            if (expense == null) return false;
            return
                ExpenseId == expense.ExpenseId
                && UserPayingId == expense.UserPayingId
                && Description == expense.Description
                && Date == expense.Date
                && Amount == expense.Amount
                && GroupId == GroupId;
        }

    }
}
