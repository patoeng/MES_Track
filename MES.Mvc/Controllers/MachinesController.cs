using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MES.Data;
using MES.Models;
using MES.Mvc.Helpers;

namespace MES.Mvc.Controllers
{
    public class MachinesController : BaseController
    {
       

        // GET: Machines
        public ActionResult Index(string searchString)
        {
            var machines = searchString == "*"
                ? Db.Machines.All().Include(m => m.MachineFamily).Include(m => m.ProductionLine)
                : Db.Machines.All().Include(m => m.MachineFamily).Include(m => m.ProductionLine).Where(m=>m.Name.Contains(searchString));
            ViewBag.SearchString = searchString;
            ViewBag.IsAdmin = UserControl.IsAdminUser(User);
            return View(machines.ToList());
        }

        // GET: Machines/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Machine machine = Db.Machines.GetById(id);
            if (machine == null)
            {
                return HttpNotFound();
            }
            ViewBag.IsAdmin = UserControl.IsAdminUser(User);
            return View(machine);
        }
        // GET: Machines/Details/5
       
        // GET: Machines/Create
        public ActionResult Create()
        {
            ViewBag.MachineFamilyId = new SelectList(Db.MachineFamilies.All(), "Id", "Name");
            ViewBag.ProductionLineId = new SelectList(Db.ProductionLines.All(), "Id", "Name");
            ViewBag.IsAdmin = UserControl.IsAdminUser(User);
            return View();
        }

        // POST: Machines/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,SerialNumber,Name,MachineFamilyId,ProductionLineId,MachineStatus,TraceabilityStatus")] Machine machine)
        {
            if (ModelState.IsValid)
            {
                Db.Machines.Add(machine);
                Db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MachineFamilyId = new SelectList(Db.MachineFamilies.All(), "Id", "Name", machine.MachineFamilyId);
            ViewBag.ProductionLineId = new SelectList(Db.ProductionLines.All(), "Id", "Name", machine.ProductionLineId);
            ViewBag.IsAdmin = UserControl.IsAdminUser(User);
            return View(machine);
        }

        // GET: Machines/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Machine machine = Db.Machines.GetById(id);
            if (machine == null)
            {
                return HttpNotFound();
            }
            ViewBag.MachineFamilyId = new SelectList(Db.MachineFamilies.All(), "Id", "Name", machine.MachineFamilyId);
            ViewBag.ProductionLineId = new SelectList(Db.ProductionLines.All(), "Id", "Name", machine.ProductionLineId);
            ViewBag.IsAdmin = UserControl.IsAdminUser(User);
            return View(machine);
        }

        // POST: Machines/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,SerialNumber,Name,MachineFamilyId,ProductionLineId,MachineStatus,TraceabilityStatus")] Machine machine)
        {
            if (ModelState.IsValid)
            {
                Db.Machines.Update(machine);
                Db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MachineFamilyId = new SelectList(Db.MachineFamilies.All(), "Id", "Name", machine.MachineFamilyId);
            ViewBag.ProductionLineId = new SelectList(Db.ProductionLines.All(), "Id", "Name", machine.ProductionLineId);
            ViewBag.IsAdmin = UserControl.IsAdminUser(User);
            return View(machine);
        }

        // GET: Machines/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Machine machine = Db.Machines.GetById(id);
            if (machine == null)
            {
                return HttpNotFound();
            }
            ViewBag.IsAdmin = UserControl.IsAdminUser(User);
            return View(machine);
        }

        // POST: Machines/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Machine machine = Db.Machines.GetById(id);
            Db.Machines.Delete(machine);
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
