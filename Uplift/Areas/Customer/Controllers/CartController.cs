using Microsoft.AspNetCore.Mvc;

namespace Uplift.Areas.Customer.Controllers
{
    public class CartController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
