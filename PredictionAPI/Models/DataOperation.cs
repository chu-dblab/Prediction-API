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
    //struct Score
    //{
    //    string Chinese;
    //    string English;
    //    string Math_A;
    //    string Math_B;
    //    string Pysics;
    //    string Chemistry;
    //    string Biology;
    //    string History;
    //    string Geography;
    //    string Citizen;
    //} 
    public class DataOperation
    {
        private string conStr = "Data Source=DBLAB6249;Initial Catalog=Prediction;Integrated Security=True";
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
            int[] score103 = {(int)ast.Chinese,(int)ast.English,(int)ast.Math_A,(int)ast.Math_B,
                                 (int)ast.Physics,(int)ast.Chemistry,(int)ast.Biology,(int)ast.History,(int)ast.Geographic,(int)ast.Citizen_and_Society};
            string[] subject = {"國文","英文","數甲","數乙","物理","化學","生物","歷史","地理","公社"};
            score104.Clear();
            this.conn.Open();
            for (int i = 0; i < 10; i++)
            {
                sqlCom = "SELECT MAX(E103.Score) As Score FROM E103,E104 WHERE E103.Ename = '" + subject[i] + "' " +
                    "AND E104.Ename = '" + subject[i] + "' AND E104.Score = " + score103[i].ToString() + " AND E104.Percentage >= E103.Percentage;";
                buffer = new SqlDataAdapter(sqlCom,this.conn);
                buffer.Fill(dt);
                score104.Add(dt.Rows[0]["Score"]);
                dt.Clear();
                buffer.Dispose();
            }
            buffer.Dispose();
            this.conn.Close();
            return score104;
        }

        /// <summary>
        /// 將學測級分轉換成等級
        /// </summary>
        /// <param name="gsat">學測成績</param>
        /// <returns>學測等級</returns>
        public ArrayList changeScoreOfGSAT2Level(Gsat gsat)
        {
            string sqlCom = null;
            int LV = 0;
            int sum = gsat.Chinese + gsat.English + gsat.Math + gsat.Science + gsat.Society;
            int[] score104OfGSAT = {gsat.Chinese, gsat.English, gsat.Math, gsat.Science, gsat.Society,sum };
            string[] subjectOfGSAT = { "國文", "英文", "數學", "自然", "社會" ,"總級分"};
            SqlDataAdapter buffer = null;
            ArrayList level = new ArrayList();
            this.conn.Open();
            for (int i = 0; i < 6; i++)
            {
                sqlCom = "SELECT Grade1,Grade2,Grade3,Grade4,Grade5 " +
                    "FROM T WHERE Tname = '" + subjectOfGSAT[i] + "'";
                buffer = new SqlDataAdapter(sqlCom, this.conn);
                buffer.Fill(dt);
                this.conn.Close();

                if (score104OfGSAT[i] < Convert.ToInt32(dt.Rows[0]["Grade1"].ToString())) LV = 0;
                else if (score104OfGSAT[i] < Convert.ToInt32(dt.Rows[0]["Grade2"].ToString())) LV = 1;
                else if (score104OfGSAT[i] < Convert.ToInt32(dt.Rows[0]["Grade3"].ToString())) LV = 2;
                else if (score104OfGSAT[i] < Convert.ToInt32(dt.Rows[0]["Grade4"].ToString())) LV = 3;
                else if (score104OfGSAT[i] < Convert.ToInt32(dt.Rows[0]["Grade5"].ToString())) LV = 4;
                else LV = 5;
                level.Add(LV);
                dt.Clear();
            }
            buffer.Dispose();
            this.conn.Close();
            return level;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public List<Result> SearchResult(Input data)
        {
            Result resultData = new Result();
            List<Result> list = new List<Result>();
            SqlDataAdapter buffer = null;
            string sqlCom = null;
            ArrayList score103 = turnTo103Score(data.grades.ast);
            ArrayList level = changeScoreOfGSAT2Level(data.grades.gsat);
            if (this.conn.State == ConnectionState.Open)
            {
                this.conn.Close();
                //this.conn.State = ConnectionState.Closed;
            }
            this.conn.Open();
            foreach (string group in data.groups)
            {
                try
                {
                    sqlCom = appendSQLString(data.grades.ast, group, score103, level,
                    data.grades.gsat.EngListeningLevel, data.expect_salary);
                    buffer = new SqlDataAdapter(sqlCom, this.conn);
                    buffer.Fill(dt);

                    //將搜尋到的東西封裝成Object
                    resultData.did = Convert.ToInt32(dt.Rows[0]["DID"].ToString());
                    resultData.uurl = dt.Rows[0]["UURL"].ToString();
                    resultData.dname = dt.Rows[0]["DName"].ToString();
                    resultData.durl = dt.Rows[0]["DURL"].ToString();
                    resultData.salary = Convert.ToInt32(dt.Rows[0]["Salary"]);
                    resultData.salaryUrl = dt.Rows[0]["SalaryURL"].ToString();
                    resultData.minScore = Convert.ToInt32(dt.Rows[0]["MinScore"]);
                    resultData.yourScore = Convert.ToInt32(dt.Rows[0]["YourScore"]);

                    //放到List中
                    list.Add(resultData);
                }
                catch(SqlException ex)
                {

                }
            }
            this.conn.Close();
            return list;
        }

        private string appendSQLString(Ast ast,string group,ArrayList score103,ArrayList level, string EL,int expectedSalary)
        {
            string command = "SELECT D.DID,D.UName,D.UURL,D.DName,D.DURL, D.Salary, D.SalaryURL, D.MinScore, ("
                    +score103[0].ToString()+"*D.EW1+"+score103[1].ToString()+"*D.EW2+"
                    +score103[2].ToString()+"*D.EW3+"+score103[3].ToString()+"*D.EW4+"
                    +score103[4].ToString()+"*D.EW5+"+score103[5].ToString()+"*D.EW6+"
                    +score103[6].ToString()+"*D.EW7+"+score103[7].ToString()+"*D.EW8+"
                    +score103[8].ToString()+"*D.EW9+"+score103[9].ToString()+"*D.EW10) As YourScore "+
                    "FROM D,DC,CG WHERE  D.DID=DC.DID AND DC.CNAME=CG.CNAME AND CG.GNAME='"+group+"' "+
                    "AND D.ELLEVEL >= '" + EL + "' AND D.TL1 <= " + level[0].ToString() + " " +
                    "AND D.TL2 <= "+ level[1].ToString() +" AND D.TL3 <= "+level[2].ToString()+" "+
                    "AND D.TL4 <= "+level[3].ToString()+" AND D.TL5 <= "+level[4].ToString()+" "+
                    "AND D.TL6 <= "+level[5].ToString()+" "+
                    "AND D.MinScore <= ("+score103[0].ToString()+"*D.EW1+"+score103[1].ToString()+"*D.EW2+"
                    +score103[2].ToString()+"*D.EW3+"+score103[3].ToString()+"*D.EW4+"
                    +score103[4].ToString()+"*D.EW5+"+score103[5].ToString()+"*D.EW6+"
                    +score103[6].ToString()+"*D.EW7+"+score103[7].ToString()+"*D.EW8+"
                    +score103[8].ToString()+"*D.EW9+"+score103[9].ToString()+"*D.EW10)*1.1 "+
                    "AND D.Salary >= "+expectedSalary.ToString()+" AND (D.EW1 = 0 OR "+ast.Chinese.ToString()+" <> NULL) "+
                    "AND (D.EW2 =0 OR " + ast.English.ToString() + " <> NULL) AND (D.EW3 = 0 OR " + ast.Math_A.ToString() + " <> NULL) "+
                    "AND (D.EW4 =0 OR "+ast.Math_B.ToString()+" <> NULL) AND (D.EW5 = 0 OR " + ast.History.ToString()+") "+
                    "ANS (D.EW6 = 0 OR "+ast.Geographic.ToString() +" <> NULL) AND (D.EW7 = 0 OR "+ast.Citizen_and_Society.ToString()+" <> NULL) "+
                    "AND (D.EW8 = 0 OR "+ast.Physics.ToString()+" <> NULL) AND (D.EW9 = 0 OR "+ast.Chemistry.ToString()+" <> NULL) "+
                    "AND (D.EW10 = 0 OR "+ast.Biology.ToString()+" <> NULL) ORDER BY D.Salary DESC;";
            return command;
        }

    }
}