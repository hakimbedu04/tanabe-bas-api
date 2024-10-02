using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SF_WebApi.Models
{
    public class ViewModelBase<T> where T : class
    {
        public int status { get; set; }
        public string message { get; set; }
        public int total { get; set; }
        public int page { get; set; }
        List<T> Result { get; set; }
    }
}