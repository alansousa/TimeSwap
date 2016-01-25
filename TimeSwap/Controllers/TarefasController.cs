using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Services;
using System.Web.Services;
using TimeSwap.Model;
using TimeSwap.Models;

namespace TimeSwap.Controllers
{
    public class TarefasController : Controller
    {
        private Context db = new Context();

        // GET: /Tarefas/
        #region CRUD
        public ActionResult Index()
        {
            var tarefa = db.TAREFA.Include(t => t.FASE);
            return View(tarefa.ToList());
        }

        // GET: /Tarefas/Details/5
        public ActionResult Details(string id = null)
        {
            TAREFA tarefa = db.TAREFA.Find(id);
            if (tarefa == null)
            {
                return HttpNotFound();
            }
            return View(tarefa);
        }

        // GET: /Tarefas/Create
        public ActionResult Create()
        {
            ViewBag.FASEID = new SelectList(db.FASE, "FASEID", "DESCRICAO");
            return View();
        }

        // POST: /Tarefas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TAREFA tarefa)
        {
            if (ModelState.IsValid)
            {
                db.TAREFA.Add(tarefa);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.FASEID = new SelectList(db.FASE, "FASEID", "DESCRICAO", tarefa.FASEID);
            return View(tarefa);
        }

        // GET: /Tarefas/Edit/5
        public ActionResult Edit(string id = null)
        {
            TAREFA tarefa = db.TAREFA.Find(id);
            if (tarefa == null)
            {
                return HttpNotFound();
            }
            ViewBag.FASEID = new SelectList(db.FASE, "FASEID", "DESCRICAO", tarefa.FASEID);
            return View(tarefa);
        }

        // POST: /Tarefas/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TAREFA tarefa)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tarefa).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.FASEID = new SelectList(db.FASE, "FASEID", "DESCRICAO", tarefa.FASEID);
            return View(tarefa);
        }

        // GET: /Tarefas/Delete/5
        public ActionResult Delete(string id = null)
        {
            TAREFA tarefa = db.TAREFA.Find(id);
            if (tarefa == null)
            {
                return HttpNotFound();
            }
            return View(tarefa);
        }

        // POST: /Tarefas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            TAREFA tarefa = db.TAREFA.Find(id);
            db.TAREFA.Remove(tarefa);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        #endregion

        #region ExecucaoTarefas
        public ActionResult Tarefas()
        {
            if (this.ControllerContext.HttpContext.Request.Cookies["Login"].Value != null)
            {
                var usuario = this.ControllerContext.HttpContext.Request.Cookies["Login"].Value;

                var tarefa = db.RECURSOTAREFA.Where(t => t.RECURSO.LOGIN == usuario);
                if (tarefa.Any())
                {
                    return View(tarefa);
                }
                else
                {
                    return View();
                }
            }
            return View("Index", "Home");
        }

        [HttpPost]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        [WebMethod()]
        public ActionResult getStart(string matricula, string projeto, int fase, string tarefa, DateTime data)
        {
            var recTarefa = db.RECURSOTAREFA.Where(model => model.PROJETOID == projeto
                        && model.FASEID == fase && model.TAREFAID == tarefa && model.RECURSOID == matricula).First();

            if (!db.RecursoTarefaExecutado.Where(model => model.projetoId == projeto
                    && model.faseId == fase && model.tarefaId == tarefa && model.recursoId == matricula).Any())
            {
                var recTaEmExecucao = new RecursoTarefaExecutado()
                {
                    recursoId = recTarefa.RECURSOID,
                    projetoId = recTarefa.PROJETOID,
                    faseId = recTarefa.FASEID,
                    tarefaId = recTarefa.TAREFAID,
                    DataHoraInicio = DateTime.Now,
                    DataInicio = (DateTime)recTarefa.DATAINICIO
                };

                var tarefaEx = db.TAREFA.Where(t => t.PROJETOID == projeto && t.FASEID == fase && t.TAREFAID == tarefa).First();

                //Status da tarefa null = nao iniciado, 1= iniciado, 2=concluído ,3=pausado,4=reiniciado,5=cancelado
                recTarefa.STATUS = 1;

                tarefaEx.INICIOREAL = recTaEmExecucao.DataHoraInicio.Date;

                db.Entry(tarefaEx).State = EntityState.Modified;
                db.Entry(recTarefa).State = EntityState.Modified;
                db.RecursoTarefaExecutado.Add(recTaEmExecucao);
                db.SaveChanges();

                var rteSalvo = db.RecursoTarefaExecutado.Where(model => model.projetoId == projeto
                        && model.faseId == fase && model.tarefaId == tarefa && model.recursoId == matricula).First();

                @ViewBag.IdRte = rteSalvo.id;
                @ViewBag.HoraInicio = rteSalvo.DataHoraInicio;

                return PartialView("_PartialBotoesTarefas", rteSalvo.RECURSOTAREFA);
            }
            else
            {
                return PartialView("_PartialBotoesTarefas", recTarefa);
            }
        }

