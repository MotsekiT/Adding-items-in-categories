using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;

namespace Firebase1.Repository.Utilities
{
    public static class Mail
    {
        public static Task SendEmail(MailMessage mailMessage)
        {
            // Credentials:
            var credentialUserName = mailMessage.To;
            var sentFrom = mailMessage.From;
            var pwd = "2022_SOD316C!";

            // Configure the client:
            System.Net.Mail.SmtpClient client =
                new System.Net.Mail.SmtpClient("smtp.gmail.com");

            client.Port = 587;
            client.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;

            // Creatte the credentials:
            System.Net.NetworkCredential credentials =
                new System.Net.NetworkCredential(credentialUserName.ToString(), pwd);

            client.EnableSsl = true;
            client.Credentials = credentials;

            // Create the message:
            var mail =
                new System.Net.Mail.MailMessage(mailMessage.From.ToString(), mailMessage.To.ToString());

            mail.From = new MailAddress(mailMessage.From.ToString(), "Organic Foods"); //Set display name of the user
            mail.Subject = mailMessage.Subject;
            mail.Body = mailMessage.Body;

            //Other parameters:
            mail.IsBodyHtml = true; //Do not forget to set this property to "true" if you use html in message body
            mail.Priority = MailPriority.Normal;
            mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
            mail.ReplyToList.Add("noreply_OrganicFoods@gmail.com");

            // Send:
            return client.SendMailAsync(mail);
        }
    }
}