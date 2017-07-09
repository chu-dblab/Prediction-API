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

namespace PredictionAPI.Controllers.api
{
    public class AccountController : ApiController
    {
        private DBOperationService db;
        private EmailService email;

        [HttpGet]
        public IHttpActionResult EmailVerify(string address,string authcode)
        {
            db = new DBOperationService();
            db.UpdateUserInfo(address, authcode, "Y");
            return Redirect("/2017/predict.html");
        }

        [HttpPost]
        public async Task<HttpResponseMessage> SignUp([FromBody] UserData userData)
        {
            //在DB中新增一位使用者
            db = new DBOperationService();
            Users data = new Users()
            {
                Email = userData.email,
                location = userData.location,
                schoolName = userData.schoolName,
                identity = userData.identity,
                verificationCode = Guid.NewGuid().ToString(),
                isPass = "N"
            };
            //發信給使用者作信箱驗證
            email = new EmailService();
            await email.SendAsync(data.Email, data.verificationCode, Request.RequestUri.ToString());

            db.CreateUser(data);
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
                Content = new ObjectContent<string>(JsonConvert.SerializeObject(result),
                          new JsonMediaTypeFormatter())
            };
            return reData;
        }

        [HttpPost]
        public HttpResponseMessage Login([FromBody]string userEMail)
        {
            db = new DBOperationService();
            try
            {
                var data = db.FindUser(userEMail);
                var cookie = new CookieHeaderValue("ticket", JsonConvert.SerializeObject(data))
                {
                    Domain = Request.RequestUri.ToString(),
                    Expires = DateTimeOffset.Now.AddMinutes(60),
                    Path = "/",
                    HttpOnly = true
                };
                //傳cookie+登入成功訊息
                var result = new HttpResponseMessage()
                {
                    Content = new ObjectContent<string>(JsonConvert.SerializeObject(data), new JsonMediaTypeFormatter())
                };
                result.Headers.AddCookies(new CookieHeaderValue[] { cookie});
                return result;
                
            }
            catch(UserNotFoundException ex)
            {
                TopObject<string> obj = new TopObject<string>()
                {
                    status = Convert.ToInt32(HttpStatusCode.NotFound),
                    input = userEMail,
                    Messege = "沒有這位使用者或Email輸入錯誤"
                };
                //送註冊JSON訊息

                var errorData = new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Content = new ObjectContent<string>(JsonConvert.SerializeObject(obj),
                          new JsonMediaTypeFormatter())
                };
                return errorData;
            }
        }
    }
}
