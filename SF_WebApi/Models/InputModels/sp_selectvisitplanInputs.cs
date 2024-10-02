using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SF_WebApi.Models.InputModels
{
    public class sp_selectvisitplanInputs
    {
        public string rep_id { get; set; }
        public int day { get; set; }
        public int month { get; set; }
        public int year { get; set; }
    }
}