using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using MES.Data;
using MES.Data.Migrations;
using MES.Models;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;

namespace MES.Mvc.Controllers
{
    public class HomeController : BaseController
    {
       
        public ActionResult Index()
        {
            var machines = db.Machines.All().Include(m => m.MachineFamily).Include(m => m.ProductionLine);
            return View(machines.ToList());
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpPost]
        public JsonResult LastStatus()
        {

            var productProcess = db.ProductProcesses.All()
                .GroupBy(m => m.MachineId)
                .Select(g=>g.OrderByDescending(x=>x.DateTime).Take(1));
                
            
           
            
           
            return Json(new { Success = true, productProcess});
        }
    }
}