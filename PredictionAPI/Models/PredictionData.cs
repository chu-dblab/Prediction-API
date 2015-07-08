using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PredictionAPI.Models
{
    //指考
    public class Ast
    {
        public int Chinese { get; set; }
        public int English { get; set; }
        public int Math_A { get; set; }
        public int Math_B { get; set; }
        public int History { get; set; }
        public int Geographic { get; set; }
        public int Citizen_and_Society { get; set; }
        public int Physics { get; set; }
        public int Chemistry { get; set; }
        public int Biology { get; set; }
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
        public Ast ast { get; set; }
        public Gsat gsat { get; set; }
    }

    public class Input
    {
        public Grades grades { get; set; }
        public List<string> groups { get; set; }
        public int expect_salary { get; set; }
    }

    public class Result
    {
        public int did { get; set; }
        public string uname { get; set; }
        public string uurl { get; set; }
        public string dname { get; set; }
        public string durl { get; set; }
        public int minScore { get; set; }
        public int salary { get; set; }
        public string salaryUrl { get; set; }
        public int yourScore { get; set; }
    }

    public class RootObject
    {
        public int status { get; set; }
        public Input input { get; set; }
        public List<Result> result { get; set; }
        public string msg { get; set; }
    }
}