using System;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using Cheapdrinks.Web.Extensions.Utilities;
using Cheapdrinks.Web.Extensions.ViewModels;
using Umbraco.Core;
using Umbraco.Core.Auditing;
using Umbraco.Web;
using Umbraco.Web.Models;

namespace Cheapdrinks.Web.Extensions.Controllers
{
    public class AuthenticationSurfaceController : BaseSurfaceController
    {
        public const string PasswordResetMemberPropertyName = "passwordResetGUID";
        public const string NewUserPropertyAlias = "newUser";
        public const string PasswordResetEmailQuerystringKey = "g";
        private readonly String EnglishEN = "en-GB";
        private static UmbracoHelper UmbracoHelper
        {
            get
            {
                return new UmbracoHelper(UmbracoContext.Current);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login([Bind(Prefix = "loginModel")]LoginModel model, string culture)
        {
            bool isNewMember = false;            

            if (ModelState.IsValid == false)
            {
                return CurrentUmbracoPage();
            }

            var memberService = ApplicationContext.Current.Services.MemberService;
            var memberItem = memberService.GetByEmail(model.Username);
            if (memberItem == null || string.IsNullOrEmpty(memberItem.RawPasswordValue) || Members.Login(model.Username, model.Password) == false)                
            {
                if (memberItem != null && memberItem.IsLockedOut)
                {
                    ModelState.AddModelError(culture.Equals(EnglishEN) ? "loginModelen" : "loginModelcy", this.CurrentPage.GetPropertyValue("userLockedOutText").ToString());
                    return CurrentUmbracoPage();
                }

                //don't add a field level error, just model level
                ModelState.AddModelError(culture.Equals(EnglishEN) ? "loginModelen" : "loginModelcy", this.CurrentPage.GetPropertyValue("invalidCredentialsText").ToString());                
                return CurrentUmbracoPage();
            }

            TempData["LoginSuccess"] = true;  

            if (memberItem != null)
            {                
                isNewMember = int.Parse(memberItem.Properties[NewUserPropertyAlias].Value.ToString()) == 1;
                // Set culture based on user chosen language
                System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo(culture);
                System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo(culture);
            }            

            if (isNewMember)
            {
                // Set Newuser value to 0 - Not a new user anymore.
                memberItem.Properties[NewUserPropertyAlias].Value = 0;
                memberService.Save(memberItem);
            }         

            //if there is a specified path to redirect to then use it
            if (model.RedirectUrl.IsNullOrWhiteSpace() == false)
            {
                return Redirect(model.RedirectUrl);
            }

            //redirect to home page            
            return Redirect(Navigation.GetHomePageUrl(this.CurrentPage));

        }

        [HttpPost]
        public ActionResult Logout()
        {
            Session.Clear();
            FormsAuthentication.SignOut();
            return Redirect("/");            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ForgottenPassword([Bind(Prefix = "forgotPasswordViewModelEnglish")]ForgotPasswordViewModel forgotPasswordViewModelEnglish, [Bind(Prefix = "forgotPasswordViewModelWelsh")]ForgotPasswordViewModel forgotPasswordViewModelWelsh, string culture)
        {
            if (!ModelState.IsValid)
            {
                return CurrentUmbracoPage();
            }

            var model = (forgotPasswordViewModelEnglish != null && forgotPasswordViewModelEnglish.EmailAddress != null) ? forgotPasswordViewModelEnglish : forgotPasswordViewModelWelsh;

            var memberService = ApplicationContext.Current.Services.MemberService;

            //Find the member with the email address
            var member = memberService.GetByEmail(model.EmailAddress);

            if (member != null)
            {
                //We found the member with that email
                //Set expiry date to 
                var expiryTime = DateTime.Now.AddHours(48);

                //Lets update resetGUID property
                member.Properties[PasswordResetMemberPropertyName].Value = expiryTime.ToString("ddMMyyyyHHmmssFFFF");
                
                var isNewMember = int.Parse(member.Properties[NewUserPropertyAlias].Value.ToString()) == 1;

                //Save the member with the updated property value
                memberService.Save(member);

                //Send user an email to reset password with GUID in it                
                new EmailUtility(this.CurrentPage).SendResetPasswordEmail(member.Email, expiryTime.ToString("ddMMyyyyHHmmssFFFF"), isNewMember);

                // Redirect to new user success page when new user                
                if (this.CurrentPage.Children != null && this.CurrentPage.Children.Count() > 0)
                {                        
                    return Redirect(Navigation.GetStatusPageUrl(this.CurrentPage, culture.Substring(0, 2)));                    
                }                
            }
            else
            {
                ModelState.AddModelError(culture.Equals(EnglishEN) ? "forgotPassworden" : "forgotPasswordcy", this.CurrentPage.GetPropertyValue("noMemberFoundText").ToString());                
                return CurrentUmbracoPage();
            }

            return CurrentUmbracoPage();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword([Bind(Prefix = "resetPasswordViewModelEnglish")]ResetPasswordViewModel resetPasswordViewModelEnglish, [Bind(Prefix = "resetPasswordViewModelWelsh")]ResetPasswordViewModel resetPasswordViewModelWelsh, string culture)
        {
            if (!ModelState.IsValid)
            {
                return CurrentUmbracoPage();
            }

            var model = (resetPasswordViewModelEnglish != null && resetPasswordViewModelEnglish.EmailAddress != null) ? resetPasswordViewModelEnglish : resetPasswordViewModelWelsh;

            //Get member from email
            var memberService = ApplicationContext.Current.Services.MemberService;

            //Find the member with the email address
            var member = memberService.GetByEmail(model.EmailAddress);

            //Ensure we have that member
            if (member == null) return CurrentUmbracoPage();

            //Get the querystring GUID
            var resetKey = Request.QueryString[PasswordResetEmailQuerystringKey];

            //Ensure we have a vlaue in QS
            if (!string.IsNullOrEmpty(resetKey))
            {
                //See if the QS matches the value on the member property
                if (member.Properties[PasswordResetMemberPropertyName].Value != null && (member.Properties[PasswordResetMemberPropertyName].Value.ToString() == resetKey))
                { 
                    //Got a match, now check to see if the 15min window hasnt expired
                    var expiryTime = DateTime.ParseExact(resetKey, "ddMMyyyyHHmmssFFFF", null);

                    //Check the current time is less than the expiry time
                    var currentTime = DateTime.Now;

                    //Check if date has NOT expired (been and gone)
                    if (currentTime.CompareTo(expiryTime) < 0)
                    {
                        if (model.Password.Length < 8)
                        {
                            //ERROR: Minimum password length check
                            ModelState.AddModelError(culture.Equals(EnglishEN) ? "resetPassworden" : "resetPasswordcy", this.CurrentPage.GetPropertyValue("passwordLengthText").ToString());
                            return CurrentUmbracoPage();
                        }

                        //Got a match, we can allow user to update password
                        memberService.SavePassword(member, model.Password);

                        //Remove the resetGUID value
                        member.Properties[PasswordResetMemberPropertyName].Value = string.Empty;

                        //Save the member
                        memberService.Save(member);

                        // Redirect to new user success page when new user                
                        if (this.CurrentPage.Children != null && this.CurrentPage.Children.Count() > 0)
                        {
                            return Redirect(Navigation.GetStatusPageUrl(this.CurrentPage, culture.Substring(0, 2)));
                        }

                        return Redirect(Navigation.GetLoginPageUrl());
                    }

                    //ERROR: Reset GUID has expired                    
                    ModelState.AddModelError(culture.Equals(EnglishEN) ? "resetPassworden" : "resetPasswordcy", this.CurrentPage.GetPropertyValue("resetGUIDText").ToString());                    
                    return CurrentUmbracoPage();
                }

                //ERROR: QS does not match what is stored on member property
                //Invalid GUID
                ModelState.AddModelError(culture.Equals(EnglishEN) ? "resetPassworden" : "resetPasswordcy", this.CurrentPage.GetPropertyValue("invalidGUIDText").ToString());                
                return CurrentUmbracoPage();
            }

            //ERROR: No QS present
            //Invalid GUID
            ModelState.AddModelError(culture.Equals(EnglishEN) ? "resetPassworden" : "resetPasswordcy", this.CurrentPage.GetPropertyValue("invalidGUIDText").ToString());            
            return CurrentUmbracoPage();
        }

        [HttpPost]
        public ActionResult RedirectToLogin()
        {
            return Redirect(Navigation.GetLoginPageUrl());
        }
    }
}
