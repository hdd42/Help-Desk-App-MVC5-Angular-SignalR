using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace HelpDeskTicketingApp.Models
{
    public static class EmailManager
    {
        private static readonly string From = "xxxxxxx@gmail.com";
        public static string To { get; set; }
        public static string Subject { get; set; }

        public static string Body { get; set; }

        private static SmtpClient _client;
        private static readonly string UserName = "xxxxxx@gmail.com";
        private static readonly string Password = "xxxxxxx";
        private static readonly int Port = 587;
        private static readonly string Host = "smtp.gmail.com";


         static EmailManager()
        {
            _client = new SmtpClient(Host, Port);
            _client.UseDefaultCredentials = false;
            _client.Credentials = new System.Net.NetworkCredential(UserName, Password);
            _client.EnableSsl = true;
        }

        public static bool SendEmail()
        {
            MailMessage mail = new MailMessage(From, To, Subject, Body);

            try
            {
                
                _client.Send(mail);
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }


        }

        public static bool SendEmailNewTicket(string email , string userName, string issueDesc)
        {
            Subject = "A New Ticket Has Been Created for your Issue!";
            Body = "Dear, "+userName;
            Body += ". A technichian will be asigned to you ticket as soon as possible. ";
            Body +="You will receive another e-mail once your issue is fixed!";
            Body += " Issue Description: " + issueDesc;


            if (email == null)
            {
                email = From;
            }
            MailMessage mail = new MailMessage(From, email, Subject, Body);

            try
            {
                _client.Send(mail);
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }


        }

        public static void SendEmailEditTicket(string _email, string _username, int issueId, string issueDesc)
        {
            Subject = "A New Ticket Has Been Created for your Issue! {" + issueId + "}";
            Body = "Dear, " + _username;
            Body += "A technichian will be asigned to you ticket as soon as possible ";
            Body += "You will receive another e-mail once your issue fixed!";
            Body += "Issue Description: " + issueDesc;


            if (_email == null)
            {
                _email = From;
            } 
            MailMessage mail = new MailMessage(From, _email, Subject, Body);

            try
            {
                _client.Send(mail);
                
            }
            catch (Exception ex)
            {

            }
        }

        internal static void SendReporterEmailAdminUpdateTicket(string issueId, string _username, string user_email, string issue_desc)
        {
            Subject = " Ticket Has Been Updated!";
            Body = "Dear, " + _username;
            Body += ". Your ticket has been updated, please login your account for more details.";
            Body += " Issue Description: " + issue_desc;

            if (user_email == null)
            {
                user_email = From;
            } 
            MailMessage mail = new MailMessage(From, user_email, Subject, Body);

            try
            {
                _client.Send(mail);

            }
            catch (Exception ex)
            {

            }
        }

        internal static void SendTechEmailAdminUpdateTicket(string issueId, string _techname, string tech_email, string issue_desc)
         {
            Subject = " An Assigned Ticket Has Been Updated!";
            Body = "Dear, " + _techname;
            Body += ". One of the tickets you are assigned to has been updated.";
            Body += " Issue Description: " + issue_desc;

            if (tech_email == null)
            {
                tech_email = From;
            }
            MailMessage mail = new MailMessage(From, tech_email, Subject, Body);

            try
            {
                _client.Send(mail);

            }
            catch (Exception ex)
            {

            }
        }

        internal static void SendEmailAdminAssignTicket(string issueId, string tech_email, string issue_desc)
        {
            Subject = " Ticket Has Been Assigned!";
            Body = "Dear, " + tech_email;
            Body += ". You have been assigned a ticket, please login to your account for more details.";
            Body += " Issue Description: " + issue_desc;

            if (tech_email == null)
            {
                tech_email = From;
            } 
            MailMessage mail = new MailMessage(From, tech_email, Subject, Body);

            try
            {
                _client.Send(mail);

            }
            catch (Exception ex)
            {

            }
        }

        public static bool SendEmailResolvedTicket(string email, string userName, int issueId)
        {
            Subject = "Your Issue has been Resolved!";
            Body = "Dear, " + userName;
            Body += ". The ticket you submited has been resolved, ";
            Body += "and your issue should be fixed!";

            if (email == null)
            {
                email = From;
            } 
            MailMessage mail = new MailMessage(From, email, Subject, Body);

            try
            {
                _client.Send(mail);
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }


        }
    }
}