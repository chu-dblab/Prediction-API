using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PredictionAPI.Models;
using PredictionAPI.Exception;
using System.Web.Http.ModelBinding;

namespace PredictionAPI.Services
{
    public class DBOperationService
    {
        private PredictionEntities db;

        public DBOperationService()
        {
            db = new PredictionEntities();
        }
        public void CreateUser(User user)
        {
            db.Users.Add(user);
            db.SaveChanges();
        }

        public User FindUser(string property)
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
            User data = db.Users.Find(code);
            data.isPass = pass;
            data.verificationCode = string.Empty;
            db.SaveChanges();
        }

        public void StoreHistory(UseHistory data)
        {
            db.UseHistories.Add(data);
            db.SaveChanges();
        }
    }
}