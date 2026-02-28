using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MU5PrototypeProject.CustomController;
using MU5PrototypeProject.Data;
using MU5PrototypeProject.Models;
using MU5PrototypeProject.Utilities;

namespace MU5PrototypeProject.Controllers
{
    public class SessionController : CognizantController
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
            string SearchClient,
            string actionButton,
            int? page,
            int? pageSizeID,
            bool showArchived = false,
            string sortDirection = "asc",
            string sortField = "Session")
        {

            //List of sort options.
            //NOTE: make sure this array has matching values to the column headings
            string[] sortOptions = new[] { "Client", "Trainer", "SessionDate" };

            //Count the number of filters applied - start by assuming no filters
            ViewData["Filtering"] = "btn-outline-secondary";
            int numberFilters = 0;
            //Then in each "test" for filtering, add to the count of Filters applied

            PopulateDropDownLists();//Data for Session filter for ddl

            var sessions = _context.Sessions
                .Include(s => s.Client)
                .Include(s => s.Trainer)
                .Include(s => s.SessionNotes)
                .Include(s => s.PhysioInfo)
                .Include(s => s.AdminStatus)
                .AsNoTracking()
                .AsQueryable();

            // Hide archived sessions by default
            if (!showArchived)
            {
                sessions = sessions.Where(s => !s.IsArchived);
            }
            else
            {
                numberFilters++; // count as a filter when showing archived
            }
            //Add as many filters as needed 
            if (!string.IsNullOrEmpty(SearchClient))
            {
                sessions = sessions.Where(s => s.Client.FirstName.ToUpper().Contains(SearchClient.ToUpper())
                                        || s.Client.LastName.ToUpper().Contains(SearchClient.ToUpper()));
                numberFilters++;
            }
            if (TrainerID.HasValue)
            {
                sessions = sessions.Where(p => p.TrainerID == TrainerID);
                numberFilters++;
            }
            //if (!string.IsNullOrEmpty(SearchSessionDate))
            //{
            //    sessions = sessions.Where(p => p.SearchSessionDate.Date.Contains(SearchSessionDate));
            //    numberFilters++;
            //}

            //Give feedback about the state of the filters
            if (numberFilters != 0)
            {
                //Toggle the Open/Closed state of the collapse depending on if we are filtering
                ViewData["Filtering"] = " btn-danger";
                //Show how many filters have been applied
                ViewData["numberFilters"] = "(" + numberFilters.ToString()
                    + " Filter" + (numberFilters > 1 ? "s" : "") + " Applied)";
                //Keep the Bootstrap collapse open
                @ViewData["ShowFilter"] = " show";
            }
            //Before we sort, see if we have called for a change of filtering or sorting
            if (!String.IsNullOrEmpty(actionButton)) //Form Submitted!
            {
                page = 1;//Reset page to start

                if (sortOptions.Contains(actionButton))//Change of sort is requested
                {
                    if (actionButton == sortField) //Reverse order on same field
                    {
                        sortDirection = sortDirection == "asc" ? "desc" : "asc";
                    }
                    sortField = actionButton;//Sort by the button clicked
                }
            }
            //Now we know which field and direction to sort by
            if (sortField == "Client")
            {
                if (sortDirection == "asc")
                {
                    sessions = sessions
                        .OrderBy(p => p.Client.FirstName);
                }
                else
                {
                    sessions = sessions
                        .OrderByDescending(p => p.Client.FirstName);
                }
            }
            else if (sortField == "Trainer")
            {
                if (sortDirection == "asc")
                {
                    sessions = sessions
                        .OrderBy(p => p.Trainer.LastName);
                }
                else
                {
                    sessions = sessions
                        .OrderByDescending(p => p.Trainer.LastName);
                }
            }
            else //Sorting by Sessio date
            {
                if (sortDirection == "asc")
                {
                    sessions = sessions
                        .OrderByDescending(p => p.SessionDate)
                        .ThenBy(p => p.Client.FirstName)
                        .ThenBy(p => p.Trainer.LastName);
                }
                else
                {
                    sessions = sessions
                        .OrderBy(p => p.SessionDate)
                        .ThenBy(p => p.Client.FirstName)
                        .ThenBy(p => p.Trainer.LastName);
                }
            }
            //Set sort for next time
            ViewData["sortField"] = sortField;
            ViewData["sortDirection"] = sortDirection;
            ViewData["showArchived"] = showArchived;

            //Handle Paging
            int pageSize = PageSizeHelper.SetPageSize(HttpContext, pageSizeID, ControllerName());
            ViewData["pageSizeID"] = PageSizeHelper.PageSizeList(pageSize);

            var pagedData = await PaginatedList<Session>.CreateAsync(sessions.AsNoTracking(), page ?? 1, pageSize);

            //return View(await sessions.ToListAsync());
            return View(pagedData);
        }


        // GET: Session/Details/5
        public async Task<IActionResult> Details(int? id, string PostResult = "Details")
        {
            if (id == null)
            {
                return NotFound();
            }

            var session = await _context.Sessions
                .Include(s => s.Client)
                .Include(s => s.Trainer)
                .Include(s => s.SessionNotes)
                .Include(s => s.PhysioInfo)
                .Include(s => s.AdminStatus)
                .Include(s => s.ExerciseSettings)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);
            if (session == null)
            {
                return NotFound();
            }

            ViewData["PostResult"] = PostResult;

            return View(session);
        }

        // GET: Session/Create
        public IActionResult Create(int? clientId)
        {
            //default session date to today
            DateTime today = DateTime.Today;
            ViewData["DefaultSessionDate"] = today.ToString("yyyy-MM-dd");

            Session session = new Session();

            if (clientId != null)
                session.ClientID = clientId.Value;

            PopulateDropDownLists(session);
            return View(session);
        }

        // POST: Session/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,SessionDate,SessionsPerWeekRecommended,IsArchived,SessionNoteID, AdminStatusID, PhysioInfoID, TrainerID, ClientID")] Session session)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(session);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Details), new { session.ID, PostResult = "Create" });
                }
            }
            catch (DbUpdateException dex)
            {
                string message = dex.GetBaseException().Message;
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }

            PopulateDropDownLists(session);
            return View(session);
        }

        // GET: Session/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var session = await _context.Sessions/*.FindAsync(id);*/
                .Include(s => s.Client)
                .Include(s => s.Trainer)
                .FirstOrDefaultAsync(s => s.ID == id);


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
            //Go get the session to update
            var sessionToUpdate = await _context.Sessions.FirstOrDefaultAsync(s => s.ID == id);
            if (sessionToUpdate == null)
            {
                return NotFound();
            }

            //Try updating it with value
            if (await TryUpdateModelAsync<Session>(sessionToUpdate, "",
                t => t.SessionDate, t => t.SessionsPerWeekRecommended,
                t => t.IsArchived, t => t.SessionNotes, t => t.AdminStatus, t => t.PhysioInfo, t => t.ExerciseSettings, t => t.TrainerID, t => t.ClientID))
            {

                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Details), new { id, PostResult = "Edit" });
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
                .Include(s => s.SessionNotes)
                .Include(s => s.PhysioInfo)
                .Include(s => s.AdminStatus)
                .Include(s => s.ExerciseSettings)
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

        private SelectList ClientSelectList(int? selectedId)
        {
            return new SelectList(_context.Clients
                .OrderBy(d => d.FirstName)
                .ThenBy(d => d.LastName), "ID", "FullName", selectedId);
        }

        private SelectList TrainerList(int? selectedId)
        {
            return new SelectList(_context.Trainers
                .OrderBy(d => d.LastName)
                .ThenBy(d => d.FirstName), "ID", "TrainerName", selectedId);
        }
        private void PopulateDropDownLists(Session? session = null)
        {
            ViewData["ClientID"] = ClientSelectList(session?.ClientID);
            ViewData["TrainerID"] = TrainerList(session?.TrainerID);
        }
        private bool SessionExists(int id)
        {
            return _context.Sessions.Any(e => e.ID == id);
        }
    }
}
