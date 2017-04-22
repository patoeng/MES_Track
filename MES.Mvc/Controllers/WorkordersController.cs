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

namespace MES.Mvc.Controllers
{
    public class WorkordersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Workorders
        public ActionResult Index()
        {
            var workorders = db.Workorders.Include(w => w.EntryThroughMachine).Include(w => w.Reference);
            return View(workorders.ToList());
        }

        // GET: Workorders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Workorder workorder = db.Workorders.Find(id);
            if (workorder == null)
            {
                return HttpNotFound();
            }
            return View(workorder);
        }

        // GET: Workorders/Create
        public ActionResult Create()
        {
            ViewBag.EntryThroughMachineId = new SelectList(db.Machines, "Id", "SerialNumber");
            ViewBag.ReferenceId = new SelectList(db.Products, "Id", "Reference");
            return View();
        }

        // POST: Workorders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Number,ReferenceId,Quantity,DateTime,EntryThroughMachineId")] Workorder workorder)
        {
            if (ModelState.IsValid)
            {
                db.Workorders.Add(workorder);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.EntryThroughMachineId = new SelectList(db.Machines, "Id", "SerialNumber", workorder.EntryThroughMachineId);
            ViewBag.ReferenceId = new SelectList(db.Products, "Id", "Reference", workorder.ReferenceId);
            return View(workorder);
        }

        // GET: Workorders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Workorder workorder = db.Workorders.Find(id);
            if (workorder == null)
            {
                return HttpNotFound();
            }
            ViewBag.EntryThroughMachineId = new SelectList(db.Machines, "Id", "SerialNumber", workorder.EntryThroughMachineId);
            ViewBag.ReferenceId = new SelectList(db.Products, "Id", "Reference", workorder.ReferenceId);
            return View(workorder);
        }

        // POST: Workorders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Number,ReferenceId,Quantity,DateTime,EntryThroughMachineId")] Workorder workorder)
        {
            if (ModelState.IsValid)
            {
                db.Entry(workorder).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.EntryThroughMachineId = new SelectList(db.Machines, "Id", "SerialNumber", workorder.EntryThroughMachineId);
            ViewBag.ReferenceId = new SelectList(db.Products, "Id", "Reference", workorder.ReferenceId);
            return View(workorder);
        }

        // GET: Workorders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Workorder workorder = db.Workorders.Find(id);
            if (workorder == null)
            {
                return HttpNotFound();
            }
            return View(workorder);
        }

        // POST: Workorders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Workorder workorder = db.Workorders.Find(id);
            db.Workorders.Remove(workorder);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
