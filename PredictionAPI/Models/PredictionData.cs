using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PredictionAPI.Models
{
    //指考
    public class Ast
    {
        public string Chinese { get; set; }
        public string English { get; set; }
        public string Math_A { get; set; } //數學甲
        public string Math_B { get; set; } //數學乙
        public string History { get; set; }
        public string Geographic { get; set; }
        public string Citizen_and_Society { get; set; }
        public string Physics { get; set; }
        public string Chemistry { get; set; }
        public string Biology { get; set; }
    }

    //學測
    public class Gsat
    {
        public int Chinese { get; set; }
        public int English { get; set; }
        public int Math { get; set; }
        public int Science { get; set; }
        public int Society { get; set; }
        public string EngListeningLevel { get; set; }
    }

    //成績
    public class Grades
    {
        public Ast ast { get; set; } //指考
        public Gsat gsat { get; set; } //學測
    }

    public class Input
    {
        public Grades grades { get; set; } //考試成績
        public List<string> location { get; set; } //學校的所在縣市
        public List<string> property { get; set; } //公私立
        public List<string> groups { get; set; } //學群
        public int expect_salary { get; set; } //期望薪資
    }

    public class Result
    {
        public int did { get; set; } //校系代碼
        public string uname { get; set; } //學校名稱
        public string uurl { get; set; } //學校網址
        public string dname { get; set; } //科系名稱
        public string durl { get; set; } //科系網址
        public double minScore { get; set; } //去年錄取最低分數
        public int salary { get; set; } //104系友薪資
        public string salaryUrl { get; set; }  //104系友薪資網址
        public double yourScore { get; set; } //總分(加權過後)
    }

    public class RootObject
    {
        public int status { get; set; }
        public Input input { get; set; }
        public List<Result> result { get; set; }
        public List<Result> resultCHU { get; set; }
        public string message { get; set; }
    }

   /****************************即時判定學測標準***************************/
    public class Enter
    {
        public Grades grades { get; set; }
    }

    public class StandarLevel
    {
        public int status { get; set; }
        public Enter enter { get; set; }
        public Dictionary<string, string> step { get; set; }
    }
   /********************************************************************/
}