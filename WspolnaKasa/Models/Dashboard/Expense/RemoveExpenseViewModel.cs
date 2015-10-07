using System.ComponentModel.DataAnnotations;

namespace WspolnaKasa.Models.Dashboard
{
    public class RemoveExpenseViewModel
    {
        [Required]
        public int ExpenseId { get; set; }
    }
}