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
using MES.Mvc.Excel;
using MES.Mvc.Helpers;

namespace MES.Mvc.Controllers
{
    public class WorkordersController : BaseController
    {
     

        // GET: Workorders
        public ActionResult Index()
        {
            var cfromDateTime = DateTime.Now;
            var ctoDateTime = DateTime.Now.AddDays(1);
            ViewBag.fromDateTime = cfromDateTime.ToString("yyyy-MM-dd HH:mm");
            ViewBag.toDateTime = ctoDateTime.ToString("yyyy-MM-dd HH:mm");
            return View();
        }

        [HttpPost]
        [MultipleButton(Name = "action", Argument = "Search")]
        public ActionResult Search(string workOrder,  string fromDateTime,
            string toDateTime)
        {
            var cfromDateTime = string.IsNullOrEmpty(fromDateTime) ? DateTime.Now : DateTime.Parse(fromDateTime);
            var ctoDateTime = string.IsNullOrEmpty(fromDateTime) ? DateTime.Now.AddDays(1) : DateTime.Parse(toDateTime);
            ViewBag.fromDateTime = cfromDateTime.ToString("yyyy-MM-dd HH:mm");
            ViewBag.toDateTime = ctoDateTime.ToString("yyyy-MM-dd HH:mm");

            var workOrders = db.Workorders.All().Where(
                m => (m.Number.Contains(workOrder) || workOrder == "") &&
                     m.DateTime >= cfromDateTime &&
                     m.DateTime <= ctoDateTime
            ).OrderByDescending(k => k.DateTime);
            return View(workOrders.ToList());
        }
        [HttpPost]
        [MultipleButton(Name = "action", Argument = "Excel")]
        public ActionResult Excel(string workOrder, string fromDateTime,
           string toDateTime)
        {
            var cfromDateTime = string.IsNullOrEmpty(fromDateTime) ? DateTime.Now : DateTime.Parse(fromDateTime);
            var ctoDateTime = string.IsNullOrEmpty(fromDateTime) ? DateTime.Now.AddDays(1) : DateTime.Parse(toDateTime);
            ViewBag.fromDateTime = cfromDateTime.ToString("yyyy-MM-dd HH:mm");
            ViewBag.toDateTime = ctoDateTime.ToString("yyyy-MM-dd HH:mm");

            var workOrders = db.Workorders.All().Where(
                m => (m.Number.Contains(workOrder) || workOrder == "") &&
                     m.DateTime >= cfromDateTime &&
                     m.DateTime <= ctoDateTime
            ).OrderByDescending(k => k.DateTime);

            var j = workOrders.ToList();
            ViewBag.ExcelFile = SummaryReports.WorkOrderToExcelFile(j);
            return View(j);
        }
        // GET: Workorders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Workorder workorder = db.Workorders.GetById(id);
            if (workorder == null)
            {
                return HttpNotFound();
            }
            return View(workorder);
        }

        // GET: Workorders/Create
        public ActionResult Create()
        {
            ViewBag.EntryThroughMachineId = new SelectList(db.Machines.All(), "Id", "SerialNumber");
            ViewBag.ReferenceId = new SelectList(db.Products.All(), "Id", "Reference");
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

            ViewBag.EntryThroughMachineId = new SelectList(db.Machines.All(), "Id", "SerialNumber", workorder.EntryThroughMachineId);
            ViewBag.ReferenceId = new SelectList(db.Products.All(), "Id", "Reference", workorder.ReferenceId);
            return View(workorder);
        }

        // GET: Workorders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Workorder workorder = db.Workorders.GetById(id);
            if (workorder == null)
            {
                return HttpNotFound();
            }
            ViewBag.EntryThroughMachineId = new SelectList(db.Machines.All(), "Id", "SerialNumber", workorder.EntryThroughMachineId);
            ViewBag.ReferenceId = new SelectList(db.Products.All(), "Id", "Reference", workorder.ReferenceId);
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
                db.Workorders.Add(workorder);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.EntryThroughMachineId = new SelectList(db.Machines.All(), "Id", "SerialNumber", workorder.EntryThroughMachineId);
            ViewBag.ReferenceId = new SelectList(db.Products.All(), "Id", "Reference", workorder.ReferenceId);
            return View(workorder);
        }

        // GET: Workorders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Workorder workorder = db.Workorders.GetById(id);
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
            Workorder workorder = db.Workorders.GetById(id);
            db.Workorders.Delete(workorder);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
            //    db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
