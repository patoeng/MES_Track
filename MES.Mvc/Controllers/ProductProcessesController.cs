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
    public class ProductProcessesController : BaseController
    {
       

        // GET: ProductProcesses
        public ActionResult Index(string reference,string machineCode, string workorderNumber, string fromDateTime, string toDateTime)
        {
            var cfromDateTime =string.IsNullOrEmpty(fromDateTime) ? DateTime.Now: DateTime.Parse(fromDateTime);
            var ctoDateTime =  string.IsNullOrEmpty(fromDateTime) ? DateTime.Now.AddDays(1) : DateTime.Parse(toDateTime);

            var productProcesses =
                db.ProductProcesses.All()
                    .Include(p => p.Machine).Include(p => p.Product).Include(p => p.Workorder)
                    .Where(m =>
                        (m.FullName.Contains(reference) || reference == "") &&
                        (m.Machine.SerialNumber.Contains(machineCode) || machineCode == "") &&
                        (m.Workorder.Number.Contains(workorderNumber) || workorderNumber == "") &&
                        m.DateTime >= cfromDateTime &&
                        m.DateTime <= ctoDateTime
                    );
           
            ViewBag.fromDateTime = cfromDateTime.ToString("yyyy-MM-dd HH:mm");
            ViewBag.toDateTime = ctoDateTime.ToString("yyyy-MM-dd HH:mm");
            return View(productProcesses.ToList());
        }

        // GET: ProductProcesses/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductProcess productProcess = db.ProductProcesses.GetById(id);
            if (productProcess == null)
            {
                return HttpNotFound();
            }
            return View(productProcess);
        }

        // GET: ProductProcesses/Create
        public ActionResult Create()
        {
            ViewBag.MachineId = new SelectList(db.Machines.All(), "Id", "SerialNumber");
            ViewBag.ProductId = new SelectList(db.Products.All(), "Id", "Reference");
            ViewBag.WorkorderId = new SelectList(db.Workorders.All(), "Id", "Number");
            return View();
        }

        // POST: ProductProcesses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FullName,WorkorderId,DateTime,MachineId,ProductId,Result")] ProductProcess productProcess)
        {
            if (ModelState.IsValid)
            {
                db.ProductProcesses.Add(productProcess);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MachineId = new SelectList(db.Machines.All(), "Id", "SerialNumber", productProcess.MachineId);
            ViewBag.ProductId = new SelectList(db.Products.All(), "Id", "Reference", productProcess.ProductId);
            ViewBag.WorkorderId = new SelectList(db.Workorders.All(), "Id", "Number", productProcess.WorkorderId);
            return View(productProcess);
        }

        // GET: ProductProcesses/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductProcess productProcess = db.ProductProcesses.GetById(id);
            if (productProcess == null)
            {
                return HttpNotFound();
            }
            ViewBag.MachineId = new SelectList(db.Machines.All(), "Id", "SerialNumber", productProcess.MachineId);
            ViewBag.ProductId = new SelectList(db.Products.All(), "Id", "Reference", productProcess.ProductId);
            ViewBag.WorkorderId = new SelectList(db.Workorders.All(), "Id", "Number", productProcess.WorkorderId);
            return View(productProcess);
        }

        // POST: ProductProcesses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FullName,WorkorderId,DateTime,MachineId,ProductId,Result")] ProductProcess productProcess)
        {
            if (ModelState.IsValid)
            {
                db.ProductProcesses.Update(productProcess);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MachineId = new SelectList(db.Machines.All(), "Id", "SerialNumber", productProcess.MachineId);
            ViewBag.ProductId = new SelectList(db.Products.All(), "Id", "Reference", productProcess.ProductId);
            ViewBag.WorkorderId = new SelectList(db.Workorders.All(), "Id", "Number", productProcess.WorkorderId);
            return View(productProcess);
        }

        // GET: ProductProcesses/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductProcess productProcess = db.ProductProcesses.GetById(id);
            if (productProcess == null)
            {
                return HttpNotFound();
            }
            return View(productProcess);
        }

        // POST: ProductProcesses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ProductProcess productProcess = db.ProductProcesses.GetById(id);
            db.ProductProcesses.Delete(productProcess);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
              //  db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
