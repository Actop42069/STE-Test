using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace STEtest.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        public IActionResult UserDashboard()
        {
            return View();
        }
    }
}
