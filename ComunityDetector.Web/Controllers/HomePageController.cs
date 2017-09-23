using ComunityDetectorServices;
using System.Web.Mvc;

namespace ComunityDetector.Web.Controllers
{
    public class HomePageController : Controller
    {
        // GET: HomePage
        public ActionResult Index()
        {
            var webCrawler = new WebCrawler();
            webCrawler.SearchWeb();

            return View();
        }
    }
}