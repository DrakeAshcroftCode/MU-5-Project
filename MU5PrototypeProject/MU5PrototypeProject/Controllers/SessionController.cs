using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MU5PrototypeProject.Data;
using MU5PrototypeProject.Models;

namespace MU5PrototypeProject.Controllers
{
    public class SessionController : Controller
    {
        private readonly MUContext _context;

        public SessionController(MUContext context)
        {
            _context = context;
        }

        // GET: Session
        public async Task<IActionResult> Index()
        {
            var sessions = _context.Sessions
                .Include(s => s.Client)
                .Include(s => s.Trainer)
                .AsNoTracking(); ;
            return View(await sessions.ToListAsync());
        }

        // GET: Session/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var session = await _context.Sessions
                .Include(s => s.Client)
                .Include(s => s.Trainer)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (session == null)
            {
                return NotFound();
            }

            return View(session);
        }

        // GET: Session/Create
        public IActionResult Create()
        {
            PopulateDropDownLists();
            //ViewData["ClientID"] = new SelectList(_context.Clients, "ID", "FirstName");
            //ViewData["TrainerID"] = new SelectList(_context.Trainers, "ID", "FirstName");
            return View();
        }

        // POST: Session/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,SessionDate,CreatedAt,SessionsPerWeekRecommended,IsArchived,TrainerID,ClientID")] Session session)
        {
            if (ModelState.IsValid)
            {
                _context.Add(session);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            PopulateDropDownLists(session);
            //ViewData["ClientID"] = new SelectList(_context.Clients, "ID", "FirstName", session.ClientID);
            //ViewData["TrainerID"] = new SelectList(_context.Trainers, "ID", "FirstName", session.TrainerID);
            return View(session);
        }

        // GET: Session/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var session = await _context.Sessions.FindAsync(id);
            if (session == null)
            {
                return NotFound();
            }
            PopulateDropDownLists(session);
            //ViewData["ClientID"] = new SelectList(_context.Clients, "ID", "FirstName", session.ClientID);
            //ViewData["TrainerID"] = new SelectList(_context.Trainers, "ID", "FirstName", session.TrainerID);
            return View(session);
        }

        // POST: Session/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,SessionDate,CreatedAt,SessionsPerWeekRecommended,IsArchived,TrainerID,ClientID")] Session session)
        {
            if (id != session.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(session);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SessionExists(session.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            PopulateDropDownLists(session);
            //ViewData["ClientID"] = new SelectList(_context.Clients, "ID", "FirstName", session.ClientID);
            //ViewData["TrainerID"] = new SelectList(_context.Trainers, "ID", "FirstName", session.TrainerID);
            return View(session);
        }

        // GET: Session/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var session = await _context.Sessions
                .Include(s => s.Client)
                .Include(s => s.Trainer)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (session == null)
            {
                return NotFound();
            }

            return View(session);
        }

        // POST: Session/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var session = await _context.Sessions.FindAsync(id);
            if (session != null)
            {
                _context.Sessions.Remove(session);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private void PopulateDropDownLists(Session? session = null)
        {

            var dQuery = from t in _context.Clients

                         orderby t.ClientName
                         select t;

            ViewData["ClientID"] = new SelectList(dQuery, "ID", "ClientName", session?.ClientID);

            ViewData["TrainerID"] = new SelectList(dQuery, "ID", "TrainerName", session?.TrainerID);

        }
        private bool SessionExists(int id)
        {
            return _context.Sessions.Any(e => e.ID == id);
        }
    }
}
