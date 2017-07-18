using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace PredictionAPI.Exception
{

    [Serializable]
    public class VerifiedException : System.Exception
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
        public VerifiedException() { Setmessage("信箱已驗證"); }
        public VerifiedException(string message) : base(message) { }
        public VerifiedException(string message, System.Exception inner) : base(message, inner) { }
        protected VerifiedException(SerializationInfo info,StreamingContext context) : base(info, context) { }
    }
}