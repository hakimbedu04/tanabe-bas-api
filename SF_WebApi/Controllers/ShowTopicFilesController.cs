using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.Ajax.Utilities;
using SF_BusinessLogics.Visit;

namespace SF_WebApi.Controllers
{
   
    public class ShowTopicFilesController : Controller
    {
        private readonly IVisitBLL _mainBll;

        public ShowTopicFilesController(IVisitBLL mainBll)
        {
            _mainBll = mainBll;
        }
        
        // GET: ShowTopicFiles
        //public ActionResult ShowTopicFiles(string path)
        //{
        //    ViewBag.Path = path;
        //    return View();
        //}

        public ActionResult Index()
        {
            ViewBag.Path = "~/Files/SP-Topics/herbe.png";
            return View();
        }

    }
}