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
        public async Task<IActionResult> Index(
            string? SearchSessionDate,
            int? TrainerID,
            int? ClientID,
            string actionButton,
            bool showArchived = false,
            string sortDirection = "asc",
            string sortField = "Session"/*, int? page, int? pageSizeID*/)
        {

            //List of sort options.
            //NOTE: make sure this array has matching values to the column headings
            string[] sortOptions = new[] { "Client", "Trainer", "SessionDate" };

            PopulateDropDownLists();//Data for Session filter for ddl

            var sessions = _context.Sessions
                .Include(s => s.Client)
                .Include(s => s.Trainer)
                .AsNoTracking()
                .AsQueryable();

            // Hide archived sessions by default
            if (!showArchived)
            {
                sessions = sessions.Where(s => !s.IsArchived);
            }

            //Add as many filters as needed 
            if (ClientID.HasValue)
            {
                sessions = sessions.Where(s => s.ClientID == ClientID);
            }
            if (TrainerID.HasValue)
            {
                sessions = sessions.Where(p => p.TrainerID == TrainerID);
            }

            // TODO: sorting logic here (if you want to re-enable it)
            // ...

            ViewData["showArchived"] = showArchived;

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
                .AsNoTracking() //needed?
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
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(session);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateException dex)
            {
                string message = dex.GetBaseException().Message;
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
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
        public async Task<IActionResult> Edit(int id)
        {
            //Go get the musician to update
            var sessionToUpdate = await _context.Sessions.FirstOrDefaultAsync(s => s.ID == id);
            if (sessionToUpdate == null)
            {
                return NotFound();
            }

            //Try updating it with value
            if (await TryUpdateModelAsync<Session>(sessionToUpdate, "",
                t => t.SessionDate, t => t.CreatedAt, t => t.SessionsPerWeekRecommended,
                t => t.IsArchived, t => t.TrainerID, t => t.ClientID))
            {
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException dex)
                {
                    string messsage = dex.GetBaseException().Message;
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
                }
            }
            PopulateDropDownLists(sessionToUpdate);
            //ViewData["ClientID"] = new SelectList(_context.Clients, "ID", "FirstName", session.ClientID);
            //ViewData["TrainerID"] = new SelectList(_context.Trainers, "ID", "FirstName", session.TrainerID);
            return View(sessionToUpdate);
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

            try
            {
                if (session != null)
                {
                    _context.Sessions.Remove(session);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException dex)
            {
                string messsage = dex.GetBaseException().Message;
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return View(session);
        }

        // POST: Session/Archive/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Archive(int id)
        {
            var session = await _context.Sessions.FirstOrDefaultAsync(s => s.ID == id);
            if (session == null)
            {
                return NotFound();
            }

            session.IsArchived = true;

            try
            {
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Unable to archive session. Try again, and if the problem persists see your system administrator.");
                return View("Details", session);
            }
        }

        // POST: Session/Unarchive/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Unarchive(int id)
        {
            var session = await _context.Sessions.FirstOrDefaultAsync(s => s.ID == id);
            if (session == null)
            {
                return NotFound();
            }

            session.IsArchived = false;

            try
            {
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Unable to unarchive session. Try again, and if the problem persists see your system administrator.");
                return View("Details", session);
            }
        }

        private void PopulateDropDownLists(Session? session = null)
        {

            var clientObjs = from t in _context.Clients

                             orderby t.FirstName
                             select t;

            ViewData["ClientID"] = new SelectList(clientObjs, nameof(Client.ID), nameof(Client.ClientName));



            var trainerObjs = from t in _context.Trainers

                              orderby t.FirstName

                              select t;
            ViewData["TrainerID"] = new SelectList(trainerObjs, nameof(Trainer.ID), nameof(Trainer.TrainerName));

        }
        private bool SessionExists(int id)
        {
            return _context.Sessions.Any(e => e.ID == id);
        }
    }
}
