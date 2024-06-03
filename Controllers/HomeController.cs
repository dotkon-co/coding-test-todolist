using DotkonBlog.Areas.Identity.Data;
using DotkonBlog.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace DotkonBlog.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<ApplicationUser> _userManagaer;

        public HomeController(ILogger<HomeController> logger, UserManager<ApplicationUser> userManagaer)
        {
            _logger = logger;
            this._userManagaer = userManagaer;
        }

        public IActionResult Index()
        {
            ViewData["UserId"] = _userManagaer.GetUserId(this.User);
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
