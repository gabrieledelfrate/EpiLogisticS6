using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace EpiLogistic.Controllers
{
    public class HomeController : Controller
    {
        // GET: Authorization
        public ActionResult Index()
        {
            return View();
        }

        public void LogIn(string username)
        {
            FormsAuthentication.SetAuthCookie(username, false);
        }

        public void LogOut()
        {
            FormsAuthentication.SignOut();
        }

        public bool IsUserAuthenticated()
        {
            return HttpContext.User.Identity.IsAuthenticated;
        }
    }
}
