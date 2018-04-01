using System;
using System.Web.Mvc;
using Cheapdrinks.Web.Extensions.ViewModels;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;

namespace Cheapdrinks.Web.Extensions.Controllers
{
    public class ResetPasswordController : RenderMvcController
    {
        public override ActionResult Index(RenderModel model)
        {
            //we will create a custom model
            var customModel = new ResetPasswordRenderViewModel(model.Content);

            return CurrentTemplate(customModel);
        }


        public ActionResult ResetPassword(RenderModel model, string g)
        {

            if (string.IsNullOrEmpty(g))
            {
                throw new ArgumentNullException("Reset password needs to have a valid Reset Guid");
            }

            //we will create a custom model
            var customModel = new ResetPasswordRenderViewModel(model.Content)
            {
                ResetGuid = g
            };

            return CurrentTemplate(customModel);
        }

    }
}
