using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Devgram.Web.Models;
using Microsoft.AspNetCore.Authorization;

namespace Devgram.Web.Controllers;

[Route("home"), Authorize]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        var a =  this.User.Identity.IsAuthenticated;
        return View();
    }
}
