using svcGangManagement.Models;
using svcGangManagement.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace svcGangManagement.Utility
{
    public class Email
    {
        protected string subject;
        protected string body = "";
        protected string recipient;
        protected string attachmentName;
        protected MemoryStream attachment = null;

        protected MailMessage mm = new MailMessage();
        protected SmtpClient smtp = new SmtpClient(Settings.Default.SMTPServer);

        protected string format = "PDF";

        private static readonly string LocalApplication = "svcGangManagement";

        public Email()
        {

        }

        public string Subject
        {
            get { return this.subject; }
            set
            {
                this.subject = value;
            }
        }

        public string Body
        {
            get
            {
                return this.body;
            }
            set
            {
                this.body = value;
            }
        }

        public string Recipient
        {
            get
            {
                return this.recipient;
            }
            set
            {
                this.recipient = value;
            }
        }

        public string AttachmentName
        {
            get
            {
                return attachmentName;
            }
            set
            {
                attachmentName = value;
            }
        }

        public MemoryStream Attachments
        {
            get
            {
                return attachment;
            }
        }

        public void Send()
        {
            try
            {
                mm.From = new MailAddress(Settings.Default.EngSysEmail, "Engineering Systems");

                mm.Bcc.Add(Settings.Default.EngSysEmail);
                mm.Subject = subject;
                mm.Body = body;
                mm.IsBodyHtml = true;

                if (recipient != "" && recipient != null)
                {
                    mm.To.Add(recipient.Replace(';', ','));
                }

                if (Settings.Default.OverrideEmail != string.Empty)
                {
                    mm = DirectEmail(mm);
                }

                if (attachment != null)
                {
                    mm.Attachments.Add(new Attachment(attachment, attachmentName));
                }

                if (!mm.To.Any())
                {
                    mm.To.Add(Settings.Default.AdminEmail.Replace(';', ','));
                }

                smtp.Send(mm);
            }
            catch (Exception e)
            {
                SendError(e);
                throw;
            }
        }

        public static void SendError(Exception ex)
        {
            string messages = ex.Message;
            Exception current = ex;
            while (current.InnerException != null)
            {
                current = current.InnerException;
                messages += " " + current.Message;
            }

            string subject = LocalApplication + " ERROR - " + ex.Message;

            string body = "";

            body = "<b>Message:</b> " + messages + "<br/>";
            body += "<b>Date:</b> " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + "<br/>";
            body += "<b>Application:</b> " + LocalApplication + "<br/>";
            body += "<b>Computer:</b> " + Environment.MachineName + "<br/>";
            body += "<b>Method:</b> " + ex.TargetSite.Name + "<br/>";
            body += "<b>Source:</b> " + ex.Source + "<br/>";
            body += "<b>User:</b> " + HttpContext.Current.Request.Headers["RacfId"] + "<br/>";
            body += "<br/>";
            body += "<b>Stack Trace:</b><br>";
            body += ex.StackTrace.Replace("\r\n", "<br/>");

            SendError(subject, body);
        }

        public static void SendError(Error_Log error)
        {
            string subject = error.App_Name + " ERROR - " + error.Error_Message;

            string body = "";

            body = "<b>Message:</b> " + error.Error_Message + "<br/>";
            body += "<b>Date:</b> " + error.Error_Date.ToShortDateString() + " " + error.Error_Time.ToShortTimeString() + "<br/>";
            body += "<b>Application:</b> " + error.App_Name + "<br/>";
            body += "<b>Computer:</b> " + error.Computer + "<br/>";
            body += "<b>Method:</b> " + error.Method + "<br/>";
            body += "<b>Source:</b> " + error.Source + "<br/>";
            body += "<b>User:</b> " + HttpContext.Current.Request.Headers["RacfId"] + "<br/>";
            body += "<br/>";
            body += "<b>Stack Trace:</b><br>";
            body += error.Stack_Trace.Replace("\r\n", "<br/>");

            SendError(subject, body);
        }

        private static bool SendError(string subject, string body)
        {
            string To = Settings.Default.AdminEmail;
            string CC = "";
            string BCC = Settings.Default.EngSysEmail;
            string ErrorStyle = "body{font-family:\"Consolas\",\"Verdana\";font-size:10pt;}";

            try
            {
                MailMessage _message = new MailMessage
                {
                    IsBodyHtml = true,
                    Sender = new MailAddress(Settings.Default.EngSysEmail),
                    From = new MailAddress(Settings.Default.EngSysEmail),
                    Subject = subject.Replace("\r\n", " "),
                };

                if (!string.IsNullOrEmpty(To))
                    AddAddresses(To, _message.To);
                if (!string.IsNullOrEmpty(CC))
                    AddAddresses(CC, _message.CC);
                if (!string.IsNullOrEmpty(BCC))
                    AddAddresses(BCC, _message.Bcc);

                _message.Body = "<html>";
                _message.Body += "<head><style>" + ErrorStyle + " </style></head>";
                _message.Body += "<body>" + body;

                if (!string.IsNullOrEmpty(Settings.Default.OverrideEmail))
                {
                    _message.To.Clear();
                    _message.Bcc.Clear();
                    _message.CC.Clear();
                    AddAddresses(Settings.Default.OverrideEmail, _message.To);

                    _message.Subject = "[FOR TESTING ONLY] " + _message.Subject;

                    _message.Body += "<br/><br/>This message would have been mailed";
                    if (!string.IsNullOrEmpty(To)) _message.Body += "<br/>To: " + To;
                    if (!string.IsNullOrEmpty(CC)) _message.Body += "<br/>CC: " + CC;
                    if (!string.IsNullOrEmpty(BCC)) _message.Body += "<br/>BCC: " + BCC;
                }
                else if (Settings.Default.AppLevel.ToUpper() != "P")
                {
                    _message.Subject = "[FOR TESTING ONLY] " + _message.Subject;
                }

                _message.Body += "</body></html>";

                SmtpClient _smtp = new SmtpClient(Settings.Default.SMTPServer, 25);
                _smtp.Send(_message);

                return true;
            }
            catch (SmtpException e)
            {
                Console.WriteLine("Email.Send Exception: " + e.Message);
                return false;
            }
        }

        private static void AddAddresses(string input, MailAddressCollection addresses)
        {
            input.Split(';').Where(x => !string.IsNullOrEmpty(x)).ToList().ForEach(x => addresses.Add(x.Trim()));
        }

        public MailMessage DirectEmail(MailMessage mm, bool error = false)
        {
            var settings = Settings.Default;
            string overrideEmail = settings.OverrideEmail;
            string appLevel = settings.AppLevel.ToUpper();

            string email = "";
            if (!string.IsNullOrEmpty(overrideEmail))
            {
                mm.To.Clear();
                mm.Bcc.Clear();
                mm.CC.Clear();
                mm.To.Add(overrideEmail);
                email = overrideEmail;
            }

            if (appLevel != "P")
            {
                mm.Subject = "[FOR TESTING ONLY] " + mm.Subject;
                mm.Body = "[For Testing Only] <br />" + mm.Body;

                if (overrideEmail != "")
                    mm.Body = "The Following Should be Emailed: <br />" + recipient + " <br />" + mm.Body;
            }

            return mm;
        }
    }
}