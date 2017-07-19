using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using MES.Models;
using MES.Mvc.Helpers;

namespace MES.Mvc.Controllers
{
    public class ProductSequenceItemsController : BaseController
    {
        

        // GET: ProductSequenceItems
       

        public ActionResult Index(string searchString, int? sequenceId)
        {
            if (searchString == null)
            {
                searchString = "*";
            }
            if (sequenceId == null)
            {
                sequenceId = 0;
            }
            var productSequenceItems = searchString == "*"
                ? Db.ProductSequenceItems.All().Include(p => p.MachineFamily).Include(p => p.ProductSequence).Where(m=>m.ProductSequenceId== sequenceId || sequenceId==0)
                : Db.ProductSequenceItems.All().Include(p => p.MachineFamily).Include(p => p.ProductSequence).Where(m=>m.MachineFamily.Name.Contains(searchString)&&(m.ProductSequenceId == sequenceId || sequenceId == 0));
            ViewBag.SearchString = searchString;
            ViewBag.IsAdmin = UserControl.IsAdminUser(User);
            return View(productSequenceItems.ToList());
        }

        // GET: ProductSequenceItems/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductSequenceItem productSequenceItem = Db.ProductSequenceItems.GetById(id);
            if (productSequenceItem == null)
            {
                return HttpNotFound();
            }
            ViewBag.IsAdmin = UserControl.IsAdminUser(User);
            return View(productSequenceItem);
        }

        // GET: ProductSequenceItems/Create
        public ActionResult Create()
        {
            ViewBag.MachineFamilyId = new SelectList(Db.MachineFamilies.All(), "Id", "Name");
            ViewBag.ProductSequenceId = new SelectList(Db.ProductSequences.All(), "Id", "Name");
            ViewBag.IsAdmin = UserControl.IsAdminUser(User);
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
                Db.ProductSequenceItems.Add(productSequenceItem);
                Db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MachineFamilyId = new SelectList(Db.MachineFamilies.All(), "Id", "Name", productSequenceItem.MachineFamilyId);
            ViewBag.ProductSequenceId = new SelectList(Db.ProductSequences.All(), "Id", "Name", productSequenceItem.ProductSequenceId);
            ViewBag.IsAdmin = UserControl.IsAdminUser(User);
            return View(productSequenceItem);
        }

        // GET: ProductSequenceItems/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductSequenceItem productSequenceItem = Db.ProductSequenceItems.GetById(id);
            if (productSequenceItem == null)
            {
                return HttpNotFound();
            }
            ViewBag.MachineFamilyId = new SelectList(Db.MachineFamilies.All(), "Id", "Name", productSequenceItem.MachineFamilyId);
            ViewBag.ProductSequenceId = new SelectList(Db.ProductSequences.All(), "Id", "Name", productSequenceItem.ProductSequenceId);
            ViewBag.IsAdmin = UserControl.IsAdminUser(User);
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
                Db.ProductSequenceItems.Update(productSequenceItem);
                Db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MachineFamilyId = new SelectList(Db.MachineFamilies.All(), "Id", "Name", productSequenceItem.MachineFamilyId);
            ViewBag.ProductSequenceId = new SelectList(Db.ProductSequences.All(), "Id", "Name", productSequenceItem.ProductSequenceId);
            ViewBag.IsAdmin = UserControl.IsAdminUser(User);
            return View(productSequenceItem);
        }
        
        // GET: ProductSequenceItems/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductSequenceItem productSequenceItem = Db.ProductSequenceItems.GetById(id);
            if (productSequenceItem == null)
            {
                return HttpNotFound();
            }
            ViewBag.IsAdmin = UserControl.IsAdminUser(User);
            return View(productSequenceItem);
        }

        // POST: ProductSequenceItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ProductSequenceItem productSequenceItem = Db.ProductSequenceItems.GetById(id);
            Db.ProductSequenceItems.Delete(productSequenceItem);
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
