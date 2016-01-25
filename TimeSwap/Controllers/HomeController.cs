using System.Linq;
using System.Web.Mvc;
using TimeSwap.Models;

namespace TimeSwap.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        private Context ctx = new Context();


        public ActionResult Index()
        {
            if (this.ControllerContext.HttpContext.Request.Cookies.AllKeys.Contains("Login"))
            {
                var cookie = this.ControllerContext.HttpContext.Request.Cookies["Login"].Value;
                if (cookie != null)
                {
                    var usuario = ctx.RECURSO.Where(u => u.LOGIN == cookie).First();
                    ViewBag.usuario = usuario;
                    ViewBag.qtdeTarefas = ctx.RECURSOTAREFA.Where(m => m.RECURSOID == usuario.MATRICULA && m.STATUS != 2).ToList().Count;
                }
            }
            return View(ctx.PROJETO.ToList());
        }
        // var resultado = ctx.RECURSO.Join(ctx.RECURSOHABILIDADE, n => n.MATRICULA, c => c.RECURSOID, (n, c) => new { n, c }).Join(ctx.HABILIDADE, f => f.c.RECURSOID, h => h.ID, (f, h) => new { f, h }).GroupBy(h.DESCRICAO).Count();
        /* 
            select h.DESCRICAO, count(DISTINCT h.DESCRICAO)
            from RECURSO r left join RECURSOHABILIDADE rh on (r.MATRICULA = rh.RECURSOID) left join HABILIDADE h on (rh.HABILIDADEID = h.ID)
            where h.DESCRICAO is not null
            group by h.DESCRICAO
         */
    }
}
