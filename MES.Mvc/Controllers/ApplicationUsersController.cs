using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using MES.Mvc.Helpers;
using MES.Mvc.Models;

namespace MES.Mvc.Controllers
{
    public class ApplicationUsersController : Controller
    {
        readonly ApplicationDbContext _db = new ApplicationDbContext();
      
       public ActionResult Index()
       {
           var applicationUsers = _db.Users;
           return View(applicationUsers.ToList());
        }

        // GET: ApplicationUsers/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser applicationUser = _db.Users.Find(id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }
            ViewBag.IsAdmin = UserControl.IsAdminUser(User);
            return View(applicationUser);
        }

        // GET: ApplicationUsers/Create
        public ActionResult Create()
        {
            // ViewBag.DepartmentId = new SelectList(db.Departments.All(), "Id", "Name");
            ViewBag.IsAdmin = UserControl.IsAdminUser(User);
            return View();
        }

        // POST: ApplicationUsers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,EmployeeRegisterNumber,Name,Level,DepartmentId,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName")] ApplicationUser applicationUser)
        {
            if (ModelState.IsValid)
            {
                _db.Users.Add(applicationUser);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            // ViewBag.DepartmentId = new SelectList(db.Departments.All(), "Id", "Name", applicationUser.DepartmentId);
            ViewBag.IsAdmin = UserControl.IsAdminUser(User);
            return View(applicationUser);
        }

        // GET: ApplicationUsers/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser applicationUser = _db.Users.Find(id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }
            // ViewBag.DepartmentId = new SelectList(db.Departments.All(), "Id", "Name", applicationUser.DepartmentId);
            ViewBag.IsAdmin = UserControl.IsAdminUser(User);
            return View(applicationUser);
        }

        // POST: ApplicationUsers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,EmployeeRegisterNumber,Name,Level,DepartmentId,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName")] ApplicationUser applicationUser)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(applicationUser).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            // ViewBag.DepartmentId = new SelectList(db.Departments.All(), "Id", "Name", applicationUser.DepartmentId);
            ViewBag.IsAdmin = UserControl.IsAdminUser(User);
            return View(applicationUser);
        }

        // GET: ApplicationUsers/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser applicationUser = _db.Users.Find(id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }
            ViewBag.IsAdmin = UserControl.IsAdminUser(User);
            return View(applicationUser);
        }

        // POST: ApplicationUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            ApplicationUser applicationUser = _db.Users.Find(id);
            _db.Users.Remove(applicationUser);
            _db.SaveChanges();
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
