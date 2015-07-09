using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;

namespace PredictionAPI.Models
{
    public class DataOperation
    {
        private const string conStr = ConfigurationManager.ConnectionStrings["Prediction"].ConnectionString;
        private SqlConnection conn;
        private DataTable dt;
        public DataOperation()
        {
            this.conn = new SqlConnection(conStr);
            dt = new DataTable();
        }

        /// <summary>
        /// 將103學年度的指考分數換算成104學年度的指考分數
        /// </summary>
        /// <param name="ast">指考成績</param>
        /// <returns>104學年度的指考成績</returns>
        /// 
        public ArrayList turnTo103Score(Ast ast)
        {
            string sqlCom = null;
            SqlDataAdapter buffer = null;
            ArrayList score104 = new ArrayList();
            int[] score103 = {ast.Chinese,ast.English,ast.Math_A,ast.Math_B,
                                 ast.Physics,ast.Chemistry,ast.Biology,ast.History,ast.Geographic,ast.Citizen_and_Society};
            string[] subject = {"國文","英文","數甲","數乙","物理","化學","生物","歷史","地理","公社"};
            for (int i = 0; i < 10; i++)
            {
                sqlCom = "SELECT MAX(E103.Score) As Score FROM E103,E104 WHERE E103.Ename = '" + subject[i] + "' " +
                    "AND E104.Ename = '" + subject[i] + "' AND E104.Score = " + score103[i].ToString() + " AND E104.Precent >= E103.Precent;";
                this.conn.Open();
                buffer = new SqlDataAdapter(sqlCom,this.conn);
                buffer.Fill(dt);
                this.conn.Close();
                score104.Add(dt.Rows[0]["Score"]);
                dt.Clear();
                buffer.Dispose();
            }
            return score104;
        }

        public ArrayList changeScoreOfGSAT2Level(Gsat gsat)
        {
            string sqlCom = null;
            int sum = gsat.Chinese + gsat.English + gsat.Math + gsat.Science + gsat.Society;
            int[] score103OfGSAT = {gsat.Chinese, gsat.English, gsat.Math, gsat.Science, gsat.Society,sum };
            string[] subjectOfGSAT = { "國文", "英文", "數學", "自然", "社會" ,"總分"};
            SqlDataAdapter buffer = null;
            ArrayList level = new ArrayList();
            for (int i = 0; i < 5; i++)
            {
                sqlCom = "SELECT Grade1,Grade2,Grade3,Grade4,Grade5 " +
                    "FROM T WHERE TNname = '" + subjectOfGSAT[i] + "' " +
                    "IF " + score103OfGSAT[i].ToString() + " < Grade1 THEN L = 0 " +
                    "ELSE IF " + score103OfGSAT[i].ToString() + " < Grade2 THEN L = 1 "+
                    "ELSE IF " + score103OfGSAT[i].ToString()+" < Grade3 THEN L = 2 "+
                    "ELSE IF " + score103OfGSAT[i].ToString() + " < Grade4 THEN L = 3 " +
                    "ELSE IF " + score103OfGSAT[i].ToString() + " < Grade5 THEN L = 4 " +
                    "ELSE L = 5";
                this.conn.Open();
                buffer = new SqlDataAdapter(sqlCom, this.conn);
                buffer.Fill(dt);
                this.conn.Close();
                level.Add(dt.Rows[0]["L"]);
            }
            return level;
        }

        public List<Result> SearchResult(Input data)
        {
            Result resultData = new Result();
            List<Result> list = new List<Result>();
            SqlDataAdapter buffer = null;
            string sqlCom = null;
            ArrayList score103 = turnTo103Score(data.grades.ast);
            ArrayList level = changeScoreOfGSAT2Level(data.grades.gsat);
            foreach (string group in data.groups)
            {
                //sqlCom = 
            }
            return null;
        }

        private string appendSQLString(string group,ArrayList score103,ArrayList level, string EL)
        {
            string command = "SELECT D.DID,D.UName,D.UURL,D.DName,D.DURL, D.Salary, D.SalaryURL, D.MinScore, ("
                    +score103[0].ToString()+"*D.EW1+"
                    +score103[1].ToString()+"*D.EW2+"
                    +score103[2].ToString()+"*D.EW3+"
                    +score103[3].ToString()+"*D.EW4+"
                    +score103[4].ToString()+"*D.EW5+"
                    +score103[5].ToString()+"*D.EW6+"
                    +score103[6].ToString()+"*D.EW7+"
                    +score103[7].ToString()+"*D.EW8+"
                    +score103[8].ToString()+"*D.EW9+"
                    +score103[9].ToString()+"*D.EW10) As YourScore FROM D,DC,CG "+
                    "WHERE  D.DID=DC.DID AND DC.CNAME=CG.CNAME AND CG.GNAME='"+group+"'";
            return command;
        }
    }
}