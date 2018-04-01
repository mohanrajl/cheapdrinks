using System.Web.Mvc;
using Cheapdrinks.Web.Extensions.ViewModels;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;

namespace Cheapdrinks.Web.Extensions.Controllers
{
    public class ForgotPasswordController : RenderMvcController
    {
        public override ActionResult Index(RenderModel model)
        {
            //we will create a custom model
            var customModel = new ForgotPasswordRenderViewModel(model.Content);

            return CurrentTemplate(customModel);
        }

    }
}
