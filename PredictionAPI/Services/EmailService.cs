using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Net;
using System.Net.Mail;
using System.IO;

namespace PredictionAPI.Services
{
    public class EmailService
    {
        public async Task SendAsync(string userEmail,string code,string requestUrl)
        {
            //設定信件內容
            MailMessage mail = new MailMessage()
            {
                From = new MailAddress("chu.predict@gmail.com")
            };
            mail.To.Add(userEmail);
            mail.Subject = "Email驗證信[此封信是由系統自動寄出，不須回覆]";
            mail.Body = GetEmailContent(userEmail,code,requestUrl);
            mail.IsBodyHtml = true;

            //設定SMTP
            SmtpClient smtp = new SmtpClient()
            {
                Host = "smtp.gmail.com",
                Port = 587,
                Credentials = new NetworkCredential("chu.predict@gmail.com", "e215@dbLab"),
                UseDefaultCredentials = false,
                EnableSsl = true
            };
            await smtp.SendMailAsync(mail);
        }

        private string GetEmailContent(string mail, string code, string url)
        {
            string content = File.ReadAllText(HttpContext.Current.Server.MapPath("~/EmailContentTemplate.html"));
            string verifyUrl = url.Replace("SignUpAsync", "EmailVerify?address=" + mail + "&authcode=" + code);
            content = content.Replace("{{vertifyURL}}", verifyUrl);
            return content;
        }
    }
}