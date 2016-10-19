using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class Transfer
    {
        [Key]
        public int TransferId { get; set; }
        
        [Required]
        public string SenderId { get; set; }
        [ForeignKey("SenderId")]
        public virtual User Sender { get; set; }

        [Required]
        public string ReceiverId { get; set; }
        [ForeignKey("ReceiverId")]
        public virtual User Receiver { get; set; }

        [Required]
        public int GroupId { get; set; }
        public virtual Group Group { get; set; }

        [Required]
        public string Description { get; set; }

        [Range(0.01, double.MaxValue)]
        [Required]
        public double Amount { get; set; }

        [Required]
        public DateTime Date { get; set; }
    }
}
