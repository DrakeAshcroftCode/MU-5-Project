using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MU5PrototypeProject.Data;
using MU5PrototypeProject.Models;

namespace MU5PrototypeProject.Controllers
{
    public class ClientsController : Controller
    {
        private readonly MUContext _context;

        public ClientsController(MUContext context)
        {
            _context = context;
        }

        // GET: /Clients
        public async Task<IActionResult> Index()
        {
            // Show only active clients for now
            var clients = await _context.Clients
                .Where(c => c.IsActive)
                .OrderBy(c => c.LastName)
                .ThenBy(c => c.FirstName)
                .ToListAsync();

            return View(clients);
        }

        // GET: /Clients/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var client = await _context.Clients
                .FirstOrDefaultAsync(c => c.ID == id);
            if (client == null) return NotFound();

            return View(client);
        }

        // GET: /Clients/Create
        public IActionResult Create()
        {
            // CreatedAt/IsActive will be set in POST
            return View();
        }

        // POST: /Clients/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FirstName,LastName,DOB,Phone,Email")] Client client)
        {
            if (ModelState.IsValid)
            {
                client.IsActive = true;
                client.CreatedAt = DateTime.UtcNow;
                _context.Add(client);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(client);
        }

        // GET: /Clients/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var client = await _context.Clients.FindAsync(id);
            if (client == null) return NotFound();

            return View(client);
        }

        // POST: /Clients/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,FirstName,LastName,DOB,Phone,Email,IsActive,CreatedAt")] Client client)
        {
            if (id != client.ID) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(client);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Clients.Any(e => e.ID == client.ID))
                        return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(client);
        }

        // GET: /Clients/Delete/5  (archive confirmation)
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var client = await _context.Clients
                .FirstOrDefaultAsync(c => c.ID == id);
            if (client == null) return NotFound();

            return View(client);
        }

        // POST: /Clients/Delete/5  (archive: set IsActive=false)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var client = await _context.Clients.FindAsync(id);
            if (client != null)
            {
                client.IsActive = false;
                _context.Attach(client).Property(c => c.IsActive).IsModified = true;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}