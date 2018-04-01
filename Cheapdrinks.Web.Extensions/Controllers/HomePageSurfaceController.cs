using Cheapdrinks.Web.Extensions.Attribute;
using Umbraco.Core;
using Umbraco.Core.Services;

namespace Cheapdrinks.Web.Extensions.Controllers
{
    [CustomAuthorize]
    public class HomePageSurfaceController : BaseSurfaceController
    {
        private IMediaService MediaService
        {
            get
            {
                return ApplicationContext.Current.Services.MediaService;
            }
        }

    }
}
