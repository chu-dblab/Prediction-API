using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PredictionAPI.Models;
using PredictionAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web.Http;

namespace PredictionAPI.Controllers.api
{
    public class StoreController : ApiController
    {
        private DBOperationService db;

        [HttpPost]
        public HttpResponseMessage History([FromBody] Input data)
        {
            db = new DBOperationService();
            ChangeDataType change = new ChangeDataType();
            CookieHeaderValue cookie = Request.Headers.GetCookies("session").FirstOrDefault();
            UseHistory history = change.Mapper(data, cookie["session"].Values["email"], DateTime.Now);
            db.StoreHistory(history);
            TopObject<Input> result = new TopObject<Input>()
            {
                status = Convert.ToInt32(HttpStatusCode.OK),
                input = data,
                Messege = "記錄成功~!!"
            };
            var resp = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ObjectContent<TopObject<Input>>(result, new JsonMediaTypeFormatter())
            };
            return resp;
        }
    }
}