using System.Linq;
using System.Net;
using System.Web.Mvc;
using MES.Models;
using MES.Mvc.Helpers;

namespace MES.Mvc.Controllers
{
    public class ProductSequencesController : BaseController
    {
        public ActionResult Index(string searchString)
        {
            var productSequences = searchString == "*"
                ? Db.ProductSequences.All()
                : Db.ProductSequences.All().Where(m => m.Name.Contains(searchString));
            ViewBag.SearchString = searchString;
            ViewBag.IsAdmin = UserControl.IsAdminUser(User);
            return View(productSequences.ToList());
        }

        // GET: ProductSequences/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductSequence productSequence = Db.ProductSequences.GetById(id);
            if (productSequence == null)
            {
                return HttpNotFound();
            }
            ViewBag.IsAdmin = UserControl.IsAdminUser(User);
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
                Db.ProductSequences.Add(productSequence);
                Db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IsAdmin = UserControl.IsAdminUser(User);
            return View(productSequence);
        }

        // GET: ProductSequences/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductSequence productSequence = Db.ProductSequences.GetById(id);
            if (productSequence == null)
            {
                return HttpNotFound();
            }
            ViewBag.IsAdmin = UserControl.IsAdminUser(User);
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
                Db.ProductSequences.Update(productSequence);
                Db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IsAdmin = UserControl.IsAdminUser(User);
            return View(productSequence);
        }

        // GET: ProductSequences/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductSequence productSequence = Db.ProductSequences.GetById(id);
            if (productSequence == null)
            {
                return HttpNotFound();
            }
            ViewBag.IsAdmin = UserControl.IsAdminUser(User);
            return View(productSequence);
        }

        // POST: ProductSequences/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ProductSequence productSequence = Db.ProductSequences.GetById(id);
            Db.ProductSequences.Delete(productSequence);
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
