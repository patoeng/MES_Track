using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using MES.Models;
using MES.Mvc.Helpers;

namespace MES.Mvc.Controllers
{
    public class ProductsController : BaseController
    {

        public ActionResult Index(string searchString)
        {
            ViewBag.IsAdmin = UserControl.IsAdminUser(User);
            var products = searchString == "*" ? Db.Products.All().Include(m => m.Sequence) : Db.Products.All().Include(m => m.Sequence).Where(m=> m.Reference.Contains(searchString.ToUpper()));
            ViewBag.SearchString = searchString;
            return View(products.ToList());
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
