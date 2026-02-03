using Microsoft.AspNetCore.Mvc;

namespace MU5PrototypeProject.Controllers
{
    public class SessionsController : Controller
    {
        // GET: /Sessions
        public IActionResult Index()
        {
            return View();
        }
    }
}