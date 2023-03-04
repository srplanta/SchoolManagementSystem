using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Models;

namespace StudentManagementSystem.Controllers
{
    public class FeeTransactionsController : Controller
    {
        private readonly StudentDbContext _context;

        public FeeTransactionsController(StudentDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var feeTransactions = _context.FeeTransactions
                .Include(f => f.Student)
                .ToList();
            return View(feeTransactions);
        }

        public IActionResult Add(int? id)
        {
            ViewBag.StudentId = new SelectList(_context.Students, "StdId", "Name");
            var previousFeeTransaction = _context.FeeTransactions
                    .FirstOrDefault(f => f.StudentId == id && f.FeePaid == null);

            if (id > 0 && previousFeeTransaction != null)
            {
                previousFeeTransaction.PreviousArrears = previousFeeTransaction.FeePayable;
                previousFeeTransaction.AdmissionFee = null;
                previousFeeTransaction.FeePaid = null;
                previousFeeTransaction.FeePayable = null;
                //previousFeeTransaction.TutionFee = 0m;
                previousFeeTransaction.Fine = null;
                previousFeeTransaction.NextArrears = null;
                previousFeeTransaction.StationaryCharges = null;

                return View(previousFeeTransaction);
            }
            else
            {
                return View(previousFeeTransaction);
            }
        }

        [HttpPost]
        public IActionResult Add(FeeTransaction newFeeTransaction)
        {
            if (newFeeTransaction.TutionFee > 0)
            {
                //Make previous fee bill read only
                var previousFeeTransaction = _context.FeeTransactions
                    .FirstOrDefault(f => f.FeeId == newFeeTransaction.FeeId);
                if (previousFeeTransaction != null && previousFeeTransaction.FeePaid == null)
                {
                    previousFeeTransaction.FeePaid = 0;
                    previousFeeTransaction.NextArrears = previousFeeTransaction.FeePayable - previousFeeTransaction.FeePaid;
                    _context.FeeTransactions.Update(previousFeeTransaction);
                    _context.SaveChanges();
                    TempData["Message"] = $"INFO: Fee Transaction against ID#{previousFeeTransaction.FeeId} is made read only.";
                }
                //Generate new fee bill in context of previous fee bill
                newFeeTransaction.FeeId = 0;
                newFeeTransaction.PreviousArrears ??= 0m;
                newFeeTransaction.AdmissionFee ??= 0m;
                newFeeTransaction.Fine ??= 0m;
                newFeeTransaction.StationaryCharges ??= 0m;
                newFeeTransaction.FeePayable = newFeeTransaction.TutionFee
                    + newFeeTransaction.PreviousArrears
                    + newFeeTransaction.AdmissionFee
                    + newFeeTransaction.Fine
                    + newFeeTransaction.StationaryCharges;
                _context.FeeTransactions.Add(newFeeTransaction);
                _context.SaveChanges();

                TempData["Message"] = $"INFO: New fee bill ID#{newFeeTransaction.FeeId}" +
                    $" is generated against student ID#{newFeeTransaction.StudentId}.";

                return RedirectToAction("Index", "Students");
            }
            else
            {
                //Genarate new fee bill
                ViewBag.StudentId = new SelectList(_context.Students, "StdId", "Name");
                return View();
            }
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || _context.FeeTransactions == null)
            {
                return NotFound();
            }
            else
            {
                var feeTransaction = _context.FeeTransactions.Find(id);
                return View(feeTransaction);
            }
        }

        [HttpPost]
        public IActionResult Edit(FeeTransaction feeTransaction)
        {
            if (feeTransaction == null)
            {
                return View();
            }
            else
            {
                _context.FeeTransactions.Update(feeTransaction);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || _context.FeeTransactions == null)
            {
                return NotFound();
            }
            else
            {
                //var student = _context.FeeTransactions.Find(id);
                var feeTransaction = _context.FeeTransactions
                    .Include(f => f.Student)
                    .FirstOrDefault(f => f.FeeId == id);
                if (feeTransaction != null)
                {
                    //if (feeTransaction.Student)
                    //{
                    //    var feeTransactions = _context.FeeTransactions.Where(f => f.StudentId == id).ToList();
                    //    foreach (var feeTransaction in feeTransactions)
                    //    {
                    //        _context.FeeTransactions.Remove(feeTransaction);
                    //    }
                    //}
                    _context.FeeTransactions.Remove(feeTransaction);
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
            if (id == null || _context.FeeTransactions == null)
            {
                return NotFound();
            }
            else
            {
                //var student = _context.FeeTransactions
                //    .Where(s => s.StdId == id)
                //    .Include(f => f.FeeTransactions);
                var feeTransaction = _context.FeeTransactions
                    .Include(f => f.Student)
                    .FirstOrDefault(f => f.FeeId == id);
                return View(feeTransaction);
            }
        }
    }
}
