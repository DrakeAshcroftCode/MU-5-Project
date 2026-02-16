using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using MU5PrototypeProject.CustomController;
using MU5PrototypeProject.Data;
using MU5PrototypeProject.Models;
using MU5PrototypeProject.Utilities;    
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MU5PrototypeProject.Controllers
{
    public class ClientController : CognizantController
    {
        private readonly MUContext _context;

        public ClientController(MUContext context)
        {
            _context = context;
        }

        // GET: Client
        public async Task<IActionResult> Index(
            string SearchName,
            string SearchPhone,
            string actionButton,
            int? page,
            int? pageSizeID,
            bool showArchived = false,       
            string sortDirection = "asc",
            string sortField = "Client")
        {
            try
            {
                //List of sort options.
                //NOTE: make sure this array has matching values to the column headings
                string[] sortOptions = new[] { "Client" };

                //Count the number of filters applied - start by assuming no filters
                ViewData["Filtering"] = "btn-outline-secondary";
                int numberFilters = 0;
                var clients = _context.Clients
                        .AsNoTracking();

                // Apply archived filter first
                if (!showArchived)
                {
                    clients = clients.Where(c => !c.IsArchived);
                }
                else
                {
                    numberFilters++; // count as a filter when showing archived
                }

                if (!string.IsNullOrEmpty(SearchName))
                {
                    clients = clients.Where(s => s.FirstName.ToUpper().Contains(SearchName.ToUpper())
                                            || s.LastName.ToUpper().Contains(SearchName.ToUpper()));
                    numberFilters++;
                }

                if (!string.IsNullOrEmpty(SearchPhone))
                {
                    clients = clients.Where(s => s.Phone.ToUpper().Contains(SearchPhone.ToUpper()));
                    numberFilters++;
                }

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
                    if (sortOptions.Contains(actionButton))//Change of sort is requested
                    {
                        page = 1; //Reset page to start when changing sort or filters
                        if (actionButton == sortField) //Reverse order on same field
                        {
                            sortDirection = sortDirection == "asc" ? "desc" : "asc";
                        }
                        sortField = actionButton;//Sort by the button clicked
                    }
                }

                if (sortField == "Client")
                {
                    if (sortDirection == "asc")
                    {
                        clients = clients
                            .OrderBy(c => c.FirstName)
                            .ThenBy(c => c.LastName);
                            
                    }
                    else
                    {
                        clients = clients
                            .OrderByDescending(c => c.FirstName)
                            .ThenBy(c => c.LastName);
                    }
                }
                ViewData["sortDirection"] = sortDirection;
                ViewData["sortField"] = sortField;
                ViewData["showArchived"] = showArchived;

                int pageSize = PageSizeHelper.SetPageSize(HttpContext, pageSizeID, ControllerName());
                ViewData["pageSizeID"] = PageSizeHelper.PageSizeList(pageSize);

                var pagedData = await PaginatedList<Client>.CreateAsync(clients.AsNoTracking(), page ?? 1, pageSize);

                return View(pagedData);
            }
            catch (Exception)
            {
                return Problem("Unable to load clients. Please try again later.");
            }
        }


        // GET: Client/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await _context.Clients
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);
            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        // GET: Client/Create
        public IActionResult Create()
        {
            DateTime today = DateTime.Today;
            DateTime maxDate = today.AddYears(-7);   // Must be at least 7
            DateTime minDate = today.AddYears(-120); 

            ViewData["MaxDOB"] = maxDate.ToString("yyyy-MM-dd");
            ViewData["MinDOB"] = minDate.ToString("yyyy-MM-dd");
            ViewData["DefaultDOB"] = maxDate.ToString("yyyy-MM-dd");

            return View();
        }

        // POST: Client/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,FirstName,LastName,DOB,Phone,Email,ClientFolderUrl,IsArchived")] Client client)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    
                    client.CreatedAt = DateTime.Now; // <- set by system

                    _context.Add(client);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Details), new { client.ID });
                }
            }
            catch (RetryLimitExceededException)
            {
                ModelState.AddModelError("", "Unable to save changes after multiple attempts. " +
                    "Try again, and if the problem persists, see your system administrator.");
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }

            return View(client);
        }

        // GET: Client/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await _context.Clients.FindAsync(id);
            if (client == null)
            {
                return NotFound();
            }
            return View(client);
        }

        // POST: Client/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id)
        {
            //Go get the client to update from the database
            var clientToUpdate = await _context.Clients.FirstOrDefaultAsync(c => c.ID == id);

            //Check if the client exists
            if (clientToUpdate == null)
            {
                return NotFound();
            }

            if (await TryUpdateModelAsync(clientToUpdate, "",
                c => c.FirstName,
                c => c.LastName,
                c => c.DOB,
                c => c.Phone,
                c => c.Email,
                c => c.ClientFolderUrl,
                c => c.IsArchived))  // CreatedAt removed here
    {
        try
        {
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ClientExists(clientToUpdate.ID))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }
        catch (RetryLimitExceededException)
        {
            ModelState.AddModelError("", "Unable to save changes after multiple attempts. " +
                "Try again, and if the problem persists, see your system administrator.");
        }
        catch (DbUpdateException)
        {
            ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
        }
    }

    return View(clientToUpdate);
        }

        // GET: Client/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await _context.Clients
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);
            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        // POST: Client/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var client = await _context.Clients
                .FirstOrDefaultAsync(m => m.ID == id);
            try
            {
                if (client != null)
                {
                    _context.Clients.Remove(client);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                //Log the error (uncomment ex variable name and write a log.)   
                ModelState.AddModelError("", "Unable to delete client. Try again, and if the problem persists see your system administrator.");
            }
            return View(client);
        }

        // POST: Client/Archive/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Archive(int id)
        {
            var client = await _context.Clients.FirstOrDefaultAsync(c => c.ID == id);
            if (client == null)
            {
                return NotFound();
            }

            client.IsArchived = true;

            try
            {
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Unable to archive client. Try again, and if the problem persists see your system administrator.");
                return View("Details", client);
            }
        }

        // POST: Client/Unarchive/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Unarchive(int id)
        {
            var client = await _context.Clients.FirstOrDefaultAsync(c => c.ID == id);
            if (client == null)
            {
                return NotFound();
            }

            client.IsArchived = false;

            try
            {
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Unable to unarchive client. Try again, and if the problem persists see your system administrator.");
                return View("Details", client);
            }
        }

        private bool ClientExists(int id)
        {
            return _context.Clients.Any(e => e.ID == id);
        }
    }
}
