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
        private string conStr = "Data Source=CSIE;Initial Catalog=Prediction;Integrated Security=True";
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
            int[] score103 = {ast.Chinese == null? 0 : Convert.ToInt32(Math.Floor(Convert.ToDouble(ast.Chinese))),
                                 ast.English == null? 0 : Convert.ToInt32(Math.Floor(Convert.ToDouble(ast.English))),
                                 ast.Math_A == null? 0 : Convert.ToInt32(Math.Floor(Convert.ToDouble(ast.Math_A))),
                                 ast.Math_B == null? 0 : Convert.ToInt32(Math.Floor(Convert.ToDouble(ast.Math_B))),
                                 ast.History == null? 0 : Convert.ToInt32(Math.Floor(Convert.ToDouble(ast.History))),
                                 ast.Geographic == null? 0 : Convert.ToInt32(Math.Floor(Convert.ToDouble(ast.Geographic))),
                                 ast.Citizen_and_Society == null? 0 : Convert.ToInt32(Math.Floor(Convert.ToDouble(ast.Citizen_and_Society))),
                                 ast.Physics == null? 0 : Convert.ToInt32(Math.Floor(Convert.ToDouble(ast.Physics))),
                                 ast.Chemistry == null? 0 : Convert.ToInt32(Math.Floor(Convert.ToDouble(ast.Chemistry))),
                                 ast.Biology == null? 0 : Convert.ToInt32(Math.Floor(Convert.ToDouble(ast.Biology)))
                             };
            string[] subject = {"國文","英文","數甲","數乙","歷史","地理","公社","物理","化學","生物"};
            score104.Clear();
            this.conn.Open();
            for (int i = 0; i < 10; i++)
            {
                sqlCom = "SELECT Min(E103.Score) As Score FROM E103,E104 WHERE E103.Ename = '" + subject[i] + "' " +
                    "AND E104.Ename = '" + subject[i] + "' AND E104.Score = " + score103[i].ToString() + " AND E104.Percentage <= E103.Percentage;";
                buffer = new SqlDataAdapter(sqlCom,this.conn);
                buffer.Fill(dt);
                if (dt.Rows[0].IsNull("Score")) score104.Add(0);
                else score104.Add(Convert.ToInt32(dt.Rows[0]["Score"]));
                dt.Clear();
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
            try
            {
                sqlCom = appendSQLString(data.grades.ast, data.groups, score103, level,
                data.grades.gsat.EngListeningLevel, data.expect_salary);

                buffer = new SqlDataAdapter(sqlCom, this.conn);
                buffer.Fill(dt);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Result resultData = new Result();
                    //將搜尋到的東西封裝成Object
                    resultData.did = Convert.ToInt32(dt.Rows[i]["DID"].ToString());
                    resultData.uname = dt.Rows[i]["UName"].ToString().Trim();
                    resultData.uurl = dt.Rows[i]["UURL"].ToString().Trim();
                    resultData.dname = dt.Rows[i]["DName"].ToString().Trim();
                    resultData.durl = dt.Rows[i]["DURL"].ToString().Trim();
                    resultData.salary = Convert.ToInt32(dt.Rows[i]["Salary"]);
                    resultData.salaryUrl = dt.Rows[i]["SalaryURL"].ToString().Trim() == Convert.ToString(0)? null : dt.Rows[i]["SalaryURL"].ToString().Trim();
                    resultData.minScore = Convert.ToDouble(dt.Rows[i]["MinScore"]);
                    resultData.yourScore = Convert.ToDouble(dt.Rows[i]["YourScore"]);
                    list.Add(resultData);  //放到List中
                }
                dt.Clear();
                this.conn.Close();
            }
            catch (SqlException ex)
            {
                this.conn.Close();
            }
            return list;
        }

        private string appendSQLString(Ast ast,List<string> groups,ArrayList score103,ArrayList level, string EL,int expectedSalary)
        {
            ArrayList data = changeToArray(ast);
            string group = appendData(groups);
            string command = "SELECT DISTINCT D.DID,D.UName,D.UURL,D.DName,D.DURL, D.Salary, D.SalaryURL, D.MinScore, ("
                    +score103[0].ToString()+"*D.EW1+"+score103[1].ToString()+"*D.EW2+"
                    +score103[2].ToString()+"*D.EW3+"+score103[3].ToString()+"*D.EW4+"
                    +score103[4].ToString()+"*D.EW5+"+score103[5].ToString()+"*D.EW6+"
                    +score103[6].ToString()+"*D.EW7+"+score103[7].ToString()+"*D.EW8+"
                    +score103[8].ToString()+"*D.EW9+"+score103[9].ToString()+"*D.EW10) As YourScore "+
                    "FROM D,DC,CG WHERE  D.DID=DC.DID AND DC.CNAME=CG.CNAME AND CG.GNAME IN ("+group+") "+
                    "AND D.ELLEVEL >= '" + EL + "' AND D.TL1 <= " + level[0].ToString() + " " +
                    "AND D.TL2 <= "+ level[1].ToString() +" AND D.TL3 <= "+level[2].ToString()+" "+
                    "AND D.TL4 <= "+level[3].ToString()+" AND D.TL5 <= "+level[4].ToString()+" "+
                    "AND D.TL6 <= "+level[5].ToString()+" "+
                    "AND D.MinScore <= ("+score103[0].ToString()+"*D.EW1+"+score103[1].ToString()+"*D.EW2+"
                    +score103[2].ToString()+"*D.EW3+"+score103[3].ToString()+"*D.EW4+"
                    +score103[4].ToString()+"*D.EW5+"+score103[5].ToString()+"*D.EW6+"
                    +score103[6].ToString()+"*D.EW7+"+score103[7].ToString()+"*D.EW8+"
                    +score103[8].ToString()+"*D.EW9+"+score103[9].ToString()+"*D.EW10)*1.1 "+
                    "AND D.Salary >= " + expectedSalary.ToString() + " AND (D.NEW1 = 0 OR " + data[0].ToString() + " IS NOT NULL) " +
                    "AND (D.NEW2 =0 OR " + data[1].ToString() + " IS NOT NULL) AND (D.NEW3 = 0 OR " + data[2].ToString() + " IS NOT NULL) " +
                    "AND (D.NEW4 =0 OR " + data[3].ToString() + " IS NOT NULL) AND (D.NEW5 = 0 OR " + data[4].ToString() + " IS NOT NULL) " +
                    "AND (D.NEW6 = 0 OR " + data[5].ToString() + " IS NOT NULL) AND (D.NEW7 = 0 OR " + data[6].ToString() + " IS NOT NULL) " +
                    "AND (D.NEW8 = 0 OR " + data[7].ToString() + " IS NOT NULL) AND (D.NEW9 = 0 OR " + data[8].ToString() + " IS NOT NULL) " +
                    "AND (D.NEW10 = 0 OR " + data[9].ToString() + " IS NOT NULL) ORDER BY D.Salary DESC;";
            return command;
        }

        private ArrayList changeToArray(Ast ast)
        {
            string[] score = { ast.Chinese ,ast.English , ast.Math_A ,ast.Math_B ,
                                 ast.History,ast.Geographic , ast.Citizen_and_Society,
                                 ast.Physics,ast.Chemistry,ast.Biology };
            ArrayList list = new ArrayList();

            for(int i=0;i<score.Length;i++)
            {
                if (String.IsNullOrEmpty(score[i])) list.Add("null");
                else list.Add(score[i]);
            }
            return list;
        }

        private string appendData(List<string> original)
        {
            string temp = null;
            int count = 1;
            var tmp = from t in original select t;
            foreach (string item in tmp)
            {
                if (count == original.Count) temp += "'" + item + "'";
                else temp += "'" + item + "', ";
                count++;
            }
            return temp;
        }
    }
}