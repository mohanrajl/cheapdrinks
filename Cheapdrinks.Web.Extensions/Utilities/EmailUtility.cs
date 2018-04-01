using System.Net.Mail;
using System.Web;
using Umbraco.Core.Models;
using System.Collections.Generic;
using System;
using Cheapdrinks.Web.Extensions.Controllers;
using Umbraco.Web;

namespace Cheapdrinks.Web.Extensions.Utilities
{
    public class EmailUtility
    {
        private IPublishedContent _currentPage;

        private string FromEmailAddress
        {
            get
            {
                return this._currentPage.GetProperty("fromEmailAddress", true).Value.ToString();
            }
        }

        private string Subject
        {
            get
            {
                return this._currentPage.GetProperty("subject").Value.ToString();                
            }
        }

        private string Body
        {
            get
            {
                return this._currentPage.GetProperty("body").Value.ToString();                
            }
        }

        public EmailUtility(IPublishedContent currentPage)
        {
            this._currentPage = currentPage;
        }

        public void SendResetPasswordEmail(string memberEmail, string resetGuid, bool isNewMember)
        {
            var mailMessage = new MailMessage
            {
                From = new MailAddress(this.FromEmailAddress)
            };

            //Send to the member's email address
            mailMessage.To.Add(memberEmail);

            //Reset link
            var baseUrl =
                HttpContext.Current.Request.Url.AbsoluteUri.Replace(HttpContext.Current.Request.Url.AbsolutePath,
                    string.Empty);
            var resetUrl = baseUrl + $"/reset-password?{AuthenticationSurfaceController.PasswordResetEmailQuerystringKey}={resetGuid}";

            //HTML Message
            mailMessage.IsBodyHtml = true;
            mailMessage.Subject = this.Subject;
            mailMessage.Body = this.Body.Replace("/resetUrl", resetUrl);

            // Credentials in web.config
            var client = new SmtpClient();
            client.Send(mailMessage);
        }

        public void SendCreateEmail(string memberEmail)
        {
            var mailMessage = new MailMessage
            {
                From = new MailAddress(this.FromEmailAddress)
            };

            //Send to the member's email address
            mailMessage.To.Add(memberEmail);
            
            //HTML Message
            mailMessage.IsBodyHtml = true;
            var subject = this._currentPage.GetProperty("subject").Value.ToString();
            var body = this._currentPage.GetProperty("body").Value.ToString();
            //if (ticket != null)
            //{
            //    subject = subject.Replace("{TicketReference}", ticket.TicketId.ToString());
            //    body = body.Replace("{TicketReference}", ticket.TicketId.ToString());
            //    body = body.Replace("{TicketTitle}", ticket.Title.ToString());
            //    body = body.Replace("{TicketDate}", ticket.LoggedDate.ToString());
            //    body = body.Replace("{TicketActionType}", ticket.TicketTypeName.ToString());
            //}

            mailMessage.Subject = subject;
            mailMessage.Body = body;

            // Credentials in web.config
            var client = new SmtpClient();
            client.Send(mailMessage);
        }

        public void SendUploadEmail(string memberEmail, IList<string> copiedMemberEmailAddress)
        {
            var mailMessage = new MailMessage
            {
                From = new MailAddress(this.FromEmailAddress)
            };

            //Send to the member's email address
            mailMessage.To.Add(memberEmail);

            if (copiedMemberEmailAddress != null && copiedMemberEmailAddress.Count > 0)
            {
                foreach (var item in copiedMemberEmailAddress)
                {
                    mailMessage.CC.Add(item);
                }                
            }

            //HTML Message
            mailMessage.IsBodyHtml = true;
            var subject = this._currentPage.GetProperty("uploadSubject").Value.ToString();
            var body = this._currentPage.GetProperty("uploadBody").Value.ToString();
            //if (ticketViewModel != null)
            //{
            //    subject = subject.Replace("{TicketReference}", ticketViewModel.TicketId.ToString());
            //    body = body.Replace("{TicketReference}", ticketViewModel.TicketId.ToString());
            //    body = body.Replace("{TicketTitle}", ticketViewModel.Title.ToString());
            //    body = body.Replace("{TicketDate}", ticketViewModel.LoggedDate.ToString());
            //    body = body.Replace("{TicketActionType}", ticketViewModel.TicketTypeName.ToString());
            //}

            mailMessage.Subject = subject;
            mailMessage.Body = body;

            // Credentials in web.config
            var client = new SmtpClient();
            client.Send(mailMessage);
        }

