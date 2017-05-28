
using System.Data.Entity;

using System.Linq;

using System.Web.Mvc;

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
            ViewBag.Message = "Front Line 1,2,3 Traceability.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "sales@edfsystem.com.";

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