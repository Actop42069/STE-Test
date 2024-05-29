using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using STEtest.Data;
using STEtest.Models;
using STEtest.ViewModels;
using System.Linq;
using System.Threading.Tasks;

namespace STEtest.Controllers
{
    [Authorize(Policy = "AdminPolicy")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> AdminDashboard()
        {
            var students = await _context.Students.ToListAsync();
            ViewBag.StudentCount = students.Count;
            return View(students);
        }

        public async Task<IActionResult> EditStudent(int id)
        {
            var student = await _context.Students
                .Include(s => s.UserProfile)
                .FirstOrDefaultAsync(s => s.StudentId == id);
            if (student == null)
            {
                return NotFound();
            }

            var model = new EditStudentViewModel
            {
                StudentId = student.StudentId,
                Name = student.Name,
                CourseId = student.CourseId,
                UserName = student.UserProfile.UserName,
                Email = student.UserProfile.Email
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditStudent(EditStudentViewModel model)
        {
            if (ModelState.IsValid)
            {
                var student = await _context.Students
                    .Include(s => s.UserProfile)
                    .FirstOrDefaultAsync(s => s.StudentId == model.StudentId);

                if (student == null)
                {
                    return NotFound();
                }

                student.Name = model.Name;
                student.CourseId = model.CourseId;
                student.UserProfile.UserName = model.UserName;
                student.UserProfile.Email = model.Email;

                _context.Update(student);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(AdminDashboard));
            }
            return View(model);
        }
    }
}
