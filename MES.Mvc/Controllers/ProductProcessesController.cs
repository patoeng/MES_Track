using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using MES.Models;
using MES.Mvc.Excel;
using MES.Mvc.Helpers;

namespace MES.Mvc.Controllers
{
    public class ProductProcessesController : BaseController
    {
        // GET: ProductProcesses
        public ActionResult Index()
        {
            var cfromDateTime = DateTime.Now;
            var ctoDateTime = DateTime.Now.AddDays(1);
            ViewBag.fromDateTime = cfromDateTime.ToString("yyyy-MM-dd HH:mm");
            ViewBag.toDateTime = ctoDateTime.ToString("yyyy-MM-dd HH:mm");
            var ddProcessResult = Enum.GetNames(typeof(ProcessResult))
              .Select(x => x.ToString())
              .ToList();
            ddProcessResult.Add("All");
            ViewBag.State = new SelectList(ddProcessResult);
            ViewBag.LastStatus = new SelectList(new List<string> { "No", "Yes" });
            ViewBag.IsAdmin = UserControl.IsAdminUser(User);
            return View();
        }

        [HttpPost]
        [MultipleButton(Name = "action", Argument = "Search")]
        public ActionResult Search(string reference,string machineCode, string workorderNumber, string fromDateTime, string toDateTime, string state, string lastStatus)
        {
            ProcessResult lastStateEnum;
            Enum.TryParse(state, out lastStateEnum);
            //
            var cfromDateTime = string.IsNullOrEmpty(fromDateTime) ? DateTime.Now : DateTime.Parse(fromDateTime);
            var ctoDateTime = string.IsNullOrEmpty(fromDateTime) ? DateTime.Now.AddDays(1) : DateTime.Parse(toDateTime);
            ViewBag.fromDateTime = cfromDateTime.ToString("yyyy-MM-dd HH:mm");
            ViewBag.toDateTime = ctoDateTime.ToString("yyyy-MM-dd HH:mm");

            var ddProcessResult = Enum.GetNames(typeof(ProcessResult))
                .Select(x => x.ToString())
                .ToList();
            ddProcessResult.Add("All");
            ViewBag.State = new SelectList(ddProcessResult);
            ViewBag.LastStatus = new SelectList(new List<string> { "No", "Yes" });
            if (lastStatus == "No")
            {
                var productProcesses =
                    Db.ProductProcesses.All()
                        .Include(p => p.Machine).Include(p => p.Product).Include(p => p.Workorder)
                        .Where(m =>
                            (m.FullName.Contains(reference) || reference == "") &&
                            (m.Machine.SerialNumber.Contains(machineCode) || machineCode == "") &&
                            (m.Workorder.Number.Contains(workorderNumber) || workorderNumber == "") &&
                            m.DateTime >= cfromDateTime &&
                            m.DateTime <= ctoDateTime &&
                            (m.Result == lastStateEnum || state.Contains("All"))
                        ).OrderByDescending(m => m.DateTime).Take(500);
                ViewBag.IsAdmin = UserControl.IsAdminUser(User);
                return View(productProcesses.OrderBy(m=>m.DateTime).ToList());
            }
            else
            {
                var productProcesses =
                    Db.ProductProcesses.All()
                        .Where(m =>
                            (m.FullName.Contains(reference) || reference == "") &&
                            (m.Machine.SerialNumber.Contains(machineCode) || machineCode == "") &&
                            (m.Workorder.Number.Contains(workorderNumber) || workorderNumber == "") &&
                            m.DateTime >= cfromDateTime &&
                            m.DateTime <= ctoDateTime &&
                            (m.Result == lastStateEnum || state.Contains("All")))
                        .OrderBy(m => m.DateTime);
                var r = from p in productProcesses
                        group p by p.FullName into g
                        select new { ProductProcess = g.ToList() };


                List<ProductProcess> pp = new List<ProductProcess>();
                foreach (var kk in r.ToList())
                {
                    var ll = kk.ProductProcess.OrderByDescending(m => m.DateTime).FirstOrDefault();
                    pp.Add(ll);
                }
                ViewBag.IsAdmin = UserControl.IsAdminUser(User);
                return View(pp.OrderBy(m=>m.DateTime).Take(1000));
            }
           
        }

