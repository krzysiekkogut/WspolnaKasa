using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WspolnaKasa.App_GlobalResources;

namespace WspolnaKasa.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required(ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "ViewModels_EmailRequired", ErrorMessage = null)]
        [EmailAddress(ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "ViewModels_InvalidEmail", ErrorMessage = null)]
        [Display(ResourceType = typeof(Translations), Name = "ViewModels_Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Imię i nazwisko są wymagane.")]
        [Display(Name = "Imię i nazwisko")]
        public string UserName { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required(ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "ViewModels_EmailRequired", ErrorMessage = null)]
        [EmailAddress(ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "ViewModels_InvalidEmail", ErrorMessage = null)]
        [Display(ResourceType = typeof(Translations), Name = "ViewModels_Email")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required(ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "ViewModels_EmailRequired", ErrorMessage = null)]
        [EmailAddress(ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "ViewModels_InvalidEmail", ErrorMessage = null)]
        [Display(ResourceType = typeof(Translations), Name = "ViewModels_Email")]
        public string Email { get; set; }

        [Required(ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "ViewModels_PasswordRequired", ErrorMessage = null)]
        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(Translations), Name = "AccountViewModels_Password")]
        public string Password { get; set; }

        [Display(ResourceType = typeof(Translations), Name = "AccountViewModels_RememberMe")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required(ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "ViewModels_EmailRequired", ErrorMessage = null)]
        [EmailAddress(ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "ViewModels_InvalidEmail", ErrorMessage = null)]
        [Display(ResourceType = typeof(Translations), Name = "ViewModels_Email")]
        public string Email { get; set; }

        [Required(ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "ViewModels_PasswordRequired", ErrorMessage = null)]
        [StringLength(100, MinimumLength = 6, ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "ViewModels_PasswordMinimumLength", ErrorMessage = null)]
        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(Translations), Name = "AccountViewModels_Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(Translations), Name = "ViewModels_PasswordConfirm")]
        [Compare("Password", ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "ViewModels_PasswordsNotMatching", ErrorMessage = null)]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Imię i nazwisko są wymagane.")]
        [Display(Name = "Imię i nazwisko")]
        public string UserName { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required(ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "ViewModels_EmailRequired", ErrorMessage = null)]
        [EmailAddress(ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "ViewModels_InvalidEmail", ErrorMessage = null)]
        [Display(ResourceType = typeof(Translations), Name = "ViewModels_Email")]
        public string Email { get; set; }

        [Required(ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "ViewModels_PasswordRequired", ErrorMessage = null)]
        [StringLength(100, MinimumLength = 6, ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "ViewModels_PasswordMinimumLength", ErrorMessage = null)]
        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(Translations), Name = "AccountViewModels_Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(Translations), Name = "ViewModels_PasswordConfirm")]
        [Compare("Password", ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "ViewModels_PasswordsNotMatching", ErrorMessage = null)]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "ViewModels_EmailRequired", ErrorMessage = null)]
        [EmailAddress(ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "ViewModels_InvalidEmail", ErrorMessage = null)]
        [Display(ResourceType = typeof(Translations), Name = "ViewModels_Email")]
        public string Email { get; set; }
    }

    public class ExternalLoginViewModel
    {
        public string Name { get; set; }

        public string Url { get; set; }

        public string State { get; set; }
    }

    public class ManageInfoViewModel
    {
        public string LocalLoginProvider { get; set; }

        public string Email { get; set; }

        public IEnumerable<UserLoginInfoViewModel> Logins { get; set; }

        public IEnumerable<ExternalLoginViewModel> ExternalLoginProviders { get; set; }
    }

    public class UserInfoViewModel
    {
        public string Email { get; set; }

        public bool HasRegistered { get; set; }

        public string LoginProvider { get; set; }
    }

    public class UserLoginInfoViewModel
    {
        public string LoginProvider { get; set; }

        public string ProviderKey { get; set; }
    }
}
