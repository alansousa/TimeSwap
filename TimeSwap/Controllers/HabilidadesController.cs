using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Services;
using System.Web.Services;
using TimeSwap.Models;
using System.Data;
using System.Data.Entity;

namespace TimeSwap.Controllers
{
    public class HabilidadesController : Controller
    {
        private Context ctx = new Context();
        //
        // GET: /Habilidades/

        public ActionResult Index()
        {
            return View(ctx.HABILIDADE.ToList());
        }
        //get
        public ActionResult Novo()
        {
            return View();
        }
        //post
        [HttpPost]
        public ActionResult Novo(HABILIDADE hHabilidade)
        {
            try
            {
                ctx.HABILIDADE.Add(hHabilidade);
                ctx.SaveChanges();

                return RedirectToAction("Index", "Habilidades");
            }
            catch (Exception ex) { return View(ex); }

        }

        public ActionResult Edit(int id = 0)
        {         
            HABILIDADE habilidade = ctx.HABILIDADE.Find(id);
            if (habilidade == null)
            {
                return HttpNotFound();
            }
            return View(habilidade);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(HABILIDADE habildiade)
        {
            if (ModelState.IsValid)
            {
                ctx.Entry(habildiade).State = EntityState.Modified;
                ctx.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(habildiade);
         }

        [HttpPost]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        [WebMethod()]
        public JsonResult Deleta(int id)
        {
            var a = (from rh in ctx.RECURSOHABILIDADE
                     where rh.HABILIDADEID == id
                     select rh);

            if (!a.Any())
            {
                var habilidade = (from h in ctx.HABILIDADE where h.ID == id select h).First();

                var JResult = new { ID = habilidade.ID, DESCRICAO = habilidade.DESCRICAO };
                ctx.HABILIDADE.Remove(habilidade);
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