        [HttpPost]
        [MultipleButton(Name = "action", Argument = "Excel")]
        public ActionResult Excel(string reference, string machineCode, string workorderNumber, string fromDateTime,
            string toDateTime, string state, string lastStatus)
        {
            ProcessResult lastStateEnum;
            Enum.TryParse(state, out lastStateEnum);
            //
            var cfromDateTime = string.IsNullOrEmpty(fromDateTime) ? DateTime.Now : DateTime.Parse(fromDateTime);
            var ctoDateTime = string.IsNullOrEmpty(fromDateTime) ? DateTime.Now.AddDays(1) : DateTime.Parse(toDateTime);
            ViewBag.fromDateTime = cfromDateTime.ToString("yyyy-MM-dd HH:mm");
            ViewBag.toDateTime = ctoDateTime.ToString("yyyy-MM-dd HH:mm");

            var ddProcessResult = Enum.GetNames(typeof(ProcessResult))
                .Select(x => x.ToString())
                .ToList();
            ddProcessResult.Add("All");
            ViewBag.State = new SelectList(ddProcessResult);
            ViewBag.LastStatus = new SelectList(new List<string> {"No", "Yes"});
            if (lastStatus == "No")
            {
                var productProcesses =
                    Db.ProductProcesses.All()
                        .Include(p => p.Machine).Include(p => p.Product).Include(p => p.Workorder)
                        .Where(m =>
                            (m.FullName.Contains(reference) || reference == "") &&
                            (m.Machine.SerialNumber.Contains(machineCode) || machineCode == "") &&
                            (m.Workorder.Number.Contains(workorderNumber) || workorderNumber == "") &&
                            m.DateTime >= cfromDateTime &&
                            m.DateTime <= ctoDateTime &&
                            (m.Result == lastStateEnum || state.Contains("All"))
                        ).OrderByDescending(m => m.DateTime);
                var j = productProcesses.ToList();
                ViewBag.ExcelFile = SummaryReports.ProductProcessToExcelFile(j);
                ViewBag.IsAdmin = UserControl.IsAdminUser(User);
                return View();
            }
            else
            {
                var productProcesses =
                    Db.ProductProcesses.All()
                        .Where(m =>
                            (m.FullName.Contains(reference) || reference == "") &&
                            (m.Machine.SerialNumber.Contains(machineCode) || machineCode == "") &&
                            (m.Workorder.Number.Contains(workorderNumber) || workorderNumber == "") &&
                            m.DateTime >= cfromDateTime &&
                            m.DateTime <= ctoDateTime &&
                            (m.Result == lastStateEnum || state.Contains("All")))
                            .OrderByDescending(m => m.DateTime);

               var r = from p in productProcesses
                                        group p by p.FullName into g
                                        select new { ProductProcess = g.ToList() };


               List<ProductProcess> pp = new List<ProductProcess>();
                foreach (var kk in r.ToList())
                {
                    var ll = kk.ProductProcess.OrderByDescending(m => m.DateTime).FirstOrDefault();
                    pp.Add(ll);
                }
                ViewBag.ExcelFile = SummaryReports.ProductProcessToExcelFile(pp.OrderBy(m => m.DateTime).ToList());
                ViewBag.IsAdmin = UserControl.IsAdminUser(User);
                return View();
            }
        }

        // GET: ProductProcesses/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductProcess productProcess = Db.ProductProcesses.GetById(id);
            if (productProcess == null)
            {
                return HttpNotFound();
            }
            ViewBag.IsAdmin = UserControl.IsAdminUser(User);
            return View(productProcess);
        }

        // GET: ProductProcesses/Create
        public ActionResult Create()
        {
            ViewBag.MachineId = new SelectList(Db.Machines.All(), "Id", "SerialNumber");
            ViewBag.ProductId = new SelectList(Db.Products.All(), "Id", "Reference");
            ViewBag.WorkorderId = new SelectList(Db.Workorders.All(), "Id", "Number");
            ViewBag.IsAdmin = UserControl.IsAdminUser(User);
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
                Db.ProductProcesses.Add(productProcess);
                Db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MachineId = new SelectList(Db.Machines.All(), "Id", "SerialNumber", productProcess.MachineId);
            ViewBag.ProductId = new SelectList(Db.Products.All(), "Id", "Reference", productProcess.ProductId);
            ViewBag.WorkorderId = new SelectList(Db.Workorders.All(), "Id", "Number", productProcess.WorkorderId);
            ViewBag.IsAdmin = UserControl.IsAdminUser(User);
            return View(productProcess);
        }

        // GET: ProductProcesses/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductProcess productProcess = Db.ProductProcesses.GetById(id);
            if (productProcess == null)
            {
                return HttpNotFound();
            }
            ViewBag.MachineId = new SelectList(Db.Machines.All(), "Id", "SerialNumber", productProcess.MachineId);
            ViewBag.ProductId = new SelectList(Db.Products.All(), "Id", "Reference", productProcess.ProductId);
            ViewBag.WorkorderId = new SelectList(Db.Workorders.All(), "Id", "Number", productProcess.WorkorderId);
            ViewBag.IsAdmin = UserControl.IsAdminUser(User);
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
                Db.ProductProcesses.Update(productProcess);
                Db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MachineId = new SelectList(Db.Machines.All(), "Id", "SerialNumber", productProcess.MachineId);
            ViewBag.ProductId = new SelectList(Db.Products.All(), "Id", "Reference", productProcess.ProductId);
            ViewBag.WorkorderId = new SelectList(Db.Workorders.All(), "Id", "Number", productProcess.WorkorderId);
            ViewBag.IsAdmin = UserControl.IsAdminUser(User);
            return View(productProcess);
        }

        // GET: ProductProcesses/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductProcess productProcess = Db.ProductProcesses.GetById(id);
            if (productProcess == null)
            {
                return HttpNotFound();
            }
            ViewBag.IsAdmin = UserControl.IsAdminUser(User);
            return View(productProcess);
        }

        // POST: ProductProcesses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ProductProcess productProcess = Db.ProductProcesses.GetById(id);
            Db.ProductProcesses.Delete(productProcess);
            Db.SaveChanges();
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
