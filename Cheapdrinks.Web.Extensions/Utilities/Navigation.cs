using System.Linq;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace Cheapdrinks.Web.Extensions.Utilities
{
    public static class Navigation
    {
        public static string TwoLetterISOLanguageName
        {
            get
            {
                return System.Threading.Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName;
            }            
        }

        public static UmbracoHelper UmbracoHelper
        {
            get
            {
                return new UmbracoHelper(UmbracoContext.Current);
            }
        }

        public static string GetLoginPageUrl()
        {
            return GetCultureUrl(UmbracoHelper.TypedContentAtRoot().First().Id, TwoLetterISOLanguageName);
        }

        public static string GetHomePageUrl(IPublishedContent currentPage)
        {
            var homePage = currentPage.GetProperty("homePage", true);                            
            if (homePage.HasValue)
            {
                return GetCultureUrl(int.Parse(homePage.Value.ToString()), TwoLetterISOLanguageName);
            }

            return GetCultureUrl(UmbracoHelper.TypedContentAtRoot().First().Id, TwoLetterISOLanguageName);
        }

        public static string GetCreateTicketPageUrl(IPublishedContent currentPage)
        {
            var createTicketPage = currentPage.GetProperty("createTicketPage", true);            
            if (createTicketPage.HasValue)
            {
                return GetCultureUrl(int.Parse(createTicketPage.Value.ToString()), TwoLetterISOLanguageName);
            }

            return GetCultureUrl(UmbracoHelper.TypedContentAtRoot().First().Id, TwoLetterISOLanguageName);
        }

        public static string GetEditTicketPageUrl(IPublishedContent currentPage)
        {
            var editTicketPage = currentPage.GetProperty("editTicketPage", true);
            if (editTicketPage.HasValue)
            {
                return GetCultureUrl(int.Parse(editTicketPage.Value.ToString()), TwoLetterISOLanguageName);
            }

            return GetCultureUrl(UmbracoHelper.TypedContentAtRoot().First().Id, TwoLetterISOLanguageName);
        }

        public static string GetAccessDeniedPageUrl(IPublishedContent currentPage)
        {
            var accessDeniedPage = currentPage.GetProperty("accessDeniedPage", true);
            if (accessDeniedPage.HasValue)
            {
                return GetCultureUrl(int.Parse(accessDeniedPage.Value.ToString()), TwoLetterISOLanguageName);
            }

            return GetCultureUrl(UmbracoHelper.TypedContentAtRoot().First().Id, TwoLetterISOLanguageName);
        }

        public static string GetStatusPageUrl(IPublishedContent currentPage, string culture)
        {            
            if (currentPage.Children != null && currentPage.Children.Count() > 0)
            {
                return GetCultureUrl(currentPage.Children.FirstOrDefault().Id, culture);
            }

            return GetCultureUrl(UmbracoHelper.TypedContentAtRoot().First().Id, TwoLetterISOLanguageName);
        }

        public static string GetCultureUrl(int nodeId, string language = null)
        {
            if (string.IsNullOrEmpty(language))
            {
                return UmbracoHelper.NiceUrl(nodeId);
            }
            else
            {
                var url = UmbracoHelper.NiceUrl(nodeId);
                // TODO: No hardcode culture names
                return url.Contains("/" + language + "/") ? url : url.Replace("/en/", "/" + language + "/");
            }
        }
    }
}
