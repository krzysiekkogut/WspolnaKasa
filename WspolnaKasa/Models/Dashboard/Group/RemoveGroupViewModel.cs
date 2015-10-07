using System.ComponentModel.DataAnnotations;
using WspolnaKasa.App_GlobalResources;

namespace WspolnaKasa.Models.Dashboard
{
    public class RemoveGroupViewModel
    {
        [Required]
        public int GroupId { get; set; }

        [Required(ErrorMessage = null, ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "Dashboard_GroupSecretRequired")]
        [Display(ResourceType = typeof(Translations), Name = "Dashboard_GroupSecret")]
        public string Secret { get; set; }
    }
}