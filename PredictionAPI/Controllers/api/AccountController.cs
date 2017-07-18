using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Security;
using System.Net.Http;
using System.Web.Http;
using PredictionAPI.Models;
using PredictionAPI.Services;
using PredictionAPI.Exception;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Collections.Specialized;

namespace PredictionAPI.Controllers.api
{
    public class AccountController : ApiController
    {
        private const string msg = "您的信箱尚未通過驗證，請到信箱收驗證信，驗證後再行登入";
        private DBOperationService db;
        private EmailService email;
        private const string redirectURL = @"http://predict.chu.edu.tw/2017/ast/login.html";
        private const string redirectURLForTest = @"http://localhost:53364/2017/ast/predict.html";

        [HttpGet]
        public IHttpActionResult EmailVerify(string address, string authcode)
        {
            db = new DBOperationService();
            db.UpdateUserInfo(address, authcode, "Y");
            return Redirect(redirectURLForTest);
            //MediaTypeFormatter tmp = new MediaTypeFormatter();
            //return Content(HttpStatusCode.OK, "<script></script>",);
        }

        [HttpPost]
        public HttpResponseMessage resendEmail(string address)
        {
            db = new DBOperationService();
            email = new EmailService();
            TopObject<string> result;
            HttpResponseMessage resp;
            var user = db.search(address);
            if (user.verificationCode != null)
            {
                email.SendMail(user.Email, user.verificationCode, Request.RequestUri.ToString());
                //丟訊息給前端
                result = new TopObject<string>()
                {
                    status = Convert.ToInt32(HttpStatusCode.OK),
                    input = address,
                    message = "已寄發驗證信至您的電子郵件信箱，請先驗證您的電子郵件信箱之後再使用本系統"
                };
                resp = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new ObjectContent<TopObject<string>>(result, new JsonMediaTypeFormatter())
                };
                return resp;
            }
            else
            {
                //
                 result = new TopObject<string>()
                {
                    status = Convert.ToInt32(HttpStatusCode.NotAcceptable),
                    input = address,
                    message = "您的信箱已驗證過了，請重新登入"
                };
                resp = new HttpResponseMessage(HttpStatusCode.NotAcceptable)
                {
                    Content = new ObjectContent<TopObject<string>>(result, new JsonMediaTypeFormatter())
                };
                return resp;
            }
        }

        [HttpPost]
        public HttpResponseMessage SignUp([FromBody] UserData userData)
        {
            //發信給使用者作信箱驗證
            email = new EmailService();
            TopObject<UserData> result;
            HttpResponseMessage reData;
            db = new DBOperationService();
            if (!db.isEmailExist(userData.email))
            {
                User data = new User()
                {
                    Email = userData.email,
                    location = userData.location,
                    schoolName = userData.schoolName,
                    identities = userData.identity,
                    verificationCode = Guid.NewGuid().ToString().Replace("-", string.Empty),
                    isPass = "N"
                };
                //在DB中新增一位使用者
                db.CreateUser(data);
                email.SendMail(data.Email, data.verificationCode, Request.RequestUri.ToString());
                //送出註冊成功的JSON 字串
                result = new TopObject<UserData>()
                {
                    status = Convert.ToInt32(HttpStatusCode.OK),
                    input = userData,
                    message = "已寄發驗證信至您的電子郵件信箱，請先驗證您的電子郵件信箱之後再使用本系統"
                };
                reData = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new ObjectContent<TopObject<UserData>>(result, new JsonMediaTypeFormatter())
                };
                return reData;
            }
            else
            {
                result = new TopObject<UserData>()
                {
                    status = Convert.ToInt32(HttpStatusCode.Created),
                    input = userData,
                    message = "此Email已經註冊過，請更換Email信箱"
                };
                reData = new HttpResponseMessage(HttpStatusCode.Created)
                {
                    Content = new ObjectContent<TopObject<UserData>>(result, new JsonMediaTypeFormatter())
                };
                return reData;
            }
        }

        [HttpPost]
        public HttpResponseMessage Login([FromBody]string email)
        {
            db = new DBOperationService();
            HttpResponseMessage result = null;
            try
            {
                var data = db.FindUser(email);
                var saveData = new NameValueCollection();
                saveData["email"] = data.Email;
                var cookie = new CookieHeaderValue("session", saveData)
                {
                    Domain = Request.RequestUri.Host,
                    Path = "/",
                    Expires = DateTime.Now.AddMinutes(60)
                };
                TopObject<string> returnData = new TopObject<string>()
                {
                    status = Convert.ToInt32(HttpStatusCode.OK),
                    input = email,
                    message = "登入成功"
                };
                //傳cookie+登入成功訊息
                result = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new ObjectContent<TopObject<string>>(returnData, new JsonMediaTypeFormatter())                      
                };
                result.Headers.AddCookies(new CookieHeaderValue[] { cookie });
                return result;
            }
            catch(EmailNotVertifyException ex)
            {
                TopObject<string> mesg = new TopObject<string>()
                {
                    input = email,
                    status = Convert.ToInt32(HttpStatusCode.Unauthorized),
                    message = msg
                };
                result = new HttpResponseMessage(HttpStatusCode.Unauthorized)
                {
                    Content = new ObjectContent<TopObject<string>>(mesg, new JsonMediaTypeFormatter())
                };
                return result;
            }
            catch(UserNotFoundException ex)
            {
                TopObject<string> obj = new TopObject<string>()
                {
                    status = Convert.ToInt32(HttpStatusCode.NotFound),
                    input = email,
                    message = "沒有這位使用者或Email輸入錯誤，請先註冊再登入"
                };
                //送註冊JSON訊息
                var errorData = new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new ObjectContent<TopObject<string>>(obj,new JsonMediaTypeFormatter())
                };
                return errorData;
            }
        }
    }
}
