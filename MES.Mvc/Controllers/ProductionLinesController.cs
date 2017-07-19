using System.Linq;
using System.Net;
using System.Web.Mvc;
using MES.Models;
using MES.Mvc.Helpers;

namespace MES.Mvc.Controllers
{
    public class ProductionLinesController : BaseController
    {
      

        public ActionResult Index(string searchString)
        {
            searchString = searchString ?? "*";
            var productionLines = searchString == "*"
                ? Db.ProductionLines.All()
                : Db.ProductionLines.All().Where(m => m.Name.Contains(searchString));
            ViewBag.SearchString = searchString;
            ViewBag.IsAdmin = UserControl.IsAdminUser(User);
            return View(productionLines.ToList());
        }

        // GET: ProductionLines/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductionLine productionLine = Db.ProductionLines.GetById(id);
            if (productionLine == null)
            {
                return HttpNotFound();
            }
            ViewBag.IsAdmin = UserControl.IsAdminUser(User);
            return View(productionLine);
        }

        // GET: ProductionLines/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ProductionLines/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] ProductionLine productionLine)
        {
            if (ModelState.IsValid)
            {
                Db.ProductionLines.Add(productionLine);
                Db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IsAdmin = UserControl.IsAdminUser(User);
            return View(productionLine);
        }

        // GET: ProductionLines/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductionLine productionLine = Db.ProductionLines.GetById(id);
            if (productionLine == null)
            {
                return HttpNotFound();
            }
            ViewBag.IsAdmin = UserControl.IsAdminUser(User);
            return View(productionLine);
        }

        // POST: ProductionLines/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] ProductionLine productionLine)
        {
            if (ModelState.IsValid)
            {
                Db.ProductionLines.Update(productionLine);
                Db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IsAdmin = UserControl.IsAdminUser(User);
            return View(productionLine);
        }

        // GET: ProductionLines/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductionLine productionLine = Db.ProductionLines.GetById(id);
            if (productionLine == null)
            {
                return HttpNotFound();
            }
            ViewBag.IsAdmin = UserControl.IsAdminUser(User);
            return View(productionLine);
        }

        // POST: ProductionLines/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ProductionLine productionLine = Db.ProductionLines.GetById(id);
            Db.ProductionLines.Delete(productionLine);
            Db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                //db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
