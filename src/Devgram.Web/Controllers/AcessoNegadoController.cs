using Microsoft.AspNetCore.Mvc;

namespace Devgram.Web.Controllers;

[Route("acesso-negado")]
public class AcessoNegadoController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}