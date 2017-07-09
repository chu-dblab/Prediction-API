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
        public HttpResponseMessage History([FromBody] Grades data)
        {
            CookieHeaderValue cookie = Request.Headers.GetCookies("ticket").FirstOrDefault();
            UserData da = JsonConvert.DeserializeObject<UserData>(cookie["ticket"].Value);
            UseHistory history = new UseHistory()
            {
                Email = da.email,
                timestamp = DateTime.Now,
                Ast_Chinese = Convert.ToDouble(data.ast.Chinese),
                Ast_English = Convert.ToDouble(data.ast.English),
                Ast_MathA = Convert.ToDouble(data.ast.Math_A),
                Ast_MathB = Convert.ToDouble(data.ast.Math_B),
                Ast_Physics = Convert.ToDouble(data.ast.Physics),
                Ast_Chemistry = Convert.ToDouble(data.ast.Chemistry),
                Ast_Biology = Convert.ToDouble(data.ast.Biology),
                Ast_History = Convert.ToDouble(data.ast.History),
                Ast_Geography = Convert.ToDouble(data.ast.Geographic),
                Ast_CitizenAndSociety = Convert.ToDouble(data.ast.Citizen_and_Society),
                Gsat_Chinese = data.gsat.Chinese,
                Gsat_English = data.gsat.English,
                Gsat_Math = data.gsat.Math,
                Gsat_Science = data.gsat.Science,
                Gsat_Society = data.gsat.Society,
                Gsat_ELLevel = data.gsat.EngListeningLevel
            };
            db.StoreHistory(history);
            TopObject<Grades> result = new TopObject<Grades>()
            {
                status = Convert.ToInt32(HttpStatusCode.OK),
                input = data,
                Messege = "記錄成功~!!"
            };
            var resp = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ObjectContent<string>(JsonConvert.SerializeObject(result), 
                          new JsonMediaTypeFormatter())
            };
            return resp;
        }
    }
}