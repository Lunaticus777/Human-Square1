using Microsoft.AspNetCore.Mvc;

namespace Human_Evolution.Controllers
{
    public class BiensController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
