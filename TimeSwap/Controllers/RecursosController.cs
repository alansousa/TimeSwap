using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Services;
using System.Web.Services;
using TimeSwap.Models;

namespace TimeSwap.Controllers
{
    public class RecursosController : Controller
    {
        //
        // GET: /Recursos/
        private Context ctx = new Context();

        public ActionResult Index()
        {
            return View(ctx.RECURSO.ToList());
        }

        //GET
        public ActionResult Novo()
        {
            @ViewBag.Cargos = ctx.CARGO.ToList();
            @ViewBag.Habilidades = ctx.HABILIDADE.ToList();

            return View();
        }

        //Post
        [HttpPost]
        public ActionResult Novo(RECURSO rRecurso)
        {
            try
            {
                ctx.RECURSO.Add(rRecurso);
                ctx.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
            }
            return View(rRecurso);
        }

        [HttpPost]
        public JsonResult NovaHabilidade(int? nivel, string idRecurso, int idHabilidade)
        {
            RECURSOHABILIDADE rHabilidade = new RECURSOHABILIDADE();

            rHabilidade.RECURSOID = ctx.RECURSO.Where(m => m.MATRICULA == idRecurso).First().MATRICULA;
            rHabilidade.HABILIDADEID = ctx.HABILIDADE.Where(m => m.ID == idHabilidade).First().ID;
            rHabilidade.NIVEL = nivel;
            ctx.RECURSOHABILIDADE.Add(rHabilidade);
            ctx.SaveChanges();

            return Json("Salvo", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ApagarHabilidade(string idRecurso, int idHabilidade)
        {
            var rh = ctx.RECURSOHABILIDADE.Where(m => m.RECURSOID == idRecurso && m.HABILIDADEID == idHabilidade);
            if (rh.Any())
            {
                ctx.RECURSOHABILIDADE.Remove(rh.First());
                ctx.SaveChanges();

                return Json("Vinculo com a Habilidade excluído", JsonRequestBehavior.AllowGet);
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }


        // GET
        public ActionResult Editar(string matricula)
        {
            @ViewBag.Cargos = ctx.CARGO.ToList();
            @ViewBag.Habilidades = ctx.HABILIDADE.ToList();

            var recurso = (from rec in ctx.RECURSO where rec.MATRICULA == matricula select rec).First();
            return View(recurso);
        }

        [HttpPost]
        public ActionResult Editar(RECURSO rRecurso)
        {
            try
            {
                var original = (from rec in ctx.RECURSO where rec.MATRICULA == rRecurso.MATRICULA select rec).First();

                if (!ModelState.IsValid)
                    return View();

                UpdateModel(original);
                ctx.SaveChanges();

                ctx.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        [WebMethod()]
        public JsonResult Deleta(string id)
        {
            var recurso = ctx.RECURSO.Where(rh => rh.MATRICULA == id);

            if (recurso.Any())
            {
                var recursohabilidade = ctx.RECURSOHABILIDADE.Where(m => m.RECURSOID == id).ToList();

                foreach (var i in recursohabilidade)
                    ctx.RECURSOHABILIDADE.Remove(i);

                var rte = ctx.RecursoTarefaExecutado.Where(m => m.recursoId == id).ToList();

                foreach (var j in rte)
                    ctx.RecursoTarefaExecutado.Remove(j);

                var rt = ctx.RECURSOTAREFA.Where(m => m.RECURSOID == id).ToList();

                foreach (var k in rt)
                    ctx.RECURSOTAREFA.Remove(k);

                var JResult = recurso.First();

                ctx.RECURSO.Remove(recurso.First());
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
