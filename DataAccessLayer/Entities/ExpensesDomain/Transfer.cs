using System;
using System.ComponentModel.DataAnnotations;

namespace DataAccessLayer.Entities.ExpensesDomain
{
    public class Transfer
    {
        [Key]
        public int TransferId { get; set; }
        
        [Required]
        public string ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

        [Required]
        public int GroupId { get; set; }
        public virtual Group Group { get; set; }

        [Required]
        public string ReceiverId { get; set; }

        [Required]
        public string Description { get; set; }

        [Range(0.01, double.MaxValue)]
        [Required]
        public double Amount { get; set; }

        [Required]
        public DateTime Date { get; set; }
    }
}
