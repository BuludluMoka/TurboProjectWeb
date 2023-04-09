using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.Extensions.Configuration.UserSecrets;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using SixLabors.ImageSharp.Formats.Tiff;
using System.ComponentModel.DataAnnotations;
using TurboAuto.Persistence.Contexts;
using TurboProject.Core.Helpers.Wrapper;
using TurboProject.Core.ViewModels;
using TurboProject.Models.Entities;
using TurboProject.Models.Entities.Identity;
using Web.Helper;

namespace TurboProject.Controllers
{
    [Authorize]
    public class AnnouncementController : Controller
    {
        private readonly TurboAutoDbContext _context;
        private readonly ImageManager _imageManager;
        private readonly UserManager<AppUser> _userManager;

        public AnnouncementController(TurboAutoDbContext context, ImageManager manager, UserManager<AppUser> userManager)
        {
            _context = context;
            _imageManager = manager;
            _userManager = userManager;
        }
        public async Task<IActionResult> CreateAnnouncement()
        {
            var model = new CreateAnnouncementModel();

            ViewBag.bantype = await BanTypeList();
            ViewBag.currency = await CurrencyList();
            ViewBag.make = await MakeList();

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> CreateAnnouncement(CreateAnnouncementModel Announce)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.bantype = await BanTypeList();
                ViewBag.currency = await CurrencyList();
                ViewBag.make = await MakeList();
                return View(Announce);
            }
            List<string> imageNames = _imageManager.UploadedFile(Announce.images);
            var userId = _userManager.GetUserId(User);

