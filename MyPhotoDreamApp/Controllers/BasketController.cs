using Microsoft.AspNetCore.Mvc;
using MyPhotoDreamApp.Service.Interfaces;
using System.Security.Claims;

namespace MyPhotoDreamApp.Controllers
{
	public class BasketController : Controller
	{
		private readonly IBasketService _basketService;

		public BasketController(IBasketService basketService)
		{
			_basketService = basketService;
		}

		[HttpGet]
		public async Task<IActionResult> Detail()
		{
			var responseOrder = await _basketService.GetItems(User.Identity.Name);
			if (responseOrder.StatusCode == Domain.Enum.StatusCode.OK)
			{
				return View(responseOrder.Data.ToList());
			}
			return RedirectToAction("Index", "Home");
		}


	}
}
