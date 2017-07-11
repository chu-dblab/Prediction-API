using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PredictionAPI.Exception
{

    [Serializable]
    public class EmailNotVertifyException : System.Exception
    {
        private string message;

        protected string GetMessage()
        {
            return message;
        }

        private void SetMessage(string value)
        {
            message = value;
        }

        public EmailNotVertifyException() { SetMessage("電子郵件沒有驗證過"); }
        public EmailNotVertifyException(string message) : base(message) { }
        public EmailNotVertifyException(string message, System.Exception inner) : base(message, inner) { }
        protected EmailNotVertifyException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}