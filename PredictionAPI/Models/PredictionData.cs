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
        public string Math_A { get; set; }
        public string Math_B { get; set; }
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
        public double minScore { get; set; }
        public int salary { get; set; }
        public string salaryUrl { get; set; }
        public double yourScore { get; set; }
    }

    public class RootObject
    {
        public int status { get; set; }
        public Input input { get; set; }
        public List<Result> result { get; set; }
        public string message { get; set; }
    }
}