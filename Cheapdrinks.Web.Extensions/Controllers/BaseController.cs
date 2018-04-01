using System.Threading.Tasks;
using Umbraco.Web.Mvc;

namespace Cheapdrinks.Web.Extensions.Controllers
{
    public abstract class BaseController : RenderMvcController
    {
        public string OrganisationName { get; set; }
    }

    public abstract class BaseSurfaceController : SurfaceController
    {
        
    }
}
