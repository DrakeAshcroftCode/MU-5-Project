using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using MU5PrototypeProject.Data;
using MU5PrototypeProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MU5PrototypeProject.Controllers
{
    public class ClientController : Controller
    {
        private readonly MUContext _context;

        public ClientController(MUContext context)
        {
            _context = context;
        }

        // GET: Client
        public async Task<IActionResult> Index(string SearchName, string SearchPhone)
        {
            try
            {
                //Count the number of filters applied - start by assuming no filters
                ViewData["Filtering"] = "btn-outline-secondary";
                int numberFilters = 0;
                var clients = await _context.Clients
                        .AsNoTracking()
                        .ToListAsync();

                if (!String.IsNullOrEmpty(SearchName))
                {
                    clients = clients.Where(s => s.FirstName.ToUpper().Contains(SearchName.ToUpper())
                                            || s.LastName.ToUpper().Contains(SearchName.ToUpper())).ToList();
                        numberFilters++;
                }
                if (!String.IsNullOrEmpty(SearchPhone))
                {
                    clients = clients.Where(s => s.Phone.ToUpper().Contains(SearchPhone.ToUpper())).ToList();
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

                return View(clients);
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
                if (client.DOB > DateTime.Today)
                {
                    ModelState.AddModelError("DOB", "Date Of Birth must not be in the future");
                }
                else if (client.Age <= 7)
                {
                    ModelState.AddModelError("DOB", "Client must be at least 7 years old");
                }
                else
                {
                    if (ModelState.IsValid)
                    {
                        client.CreatedAt = DateTime.Now; // <- set by system

                        _context.Add(client);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Details), new { client.ID });
                    }
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

        private bool ClientExists(int id)
        {
            return _context.Clients.Any(e => e.ID == id);
        }
    }
}
