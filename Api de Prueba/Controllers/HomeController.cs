using Microsoft.AspNetCore.Mvc;

namespace Api_de_Prueba.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
