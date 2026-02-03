using Microsoft.AspNetCore.Mvc;

namespace MU5PrototypeProject.Controllers
{
    public class ClientsController : Controller
    {
        // Client list page
        public IActionResult Index()
        {
            return View();
        }

        // Add/Edit page
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            // id is optional for now. Will change when Nasra adds validation.
            ViewBag.ClientId = id;
            return View();
        }

        // Details/Archive page
        [HttpGet]
        public IActionResult Details(int id = 1)
        {
            ViewBag.ClientId = id;
            return View();
        }

        // Fake "Archive" (no real logic yet)
        [HttpPost]
        public IActionResult Archive(int id)
        {
            // Later: backend will actually archive
            return RedirectToAction("Index");
        }
    }
}
