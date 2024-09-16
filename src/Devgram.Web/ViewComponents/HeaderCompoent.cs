using Microsoft.AspNetCore.Mvc;

namespace Devgram.Web.ViewComponents;

public class HeaderComponent : ViewComponent
{
    public HeaderComponent()
    {
    }

    public IViewComponentResult Invoke()
    {
        return View();
    }
}