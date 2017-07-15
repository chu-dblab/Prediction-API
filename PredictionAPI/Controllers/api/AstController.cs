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
using PredictionAPI.Services;

namespace PredictionAPI.Controllers.api
{
    public class AstController : ApiController
    {
        
        //TODO: 塞資料的動作可以給AutoMapper來做
        [HttpPost]
        public HttpResponseMessage analysis([FromBody] Input obj)
        {
            try
            {
                DataOperation op = new DataOperation();
                List<Result> list = op.SearchResult(obj,false);
                List<Result> listCHU = op.SearchResult(obj, true);
                RootObject rootData = new RootObject()
                {
                    status = Convert.ToInt32(HttpStatusCode.OK),
                    input = obj,
                    result = list,
                    resultCHU = listCHU,
                    message = "Success~!!"
                };
                var result = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new ObjectContent<RootObject>(rootData, new JsonMediaTypeFormatter())
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
                        new JProperty("input",obj),
                        new JProperty("Message","解析錯誤~!!")), new JsonMediaTypeFormatter())
                };
                return result;
            }
        }
    }
}
