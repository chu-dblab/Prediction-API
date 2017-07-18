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

        public User search(string Email)
        {
            User user = db.Users.SingleOrDefault(x => x.Email == Email);
            if(user != null)
            {
                return user;
            }
            else
            {
                throw new UserNotFoundException();
            }
        }

        public bool isEmailExist(string email)
        {
            var user = db.Users.SingleOrDefault(X => X.Email == email);
            if (user != null) return true;
            else return false;
        }

        public User FindUser(string property)
        {
            var data = db.Users.Find(property);
            if (data != null )
            {
                if (!data.isPass.Equals("Y")) throw new EmailNotVertifyException();
                else return data;
            }
            else
            {
                throw new UserNotFoundException();
            }
        }

        public void UpdateUserInfo(string property,string code,string pass)
        {
            var data = db.Users.SingleOrDefault(x => x.verificationCode == code && x.Email == property);
            if (data.isPass.Equals("N"))
            {
                data.verificationCode = string.Empty;
                data.isPass = "Y";
                db.SaveChanges();
            }
            else
            {
                throw new VerifiedException();
            }
        }

        public void StoreHistory(UseHistory data)
        {
            db.UseHistories.Add(data);
            db.SaveChanges();
        }
    }
}