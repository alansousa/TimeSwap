using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using TimeSwap.Models;

namespace TimeSwap.Controllers.TimeSwap
{
    public class FaseController : Controller
    {
        private Context db = new Context ();

        //
        // GET: /Fase/

        public ActionResult Index()
        {
            var fase = db.FASE.Include(f => f.PROJETO);
            return View(fase.ToList());
        }

        //
        // GET: /Fase/Details/5

        public ActionResult Details(int id = 0)
        {
            FASE fase = db.FASE.Find(id);
            if (fase == null)
            {
                return HttpNotFound();
            }
            return View(fase);
        }

        //
        // GET: /Fase/Create

        public ActionResult Create()
        {
            ViewBag.PROJETOID = new SelectList(db.PROJETO, "CODIGO", "NOME");
            return View();
        }

        //
        // POST: /Fase/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(FASE fase)
        {
            if (ModelState.IsValid)
            {
                db.FASE.Add(fase);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PROJETOID = new SelectList(db.PROJETO, "CODIGO", "NOME", fase.PROJETOID);
            return View(fase);
        }

        //
        // GET: /Fase/Edit/5

        public ActionResult Edit(int id = 0)
        {
            FASE fase = db.FASE.Find(id);
            if (fase == null)
            {
                return HttpNotFound();
            }
            ViewBag.PROJETOID = new SelectList(db.PROJETO, "CODIGO", "NOME", fase.PROJETOID);
            return View(fase);
        }

        //
        // POST: /Fase/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(FASE fase)
        {
            if (ModelState.IsValid)
            {
                db.Entry(fase).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PROJETOID = new SelectList(db.PROJETO, "CODIGO", "NOME", fase.PROJETOID);
            return View(fase);
        }

        //
        // GET: /Fase/Delete/5

        public ActionResult Delete(int id = 0)
        {
            FASE fase = db.FASE.Find(id);
            if (fase == null)
            {
                return HttpNotFound();
            }
            return View(fase);
        }

        //
        // POST: /Fase/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            FASE fase = db.FASE.Find(id);
            db.FASE.Remove(fase);
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