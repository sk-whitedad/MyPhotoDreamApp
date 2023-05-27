using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyPhotoDreamApp.Service.Interfaces;
using System.Security.Claims;

namespace MyPhotoDreamApp.Controllers
{
	[Authorize]
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
				var _responseOrder = responseOrder.Data.ToList().Where(x => x.CheckConfirm == false).ToList();
				return View(_responseOrder);
			}
			return RedirectToAction("Index", "Home");
		}


	}
}
