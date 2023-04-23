﻿using Microsoft.AspNetCore.Mvc;
using MyPhotoDreamApp.Domain.Entity;
using MyPhotoDreamApp.Models;
using System.Diagnostics;

namespace MyPhotoDreamApp.Controllers
{
    public class HomeController : Controller
    {
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

          [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}