using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PredictionAPI.Models;
using PredictionAPI.Exception;

namespace PredictionAPI.Services
{
    public class DBOperationService
    {
        private Prediction_2016Entities db;

        public DBOperationService()
        {
            db = new Prediction_2016Entities();
        }
        public void CreateUser(Users user)
        {
            db.Users.Add(user);
            db.SaveChanges();
        }

        public Users FindUser(string property)
        {
            var data = db.Users.Find(property);
            if (data != null)
            {
                return data;
            }
            else
            {
                throw new UserNotFoundException();
            }
        }

        public void UpdateUserInfo(string property,string code,string pass)
        {
            Users data = db.Users.Find(property);
            data.isPass = pass;
            data.verificationCode = code;
            db.SaveChanges();
        }

        public void StoreHistory(UseHistory data)
        {
            db.UseHistories.Add(data);
            db.SaveChanges();
        }
    }
}