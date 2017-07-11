using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Net;
using System.Net.Mail;
using System.IO;
using System.Text;

namespace PredictionAPI.Services
{
    public class EmailService
    {
        public void SendMail(string userEmail,string code,string requestUrl)
        {
            //設定信件內容
            MailMessage mail = new MailMessage()
            {
                From = new MailAddress(@"chu.predict@gmail.com"),
                SubjectEncoding = Encoding.UTF8,
                BodyEncoding = Encoding.UTF8
                
            };
            mail.To.Add(userEmail);
            mail.Subject = "Email驗證信[此封信是由系統自動寄出，不須回覆]";
            mail.Body = GetEmailContent(userEmail,code,requestUrl);
            mail.IsBodyHtml = true;
            mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnSuccess;

            //設定SMTP
            SmtpClient smtp = new SmtpClient()
            {
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(@"chu.predict@gmail.com", "e215@dbLab"),
                Port = 587,
                Host = "smtp.gmail.com",
                EnableSsl = true
            };
            // await smtp.SendMailAsync(mail);
            try
            {
                smtp.Send(mail);
            }
            catch(SmtpException error)
            {

            }
        }

        private string GetEmailContent(string mail, string code, string url)
        {
            string content = File.ReadAllText(HttpContext.Current.Server.MapPath("~/EmailContentTemplate.html"));
            //string verifyUrl = url.Replace("SignUp", "EmailVerify?address=" + mail + "&authcode=" + code);
            content = content.Replace("{{vertifyURL}}", @"http://140.126.11.158/api/Account/EmailVerify?address=" + mail + "&authcode=" + code);
            return content;
        }
    }
}