using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Services;
using System.Web.Services;
using TimeSwap.Models;

namespace TimeSwap.Controllers
{
    public class CargosController : Controller
    {
        //
        // GET: /Cargos/
        private Context ctx = new Context();

        public ActionResult Index()
        {
            return View(ctx.CARGO.ToList());
        }

        //GET
        public ActionResult Novo()
        {
            return View();
        }

        //Post
        [HttpPost]
        public ActionResult Novo(CARGO cCargo)
        {
            try
            {
                ctx.CARGO.Add(cCargo);
                ctx.SaveChanges();
            }
            catch (Exception ex)
            {
                return View(ex);
            }
            return RedirectToAction("Index", "Cargos");
        }

        public ActionResult Edit(int id = 0)
        {            
            CARGO cargo = ctx.CARGO.Find(id);
            if (cargo == null)
            {
                return HttpNotFound();
            }
            return View(cargo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CARGO cargo)
        {
            if (ModelState.IsValid)
            {
                ctx.Entry(cargo).State = EntityState.Modified;
                ctx.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cargo);
         }

        [HttpPost]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        [WebMethod()]
        public JsonResult Deleta(int id)
        {
            var a = (from rec in ctx.RECURSO
                     where rec.CARGOID == id
                     select rec);

            if (!a.Any())
            {
                var cargo = (from c in ctx.CARGO where c.ID == id select c).First();

                var JResult = new { ID = cargo.ID, DESCRICAO = cargo.CARGO1 };
                ctx.CARGO.Remove(cargo);
                ctx.SaveChanges();
                return Json(JResult, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var JResult = "";
                return Json(JResult, JsonRequestBehavior.AllowGet);
            }
        }

    }
}
