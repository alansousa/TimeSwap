using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TimeSwap.Models;
using java.util;
using net.sf.mpxj.mpp;
using net.sf.mpxj;
using System.IO;
using System.Web.Script.Services;
using System.Web.Services;

namespace TimeSwap.Controllers
{
    public class ProjetosController : Controller
    {
        public static Task proj { get; set; }
        public static List<Task> faseImp;
        public static List<Task> tarefaImp;
        //public static List<Task> tarefaFilhos = new List<Task> { };

        private Context ctx = new Context();

        // GET: /Projetos/

        public ActionResult Index()
        {
            return View(ctx.PROJETO.ToList());
        }

        public ActionResult Visualizar(string id)
        {
            var projeto = ctx.PROJETO.Where(proj => proj.CODIGO == id).First();

            @ViewBag.recursoTarefa = ctx.RECURSOTAREFA.Where(m => m.PROJETOID == id).ToList();
            @ViewBag.fases = ctx.RECURSOTAREFA.Where(m => m.PROJETOID == id).ToList();
            @ViewBag.projeto = projeto.CODIGO;

            return View(projeto);
        }

        public ActionResult AlocarRecurso(string id, int fase, string tarefa)
        {
            var tarefaDatas = (from proj in ctx.TAREFA
                               where proj.PROJETOID == id && proj.FASEID == fase
                               && proj.TAREFAID == tarefa
                               select proj).First();

            @ViewBag.inicioBaseline = String.Format("{0:dd/MM/yyyy}", tarefaDatas.INICIOBASE);
            @ViewBag.fimBaseLine = String.Format("{0:dd/MM/yyyy}", tarefaDatas.FIMBASE);
            @ViewBag.Tarefa = tarefaDatas.DESCRICAO;

            @ViewBag.Recursos = ctx.RECURSO.ToList();

            @ViewBag.projetoId = tarefaDatas.PROJETOID;
            @ViewBag.faseId = tarefaDatas.FASEID;
            @ViewBag.tarefaId = tarefaDatas.TAREFAID;

            return View();
        }

        #region Recursos
        [HttpPost]
        [WebMethod()]
        public ActionResult getRecursos(string projeto)
        {

            var listaNomes = (from rec in ctx.RECURSOTAREFA
                              group rec by new { rec.RECURSOID, rec.RECURSO } into r
                              select new
                              {
                                  RECURSOID = r.Key.RECURSOID,
                                  RECURSO = r.Key.RECURSO.NOME,
                              }).ToList();

            return PartialView("_PartialListaRecurso", listaNomes);

        }

        //VisualizarRecursos?id=0011&fase=1&tarefa=1.1
        public ActionResult VisualizarRecursos(string id, int fase, string tarefa)
        {
            var recursos = ctx.RECURSOTAREFA.Where(m => m.PROJETOID == id && m.FASEID == fase && m.TAREFAID == tarefa).ToList();

            @ViewBag.projeto = recursos.First().PROJETOID;

            return View(recursos);
        }


