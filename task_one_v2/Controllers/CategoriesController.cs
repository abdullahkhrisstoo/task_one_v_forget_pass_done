using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using task_one_v2.App_Core.FileHelper;
using task_one_v2.App_Core.StateMangement;
using task_one_v2.Models;

namespace task_one_v2.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ModelContext _context;
        private readonly FileHelper _fileHelper;
        private readonly IStringLocalizer<CategoriesController> _localizer;
        private readonly IAdminManager _adminManager;

        public CategoriesController(ModelContext context, FileHelper fileHelper, IStringLocalizer<CategoriesController> localizer, IAdminManager adminManager)
        {
            _context = context;
            _fileHelper = fileHelper;
            _localizer = localizer;
            _adminManager = adminManager;
        }

        // GET: Categories
        public async Task<IActionResult> Index()
        {
              return _context.Categories != null ? 
                          View(await _context.Categories.ToListAsync()) :
                          Problem("Entity set 'ModelContext.Categories'  is null.");
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CategoryId,CategoryName,FormFile")] Category category)
        {
           bool check= await _adminManager.CreateCategory(category, ModelState);
            if (check == true)
            {
                TempData["SuccessMessage"] = _localizer["SuccessMessage"].Value;
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["ErrorMessage"] = _localizer["ErrorMessage"].Value;
                return View(category);
            }
        }

        // GET: Categories/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null ) return NotFound();
           
            var category = await _context.Categories.FindAsync(id);

            if (category == null)  return NotFound();
            return View(category);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal? id, [Bind("CategoryId,CategoryName,FormFile")] Category category )
        {
            bool check = await _adminManager.EditCategory(id, category);
            if (!ModelState.IsValid)
            {
                return View(category);
            }
            if (check == false)
            {
                TempData["ErrorMessage"] = _localizer["ErrorMessage"].Value;
                return View(category);
            }
            else {
                TempData["SuccessMessage"] = _localizer["SuccessMessage"].Value;
                return RedirectToAction(nameof(Index));
            }
           
           
        }
       
        // GET: Categories/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.Categories == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .FirstOrDefaultAsync(m => m.CategoryId == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }


        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            try
            {
                var category = await _context.Categories.FindAsync(id);
                if (category == null)
                {
                    return NotFound();
                }

                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = _localizer["SuccessMessage"].Value;
                return RedirectToAction(nameof(Index));

            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = _localizer["ErrorMessage"].Value;
                return RedirectToAction(nameof(Index)); 
            }
        }










       
    }
}
