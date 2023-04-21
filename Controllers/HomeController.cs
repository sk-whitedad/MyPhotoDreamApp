using Microsoft.AspNetCore.Mvc;
using MyPhotoDreamApp.Helpers;
using MyPhotoDreamApp.Models;
using MyPhotoDreamApp.ViewModels;
using System.Diagnostics;

namespace MyPhotoDreamApp.Controllers
{
    public class HomeController : Controller
    {
        ApplicationDbContext _db;

        public HomeController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Contacts()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Authentication()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Authentication(User user)
        {
            // проходим аутентификацию
            return View();
        }

        [AcceptVerbs("Get", "Post")]
        public IActionResult CheckPhoneNumber(UserViewModel user)
        {
            if (user.PhoneNumber == "123456789")
                return Json(false);
            return Json(true);
        }

        [HttpGet]
        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registration(UserViewModel user)
        {
            if (ModelState.IsValid)
            {
                await _db.Users.AddAsync(new User
                {
                    PhoneNumber = user.PhoneNumber,
                    Password = HashPasswordHelper.HashPassowrd(user.Password),
                    Role = Enum.Role.User,
                });
                await _db.SaveChangesAsync();
                return RedirectToAction("Authentication");
            }
            return View(user);
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}