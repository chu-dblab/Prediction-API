using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;

namespace PredictionAPI.Models
{
    public class DataOperation
    {
        private DataTable dt;
        private QueryData query;
        public DataOperation()
        {
            dt = null;
            query = new QueryData();
        }

        /// <summary>
        /// 將103學年度的指考分數換算成104學年度的指考分數
        /// </summary>
        /// <param name="ast">指考成績</param>
        /// <returns>104學年度的指考成績</returns>
        /// 
        public ArrayList turnToOldScore(Ast ast)
        {
            string sqlCom = null;
            ArrayList oldScore = new ArrayList();
            int[] newScore = {   ast.Chinese == null? 0 : Convert.ToInt32(Math.Floor(Convert.ToDouble(ast.Chinese))),
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
            oldScore.Clear();
            for (int i = 0; i < newScore.Length; i++)
            {
                sqlCom = "SELECT Max(OldScoreData.Score) As Score FROM OldScoreData,NewScoreData WHERE OldScoreData.Ename = '" + subject[i] + "' " +
                    "AND NewScoreData.Ename = '" + subject[i] + "' AND NewScoreData.Score = " + newScore[i].ToString() + " AND NewScoreData.Percentage >= OldScoreData.Percentage;";
                dt = query.search(sqlCom);
                if (dt.Rows[0].IsNull("Score")) oldScore.Add(0);
                else oldScore.Add(Convert.ToInt32(dt.Rows[0]["Score"]));
                dt.Clear();
            }
            return oldScore;
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
            //int sum = gsat.Chinese + gsat.English + gsat.Math + gsat.Science + gsat.Society;
            int[] scoreOfGSAT = {gsat.Chinese, gsat.English, gsat.Math, gsat.Science, gsat.Society,gsat.TotalScore };
            string[] subjectOfGSAT = { "國文", "英文", "數學", "自然", "社會" ,"總級分"};
            ArrayList level = new ArrayList();
            for (int i = 0; i < 6; i++)
            {
                sqlCom = "SELECT Grade1,Grade2,Grade3,Grade4,Grade5 " +
                    "FROM T WHERE TName = '" + subjectOfGSAT[i] + "'";
                dt = query.search(sqlCom);
                if (scoreOfGSAT[i] < Convert.ToInt32(dt.Rows[0]["Grade1"].ToString())) LV = 0;
                else if (scoreOfGSAT[i] < Convert.ToInt32(dt.Rows[0]["Grade2"].ToString())) LV = 1;
                else if (scoreOfGSAT[i] < Convert.ToInt32(dt.Rows[0]["Grade3"].ToString())) LV = 2;
                else if (scoreOfGSAT[i] < Convert.ToInt32(dt.Rows[0]["Grade4"].ToString())) LV = 3;
                else if (scoreOfGSAT[i] < Convert.ToInt32(dt.Rows[0]["Grade5"].ToString())) LV = 4;
                else LV = 5;
                level.Add(LV);
                dt.Clear();
            }
            return level;
        }

        /// <summary>
        /// 搜尋符合條件之校系
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public List<Result> SearchResult(Input data,bool isCHU)
        {
            List<Result> list = new List<Result>();
            string sqlCom = null;
            ArrayList oldScore = turnToOldScore(data.grades.ast);
            ArrayList level = changeScoreOfGSAT2Level(data.grades.gsat);
            try
            {
                sqlCom = appendSQLString(data.grades.ast, data.groups, oldScore, level,
                data.grades.gsat.EngListeningLevel, data.expect_salary,data.location,data.property,isCHU);
                dt = query.search(sqlCom);
                for (int i = 0; i < dt.Rows.Count && dt.Rows.Count != 0; i++)
                {
                    Result resultData = new Result();
                    //將搜尋到的東西封裝成Object
                    resultData.did = dt.Rows[i]["DID"].ToString();
                    resultData.examURL = dt.Rows[i]["ExamURL"].ToString();
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
                return list;
            }
            catch (SqlException ex)
            {
                return list;
            }
        }

        /// <summary>
        /// 組SQL字串
        /// </summary>
        /// <param name="ast"></param>
        /// <param name="groups"></param>
        /// <param name="oldScore"></param>
        /// <param name="level"></param>
        /// <param name="EL"></param>
        /// <param name="expectedSalary"></param>
        /// <param name="location"></param>
        /// <param name="property"></param>
        /// <param name="isCHU"></param>
        /// <returns></returns>
        private string appendSQLString(Ast ast,List<string> groups,ArrayList oldScore, 
            ArrayList level, string EL,int expectedSalary, List<string> location, List<string> property, bool isCHU)
        {
            ArrayList data = changeToArray(ast);
            string group = appendData(groups);
            string city = appendData(location);
            string attribute = appendData(property);
            string condition = (isCHU ? "AND D.UName = '中華大學' " : "AND D.City IN (" + city + ") " + "AND D.Property IN (" + attribute + ") ");
            string command = "SELECT DISTINCT D.DID,D.ExamURL,D.UName,D.UURL,D.DName,D.DURL, D.Salary, D.SalaryURL, D.MinScore, ("
                    + oldScore[0].ToString()+"*D.EW1+"+ oldScore[1].ToString()+"*D.EW2+"
                    + oldScore[2].ToString()+"*D.EW3+"+ oldScore[3].ToString()+"*D.EW4+"
                    + oldScore[4].ToString()+"*D.EW5+"+ oldScore[5].ToString()+"*D.EW6+"
                    + oldScore[6].ToString()+"*D.EW7+"+ oldScore[7].ToString()+"*D.EW8+"
                    + oldScore[8].ToString()+"*D.EW9+"+ oldScore[9].ToString()+"*D.EW10) As YourScore "+
                    "FROM D,DC,CG WHERE  D.DID=DC.DID AND DC.CNAME=CG.CNAME AND CG.GNAME IN ("+group+") "+ condition +
                    "AND D.ELLEVEL >= '" + EL + "' AND D.TL1 <= " + level[0].ToString() + " " +
                    "AND D.TL2 <= "+ level[1].ToString() +" AND D.TL3 <= "+level[2].ToString()+" "+
                    "AND D.TL4 <= "+level[3].ToString()+" AND D.TL5 <= "+level[4].ToString()+" "+
                    "AND D.TL6 <= "+level[5].ToString()+" "+
                    "AND D.MinScore <= ("+ oldScore[0].ToString()+"*D.EW1+"+ oldScore[1].ToString()+"*D.EW2+"
                    + oldScore[2].ToString()+"*D.EW3+"+ oldScore[3].ToString()+"*D.EW4+"
                    + oldScore[4].ToString()+"*D.EW5+"+ oldScore[5].ToString()+"*D.EW6+"
                    + oldScore[6].ToString()+"*D.EW7+"+ oldScore[7].ToString()+"*D.EW8+"
                    + oldScore[8].ToString()+"*D.EW9+"+ oldScore[9].ToString()+"*D.EW10)*1.1 "+
                    "AND D.Salary >= " + expectedSalary.ToString() + " AND (D.NEW1 = 0 OR " + data[0].ToString() + " IS NOT NULL) " +
                    "AND (D.NEW2 = 0 OR " + data[1].ToString() + " IS NOT NULL) AND (D.NEW3 = 0 OR " + data[2].ToString() + " IS NOT NULL) " +
                    "AND (D.NEW4 = 0 OR " + data[3].ToString() + " IS NOT NULL) AND (D.NEW5 = 0 OR " + data[4].ToString() + " IS NOT NULL) " +
                    "AND (D.NEW6 = 0 OR " + data[5].ToString() + " IS NOT NULL) AND (D.NEW7 = 0 OR " + data[6].ToString() + " IS NOT NULL) " +
                    "AND (D.NEW8 = 0 OR " + data[7].ToString() + " IS NOT NULL) AND (D.NEW9 = 0 OR " + data[8].ToString() + " IS NOT NULL) " +
                    "AND (D.NEW10 = 0 OR " + data[9].ToString() + " IS NOT NULL) ORDER BY D.Salary DESC;";
            return command;
        }

        /// <summary>
        /// 將物件轉換成陣列
        /// </summary>
        /// <param name="ast"></param>
        /// <returns></returns>
        private ArrayList changeToArray(Ast ast)
        {
            string[] score = { ast.Chinese ,ast.English , ast.Math_A ,ast.Math_B ,
                                 ast.History,ast.Geographic , ast.Citizen_and_Society,
                                 ast.Physics,ast.Chemistry,ast.Biology };
            ArrayList list = new ArrayList();

            for (int i = 0; i < score.Length; i++)
            {
                if (String.IsNullOrEmpty(score[i])) list.Add("null");
                else list.Add(score[i]);
            }
            return list;
        }

        /// <summary>
        /// 拼接字串
        /// </summary>
        /// <param name="original"></param>
        /// <returns></returns>
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
