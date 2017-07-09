using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PredictionAPI.Exception
{

    [Serializable]
    public class UserNotFoundException : System.Exception
    {
        protected string Message { get; private set; }
        public UserNotFoundException()
        {
            Message = "使用者不存在";
        }
        public UserNotFoundException(string message) : base(message) { }
        public UserNotFoundException(string message, System.Exception inner) : base(message, inner) { }
        protected UserNotFoundException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}