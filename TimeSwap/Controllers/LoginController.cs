using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TimeSwap.Models;

namespace TimeSwap.Controllers
{
    public class LoginController : Controller
    {
        //
        // GET: /Login/
        private Context ctx = new Context();
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index (string login, string senha)
        {
          
                try
                {
                    var query = ctx.RECURSO.Where(u => u.LOGIN == login && u.SENHA == senha).First();

                    //FormsAuthentication.SetAuthCookie(query.LOGIN, false);
                    HttpCookie cookie = new HttpCookie("Login");
                    cookie.Value = query.LOGIN;

                    this.ControllerContext.HttpContext.Response.Cookies.Add(cookie);

                    return RedirectToAction("index", "home");

                }
                catch
                {
                    ViewBag.resultado = "Login ou/e Senha Inválidos.";

                }

            

            return View();

        }
            
        public ActionResult LogOff()
        {
            if (this.ControllerContext.HttpContext.Request.Cookies.AllKeys.Contains("Login"))
            {
                var cookie = new HttpCookie("Login");
                cookie.Expires = DateTime.Now.AddDays(-1d);
                Response.Cookies.Add(cookie);
            }
            return RedirectToAction("Index", "Home");
        }


/*        public RECURSO GetUsuario()
        {
            string login = HttpContext.User.Identity.Name;
            if (login == null)
            {
                return null;
            }
            else
            {
                private Context db = new Context();
                var user = (from u in ctx.RECURSO where u.LOGIN == login select u).SingleOrDefault();
                return user;
            }
        }*/

    }
}
