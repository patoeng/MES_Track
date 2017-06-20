using System.Linq;
using System.Net;
using System.Web.Mvc;
using MES.Models;
using MES.Mvc.Helpers;

namespace MES.Mvc.Controllers
{
    public class DepartmentsController : BaseController
    {
        

      
        public ActionResult Index(string searchString)
        {
            var department = searchString == "*"
                ? Db.Departments.All()
                : Db.Departments.All().Where(m => m.Name.Contains(searchString));
            ViewBag.SearchString = searchString;
            ViewBag.IsAdmin = UserControl.IsAdminUser(User);
            return View(department.ToList());
        }

        // GET: Departments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Department department = Db.Departments.GetById(id);
            if (department == null)
            {
                return HttpNotFound();
            }
            ViewBag.IsAdmin = UserControl.IsAdminUser(User);
            return View(department);
        }

        // GET: Departments/Create
        public ActionResult Create()
        {
            ViewBag.IsAdmin = UserControl.IsAdminUser(User);
            return View();
        }

        // POST: Departments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] Department department)
        {
            if (ModelState.IsValid)
            {
                Db.Departments.Add(department);
                Db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IsAdmin = UserControl.IsAdminUser(User);
            return View(department);
        }

        // GET: Departments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Department department = Db.Departments.GetById(id);
            if (department == null)
            {
                return HttpNotFound();
            }
            ViewBag.IsAdmin = UserControl.IsAdminUser(User);
            return View(department);
        }

        // POST: Departments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] Department department)
        {
            if (ModelState.IsValid)
            {
                Db.Departments.Update(department);
                Db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IsAdmin = UserControl.IsAdminUser(User);
            return View(department);
        }

        // GET: Departments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Department department = Db.Departments.GetById(id);
            if (department == null)
            {
                return HttpNotFound();
            }
            ViewBag.IsAdmin = UserControl.IsAdminUser(User);
            return View(department);
        }

        // POST: Departments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Department department = Db.Departments.GetById(id);
            Db.Departments.Delete(department);
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
