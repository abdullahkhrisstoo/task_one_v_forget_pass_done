using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using task_one_v2.Models;

namespace task_one_v2.Controllers
{
    public class UserCredentialsController : Controller
    {
        private readonly ModelContext _context;

        public UserCredentialsController(ModelContext context)
        {
            _context = context;
        }

        // GET: UserCredentials
        public async Task<IActionResult> Index()
        {
            var modelContext = _context.UserCredentials.Include(u => u.User);
            return View(await modelContext.ToListAsync());
        }

        // GET: UserCredentials/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.UserCredentials == null)
            {
                return NotFound();
            }

            var userCredential = await _context.UserCredentials
                .Include(u => u.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userCredential == null)
            {
                return NotFound();
            }

            return View(userCredential);
        }

        // GET: UserCredentials/Create
        public IActionResult Create()
        {
            ViewData["Userid"] = new SelectList(_context.UserInfos, "Userid", "FirstName");
            return View();
        }

        // POST: UserCredentials/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Email,Password,Userid")] UserCredential userCredential)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userCredential);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Userid"] = new SelectList(_context.UserInfos, "Userid", "FirstName", userCredential.Userid);
            return View(userCredential);
        }

        // GET: UserCredentials/Edit/5




















        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.UserCredentials == null)
            {
                return NotFound();
            }

            var userCredential = await _context.UserCredentials.FindAsync(id);
            if (userCredential == null)
            {
                return NotFound();
            }
            ViewData["Userid"] = new SelectList(_context.UserInfos, "Userid", "FirstName", userCredential.Userid);
            return View(userCredential);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Id,Email,Password,Userid")] UserCredential userCredential)
        {
            if (id != userCredential.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userCredential);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserCredentialExists(userCredential.Id))
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
            ViewData["Userid"] = new SelectList(_context.UserInfos, "Userid", "FirstName", userCredential.Userid);
            return View(userCredential);
        }

        // GET: UserCredentials/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.UserCredentials == null)
            {
                return NotFound();
            }

            var userCredential = await _context.UserCredentials
                .Include(u => u.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userCredential == null)
            {
                return NotFound();
            }

            return View(userCredential);
        }

        // POST: UserCredentials/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.UserCredentials == null)
            {
                return Problem("Entity set 'ModelContext.UserCredentials'  is null.");
            }
            var userCredential = await _context.UserCredentials.FindAsync(id);
            if (userCredential != null)
            {
                _context.UserCredentials.Remove(userCredential);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserCredentialExists(decimal id)
        {
          return (_context.UserCredentials?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
