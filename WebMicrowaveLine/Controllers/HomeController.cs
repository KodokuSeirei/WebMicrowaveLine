using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WebMicrowaveLine.Models;

namespace WebMicrowaveLine.Controllers
{
    public class HomeController : Controller
    {

        UserManager<User> _userManager;
        ApplicationContext db;

        private IStringLocalizer<HomeController> _localizer;
        public HomeController(ApplicationContext context, UserManager<User> userManager, IStringLocalizer<HomeController> localizer)
        {
            _userManager = userManager;
            db = context;
            _localizer = localizer;
        }
        //Отображение очереди
        public async Task<IActionResult> Index(SortState sortOrder = SortState.PositionAsc)
        {
            IQueryable<Queue> queues = db.Queues;
            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user.Position == 0 || user == null)
                    ViewBag.Error = _localizer["YouAreNotInQueue"];
                else ViewBag.Error = _localizer["YouAreInQueue"];
            }
            //Сортировка
            #region
            ViewData["RoomSort"] = sortOrder == SortState.RelaxRoomNameAsc ? SortState.RelaxRoomNameDesc : SortState.RelaxRoomNameAsc;
            ViewData["MicrowaveSort"] = sortOrder == SortState.MicrowaveNameAsc ? SortState.MicrowaveNameDesc : SortState.MicrowaveNameAsc;
            ViewData["PositionSort"] = sortOrder == SortState.PositionAsc ? SortState.PositionDesc : SortState.PositionAsc;
            ViewData["UserNameSort"] = sortOrder == SortState.UserNameAsc ? SortState.UserNameDesc : SortState.UserNameAsc;
            ViewData["UserEmailSort"] = sortOrder == SortState.UserEmailAsc ? SortState.UserEmailDesc : SortState.UserEmailAsc;
            switch (sortOrder)
            {
                case SortState.RelaxRoomNameAsc:
                    queues = queues.OrderBy(s => s.RelaxRoomName);
                    break;
                case SortState.RelaxRoomNameDesc:
                    queues = queues.OrderByDescending(s => s.RelaxRoomName);
                    break;
                case SortState.MicrowaveNameAsc:
                    queues = queues.OrderBy(s => s.MicrowaveName);
                    break;
                case SortState.MicrowaveNameDesc:
                    queues = queues.OrderByDescending(s => s.MicrowaveName);
                    break;
                case SortState.PositionAsc:
                    queues = queues.OrderByDescending(s => s.NumberPosition);

                    break;
                case SortState.UserNameAsc:
                    queues = queues.OrderBy(s => s.UserName);
                    break;
                case SortState.UserEmailAsc:
                    queues = queues.OrderBy(s => s.UserEmail);
                    break;
                case SortState.UserEmailDesc:
                    queues = queues.OrderByDescending(s => s.UserEmail);
                    break;
                default:
                    queues = queues.OrderByDescending(s => s.UserName);
                    break;
            }

            #endregion
            return View(await queues.AsNoTracking().ToListAsync());
        }
        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            return LocalRedirect(returnUrl);
        }
        //Удаление из очереди
        public async Task<ActionResult> Delete(int id)
        {
            var queue = await db.Queues.FindAsync(id);
            var user = await db.Users.FirstOrDefaultAsync(o => o.UserName == queue.UserName);

            if (queue != null)
            {
                user.Position = 0;
                db.Queues.Remove(queue);
                await db.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }


        public async Task<ActionResult> ExitTheQueue()
        {
           
            var user = await _userManager.GetUserAsync(User);
            var queue = await db.Queues.FirstOrDefaultAsync(o => o.UserName == user.UserName);
            if (user.Position != 0 && queue != null)
            {
                user.Position = 0;
                db.Queues.Remove(queue);
                await db.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }

        public IActionResult MicrowaveSelection()
        {
            return View();
        }
        //Запись на микроволновку
        #region
        public async Task<IActionResult> ToGetInQueue(string action)
        {
            var user = await _userManager.GetUserAsync(User);
            if (ModelState.IsValid && user.Position == 0)
            {

                string relaxRoom;
                string microwave;
                switch (action)
                {
                    case "firstMicrowave":
                        relaxRoom = "Царский релакс рум";
                        microwave = "Микроволновка.1.1";
                        break;
                    case "secondMicrowave":
                        relaxRoom = "Царский релакс рум";
                        microwave = "Микроволновка.1.2";
                        break;
                    case "thirdMicrowave":
                        relaxRoom = "Обычный релакс рум";
                        microwave = "Микроволновка.2.1";
                        break;
                    default:
                        relaxRoom = "Обычный релакс рум";
                        microwave = "Микроволновка.2.2";
                        break;
                }

                if (db.Queues.Where(p => p.MicrowaveName == microwave).Count() != 0)
                { user.Position = db.Queues.Where(p => p.MicrowaveName == microwave).OrderByDescending(x => x.NumberPosition).FirstOrDefault().NumberPosition + 1; }
                else
                { user.Position = 1; }
                Queue queue = new Queue { RelaxRoomName = relaxRoom, MicrowaveName = microwave, NumberPosition = user.Position, UserName = user.UserName, UserEmail = user.Email };
                db.Queues.Add(queue);
                await db.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }
        #endregion

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
