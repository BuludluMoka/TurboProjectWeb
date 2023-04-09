using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TurboAuto.Persistence.Contexts;
using TurboProject.Core.ViewModels;
using TurboProject.Models.Entities.Identity;

namespace TurboProject.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly TurboAutoDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public AccountController(TurboAutoDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<IActionResult> MyAnnouncement()
        {
            var userId = _userManager.GetUserId(User);
            var AnnList = await _context.Announcements
                  .Include(x => x.Model)
                  .ThenInclude(x => x.Make)
                  .Include(x => x.Currency)
                  .Include(x => x.CarImageFiles)
                  .Where(x => x.AppUserId == userId)
                  .Select(x => new ListAnnouncementModel()
                  {
                      Id = x.Id,
                      CarName = x.Model.ModelName + x.Model.Make.MakeName,
                      Price = x.Price,
                      Year = x.Year,
                      Currencie = x.Currency.CurrencyCode,
                      Image = x.CarImageFiles.FirstOrDefault().Image
                  }).AsNoTracking().ToListAsync();

            return View(AnnList);
        }
        public async Task<IActionResult> DeleteAnnouncement(long id)
        {
            var annoucement = await _context.Announcements.FirstOrDefaultAsync(x => x.Id == id);
            if (annoucement == null) return NotFound();
            _context.Announcements.Remove(annoucement);
            await _context.SaveChangesAsync();  

            return Ok();
        }
    }
}
