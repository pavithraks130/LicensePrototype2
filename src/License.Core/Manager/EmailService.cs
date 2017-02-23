using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace License.Core.Manager
{
    class EmailService :IIdentityMessageService
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

        public Task SendAsync(IdentityMessage message)
        {
            return configSendGridasync(message);
        }

        private Task configSendGridasync(IdentityMessage message)
        {
            try
            {

                MailMessage msg = new MailMessage();

                msg.From = new MailAddress(UserName);

                msg.To.Add(message.Destination);

                //msg.Priority = Priority;
                msg.Subject = message.Subject;
                msg.Body = message.Body;
                msg.IsBodyHtml = true;

                SmtpClient client = new SmtpClient();
                client.Host = Host;
                client.Port = Port;
                //  client.Timeout = 30;
                client.EnableSsl = true;
                client.UseDefaultCredentials = true;
                client.Credentials = new NetworkCredential(UserName, Password);

                client.Send(msg);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Task.FromResult(0);
        }
    }
}
