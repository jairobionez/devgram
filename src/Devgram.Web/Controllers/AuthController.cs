
using Devgram.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Devgram.Web.Controllers;

[Route("login")]
public class AuthController : Controller
{
    private readonly ILogger<AuthController> logger;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;

    public AuthController(ILogger<AuthController> logger, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
    {
        this.logger = logger;
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpPost("nova-conta")]
    public async Task<IActionResult> NovaConta()
    {
        return View();
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> LoginAsync(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            var result = await
                _signInManager.PasswordSignInAsync(model.Email, model.Senha, false, false);
            
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            
            ModelState.AddModelError(string.Empty, "Email ou senha incorretos.");
        }
        return View(model);
    }
}