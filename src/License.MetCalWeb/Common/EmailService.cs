using System;
using System.Configuration;
using System.Net;
using System.Net.Mail;

namespace License.MetCalWeb.Common
{
    public class EmailService
    {
        public String UserName
        {
            get
            {
                return ConfigurationManager.AppSettings["UserName"].ToString();
            }
        }

        public String Password
        {
            get
            {
                return ConfigurationManager.AppSettings["Password"].ToString();
            }
        }

        public int Port
        {
            get
            {
                return Convert.ToInt32(ConfigurationManager.AppSettings["Port"]);
            }
        }

        public String Host
        {
            get
            {
                return ConfigurationManager.AppSettings["Host"].ToString();
            }
        }

        public void SendEmail(string toEmail, string subject, string body)
        {
            try
            {

                MailMessage msg = new MailMessage();
                msg.From = new MailAddress(UserName);
                msg.To.Add(toEmail);
                msg.Subject = subject;
                msg.Body = body;
                msg.IsBodyHtml = true;

                SmtpClient client = new SmtpClient();
                client.Host = Host;
                client.Port = Port;
                client.EnableSsl = true;
                client.UseDefaultCredentials = true;
                client.Credentials = new NetworkCredential(UserName, Password);
                client.Send(msg);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
