using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Diagnostics;
using TurboAuto.Persistence.Contexts;
using TurboProject.Core.Helpers.Wrapper;
using TurboProject.Core.ViewModels;
using TurboProject.Models;
using TurboProject.Models.Entities;

namespace TurboProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly TurboAutoDbContext _context;

        public HomeController(TurboAutoDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {

            ViewBag.bantype = await BanTypeList();
            ViewBag.currency = await CurrencyList();
            ViewBag.make = await MakeList();
            var AnnList = _context.Announcements
                .Include(x => x.Model)
                .ThenInclude(x => x.Make)
                .Include(x => x.Currency)
                .Include(x => x.CarImageFiles)
                .Select(x => new FilterAnnouncements()
                {
                    Id = x.Id,
                    MakeName = x.Model.Make.MakeName,
                    ModelName = x.Model.ModelName,
                    Price = x.Price,
                    Year = x.Year,
                    Currency = x.Currency.CurrencyCode,
                    Image = x.CarImageFiles.FirstOrDefault().Image
                }).AsNoTracking().AsQueryable();

            var totalRecord = AnnList.Count();
            var model = await AnnList.Take(8).ToListAsync(); 
            return View(new PagedResponse<List<FilterAnnouncements>>(model, 1, 8, totalRecord));
        }

        public IActionResult Privacy()
        {
            return View();
        }
        [NonAction]
        public async Task<List<SelectListItem>> BanTypeList()
        {
            var banTypeList = await _context.BanTypes.Select(x => new SelectListItem()
            {
                Value = x.Id.ToString(),
                Text = x.Ban,
            }).ToListAsync();
            banTypeList.Insert(0, new SelectListItem
            {
                Value = "",
                Text = "Choose BanType",
                Selected = true
            });
            return banTypeList;
        }
        [NonAction]
        public async Task<List<SelectListItem>> CurrencyList()
        {
            var currencyList = await _context.Currencies.Select(x => new SelectListItem()
            {
                Value = x.Id.ToString(),
                Text = x.CurrencyCode,
            }).ToListAsync();
            return currencyList;
        }
        [NonAction]
        public async Task<List<SelectListItem>> MakeList()
        {
            var makeList = await _context.Makes.Select(x => new SelectListItem()
            {
                Value = x.Id.ToString(),
                Text = x.MakeName,
            }).ToListAsync();
            makeList.Insert(0, new SelectListItem
            {
                Value = "",
                Text = "Choose Make",
                Selected = true
            });
            return makeList;
        }

    }
}