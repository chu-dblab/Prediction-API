using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PredictionAPI.Models
{

    public class TopObject<T>
    {
        public int status { get; set; }
        public T input { get; set; }
        public string message { get; set; }
    }

}