
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using MES.Mvc.Helpers;

namespace MES.Mvc.Controllers
{
    public class HomeController : BaseController
    {
       
        public ActionResult Index()
        {
            var machines = Db.Machines.All().Include(m => m.MachineFamily).Include(m => m.ProductionLine);
            ViewBag.IsAdmin = UserControl.IsAdminUser(User);
            return View(machines.ToList());
        }

        public ActionResult About()
        {
            ViewBag.Message = "Front Line 1,2,3 Traceability.";
            ViewBag.IsAdmin = UserControl.IsAdminUser(User);
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "sales@edfsystem.com.";
            ViewBag.IsAdmin = UserControl.IsAdminUser(User);
            return View();
        }

        [HttpPost]
        public JsonResult LastStatus()
        {

            var productProcess = Db.ProductProcesses.All()
                .GroupBy(m => m.MachineId)
                .Select(g=>g.OrderByDescending(x=>x.DateTime).Take(1));  
            return Json(new { Success = true, productProcess});
        }
    }
}