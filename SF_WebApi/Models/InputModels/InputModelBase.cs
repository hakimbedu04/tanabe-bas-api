﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SF_WebApi.Models.InputModels
{
    public class InputModelBase
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}