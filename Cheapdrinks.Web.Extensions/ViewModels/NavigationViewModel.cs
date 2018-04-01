using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace Cheapdrinks.Web.Extensions.ViewModels
{
    public class NavigationViewModel
    {
        private UmbracoHelper UmbracoHelper
        {
            get
            {
                return new UmbracoHelper(UmbracoContext.Current);
            }
        }        

        public CultureInfo CurrentLanguage { get; set; }

        public string CurrenPageUrl { get; set; }

        public IList<CultureInfo> Languages { get; set; }

        public PageInformation HomePageInformation { get; set; }

        public PageInformation CreateTicketPageInformation { get; set; }

        public PageInformation MyTicketsPageInformation { get; set; }

        public PageInformation AllTicketsPageInformation { get; set; }

        public PageInformation TriagePageInformation { get; set; }

        public PageInformation FaqPageInformation { get; set; }

        public NavigationViewModel(IPublishedContent content)
        {
            this.CurrenPageUrl = HttpContext.Current.Request.Path; 
            var languages = ApplicationContext.Current.Services.LocalizationService.GetAllLanguages();            
            if (languages != null && languages.Count() > 0)
            {
                if (languages.Any(x => x.CultureInfo.DisplayName.Equals(System.Threading.Thread.CurrentThread.CurrentCulture.DisplayName)))
                {
                    this.CurrentLanguage = languages.FirstOrDefault(x => x.CultureInfo.DisplayName.Equals(System.Threading.Thread.CurrentThread.CurrentCulture.DisplayName)).CultureInfo;
                }

                this.Languages = new List<CultureInfo>();
                foreach (var language in languages.Where(x => !x.CultureInfo.DisplayName.Equals(System.Threading.Thread.CurrentThread.CurrentCulture.DisplayName)))
                {
                    this.Languages.Add(language.CultureInfo);
                }
            }

            if (content != null)
            {
                // Home Page
                this.HomePageInformation = new PageInformation();
                if (!string.IsNullOrWhiteSpace(content.DocumentTypeAlias))
                {
                    HomePageInformation.Alias = content.DocumentTypeAlias;
                }

                if (content.GetProperty("homePage", true) != null)
                {
                    HomePageInformation.Url = UmbracoHelper.NiceUrl(int.Parse(content.GetProperty("homePage", true).Value.ToString()));
                }

                if (content.GetPropertyValue("homeMenu") != null)
                {
                    HomePageInformation.Name = content.GetPropertyValue("homeMenu").ToString();
                }

                // Create Page
                this.CreateTicketPageInformation = new PageInformation();
                if (content.GetProperty("createTicketPage", true) != null)
                {
                    CreateTicketPageInformation.Url = UmbracoHelper.NiceUrl(int.Parse(content.GetProperty("createTicketPage", true).Value.ToString()));
                }

                if (content.GetPropertyValue("createTicket") != null)
                {
                    CreateTicketPageInformation.Name = content.GetPropertyValue("createTicket").ToString();
                }

                // My Tickets Page
                this.MyTicketsPageInformation = new PageInformation();
                if (content.GetProperty("myTicketsPage", true) != null)
                {
                    MyTicketsPageInformation.Url = UmbracoHelper.NiceUrl(int.Parse(content.GetProperty("myTicketsPage", true).Value.ToString()));
                }

                if (content.GetPropertyValue("myTickets") != null)
                {
                    MyTicketsPageInformation.Name = content.GetPropertyValue("myTickets").ToString();
                }

                // All Tickets Page
                this.AllTicketsPageInformation = new PageInformation();
                if (content.GetProperty("allTicketsPage", true) != null)
                {
                    AllTicketsPageInformation.Url = UmbracoHelper.NiceUrl(int.Parse(content.GetProperty("allTicketsPage", true).Value.ToString()));
                }

                if (content.GetPropertyValue("allTickets") != null)
                {
                    AllTicketsPageInformation.Name = content.GetPropertyValue("allTickets").ToString();
                }

                // Triage Page
                this.TriagePageInformation = new PageInformation();
                if (content.GetProperty("triagePage", true) != null)
                {
                    TriagePageInformation.Url = UmbracoHelper.NiceUrl(int.Parse(content.GetProperty("triagePage", true).Value.ToString()));
                }

                if (content.GetPropertyValue("triage") != null)
                {
                    TriagePageInformation.Name = content.GetPropertyValue("triage").ToString();
                }

                // Faq Page
                this.FaqPageInformation = new PageInformation();
                if (content.GetProperty("faqPage", true) != null)
                {
                    FaqPageInformation.Url = UmbracoHelper.NiceUrl(int.Parse(content.GetProperty("faqPage", true).Value.ToString()));
                }

                if (content.GetPropertyValue("faq") != null)
                {
                    FaqPageInformation.Name = content.GetPropertyValue("faq").ToString();
                }
            }
        }
    }

    public class PageInformation
    {
        public string Name { get; set; }

        public string Url { get; set; }

        public string Alias { get; set; }
    }
}
