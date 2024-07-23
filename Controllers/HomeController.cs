using Microsoft.AspNetCore.Mvc;
using RingoMediaApp.Interfaces;

public class HomeController : Controller
{
    public HomeController(IEmailSender emailSender)
    {
        
    }

    public IActionResult Index()
    {
        return View();
    }
}
