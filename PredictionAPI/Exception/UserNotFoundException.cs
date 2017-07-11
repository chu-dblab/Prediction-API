using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PredictionAPI.Exception
{

    [Serializable]
    public class UserNotFoundException : System.Exception
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

        public UserNotFoundException()
        {
            Setmessage("使用者不存在");
        }
        public UserNotFoundException(string message) : base(message) { }
        public UserNotFoundException(string message, System.Exception inner) : base(message, inner) { }
        protected UserNotFoundException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}