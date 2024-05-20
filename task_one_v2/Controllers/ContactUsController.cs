using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using task_one_v2.App_Core.StateMangement;
using task_one_v2.Models;

namespace task_one_v2.Controllers
{
    public class ContactUsController : Controller
    {
        private readonly ModelContext _context;
        private readonly IUserManager _userManger;

        public ContactUsController(ModelContext context, IUserManager userManger)
        {
            _context = context;
            _userManger = userManger;
        }



        // GET: ContactUs/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.ContactUs == null)
            {
                return NotFound();
            }

            var contactU = await _context.ContactUs
                .FirstOrDefaultAsync(m => m.Contactid == id);
            if (contactU == null)
            {
                return NotFound();
            }

            return View(contactU);
        }





        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.ContactUs == null)
            {
                return NotFound();
            }

            var contactU = await _context.ContactUs
                .FirstOrDefaultAsync(m => m.Contactid == id);
            if (contactU == null)
            {
                return NotFound();
            }

            return View(contactU);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.ContactUs == null)
            {
                return Problem("Entity set 'ModelContext.ContactUs'  is null.");
            }
            var contactU = await _context.ContactUs.FindAsync(id);
            if (contactU != null)
            {
                _context.ContactUs.Remove(contactU);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction("AllContactUs", "Admin");
        }

        private bool ContactUExists(decimal id)
        {
          return (_context.ContactUs?.Any(e => e.Contactid == id)).GetValueOrDefault();
        }



        public IActionResult CreateContact() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateContact([Bind("Contactid,Name,Email,Subject,Message")] ContactU contactU)
        {
            if (ModelState.IsValid)
            {
                bool isSuccess = await _userManger.SendContactUs(contactU);
                if (isSuccess) return RedirectToAction("Home","Home");
                else return NotFound();
            }
            return View(contactU);
        }
    }
}
