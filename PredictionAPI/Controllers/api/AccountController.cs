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
        private const string msg = "您的信箱讓未通過驗證，請到信箱收信";
        private DBOperationService db;
        private EmailService email;
        private const string redirectURL = @"http://predict.chu.edu.tw/2017/ast/login.html";
        private const string redirectURLForTest = @"http://140.126.11.158/2017/ast/login.html";

        [HttpGet]
        public IHttpActionResult EmailVerify(string address,string authcode)
        {
            db = new DBOperationService();
            db.UpdateUserInfo(address, authcode, "Y");
            return Redirect(redirectURLForTest);
        }

        [HttpPost]
        public HttpResponseMessage SignUp([FromBody] UserData userData)
        {
            //在DB中新增一位使用者
            db = new DBOperationService();
            User data = new User()
            {
                Email = userData.email,
                location = userData.location,
                schoolName = userData.schoolName,
                identities = userData.identity,
                verificationCode = Guid.NewGuid().ToString().Trim('-'),
                isPass = "N"
            };
            //發信給使用者作信箱驗證
            email = new EmailService();
            email.SendMail(data.Email, data.verificationCode, Request.RequestUri.ToString());
            if(ModelState.IsValid)
            {
                db.CreateUser(data);
            }
            //送出註冊成功的JSON 字串
            TopObject<UserData> result = new TopObject<UserData>()
            {
                status = Convert.ToInt32(HttpStatusCode.OK),
                input = userData,
                Messege = "已寄發驗證信至您的電子郵件信箱，請先驗證您的電子郵件信箱之後再使用本系統"
            };
            var reData = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = new ObjectContent<TopObject<UserData>>(result,new JsonMediaTypeFormatter())
            };
            return reData;
        }

        [HttpPost]
        public HttpResponseMessage Login([FromBody]string email)
        {
            db = new DBOperationService();
            try
            {
                var data = db.FindUser(email);
                HttpResponseMessage result = null;
                if (data.isPass != "N")
                {
                    var saveData = new NameValueCollection();
                    saveData["email"] = data.Email;
                    var cookie = new CookieHeaderValue("session", saveData)
                    {
                        Domain = Request.RequestUri.Host,
                        Path = "/",
                        Expires = DateTime.Now.AddDays(1),
                        HttpOnly = true,
                    };
                    TopObject<string> returnData = new TopObject<string>()
                    {
                        status = Convert.ToInt32(HttpStatusCode.OK),
                        input = email,
                        Messege = "登入成功"
                    };
                    //傳cookie+登入成功訊息
                    result = new HttpResponseMessage()
                    {
                        Content = new ObjectContent<TopObject<string>>(returnData, new JsonMediaTypeFormatter()),
                        StatusCode = HttpStatusCode.OK                      
                    };
                    result.Headers.AddCookies(new CookieHeaderValue[] { cookie });
                    return result;
                }
                else
                {
                    TopObject<string> mesg = new TopObject<string>()
                    {
                        input = email,
                        status = Convert.ToInt32(HttpStatusCode.Unauthorized),
                        Messege = msg
                    };
                    result = new HttpResponseMessage()
                    {
                        Content = new ObjectContent<TopObject<string>>(mesg, new JsonMediaTypeFormatter()),
                        StatusCode = HttpStatusCode.Unauthorized,
                    };
                    return result;
                }
            }
            catch(UserNotFoundException ex)
            {
                TopObject<string> obj = new TopObject<string>()
                {
                    status = Convert.ToInt32(HttpStatusCode.NotFound),
                    input = email,
                    Messege = "沒有這位使用者或Email輸入錯誤"
                };
                //送註冊JSON訊息
                var errorData = new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Content = new ObjectContent<TopObject<string>>(obj,new JsonMediaTypeFormatter())
                };
                return errorData;
            }
        }
    }
}
