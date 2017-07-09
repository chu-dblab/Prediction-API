using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PredictionAPI.Exception
{

    [Serializable]
    public class EmailNotVertifyException : System.Exception
    {
        protected string Message { get; private set; }
        public EmailNotVertifyException() { Message = "電子郵件沒有驗證過"; }
        public EmailNotVertifyException(string message) : base(message) { }
        public EmailNotVertifyException(string message, System.Exception inner) : base(message, inner) { }
        protected EmailNotVertifyException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}