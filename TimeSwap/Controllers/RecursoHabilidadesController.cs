using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Services;
using System.Web.Services;
using TimeSwap.Models;

namespace TimeSwap.Controllers
{
    public class RecursoHabilidadesController : Controller
    {
        private Context ctx = new Context();

        #region Consulta
        [HttpPost]
        [ScriptMethod(ResponseFormat=ResponseFormat.Json)]
        [WebMethod()]
        public JsonResult getHabilidades(string recurso) {

            var habilidades = (from rh in ctx.RECURSOHABILIDADE
                               where rh.RECURSOID == recurso
                               select new
                               {
                                   HABILIDADE = rh.HABILIDADE.DESCRICAO,
                                   HABILIDADEID = rh.HABILIDADEID,
                                   RECURSO = rh.RECURSO.NOME,
                                   RECURSOID = rh.RECURSOID,
                                   NIVEL = rh.NIVEL
                               }).ToList();


            return Json(habilidades, JsonRequestBehavior.AllowGet);
        }
        #endregion

        //
        // GET: /RecursoHabilidades/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /RecursoHabilidades/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /RecursoHabilidades/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /RecursoHabilidades/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /RecursoHabilidades/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /RecursoHabilidades/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /RecursoHabilidades/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /RecursoHabilidades/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
