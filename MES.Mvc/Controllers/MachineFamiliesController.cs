using System.Linq;
using System.Net;
using System.Web.Mvc;
using MES.Models;
using MES.Mvc.Helpers;

namespace MES.Mvc.Controllers
{
    public class MachineFamiliesController : BaseController
    {
        public ActionResult Index(string searchString)
        {
            var machineFamilies = searchString == "*"
                ? Db.MachineFamilies.All()
                : Db.MachineFamilies.All().Where(m => m.Name.Contains(searchString));
            ViewBag.SearchString = searchString;
            ViewBag.IsAdmin = UserControl.IsAdminUser(User);
            return View(machineFamilies.ToList());
        }

        // GET: MachineFamilies/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MachineFamily machineFamily = Db.MachineFamilies.GetById(id);
            if (machineFamily == null)
            {
                return HttpNotFound();
            }
            return View(machineFamily);
        }

        // GET: MachineFamilies/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MachineFamilies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] MachineFamily machineFamily)
        {
            if (ModelState.IsValid)
            {
                Db.MachineFamilies.Add(machineFamily);
                Db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(machineFamily);
        }

        // GET: MachineFamilies/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MachineFamily machineFamily = Db.MachineFamilies.GetById(id);
            if (machineFamily == null)
            {
                return HttpNotFound();
            }
            return View(machineFamily);
        }

        // POST: MachineFamilies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] MachineFamily machineFamily)
        {
            if (ModelState.IsValid)
            {
                Db.MachineFamilies.Update(machineFamily);
                Db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(machineFamily);
        }

        // GET: MachineFamilies/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MachineFamily machineFamily = Db.MachineFamilies.GetById(id);
            if (machineFamily == null)
            {
                return HttpNotFound();
            }
            return View(machineFamily);
        }

        // POST: MachineFamilies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MachineFamily machineFamily = Db.MachineFamilies.GetById(id);
            Db.MachineFamilies.Delete(machineFamily);
            Db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
               // db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
