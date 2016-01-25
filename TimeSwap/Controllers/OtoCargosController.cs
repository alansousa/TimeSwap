using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using TimeSwap.Models;

namespace TimeSwap.Controllers.TimeSwap
{
    public class OtoCargosController : Controller
    {
        private Context db = new Context ();

        //
        // GET: /Cargos/

        public ActionResult Index()
        {
            return View(db.CARGO.ToList());
        }

        //
        // GET: /Cargos/Details/5

        public ActionResult Details(int id = 0)
        {
            CARGO cargo = db.CARGO.Find(id);
            if (cargo == null)
            {
                return HttpNotFound();
            }
            return View(cargo);
        }

        //
        // GET: /Cargos/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Cargos/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CARGO cargo)
        {
            if (ModelState.IsValid)
            {
                db.CARGO.Add(cargo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(cargo);
        }

        //
        // GET: /Cargos/Edit/5

        public ActionResult Edit(int id = 0)
        {
            CARGO cargo = db.CARGO.Find(id);
            if (cargo == null)
            {
                return HttpNotFound();
            }
            return View(cargo);
        }

        //
        // POST: /Cargos/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CARGO cargo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cargo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cargo);
        }

        //
        // GET: /Cargos/Delete/5

        public ActionResult Delete(int id = 0)
        {
            CARGO cargo = db.CARGO.Find(id);
            if (cargo == null)
            {
                return HttpNotFound();
            }
            return View(cargo);
        }

        //
        // POST: /Cargos/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CARGO cargo = db.CARGO.Find(id);
            db.CARGO.Remove(cargo);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}