        [WebMethod()]
        public ActionResult pausar(int id)
        {
            var rte = db.RecursoTarefaExecutado.Where(m => m.id == id).First();
            var rt = db.RECURSOTAREFA.Where(model => model.PROJETOID == rte.projetoId
                && model.FASEID == rte.faseId && model.TAREFAID == rte.tarefaId && model.RECURSOID == rte.recursoId).First();

            //Status da tarefa null = nao iniciado, 1= iniciado, 2=concluído ,3=pausado,4=reiniciado,5=cancelado
            rt.STATUS = 3;
            rte.DataHoraFim = DateTime.Now;

            db.Entry(rte).State = EntityState.Modified;
            db.Entry(rt).State = EntityState.Modified;
            db.SaveChanges();

            var rteSalvo = db.RecursoTarefaExecutado.Where(m => m.id == id).First();

            return PartialView("_PartialBotoesTarefas", rteSalvo.RECURSOTAREFA);
        }

        [WebMethod()]
        public ActionResult retomar(int id)
        {
            var rteParada = db.RecursoTarefaExecutado.Where(m => m.id == id).First();
            var rt = rteParada.RECURSOTAREFA;
            var rteReiniciada = new RecursoTarefaExecutado()
            {
                recursoId = rteParada.recursoId,
                projetoId = rteParada.projetoId,
                faseId = rteParada.faseId,
                tarefaId = rteParada.tarefaId,
                DataHoraInicio = DateTime.Now,
                DataInicio = (DateTime)rteParada.DataInicio
            };

            //Status da tarefa null = nao iniciado, 1= iniciado, 2=concluído ,3=pausado,4=reiniciado,5=cancelado
            rt.STATUS = 4;

            db.Entry(rt).State = EntityState.Modified;
            db.RecursoTarefaExecutado.Add(rteReiniciada);
            db.SaveChanges();

            var rteSalvo = db.RecursoTarefaExecutado.Where(m => m.projetoId == rteParada.projetoId
                && m.faseId == rteParada.faseId && m.tarefaId == rteParada.tarefaId && m.recursoId == rteParada.recursoId).First();

            @ViewBag.IdRte = rteSalvo.id;
            @ViewBag.HoraInicio = rteSalvo.DataHoraInicio;

            return PartialView("_PartialBotoesTarefas", rteSalvo.RECURSOTAREFA);
        }

        [HttpPost]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        [WebMethod()]
        public JsonResult concluir(int id)
        {
            var rte = db.RecursoTarefaExecutado.Where(m => m.id == id).First();
            var rt = db.RECURSOTAREFA.Where(model => model.PROJETOID == rte.projetoId
                && model.FASEID == rte.faseId && model.TAREFAID == rte.tarefaId && model.RECURSOID == rte.recursoId).First();
            var t = db.TAREFA.Where(model => model.PROJETOID == rte.projetoId && model.FASEID == rte.faseId && model.TAREFAID == rte.tarefaId).First();

            //Status da tarefa null = nao iniciado, 1= iniciado, 2=concluído ,3=pausado,4=cancelado
            rt.STATUS = 2;
            rt.DATAFIM = DateTime.Now.Date;
            rte.DataHoraFim = DateTime.Now;
            t.FIMREAL = DateTime.Now.Date;

            var spanIntervaloDatas = TimeSpan.FromHours(rte.DataHoraFim.Value.Subtract(rte.DataHoraFim.Value).TotalHours);

            db.Entry(rte).State = EntityState.Modified;
            db.Entry(rt).State = EntityState.Modified;
            db.Entry(t).State = EntityState.Modified;
            db.SaveChanges();

            var rteSalvo = db.RecursoTarefaExecutado.Where(m => m.id == id).First();

            var jah = RetornaHoraTrabalhada(rteSalvo.RECURSOTAREFA);

            return Json(rteSalvo.RECURSOTAREFA.TAREFA.DESCRICAO, JsonRequestBehavior.AllowGet);
        }

        private DateTime RetornaHoraTrabalhada(RECURSOTAREFA rt)
        {
            var b = db.RecursoTarefaExecutado.Where(m => m.RECURSOTAREFA == rt).ToList();
            DateTime horas = new DateTime();

            foreach (var item in b)
            {
                var f = item.DataHoraFim.Value.Subtract(item.DataHoraInicio);
                horas = horas.Add(new TimeSpan(f.Days, f.Hours, f.Minutes, f.Seconds));
            }

            return horas;
        }

        [WebMethod()]
        public JsonResult cancelar(int id)
        {
            var a = new RecursoTarefaExecutado();

            return Json("", JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}