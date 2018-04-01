using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Web.Models;

namespace Cheapdrinks.Web.Extensions.ViewModels
{
    public class RegisterDetailsRenderViewModel : RenderModel
    {
        public RegisterDetailsModel RegisterDetails { get; set; }

        //public RegisterDetailsRenderViewModel() : this(UmbracoContext.Current.PublishedContentRequest.PublishedContent) { }

        //// Main constructor.
        public RegisterDetailsRenderViewModel(IPublishedContent content) : base(content) { }

        //public RegisterDetailsRenderViewModel(IPublishedContent content) : base(content)
        //{

        //}

        //public RegisterDetailsRenderViewModel() : base(UmbracoContext.Current.PublishedContentRequest.PublishedContent)
        //{

        //}
    }

    public class RegisterDetailsViewModel
    {
        public RegisterDetailsModel RegisterDetails { get; set; }
    }

    public class RegisterDetailsModel
    {
        public string OrganisationCode { get; set; }

        public string ActivationCode { get; set; }

        public string OrganisationName { get; set; }

        public string OrganisationAddress { get; set; }

        public Guid RegistrationGuid { get; set; }

        [Required]
        public string CoordinatorForename { get; set; }

        [Required]
        public string CoordinatorSurname { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string CoordinatorEmail { get; set; }

        [Required]
        public string CoordinatorTelephone { get; set; }

        public bool IsSubmitted { get; set; }

        public DateTime? DateSubmitted { get; set; }

        [Required]
        public string HeadForename { get; set; }

        [Required]
        public string HeadSurname { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string HeadEmail { get; set; }

        [Required]
        public string HeadTelephone { get; set; }
    }
}
