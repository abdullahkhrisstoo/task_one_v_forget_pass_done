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
    public class UserInfoesController : Controller
    {
        private readonly ModelContext _context;

        public UserInfoesController(ModelContext context)
        {
            _context = context;
        }

        // GET: UserInfoes
        public async Task<IActionResult> Index()
        {
            var modelContext = _context.UserInfos.Include(u => u.Role).Include(u => u.VerificationStatus);
            return View(await modelContext.ToListAsync());
        }

        // GET: UserInfoes/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.UserInfos == null)
            {
                return NotFound();
            }

            var userInfo = await _context.UserInfos
                .Include(u => u.Role)
                .Include(u => u.VerificationStatus)
                .FirstOrDefaultAsync(m => m.Userid == id);
            if (userInfo == null)
            {
                return NotFound();
            }

            return View(userInfo);
        }

        // GET: UserInfoes/Create
        public IActionResult Create()
        {
            ViewData["Roleid"] = new SelectList(_context.Roles, "RoleId", "RoleId");
            ViewData["VerificationStatusId"] = new SelectList(_context.UserVerificationStatuses, "VerificationStatusId", "VerificationStatusId");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Userid,FirstName,LastName,Image,Nationality,Roleid,VerificationStatusId,CertificateImage")] UserInfo userInfo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userInfo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Roleid"] = new SelectList(_context.Roles, "RoleId", "RoleId", userInfo.Roleid);
            ViewData["VerificationStatusId"] = new SelectList(_context.UserVerificationStatuses, "VerificationStatusId", "VerificationStatusId", userInfo.VerificationStatusId);
            return View(userInfo);
        }

        // GET: UserInfoes/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.UserInfos == null)
            {
                return NotFound();
            }

            var userInfo = await _context.UserInfos.FindAsync(id);
            if (userInfo == null)
            {
                return NotFound();
            }
            ViewData["Roleid"] = new SelectList(_context.Roles, "RoleId", "RoleId", userInfo.Roleid);
            ViewData["VerificationStatusId"] = new SelectList(_context.UserVerificationStatuses, "VerificationStatusId", "VerificationStatusId", userInfo.VerificationStatusId);
            return View(userInfo);
        }

        // POST: UserInfoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, UserInfo userInfo)
        {
            if (id != userInfo.Userid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userInfo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserInfoExists(userInfo.Userid))
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
            ViewData["Roleid"] = new SelectList(_context.Roles, "RoleId", "RoleId", userInfo.Roleid);
            ViewData["VerificationStatusId"] = new SelectList(_context.UserVerificationStatuses, "VerificationStatusId", "VerificationStatusId", userInfo.VerificationStatusId);
            return View(userInfo);
        }

        // GET: UserInfoes/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.UserInfos == null)
            {
                return NotFound();
            }

            var userInfo = await _context.UserInfos
                .Include(u => u.Role)
                .Include(u => u.VerificationStatus)
                .FirstOrDefaultAsync(m => m.Userid == id);
            if (userInfo == null)
            {
                return NotFound();
            }

            return View(userInfo);
        }

        // POST: UserInfoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.UserInfos == null)
            {
                return Problem("Entity set 'ModelContext.UserInfos'  is null.");
            }
            var userInfo = await _context.UserInfos.FindAsync(id);
            if (userInfo != null)
            {
                _context.UserInfos.Remove(userInfo);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserInfoExists(decimal id)
        {
          return (_context.UserInfos?.Any(e => e.Userid == id)).GetValueOrDefault();
        }
    }
}
