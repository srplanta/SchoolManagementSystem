using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Models;

namespace StudentManagementSystem.Controllers
{
    public class StudentsController : Controller
    {
        private readonly StudentDbContext _context;

        public StudentsController(StudentDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var students = _context.Students.Include(f => f.FeeTransactions).ToList();
            return View(students);
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(Student student)
        {
            if (student == null)
            {
                return View();
            }
            else
            {
                _context.Students.Add(student);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || _context.Students == null)
            {
                return NotFound();
            }
            else
            {
                var student = _context.Students.Find(id);
                return View(student);
            }
        }

        [HttpPost]
        public IActionResult Edit(Student student)
        {
            if (student == null)
            {
                return View();
            }
            else
            {
                _context.Students.Update(student);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || _context.Students == null)
            {
                return NotFound();
            }
            else
            {
                //var student = _context.Students.Find(id);
                var student = _context.Students
                    .Include(s => s.FeeTransactions)
                    .FirstOrDefault(s => s.StdId == id);
                if (student != null)
                {
                    if (student.FeeTransactions.Count > 0)
                    {
                        var feeTransactions = _context.FeeTransactions
                            .Where(f => f.StudentId == id)
                            .ToList();
                        foreach (var feeTransaction in feeTransactions)
                        {
                            _context.FeeTransactions.Remove(feeTransaction);
                        }
                    }
                    _context.Students.Remove(student);
                    _context.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    return NotFound();
                }
            }
        }

        public IActionResult Details(int? id)
        {
            if (id == null || _context.Students == null)
            {
                return NotFound();
            }
            else
            {
                //var student = _context.Students
                //    .Where(s => s.StdId == id)
                //    .Include(f => f.FeeTransactions);
                var student = _context.Students
                    .Include(s => s.FeeTransactions)
                    .FirstOrDefault(s => s.StdId == id);
                return View(student);
            }
        }
    }
}
