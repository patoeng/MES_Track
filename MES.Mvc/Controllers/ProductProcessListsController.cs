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
    public class ProductProcessListsController : BaseController
    {
       

        // GET: ProductProcessLists
        public ActionResult Index()
        {
            var productProcessLists = Db.ProductProcessLists.All().Include(p => p.Product).Include(p => p.Workorder);
            return View(productProcessLists.ToList());
        }

        // GET: ProductProcessLists/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductProcessList productProcessList = Db.ProductProcessLists.GetById(id);
            if (productProcessList == null)
            {
                return HttpNotFound();
            }
            return View(productProcessList);
        }

        // GET: ProductProcessLists/Create
        public ActionResult Create()
        {
            ViewBag.ProductId = new SelectList(Db.Products.All(), "Id", "Reference");
            ViewBag.WorkorderId = new SelectList(Db.Workorders.All(), "Id", "Number");
            return View();
        }

        // POST: ProductProcessLists/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FullName,WorkorderId,DateTime,ProductId,Remarks,UseProductSequenceId")] ProductProcessList productProcessList)
        {
            if (ModelState.IsValid)
            {
                Db.ProductProcessLists.Add(productProcessList);
                Db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ProductId = new SelectList(Db.Products.All(), "Id", "Reference", productProcessList.ProductId);
            ViewBag.WorkorderId = new SelectList(Db.Workorders.All(), "Id", "Number", productProcessList.WorkorderId);
            return View(productProcessList);
        }

        // GET: ProductProcessLists/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductProcessList productProcessList = Db.ProductProcessLists.GetById(id);
            if (productProcessList == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProductId = new SelectList(Db.Products.All(), "Id", "Reference", productProcessList.ProductId);
            ViewBag.WorkorderId = new SelectList(Db.Workorders.All(), "Id", "Number", productProcessList.WorkorderId);
            return View(productProcessList);
        }

        // POST: ProductProcessLists/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FullName,WorkorderId,DateTime,ProductId,Remarks,UseProductSequenceId")] ProductProcessList productProcessList)
        {
            if (ModelState.IsValid)
            {
                Db.ProductProcessLists.Update(productProcessList);
                Db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProductId = new SelectList(Db.Products.All(), "Id", "Reference", productProcessList.ProductId);
            ViewBag.WorkorderId = new SelectList(Db.Workorders.All(), "Id", "Number", productProcessList.WorkorderId);
            return View(productProcessList);
        }

        // GET: ProductProcessLists/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductProcessList productProcessList = Db.ProductProcessLists.GetById(id);
            if (productProcessList == null)
            {
                return HttpNotFound();
            }
            return View(productProcessList);
        }

        // POST: ProductProcessLists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ProductProcessList productProcessList = Db.ProductProcessLists.GetById(id);
            Db.ProductProcessLists.Delete(productProcessList);
            Db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Db.ProductProcessLists.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
