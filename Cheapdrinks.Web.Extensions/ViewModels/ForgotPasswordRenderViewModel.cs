using System.ComponentModel.DataAnnotations;
using Umbraco.Core.Models;
using Umbraco.Web.Models;

namespace Cheapdrinks.Web.Extensions.ViewModels
{
    public class ForgotPasswordRenderViewModel : RenderModel
    {
        public ForgotPasswordRenderViewModel() : base(null)
        {
            
        }
        public string EmailAddress { get; set; }

        public ForgotPasswordRenderViewModel(IPublishedContent content) : base(content)
        {
        }

    }

    public class ForgotPasswordViewModel 
    {        
        [Required]
        [EmailAddress]
        public string EmailAddress { get; set; }
    }
}
