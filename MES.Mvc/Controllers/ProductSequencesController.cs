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
    public class ProductSequencesController : BaseController
    {
      
        

        public ActionResult Index()
        {
           
            return View(db.ProductSequences.All().ToList());
        }

        // GET: ProductSequences/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductSequence productSequence = db.ProductSequences.GetById(id);
            if (productSequence == null)
            {
                return HttpNotFound();
            }
            return View(productSequence);
        }

        // GET: ProductSequences/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ProductSequences/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] ProductSequence productSequence)
        {
            if (ModelState.IsValid)
            {
                db.ProductSequences.Add(productSequence);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(productSequence);
        }

        // GET: ProductSequences/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductSequence productSequence = db.ProductSequences.GetById(id);
            if (productSequence == null)
            {
                return HttpNotFound();
            }
            return View(productSequence);
        }

        // POST: ProductSequences/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] ProductSequence productSequence)
        {
            if (ModelState.IsValid)
            {
                db.ProductSequences.Update(productSequence);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(productSequence);
        }

        // GET: ProductSequences/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductSequence productSequence = db.ProductSequences.GetById(id);
            if (productSequence == null)
            {
                return HttpNotFound();
            }
            return View(productSequence);
        }

        // POST: ProductSequences/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ProductSequence productSequence = db.ProductSequences.GetById(id);
            db.ProductSequences.Delete(productSequence);
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
