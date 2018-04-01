using System.Collections.Generic;
using Umbraco.Core.Models;
using Umbraco.Web.Models;

namespace Cheapdrinks.Web.Extensions.ViewModels
{
    public class FaqRenderViewModel : RenderModel
    {
        public FaqRenderViewModel() : base(null)
        {

        }

        public IList<FaqModel> Faqs { get; set; }

        public NavigationViewModel NavigationViewModel { get; set; }

        public FaqRenderViewModel(IPublishedContent content) : base(content)
        {
        }
    }

    public class FaqViewModel
    {
        public IList<FaqModel> Faqs { get; set; }
    }

    public class FaqModel
    {
        public string Question { get; set; }

        public string Answer { get; set; }
    }
}
