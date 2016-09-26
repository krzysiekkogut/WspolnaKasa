using System.ComponentModel.DataAnnotations;

namespace WspolnaKasa.Models.Dashboard
{
    public class RemoveTransferViewModel
    {
        [Required]
        public int TransferId { get; set; }
    }
}