            ICollection<CarImageFile> carImageFiles = imageNames.Select(x => new CarImageFile()
            {
                Image = x,
            }).ToList();
            var Announcement = new Announcement()
            {
                AppUserId = userId,
                BanTypeId = Announce.BanTypeId,
                CarImageFiles = carImageFiles,
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
                Price = Announce.Price,
                ModelId = Announce.ModelId,
                CurrencyId = Announce.CurrencyId,
                Year = (int)Announce.Year
            };
            _context.Announcements.Add(Announcement);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }
        [AllowAnonymous]
        [HttpGet("{id:long}")]
        public async Task<IActionResult> DetailAnnouncement(long id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var entity = await _context.Announcements
                .Include(x => x.Model)
                .ThenInclude(x => x.Make)
                .Include(x => x.CarImageFiles)
                .Include(x => x.AppUser)
                .Include(x => x.BanType)
                .Include(x => x.Currency)
                .Where(x => x.Id == id)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
            if (entity == null)
            {
                return NotFound();
            }

            DetailsAnnouncementModel model = new DetailsAnnouncementModel()
            {
                Id = entity.Id,
                Make = entity.Model.Make.MakeName,
                Model = entity.Model.ModelName,
                BanType = entity.BanType.Ban,
                Currencie = entity.Currency.CurrencyCode,
                Price = entity.Price,
                Year = entity.Year,
                RelaseDate = entity.CreatedDate,
                OwnerContact = entity.AppUser.Email,
                CarImageFiles = entity.CarImageFiles.Select(x => x.Image).ToList()
            };

            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> EditAnnouncement(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var userId = _userManager.GetUserId(User);
            ViewBag.makes = await MakeList();
            ViewBag.bantypes = await BanTypeList();
            ViewBag.currencies = await CurrencyList();
            var Announcement = await _context.Announcements
                .Include(x => x.Model)
                .ThenInclude(x => x.Make)
                .Include(x => x.CarImageFiles)
                .Include(x => x.AppUser)
                .Include(x => x.BanType)
                .Include(x => x.Currency)
                .Where(an => an.AppUserId == userId && an.Id == id)
                .FirstOrDefaultAsync();
            if (Announcement == null) return NotFound();
            ViewBag.model = await GetModelList(Announcement.Model.MakeId);
            var model = new EditAnnouncementModel()
            {
                AnnouncementId = Announcement.Id,
                MakeId = Announcement.Model.Make.Id,
                ModelId = Announcement.Model.Id,
                BanTypeId = Announcement.BanTypeId,
                CurrencyId = Announcement.CurrencyId,
                Price = Announcement.Price,
                Year = Announcement.Year,
                CarImageFiles = Announcement.CarImageFiles.Select(x => x.Image).ToList()
            };

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> EditAnnouncement(EditAnnouncementModel model, [Required]List<IFormFile> images)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.bantype = await BanTypeList();
                ViewBag.currency = await CurrencyList();
                ViewBag.make = await MakeList();
                model.CarImageFiles = await _context.CarImageFiles.Where(x=>x.AnnouncementId == model.AnnouncementId)
                    .Select(x => x.Image).ToListAsync();
                return View(model);
            }
            List<string> imageNames = _imageManager.UploadedFile(images);

            ICollection<CarImageFile> carImageFiles = imageNames.Select(x => new CarImageFile()
            {
                Image = x,
            }).ToList();

            var announcement = await _context.Announcements.FirstOrDefaultAsync(x => x.Id == model.AnnouncementId);
            if (announcement == null) return NotFound();
            announcement.ModelId = model.ModelId;
            announcement.BanTypeId = model.BanTypeId;
             announcement.CurrencyId = model.CurrencyId;
             announcement.Price = model.Price;
            announcement.Year = (int)model.Year;
             announcement.UpdatedDate = DateTime.Now;
             announcement.CarImageFiles = carImageFiles;
            _context.Announcements.Update(announcement);
             await _context.SaveChangesAsync();

            return RedirectToAction("MyAnnouncement", "Account");
        }
        public async Task<IActionResult> DeleteAnnouncementImg(string imgName)
        {
            var img = await _context.CarImageFiles.FirstOrDefaultAsync(x => x.Image == imgName);
            if (img != null)
            {
                _context.CarImageFiles.Remove(img);
                await _context.SaveChangesAsync();
                _imageManager.DeleteImage(img.Image);
            }
            return Redirect("/Announcement/EditAnnouncement/" + img.AnnouncementId);
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<List<SelectListItem>> GetModelList(long id)
        {
            return await _context.Models.Where(x => x.MakeId == id).Select(x => new SelectListItem()
            {
                Value = x.Id.ToString(),
                Text = x.ModelName
            }).ToListAsync();
        }
        [AllowAnonymous]
        public async Task<PartialViewResult> AnnouncementFilterList([FromQuery] GetAnnounceFilterParams model)
        {
            int totalRecords;
            var ListAnnouncement = _context.GetAnnouncements(model, out totalRecords);
            return PartialView("_AnnouncementFilterListPartial",
                 new PagedResponse<List<FilterAnnouncements>>(ListAnnouncement, model.PageIndex, model.PageSize, totalRecords));
        }

        //public async Task<PartialViewResult> AnnouncementList([FromQuery] int? page, string? searchString, string? sort)
        //{
        //    PaginationFilter Filter = new PaginationFilter(page ?? 1, 8);

        //    IQueryable<Announcement> AnnList = _context.Announcements
        //         .Include(x => x.Model)
        //         .ThenInclude(x => x.Make)
        //         .Include(x => x.Currency)
        //         .Include(x => x.CarImageFiles).AsNoTracking().AsQueryable();
        //    //if (!string.IsNullOrEmpty(searchString))
        //    //{
        //    //    AnnList = AnnList.Where(x => x.Tittle.Contains(searchString));
        //    //}
        //    //switch (sort)
        //    //{
        //    //    case "newest":
        //    //        AnnList = AnnList.OrderByDescending(x => x.CreatedDate);
        //    //        break;
        //    //    case "votes":
        //    //        AnnList = AnnList.OrderByDescending(x => x.VoteCount);
        //    //        break;
        //    //    case "view":
        //    //        AnnList = AnnList.OrderBy(x => x.ViewCount).ThenByDescending(x => x.VoteCount);
        //    //        break;

        //    //    default:
        //    //        break;
        //    //}
        //    var totalPost = AnnList.Count();
        //    List<ListAnnouncementModel> model = await AnnList.Select(x => new ListAnnouncementModel()
        //    {
        //        Id = x.Id,
        //        CarName = x.Model.ModelName + x.Model.Make.MakeName,
        //        Price = x.Price,
        //        Year = x.Year,
        //        Currencie = x.Currency.CurrencyCode,
        //        Image = x.CarImageFiles.FirstOrDefault().Image
        //    }).Skip((Filter.PageNumber - 1) * (Filter.PageSize))
        //       .Take(Filter.PageSize)
        //       .ToListAsync();

        //    return PartialView("_AnnouncementListPartial",
        //        new PagedResponse<List<ListAnnouncementModel>>(model, Filter.PageNumber, Filter.PageSize, totalPost));
        //}




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