        [HttpPost]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        [WebMethod()]
        public JsonResult AlocarRecurso(string id, int fase, string tarefa, string recursoId)
        {
            string result = "";
            var recursoTarefa = new RECURSOTAREFA();
            try
            {
                recursoTarefa.RECURSOID = recursoId;
                recursoTarefa.PROJETOID = id;
                recursoTarefa.FASEID = fase;
                recursoTarefa.TAREFAID = tarefa;
                recursoTarefa.DATAINICIO = DateTime.Now;
                ctx.RECURSOTAREFA.Add(recursoTarefa);
                ctx.SaveChanges();
                result = "1";
            }
            catch (Exception ex)
            {
                result = "2";
                @ViewBag.erro = ex.Message;
            }

            return Json(result, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public ActionResult deletaRecTarefa(string id, int fase, string tarefa)
        {
            return View("");
        }

        #region PartialViewAlocarRecursos
        [HttpPost]
        [WebMethod()]
        public ActionResult getRHByNamePartial(string nome = "")
        {
            if (nome.Length < 1)
            {
                var listaNomes = (from rh in ctx.RECURSOHABILIDADE
                                  group rh by new { rh.RECURSOID, rh.RECURSO } into rhId
                                  select rhId.Key.RECURSO).ToList();

                return PartialView("_PartialRecurso", listaNomes.OrderByDescending(m => m.RECURSOHABILIDADE.Average(q => q.NIVEL).Value).ToList());
            }
            else
            {
                var listaNomes = (from rh in ctx.RECURSOHABILIDADE
                                  where rh.RECURSO.NOME == nome
                                  group rh by new { rh.RECURSOID, rh.RECURSO } into rhId
                                  select rhId.Key.RECURSO).ToList();
                return PartialView("_PartialRecurso", listaNomes.OrderByDescending(m => m.RECURSOHABILIDADE.Average(q => q.NIVEL).Value).ToList());
            }
        }

        [HttpPost]
        [WebMethod()]
        public ActionResult getVerMais(string id)
        {
            var aux = (from rh in ctx.RECURSOHABILIDADE
                       where rh.RECURSOID == id
                       orderby rh.NIVEL, rh.HABILIDADE.DESCRICAO
                       select rh).ToList();

            return PartialView("_PartialVerMais", aux);
        }

        [HttpPost]
        [WebMethod()]
        public ActionResult getRHByAbility(string habilidade = "")
        {
            if (habilidade.Length < 1)
            {
                var aux = (from rh in ctx.RECURSOHABILIDADE
                           group rh by new { rh.RECURSOID, rh.RECURSO } into rhId
                           select rhId.Key.RECURSO).ToList();
                return PartialView("_PartialRecurso", aux.OrderByDescending(m=>m.RECURSOHABILIDADE.Average(q => q.NIVEL).Value).ToList());
            }
            else
            {
                var aux = (from rh in ctx.RECURSOHABILIDADE
                           where rh.HABILIDADE.DESCRICAO.Contains(habilidade)
                           group rh by new { rh.RECURSOID, rh.RECURSO } into rhId
                           select rhId.Key.RECURSO).ToList();
                return PartialView("_PartialRecurso", aux.OrderByDescending(m => m.RECURSOHABILIDADE.Average(q => q.NIVEL).Value).ToList());
            }
        }
        /*
        [HttpPost]
        [WebMethod()]
        public ActionResult getRHByLevel(int nivel = 0)
        {
            if (nivel ==0)
            {
                var aux = (from rh in ctx.RECURSOHABILIDADE
                           group rh by new { rh.RECURSOID, rh.RECURSO } into rhId
                           select rhId.Key.RECURSO).ToList();
                return PartialView("_PartialRecurso", aux);
            }
            else
            {
                var aux = (from rh in ctx.RECURSOHABILIDADE
                           where rh.NIVEL.Equals(nivel) || rh.NIVEL.Value >= nivel
                           orderby rh.NIVEL, rh.HABILIDADE.DESCRICAO, rh.RECURSO.NOME
                           select rh.RECURSO).ToList();
                return PartialView("_PartialRecurso", aux);
            }
        }*/


        #endregion
        public ActionResult ModalTeste()
        {
            return View();
        }
        #endregion

        #region JsonMethods
        [HttpPost]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        [WebMethod()]
        public JsonResult getRHByName(string nome)
        {
            var aux = (from rh in ctx.RECURSOHABILIDADE
                       where rh.RECURSO.NOME.Contains(nome)
                       orderby rh.NIVEL, rh.HABILIDADE.DESCRICAO, rh.RECURSO.NOME
                       select new
                       {
                           HABILIDADEID = rh.HABILIDADEID,
                           HABILIDADE = rh.HABILIDADE.DESCRICAO,
                           RECURSOID = rh.RECURSOID,
                           RECURSO = rh.RECURSO.NOME,
                           NIVEL = rh.NIVEL
                       }).ToList();

            return Json(aux, JsonRequestBehavior.AllowGet);
        }

        #endregion


        #region Projeto

        public ActionResult Importar()
        {
            var Gerente = from c in ctx.RECURSO
                          where c.CARGOID == 1
                          select c;

            ViewBag.GERENTEID = new SelectList(Gerente, "MATRICULA", "NOME");
            return View();
        }

        [HttpPost]
        public ActionResult Importar(PROJETO projeto, HttpPostedFileBase caminho)
        {
            //Response.Cache.SetCacheability(HttpCacheability.NoCache);
            //Response.Cache.SetExpires(DateTime.Now);
            //Response.Cache.SetNoServerCaching();
            //Response.Cache.SetNoStore();

            HttpPostedFileBase File = Request.Files["uploadFile"];

            string filePath = Path.Combine(HttpContext.Server.MapPath("../Arquivos"), Path.GetFileName(caminho.FileName));
            if (caminho.ContentLength > 0)
            {
                caminho.SaveAs(filePath);
            }
            var ifile = @filePath;

            var reader = new MPPReader();
            FASE faseClass;
            var anoAux = 0;
            var mesAux = 0;
            var diaAux = 0;

            net.sf.mpxj.ProjectFile file = reader.read(ifile);
            List tasks = file.getAllTasks();

            proj = (Task)tasks.toArray().GetValue(1);
            projeto.NOME = proj.getName();
            projeto.CAMINHO = filePath;

            try
            {
                ctx.PROJETO.Add(projeto);
                ctx.SaveChanges();
            }
            catch (Exception ex)
            {
                return View(ex);
            }

            faseImp = new List<Task> { };

            for (int i = 0; i < proj.getChildTasks().toArray().Length; i++)
            {
                faseImp.Add((Task)proj.getChildTasks().toArray().GetValue(i));
            }

            for (int j = 0; j < faseImp.ToArray().Length; j++)
            {
                var tk = (Task)faseImp.ToArray().GetValue(j);
                faseClass = new FASE();

                faseClass.PROJETOID = projeto.CODIGO;
                faseClass.FASEID = j + 1;//tk.getID().intValue();
                faseClass.DESCRICAO = tk.getName();
                faseClass.DURACAO = Convert.ToDouble(tk.getDuration().ToString().Replace(".", ",").Replace("d", ""));

                anoAux = Int32.Parse(new java.text.SimpleDateFormat("yyyy").format(tk.getStart()));
                mesAux = Int32.Parse(new java.text.SimpleDateFormat("MM").format(tk.getStart()));
                diaAux = Int32.Parse(new java.text.SimpleDateFormat("dd").format(tk.getStart()));
                faseClass.INICIOBASE = new DateTime(anoAux, mesAux, diaAux);

                anoAux = Int32.Parse(new java.text.SimpleDateFormat("yyyy").format(tk.getFinish()));
                mesAux = Int32.Parse(new java.text.SimpleDateFormat("MM").format(tk.getFinish()));
                diaAux = Int32.Parse(new java.text.SimpleDateFormat("dd").format(tk.getFinish()));
                faseClass.FIMBASE = new DateTime(anoAux, mesAux, diaAux);

                try
                {
                    ctx.FASE.Add(faseClass);
                    ctx.SaveChanges();
                }
                catch (Exception ex)
                {
                    return View(ex);
                }
                getFilhos(tk, faseClass.FASEID, projeto.CODIGO);
            }
            return RedirectToAction("Index", "Projetos");
        }

        private void getFilhos(Task task, int faseId, string projetoId)
        {
            TAREFA tarefaClass;
            var t = task.getChildTasks();
            var aux = t.toArray();
            var anoAux = 0;
            var mesAux = 0;
            var diaAux = 0;

            if (aux.Length > 0)
            {
                for (int i = 0; i < aux.Length; i++)
                {
                    tarefaClass = new TAREFA();
                    var objAux = (Task)aux.GetValue(i);
                    tarefaClass.TAREFAID = faseId + "." + (i + 1);
                    tarefaClass.DESCRICAO = objAux.getName();
                    tarefaClass.DURACAO = Convert.ToDouble(objAux.getDuration().ToString().Replace(".", ",").Replace("d", ""));
                    tarefaClass.FASEID = faseId;
                    tarefaClass.PROJETOID = projetoId;
                    tarefaClass.LINHA = objAux.getID().intValue();

                    anoAux = Int32.Parse(new java.text.SimpleDateFormat("yyyy").format(objAux.getStart()));
                    mesAux = Int32.Parse(new java.text.SimpleDateFormat("MM").format(objAux.getStart()));
                    diaAux = Int32.Parse(new java.text.SimpleDateFormat("dd").format(objAux.getStart()));
                    tarefaClass.INICIOBASE = new DateTime(anoAux, mesAux, diaAux);

                    anoAux = Int32.Parse(new java.text.SimpleDateFormat("yyyy").format(objAux.getFinish()));
                    mesAux = Int32.Parse(new java.text.SimpleDateFormat("MM").format(objAux.getFinish()));
                    diaAux = Int32.Parse(new java.text.SimpleDateFormat("dd").format(objAux.getFinish()));
                    tarefaClass.FIMBASE = new DateTime(anoAux, mesAux, diaAux);

                    if (objAux.getChildTasks().toArray().Length > 0)
                    {
                        tarefaClass.TIPO = 2;
                        getFilhos(objAux, faseId, projetoId);
                    }
                    else
                    {
                        tarefaClass.TIPO = 1;
                    }

                    var ttarefa = (from u in ctx.TAREFA
                                   where tarefaClass.PROJETOID == u.PROJETOID
                                   && tarefaClass.FASEID == u.FASEID
                                   && tarefaClass.TAREFAID == u.TAREFAID
                                   select u).FirstOrDefault();
                    if (ttarefa == null)
                    {
                        try
                        {
                            ctx.TAREFA.Add(tarefaClass);
                            ctx.SaveChanges();
                        }
                        catch (Exception ex)
                        {
                            var exx = ex;
                        }
                    }

                }
            }
        }

        #endregion
    }
}
