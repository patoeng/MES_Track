using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using MES.Models;
using MES.Mvc.Excel;
using MES.Mvc.Helpers;
using MES.Mvc.Models;

namespace MES.Mvc.Controllers
{
    public class ProductsController : BaseController
    {
        

        public ActionResult Index(string reference, string article, int? sequenceId , string dum)
        {
            ViewBag.IsAdmin = UserControl.IsAdminUser(User);
            sequenceId = sequenceId ?? 0;
            reference = reference ?? "";
            article = article ?? "";
            var products = Db.Products.All().Include(m => m.Sequence)
                .Where(m => (m.Reference.Contains(reference.ToUpper()) || reference == "") && (m.ArticleNumber.Contains(article) || article == "") && (m.SequenceId == sequenceId || sequenceId == 0));
            ViewBag.SearchString = reference;

            var sequences = Db.ProductSequences.All().ToList();
            sequences.Insert(0, new ProductSequence { Id = 0, Name = @"All Sequences" });
            ViewBag.SequenceId = new SelectList(sequences, "Id", "Name", sequenceId );
            ViewBag.reference = reference;
            ViewBag.article = article;

            return View(products.ToList());
        }
        public ActionResult Import()
        {
            ViewBag.IsAdmin = UserControl.IsAdminUser(User);
            return View();
        }
        [HttpPost]
        public ActionResult FileImport(ExcelFileModel model)
        {
            ViewBag.IsAdmin = UserControl.IsAdminUser(User);
            string fileName = "";
            string path = "";
            try
            {
                if (model.File.ContentLength > 0)
                {
                    fileName = Path.GetFileName(model.File.FileName);
                    path = Path.Combine(Server.MapPath("~/UploadedFiles/xlsx/"), fileName);
                    model.File.SaveAs(path);
                }
                ViewBag.Filename = path;
                ViewBag.Message = "File Uploaded Successfully!!";
                var list = ProductExcelImportToList.Parse(path);

                    ViewBag.ListCount = list.Count;

                return View(list);
            }
            catch (Exception ex)
            {
                ViewBag.Filename = fileName;
                ViewBag.Message = "File upload failed!! "+ex.Message;
                return View(new List<Product>());
            }
        }
        [HttpPost]
        [MultipleButton(Name = "action", Argument = "Index")]
        public ActionResult Index(string reference, string article, int? sequenceId)
        {
            ViewBag.IsAdmin = UserControl.IsAdminUser(User);
            sequenceId = sequenceId ?? 0;
            reference = reference ?? "";
            article = article ?? "";
            var products = Db.Products.All().Include(m => m.Sequence)
                .Where(m=>( m.Reference.Contains(reference.ToUpper()) || reference=="") && (m.ArticleNumber.Contains(article)|| article=="")&&(m.SequenceId==sequenceId || sequenceId==0));
            ViewBag.SearchString = reference;

            var sequences = Db.ProductSequences.All().ToList();
            sequences.Insert(0, new ProductSequence { Id = 0, Name = @"All Sequences" });
            ViewBag.SequenceId = new SelectList(sequences, "Id", "Name", sequenceId);
            ViewBag.reference = reference;
            ViewBag.article = article;

            return View(products.ToList());
        }
        [HttpPost]
        [MultipleButton(Name = "action", Argument = "Excel")]
        public ActionResult Excel(string reference, string article, int? sequenceId)
        {
            ViewBag.IsAdmin = UserControl.IsAdminUser(User);
            sequenceId = sequenceId ?? 0;
            reference = reference ?? "";
            article = article ?? "";
            var products = Db.Products.All().Include(m => m.Sequence)
                .Where(m => (m.Reference.Contains(reference.ToUpper()) || reference == "") && (m.ArticleNumber.Contains(article) || article == "") && (m.SequenceId == sequenceId || sequenceId == 0));
            ViewBag.SearchString = reference;

            var sequences = Db.ProductSequences.All().ToList();
  
           
            ViewBag.reference = reference;
            ViewBag.article = article;

            ViewBag.ExcelFile = SummaryReports.ExportProduct(products.ToList(), sequences, Db.ProductSequenceItems.All().ToList());

            sequences.Insert(0, new ProductSequence { Id = 0, Name = @"All Sequences" });
            ViewBag.SequenceId = new SelectList(sequences, "Id", "Name", sequenceId ?? 0);
            return View();
        }
        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = Db.Products.GetById(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.IsAdmin = UserControl.IsAdminUser(User);
            return View(product);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            ViewBag.SequenceId = new SelectList(Db.ProductSequences.All(), "Id", "Name");
            ViewBag.IsAdmin = UserControl.IsAdminUser(User);
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Reference,ArticleNumber,SequenceId")] Product product)
        {
           
            if (ModelState.IsValid)
            {
                Db.Products.Add(product);
                Db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IsAdmin = UserControl.IsAdminUser(User);
            return View(product);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = Db.Products.GetById(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.IsAdmin = UserControl.IsAdminUser(User);
            ViewBag.SequenceId = new SelectList(Db.ProductSequences.All(), "Id", "Name",product.SequenceId);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Reference,ArticleNumber,SequenceId")] Product product)
        {
            if (ModelState.IsValid)
            {
                Db.Products.Update(product);
                Db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IsAdmin = UserControl.IsAdminUser(User);
            return View(product);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = Db.Products.GetById(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.IsAdmin = UserControl.IsAdminUser(User);
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = Db.Products.GetById(id);
            Db.Products.Delete(product);
            Db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpPost]
        public JsonResult ImportInsert(int? id ,string reference, string article, int  sequenceId)
        {
            var insert = "Insert ";
            try
            {
                var prod = Db.Products.All().FirstOrDefault(m => m.Reference == reference);
                if (prod == null)
                {
                    prod = new Product
                    {
                        SequenceId = sequenceId,
                        ArticleNumber = article,
                        Reference = reference
                    };
                    Db.Products.Add(prod);
                }
                else
                {
                    prod.SequenceId = sequenceId;
                    prod.ArticleNumber = article;
                    prod.Reference = reference;
                    Db.Products.Update(prod);
                    insert = "Update ";
                }
                Db.Products.SaveChanges();
                return Json(new {Success = true, Id = id, Insert= insert });
            }
            catch
            {
                return Json(new { Success = false, Id = id , Insert= insert });
            }
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
