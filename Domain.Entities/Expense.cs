using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class Expense
    {
        public Expense()
        {
            Participants = new List<User>();
        }

        [Key]
        public int ExpenseId { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Range(0.01, double.MaxValue)]
        [Required]
        public double Amount { get; set; }

        public string UserPayingId { get; set; }

        [ForeignKey("UserPayingId")]
        public virtual User UserPaying { get; set; }
        
        public virtual List<User> Participants { get; set; }

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
