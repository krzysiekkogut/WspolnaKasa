using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WspolnaKasa.App_GlobalResources;

namespace WspolnaKasa.Models.Dashboard
{
    public class ExpenseViewModel
    {
        [Required]
        public int ExpenseId { get; set; }

        [Required(ErrorMessage = null, ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "Dashboard_GroupRequired")]
        public int GroupId { get; set; }

        public string Group { get; set; }

        public string User { get; set; }

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

        public bool Editable { get; set; }

        public List<string> Participants { get; set; }
    }
}