using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Net;
using System.Net.Mail;
using System.IO;
using System.Text;
using PredictionAPI.Exception;

namespace PredictionAPI.Services
{
    public class EmailService
    {
        public EmailService()
        {

        }
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
            mail.Subject = "升大學職涯型落點分析Email驗證信[此封信是由系統自動寄出，不須回覆]";
            mail.Body = GetEmailContent(userEmail,code,requestUrl);
            mail.IsBodyHtml = true;
            
            //設定SMTP
            SmtpClient smtp = new SmtpClient()
            {
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(@"chu.predict@gmail.com", "e215@dbLab"),
                Port = 587,
                Host = "smtp.gmail.com",
                EnableSsl = true
            };
            smtp.Send(mail);
        }

        private string GetEmailContent(string mail, string code, string url)
        {
            const string verifyURL = @"http://predict.chu.edu.tw/2017/ast/api/Account/EmailVerify?address=";
            //const string verifyURLForTest = @"http://140.126.11.158/2017/ast/api/Account/EmailVerify?address=";
            string content = File.ReadAllText(HttpContext.Current.Server.MapPath("~/EmailContentTemplate.html"));
            //content = content.Replace("{{vertifyURL}}", verifyURL + mail + "&authcode=" + code);
            content = content.Replace("{{vertifyURL}}", verifyURL + mail + "&authcode=" + code);
            return content;
        }
    }
}