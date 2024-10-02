using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SF_WebApi.Models
{
    public class ResponseModel
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public string DetailMessage { get; set; }
        public int TotalRecords { get; set; }
        public int TotalPages { get; set; }
        public object Result { get; set; }
    }
}