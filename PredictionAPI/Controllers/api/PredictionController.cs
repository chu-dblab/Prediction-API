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
using System.Web.Http.Cors;

namespace PredictionAPI.Controllers.api
{
    public class PredictionController : ApiController
    {        
        //[HttpGet]
        //public HttpResponseMessage groups()
        //{
        //    using(PredictionEntities predict = new PredictionEntities())
        //    {
        //        var result = from groups in predict.CGs
        //                     select groups.Gname;
        //        var data = new HttpResponseMessage(HttpStatusCode.OK)
        //        {
        //            Content = new ObjectContent<JObject>(new JObject(new JProperty("status", HttpStatusCode.OK),
        //                new JProperty("groups", result),
        //                new JProperty("msg", "")), new JsonMediaTypeFormatter())
        //        };
        //        return data;
        //    }
        //}

        [HttpPost]
        public HttpResponseMessage analysis([FromBody] JObject data)
        {
            try
            {
                Input obj = JsonConvert.DeserializeObject<Input>(data.ToString());
                DataOperation op = new DataOperation();
                List<Result> list = op.SearchResult(obj);
                RootObject rootData = new RootObject();
                rootData.status = Convert.ToInt32(HttpStatusCode.OK);
                rootData.input = obj;
                rootData.result = list;
                rootData.message = "Success~!!";
                JObject jsonData = JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(rootData));
                var result = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new ObjectContent<JObject>(jsonData, new JsonMediaTypeFormatter())
                };
                return result;
            }
            catch(JsonException ex)
            {
                var result = new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new ObjectContent<JObject>(
                        new JObject(
                        new JProperty("status",HttpStatusCode.BadRequest),
                        new JProperty("input",data),
                        new JProperty("Message","解析錯誤~!!")), new JsonMediaTypeFormatter())
                };
                return result;
            }
        }
    }
}
