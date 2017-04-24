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
    public class ProductSequenceItemsController : BaseController
    {
        

        // GET: ProductSequenceItems
       

        public ActionResult Index(string searchString)
        {

            var productSequenceItems = searchString == "*"
                ? db.ProductSequenceItems.All().Include(p => p.MachineFamily).Include(p => p.ProductSequence)
                : db.ProductSequenceItems.All().Include(p => p.MachineFamily).Include(p => p.ProductSequence).Where(m=>m.MachineFamily.Name.Contains(searchString));
            ViewBag.SearchString = searchString;
            return View(productSequenceItems.ToList());
        }

        // GET: ProductSequenceItems/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductSequenceItem productSequenceItem = db.ProductSequenceItems.GetById(id);
            if (productSequenceItem == null)
            {
                return HttpNotFound();
            }
            return View(productSequenceItem);
        }

        // GET: ProductSequenceItems/Create
        public ActionResult Create()
        {
            ViewBag.MachineFamilyId = new SelectList(db.MachineFamilies.All(), "Id", "Name");
            ViewBag.ProductSequenceId = new SelectList(db.ProductSequences.All(), "Id", "Name");
            return View();
        }

        // POST: ProductSequenceItems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Level,MachineFamilyId,ProductSequenceId")] ProductSequenceItem productSequenceItem)
        {
            if (ModelState.IsValid)
            {
                db.ProductSequenceItems.Add(productSequenceItem);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MachineFamilyId = new SelectList(db.MachineFamilies.All(), "Id", "Name", productSequenceItem.MachineFamilyId);
            ViewBag.ProductSequenceId = new SelectList(db.ProductSequences.All(), "Id", "Name", productSequenceItem.ProductSequenceId);
            return View(productSequenceItem);
        }

        // GET: ProductSequenceItems/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductSequenceItem productSequenceItem = db.ProductSequenceItems.GetById(id);
            if (productSequenceItem == null)
            {
                return HttpNotFound();
            }
            ViewBag.MachineFamilyId = new SelectList(db.MachineFamilies.All(), "Id", "Name", productSequenceItem.MachineFamilyId);
            ViewBag.ProductSequenceId = new SelectList(db.ProductSequences.All(), "Id", "Name", productSequenceItem.ProductSequenceId);
            return View(productSequenceItem);
        }

        // POST: ProductSequenceItems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Level,MachineFamilyId,ProductSequenceId")] ProductSequenceItem productSequenceItem)
        {
            if (ModelState.IsValid)
            {
                db.ProductSequenceItems.Update(productSequenceItem);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MachineFamilyId = new SelectList(db.MachineFamilies.All(), "Id", "Name", productSequenceItem.MachineFamilyId);
            ViewBag.ProductSequenceId = new SelectList(db.ProductSequences.All(), "Id", "Name", productSequenceItem.ProductSequenceId);
            return View(productSequenceItem);
        }
        
        // GET: ProductSequenceItems/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductSequenceItem productSequenceItem = db.ProductSequenceItems.GetById(id);
            if (productSequenceItem == null)
            {
                return HttpNotFound();
            }
            return View(productSequenceItem);
        }

        // POST: ProductSequenceItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ProductSequenceItem productSequenceItem = db.ProductSequenceItems.GetById(id);
            db.ProductSequenceItems.Delete(productSequenceItem);
            db.SaveChanges();
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
