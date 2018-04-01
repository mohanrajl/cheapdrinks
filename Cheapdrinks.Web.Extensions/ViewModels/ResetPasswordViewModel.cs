using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc.Html;
using Umbraco.Core.Models;
using Umbraco.Web.Models;

namespace Cheapdrinks.Web.Extensions.ViewModels
{
    public class ResetPasswordRenderViewModel : RenderModel
    {
        [DisplayName("Email Address")]
        public string EmailAddress { get; set; }

        [DisplayName("Password")]
        public string Password { get; set; }

        [DisplayName("Confirm Password")]
        public string ConfirmPassword { get; set; }

        public string ResetGuid { get; set; }

        public ResetPasswordRenderViewModel(IPublishedContent content) : base(content)
        {

        }
    }

    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string EmailAddress { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Password and Confirmation Password must match.")]
        public string ConfirmPassword { get; set; }

        public string ResetGuid { get; set; }

    }
}
