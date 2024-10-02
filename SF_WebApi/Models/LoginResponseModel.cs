using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SF_WebApi.Models
{
    public class LoginResponseModel
    {
        public string Auth { get; set; }
        public bool Status { get; set; }
        public string Message { get; set; }
        public object Result { get; set; }
        public string DetailMessage { get; set; }
    }
}