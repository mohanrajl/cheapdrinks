using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Cheapdrinks.Web.Extensions.Attribute;
using Cheapdrinks.Web.Extensions.ViewModels;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using Umbraco.Web.Models;

namespace Cheapdrinks.Web.Extensions.Controllers
{
    [CustomAuthorize]
    public class HomePageController : BaseController
    {
        private IMediaService MediaService
        {
            get
            {
                return ApplicationContext.Current.Services.MediaService;
            }
        }

        private string CurrentMemberOrganisationCode
        {
            get
            {
                return UmbracoContext.Application.Services.MemberService.GetByUsername(Members.CurrentUserName).GetValue<string>("organisation");
            }
        }

        public override ActionResult Index(RenderModel model)
        {
            var homePageRenderViewModel = new HomePageRenderViewModel(model.Content);
            homePageRenderViewModel.NavigationViewModel = new NavigationViewModel(this.CurrentPage);
            return CurrentTemplate(homePageRenderViewModel);
        }

        /// <summary>
        /// Function to return existing media file.
        /// </summary>
        /// <returns>Media file.</returns>
        private IList<IMedia> GetExistingMediaFiles(string ticketTemplatesFolderName)
        {
            IMedia uploadsMediaFolder = this.MediaService.GetRootMedia().Where(m => m.Name.Equals(ticketTemplatesFolderName)).FirstOrDefault();
            if (uploadsMediaFolder != null)
            {   
                if (uploadsMediaFolder.Children().Count() > 0)
                {
                    return uploadsMediaFolder.Children().ToList();
                }
            }

            return null;
        }
    }
}
