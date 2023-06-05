using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyPhotoDreamApp.Domain.Entity;
using MyPhotoDreamApp.Domain.Enum;
using MyPhotoDreamApp.Domain.ViewModels.Account;
using MyPhotoDreamApp.Service.Interfaces;
using System.Security.Claims;

namespace MyPhotoDreamApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }


        [HttpGet]
        public IActionResult Login(string returnUrl = "/Home/Index")
        {
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {

            if (ModelState.IsValid)
            {
                var response = await _accountService.Login(model);
                if (response.StatusCode == Domain.Enum.StatusCode.OK)
                {
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(response.Data));

                    return Redirect(returnUrl);
                }
                ModelState.AddModelError("", response.Description);

            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var response = await _accountService.Register(model);
                if (response.StatusCode == Domain.Enum.StatusCode.OK)
                {
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(response.Data));

                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", response.Description);
            }
            return View(model);
        }


        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllUsers()
        {
            var response = await _accountService.GetUsers();
            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                return View(response.Data);
            }
            return RedirectToAction("Error");
        }


        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditUser(int id)
        {
            var response = await _accountService.GetUser(id);
            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                List<string> sourse = new List<string>();
                sourse.Add(Role.User.ToString());
                sourse.Add(Role.Moderator.ToString());
                sourse.Add(Role.Admin.ToString());
                SelectList selectList = new SelectList(sourse, response.Data.Role.ToString());
                ViewBag.SelectItems = selectList;
                UserViewModel model = new UserViewModel()
                {
                    Id = id,
                    PhoneNumber = response.Data.PhoneNumber
                };
                return View(model);
            }
            return RedirectToAction("Error");
        }


        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditUser(int id, UserViewModel user, string SelectedItem)
        {
            if (ModelState.IsValid)
            {
                var response = await _accountService.GetUser(id);
                if (response.StatusCode == Domain.Enum.StatusCode.OK)
                {
                    Role role = Role.User;
                    if (SelectedItem == "User") role = Role.User;
                    if (SelectedItem == "Moderator") role = Role.Moderator;
                    if (SelectedItem == "Admin") role = Role.Admin;

                    user.Id = id;
                    user.Role = role;

                    var responseEdit = await _accountService.EditUser(id, user);
                    if (responseEdit.StatusCode == Domain.Enum.StatusCode.OK)
                    {
                        return RedirectToAction("GetAllUsers", "Account");
                    }
                    List<string> sourse = new List<string>();
                    sourse.Add(Role.User.ToString());
                    sourse.Add(Role.Moderator.ToString());
                    sourse.Add(Role.Admin.ToString());
                    SelectList selectList = new SelectList(sourse, SelectedItem);
                    ViewBag.SelectItems = selectList;

                    ModelState.AddModelError("", responseEdit.Description);
                }

            }
            return View(user);
        }

    }
}
