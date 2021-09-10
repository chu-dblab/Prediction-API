using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace PredictionAPI.Exception
{

    [Serializable]
    public class EmailAddressNotExistException : System.Exception
    {
        private string message;

        protected string Getmessage()
        {
            return message;
        }

        private void Setmessage(string value)
        {
            message = value;
        }
        public EmailAddressNotExistException() { Setmessage("無此信箱，請輸入正確的Email地址"); }
        public EmailAddressNotExistException(string message) : base(message) { }
        public EmailAddressNotExistException(string message, System.Exception inner) : base(message, inner) { }
        protected EmailAddressNotExistException(
        SerializationInfo info,
        StreamingContext context) : base(info, context) { }
    }
}