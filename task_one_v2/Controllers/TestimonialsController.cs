using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using task_one_v2.App_Core.ConstString;
using task_one_v2.App_Core.StateMangement;
using task_one_v2.Models;

namespace task_one_v2.Controllers
{
    public class TestimonialsController : Controller
    {
        private readonly ModelContext _context;
        private readonly IAuthManager _authManager;

        public TestimonialsController(ModelContext context, IAuthManager authManager)
        {
            _context = context;
            _authManager = authManager;
        }

        public async Task<IActionResult> Index()
        {
            var modelContext = _context.Testimonials.Include(t => t.ApprovalStatus).Include(t => t.User);
            return View(await modelContext.ToListAsync());
        }

        public IActionResult CreateTestimonial()
        {
            ViewData["ApprovalStatusId"] = new SelectList(_context.TestimonialApprovalStatuses, "ApprovalStatusId", "StatusName");
            ViewData["UserId"] = new SelectList(_context.UserInfos, "Userid", "Userid");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateTestimonial([Bind("TestimonialId,UserId,TestimonialText,DateTime,StatusName,Userimg")] Testimonial testimonial)
        {
            var currentUserData = _authManager.GetCurrentUserData();
            if (currentUserData == null) return NotFound();
            if (String.IsNullOrEmpty(testimonial.TestimonialText)) {
                ModelState.AddModelError("","");
            }
            testimonial.DateTime = DateTime.Now;
            testimonial.ApprovalStatusId = ConstantApp.pendingTestimonial;
            testimonial.UserId = currentUserData?.Userid ?? 0;
            testimonial.Userimg = currentUserData?.Image?? "~/Globalimg/chef_demo.jpg";
            if (ModelState.IsValid)
            {
                _context.Add(testimonial);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ApprovalStatusId"] = new SelectList(_context.TestimonialApprovalStatuses, "ApprovalStatusId", "StatusName", testimonial.ApprovalStatusId);
            ViewData["UserId"] = new SelectList(_context.UserInfos, "Userid", "Userid", testimonial.UserId);
            return View(testimonial);
        }
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.Testimonials == null)
            {
                return NotFound();
            }
            var testimonial = await _context.Testimonials
                .Include(t => t.ApprovalStatus)
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.TestimonialId == id);
            if (testimonial == null)
            {
                return NotFound();
            }
            return View(testimonial);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.Testimonials == null)
            {
                return Problem("Entity set 'ModelContext.Testimonials'  is null.");
            }
            var testimonial = await _context.Testimonials.FindAsync(id);
            if (testimonial != null)
            {
                _context.Testimonials.Remove(testimonial);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TestimonialExists(decimal id)
        {
          return (_context.Testimonials?.Any(e => e.TestimonialId == id)).GetValueOrDefault();
        }
    }
}
