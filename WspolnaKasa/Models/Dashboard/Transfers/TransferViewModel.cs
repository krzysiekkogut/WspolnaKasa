using System;
using System.ComponentModel.DataAnnotations;
using WspolnaKasa.App_GlobalResources;

namespace WspolnaKasa.Models.Dashboard
{
    public class TransferViewModel
    {
        public int TransferId { get; set; }
        public string UserFrom { get; set; }

        [Required]
        public string UserToId { get; set; }

        public string UserTo { get; set; }

        [Required]
        public int GroupId { get; set; }

        public string Group { get; set; }

        [Display(ResourceType = typeof(Translations), Name = "Dashboard_Description")]
        [Required(ErrorMessage = null, ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "Dashboard_DescriptionRequired")]
        public string Description { get; set; }

        [Display(ResourceType = typeof(Translations), Name = "Dashboard_Amount")]
        [Range(0.01, double.MaxValue, ErrorMessage = null, ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "Dashboard_PositiveAmount")]
        [Required(ErrorMessage = null, ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "Dashboard_AmountRequired")]
        public double Amount { get; set; }

        [Display(ResourceType = typeof(Translations), Name = "Dashboard_Date")]
        [Required(ErrorMessage = null, ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "Dashboard_DateRequired")]
        public DateTime Date { get; set; }
    }
}