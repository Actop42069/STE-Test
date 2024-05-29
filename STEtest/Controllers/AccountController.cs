using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using STEtest.Data;
using STEtest.Models;
using STEtest.ViewModels;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace STEtest.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<UserProfile> _userManager;
        private readonly SignInManager<UserProfile> _signInManager;
        private readonly ApplicationDbContext _context;

        public AccountController(UserManager<UserProfile> userManager, SignInManager<UserProfile> signInManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new UserProfile { UserName = model.UserName, Email = model.Email, UserType = model.UserType };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    if (model.UserType == "Admin")
                    {
                        var admin = new Admin
                        {
                            Name = model.Name,
                            UserName = model.UserName,
                            Position = model.Position,
                            UserProfile = user
                        };
                        _context.Admins.Add(admin);
                    }
                    else if (model.UserType == "Student")
                    {
                        var student = new Student
                        {
                            Name = model.Name,
                            CourseId = model.CourseId,
                            UserProfile = user
                        };
                        _context.Students.Add(student);
                    }

                    await _context.SaveChangesAsync();
                    await _userManager.AddClaimAsync(user, new Claim("UserType", user.UserType));
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Login", "Account");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    var user = await _userManager.FindByNameAsync(model.UserName);
                    var claims = await _userManager.GetClaimsAsync(user);
                    var userTypeClaim = claims.FirstOrDefault(c => c.Type == "UserType");

                    if (userTypeClaim?.Value == "Admin")
                    {
                        return RedirectToAction("AdminDashboard", "Admin");
                    }
                    else if (userTypeClaim?.Value == "Student")
                    {
                        return RedirectToAction("UserDashboard", "User");
                    }
                }
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
