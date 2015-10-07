using System.ComponentModel.DataAnnotations;
using WspolnaKasa.App_GlobalResources;

namespace WspolnaKasa.Models.Dashboard
{
    public class EditGroupViewModel
    {
        [Required]
        public int GroupId { get; set; }

        [Required(ErrorMessage = null, ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "Dashboard_GroupNameRequired")]
        [Display(ResourceType = typeof(Translations), Name = "Dashboard_NewGroupName")]
        public string NewName { get; set; }
    }
}