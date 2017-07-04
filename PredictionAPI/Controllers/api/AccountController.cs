using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PredictionAPI.Controllers.api
{
    public class AccountController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage SignUp([FromBody] JObject userData)
        {
            //在DB中新增一位使用者
            //發信給使用者作信箱驗證
            //送出註冊成功的JSON 字串
            return new HttpResponseMessage(HttpStatusCode.NoContent);
        }

        [HttpPost]
        public HttpResponseMessage Login([FromBody]string userEMail)
        {
            return new HttpResponseMessage(HttpStatusCode.NoContent);
        }
    }
}
