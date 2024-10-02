using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SF_Domain.Inputs;

namespace SF_WebApi.Models.InputModels
{
    public class LoginInputs : LogLoginInput
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}