        public void SendUpdateEmail(string memberEmail, IList<string> copiedMemberEmailAddress, int actionType)
        {
            var mailMessage = new MailMessage
            {
                From = new MailAddress(this.FromEmailAddress)
            };

            //Send to the member's email address
            mailMessage.To.Add(memberEmail);

            if (copiedMemberEmailAddress != null && copiedMemberEmailAddress.Count > 0)
            {
                foreach (var item in copiedMemberEmailAddress)
                {
                    mailMessage.CC.Add(item);
                }
            }

            //HTML Message
            mailMessage.IsBodyHtml = true;
            var subject = string.Empty;
            var body = string.Empty;
            if (actionType == 1)
            {
                subject = this._currentPage.GetProperty("fileNoteSubject").Value.ToString();
                body = this._currentPage.GetProperty("fileNoteBody").Value.ToString();
            }
            else if (actionType == 2)
            {
                subject = this._currentPage.GetProperty("updateSubject").Value.ToString();
                body = this._currentPage.GetProperty("updateBody").Value.ToString();
            }
            else if (actionType == 3)
            {
                subject = this._currentPage.GetProperty("resolveSubject").Value.ToString();
                body = this._currentPage.GetProperty("resolveBody").Value.ToString();
            }

            //if (ticketViewModel != null)
            //{
            //    subject = subject.Replace("{TicketReference}", ticketViewModel.TicketId.ToString());
            //    body = body.Replace("{TicketReference}", ticketViewModel.TicketId.ToString());
            //    body = body.Replace("{TicketTitle}", ticketViewModel.Title.ToString());
            //    body = body.Replace("{TicketDate}", ticketViewModel.LoggedDate.ToString());
            //    body = body.Replace("{TicketActionType}", ticketViewModel.TicketTypeName.ToString());
            //}

            mailMessage.Subject = subject;
            mailMessage.Body = body;

            // Credentials in web.config
            var client = new SmtpClient();
            client.Send(mailMessage);
        }

        public void SendUpdateOwnerEmail(string memberEmail, object copyEmails)
        {
            var mailMessage = new MailMessage
            {
                From = new MailAddress(this.FromEmailAddress)
            };

            //Send to the member's email address
            mailMessage.To.Add(memberEmail);

            if (copyEmails != null)
            {
                var copyEmailList = (List<string>)copyEmails;
                if (copyEmailList != null && copyEmailList.Count > 0)
                {
                    foreach (var item in copyEmailList)
                    {
                        mailMessage.CC.Add(item);
                    }
                }
            }

            //HTML Message
            mailMessage.IsBodyHtml = true;
            var subject = this._currentPage.GetProperty("ownerSubject").Value.ToString();
            var body = this._currentPage.GetProperty("ownerBody").Value.ToString();
            //if (ticketViewModel != null)
            //{
            //    subject = subject.Replace("{TicketReference}", ticketViewModel.TicketId.ToString());
            //    body = body.Replace("{TicketReference}", ticketViewModel.TicketId.ToString());
            //    body = body.Replace("{TicketTitle}", ticketViewModel.Title.ToString());
            //    body = body.Replace("{TicketDate}", ticketViewModel.LoggedDate.ToString());
            //    body = body.Replace("{TicketActionType}", ticketViewModel.TicketTypeName.ToString());
            //}

            mailMessage.Subject = subject;
            mailMessage.Body = body;

            // Credentials in web.config
            var client = new SmtpClient();
            client.Send(mailMessage);
        }

        public void SendEmail(string memberEmail, IList<string> copyToEmails, string subjectAlias, string bodyAlias)
        {
            var mailMessage = new MailMessage
            {
                From = new MailAddress(this.FromEmailAddress)
            };

            //Send to the member's email address
            mailMessage.To.Add(memberEmail);

            if (copyToEmails != null && copyToEmails.Count > 0)
            {
                foreach (var item in copyToEmails)
                {
                    mailMessage.Bcc.Add(item);
                }
            }

            //HTML Message
            mailMessage.IsBodyHtml = true;
            mailMessage.Subject = this._currentPage.GetPropertyValue(subjectAlias).ToString();
            mailMessage.Body = this._currentPage.GetPropertyValue(bodyAlias).ToString();

            // Credentials in web.config
            var client = new SmtpClient();
            client.Send(mailMessage);
        }
    }
}
