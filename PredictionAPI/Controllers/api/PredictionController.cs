using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PredictionAPI.Models;

namespace PredictionAPI.Controllers.api
{
    public class PredictionController : ApiController
    {        
        [HttpGet]
        public HttpResponseMessage groups()
        {
            using(PredictionEntities predict = new PredictionEntities())
            {
                var result = from groups in predict.CGs
                             select groups.Gname;
                var data = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new ObjectContent<JObject>(new JObject(new JProperty("status", HttpStatusCode.OK),
                        new JProperty("groups", result),
                        new JProperty("msg", "")), new JsonMediaTypeFormatter())
                };
                return data;
            }
        }

        [HttpPost]
        public HttpResponseMessage analysis([FromBody] JObject data)
        {
            Input obj = JsonConvert.DeserializeObject<Input>(data.ToString());
            var result = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("Data:"+obj.grades.ast.English)
            };
            return result;
        }
    }
}
