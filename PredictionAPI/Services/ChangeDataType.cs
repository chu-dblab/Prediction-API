using PredictionAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PredictionAPI.Services
{
    public class ChangeDataType
    {
        public UseHistory Mapper(Input data,string mail,DateTime timestamp)
        {
            Gsat gsat = data.grades.gsat;
            Ast ast = data.grades.ast;
            string location = MapToString(data.location);
            string departGroup = MapToString(data.groups);
            string property = MapToString(data.property);
            UseHistory history = new UseHistory()
            {
                Email = mail,
                timestamp = timestamp,
                Ast_Chinese = Convert.ToDouble(ast.Chinese),
                Ast_English = Convert.ToDouble(ast.English),
                Ast_MathA = Convert.ToDouble(ast.Math_A),
                Ast_MathB = Convert.ToDouble(ast.Math_B),
                Ast_Physics = Convert.ToDouble(ast.Physics),
                Ast_Chemistry = Convert.ToDouble(ast.Chemistry),
                Ast_Biology = Convert.ToDouble(ast.Biology),
                Ast_History = Convert.ToDouble(ast.History),
                Ast_Geography = Convert.ToDouble(ast.Geographic),
                Ast_CitizenAndSociety = Convert.ToDouble(ast.Citizen_and_Society),
                Gsat_Chinese = gsat.Chinese,
                Gsat_English = gsat.English,
                Gsat_Math = gsat.Math,
                Gsat_Science = gsat.Science,
                Gsat_Society = gsat.Society,
                Gsat_ELLevel = gsat.EngListeningLevel,
                Property = property,
                location = location,
                Departgroup = departGroup,
                expectSalary = data.expect_salary
            };
            return history;
        }

        private string MapToString(List<string> data)
        {
            string tmp = null;
            if (data.Count == 19 || data.Count == 2)
            {
                return tmp = "全部";
            }
            else
            {
                foreach (var item in data)
                {
                    tmp += item + ",";
                }
                return tmp.TrimEnd(',');
            }
        }
    }
}