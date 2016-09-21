using System.Web.Mvc;

namespace MVC_Web_API.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index() => View();
    }
}