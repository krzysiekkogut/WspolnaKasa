using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using WspolnaKasa.App_GlobalResources;

namespace WspolnaKasa.Models
{
    public class IndexViewModel
    {
        public bool HasPassword { get; set; }

        [Display(ResourceType = typeof(Translations), Name = "Manage_Index_ExternalLogin")]
        public IList<UserLoginInfo> Logins { get; set; }

        [Display(ResourceType = typeof(Translations), Name = "Manage_Index_DisplayName")]
        public string DisplayName { get; set; }
    }

    public class ManageLoginsViewModel
    {
        public IList<UserLoginInfo> CurrentLogins { get; set; }
        public IList<AuthenticationDescription> OtherLogins { get; set; }
    }

    public class FactorViewModel
    {
        public string Purpose { get; set; }
    }

    public class SetPasswordViewModel
    {
        [Required(ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "ViewModels_PasswordRequired", ErrorMessage = null)]
        [StringLength(100, MinimumLength = 6, ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "ViewModels_PasswordMinimumLength", ErrorMessage = null)]
        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(Translations), Name = "ManageViewModels_NewPassword")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(Translations), Name = "ViewModels_PasswordConfirm")]
        [Compare("NewPassword", ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "ViewModels_PasswordsNotMatching", ErrorMessage = null)]
        public string ConfirmPassword { get; set; }
    }

    public class ChangePasswordViewModel
    {
        [Required(ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "ViewModels_PasswordRequired", ErrorMessage = null)]
        [StringLength(100, MinimumLength = 6, ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "ViewModels_PasswordMinimumLength", ErrorMessage = null)]
        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(Translations), Name = "ManageViewModels_CurrentPassword")]
        public string OldPassword { get; set; }

        [Required(ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "ViewModels_PasswordRequired", ErrorMessage = null)]
        [StringLength(100, MinimumLength = 6, ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "ViewModels_PasswordMinimumLength", ErrorMessage = null)]
        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(Translations), Name = "ManageViewModels_NewPassword")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(Translations), Name = "ViewModels_PasswordConfirm")]
        [Compare("NewPassword", ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "ViewModels_PasswordsNotMatching", ErrorMessage = null)]
        public string ConfirmPassword { get; set; }
    }
}