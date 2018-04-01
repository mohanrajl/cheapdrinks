using System.Collections.Generic;
using Umbraco.Core.Models;
using Umbraco.Web.Models;

namespace Cheapdrinks.Web.Extensions.ViewModels
{
    public class HomePageRenderViewModel : RenderModel
    {
        public HomePageRenderViewModel() : base(null)
        {

        }

        public int MyTicketsCount { get; set; }

        public int AllTicketsCount { get; set; }

        public int TriageCount { get; set; }

        public string OrganisationName { get; set; }

        public NavigationViewModel NavigationViewModel { get; set; }
        
        public HomePageRenderViewModel(IPublishedContent content) : base(content)
        {
        }
    }

    public class HomePageViewModel
    {
    }
}
