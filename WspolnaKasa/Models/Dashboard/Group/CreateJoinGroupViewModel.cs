using System.ComponentModel.DataAnnotations;
using WspolnaKasa.App_GlobalResources;

namespace WspolnaKasa.Models.Dashboard
{
    public class CreateJoinGroupViewModel
    {
        [Required(ErrorMessage = null, ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "Dashboard_GroupNameRequired")]
        [Display(ResourceType = typeof(Translations), Name="Dashboard_GroupName")]
        public string Name { get; set; }

        [Required(ErrorMessage = null, ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "Dashboard_GroupSecretRequired")]
        [Display(ResourceType = typeof(Translations), Name = "Dashboard_GroupSecret")]
        public string Secret { get; set; }
    }
}