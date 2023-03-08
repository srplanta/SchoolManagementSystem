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

        [HttpGet]
        public IActionResult Add(int? id)   //id => studentId
        {
            ViewBag.StudentId = new SelectList(_context.Students, "StdId", "Name");

            if (id > 0)
            {
                //fetch previous unpaid fee bill if any?
                var outstandingFeeTransaction = _context.FeeTransactions
                    .FirstOrDefault(f => f.StudentId == id && f.FeePaid == null);
                if (outstandingFeeTransaction != null)
                {
                    //If any outstandingFeeTransaction found, make view form filled ready from previous record
                    //Database record will not be affected with this
                    //Following outstandingFeeTransaction.PreviousArrears are outstandingFeeTransaction.NextArrears in actual but ...
                    //will be used as newFeeTransaction.PreviousArrears in view form.
                    //Actual database operation will be handled in [HttpPost] Add(FeeTransaction newFeeTransaction) method.
                    outstandingFeeTransaction.PreviousArrears = outstandingFeeTransaction.FeePayable;
                    outstandingFeeTransaction.AdmissionFee = null;
                    outstandingFeeTransaction.FeePaid = null;
                    outstandingFeeTransaction.FeePayable = null;
                    outstandingFeeTransaction.Fine = null;
                    outstandingFeeTransaction.NextArrears = null;
                    outstandingFeeTransaction.StationaryCharges = null;
                    outstandingFeeTransaction.Student = _context.Students.Find(id) ?? new Student();
                }
                return View(outstandingFeeTransaction);
            }
            else
            {
                //If previous unpaid/unsettelled fee bill not found, generate a new blank form
                return View();
            }
        }

        [HttpPost]
        public IActionResult Add(FeeTransaction newFeeTransaction)
        {
            if (newFeeTransaction.TutionFee > 0)    //New fee bill requested?
            {
                //fetch previous unpaid fee bill if any?
                var outstandingFeeTransaction = _context.FeeTransactions
                    .FirstOrDefault(f => f.StudentId == newFeeTransaction.StudentId && f.FeePaid == null);
                //Make previous fee bill read only
                if (outstandingFeeTransaction != null && outstandingFeeTransaction.FeePaid == null)
                {
                    outstandingFeeTransaction.FeePaid = 0;
                    outstandingFeeTransaction.NextArrears = outstandingFeeTransaction.FeePayable - outstandingFeeTransaction.FeePaid;
                    _context.FeeTransactions.Update(outstandingFeeTransaction);
                    _context.SaveChanges();

                    //newFeeTransaction.PreviousArrears = outstandingFeeTransaction.NextArrears;
                    //newFeeTransaction.FeeId = 0;
                    //TempData["Message"] = $"INFO: Fee Transaction against ID#{outstandingFeeTransaction.FeeId} is made read only.";
                }
                //Generate new fee bill in context of previous fee bill

                newFeeTransaction.PreviousArrears ??= 0m;
                newFeeTransaction.AdmissionFee ??= 0m;
                newFeeTransaction.Fine ??= 0m;
                newFeeTransaction.StationaryCharges ??= 0m;
                newFeeTransaction.FeePayable ??= newFeeTransaction.TutionFee
                    + newFeeTransaction.PreviousArrears
                    + newFeeTransaction.AdmissionFee
                    + newFeeTransaction.Fine
                    + newFeeTransaction.StationaryCharges;
                newFeeTransaction.Student = _context.Students.Find(newFeeTransaction.StudentId) ?? new Student();
                _context.FeeTransactions.Add(newFeeTransaction);
                _context.SaveChanges();

                TempData["Message"] = $"INFO: New fee bill ID#{newFeeTransaction.FeeId}" +
                    $" is generated against student ID#{newFeeTransaction.StudentId}.";

                //return RedirectToAction("Index", "Students");
                return RedirectToAction("Index");
            }
            else
            {
                //Genarate new fee bill
                ViewBag.StudentId = new SelectList(_context.Students, "StdId", "Name");
                return View();
            }
        }

        public IActionResult Edit(int? id)  //id => FeeId
        {
            ViewBag.StudentId = new SelectList(_context.Students, "StdId", "Name");
            if (id == null || _context.FeeTransactions == null)
            {
                return NotFound();
            }
            else
            {
                var feeTransaction = _context.FeeTransactions.Find(id);
                if ((feeTransaction?.FeePaid) != null)
                {
                    TempData["FeePaid"] = true;
                    TempData["Message"] = "INFO: This fee bill is already setteled down and cannot be edited.";
                }
                else
                {
                    TempData["FeePaid"] = false;
                }
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

        // *********************************************************
        //                  UTILITY METHODS
        // *********************************************************
        [HttpPost]
        public FeeTransaction OutstandingFeeTransaction(int id)         //id=>studentId
        {
            //FeeTransaction outstandingFeeTransaction = new();
            var outstandingFeeTransaction = _context.FeeTransactions
                .FirstOrDefault(f => f.StudentId == id && f.FeePaid == null);
            if (outstandingFeeTransaction != null)
            {
                //If previous unpaid fee bill found, make view form filled ready from previous record
                //Database record will not be affected with this
                //Following outstandingFeeTransaction.PreviousArrears are outstandingFeeTransaction.NextArrears in actual but ...
                //will be used as newFeeTransaction.PreviousArrears in view form.
                //Actual database operation will be handled in [HttpPost] Add(FeeTransaction newFeeTransaction) method.
                outstandingFeeTransaction.PreviousArrears = outstandingFeeTransaction.FeePayable;
                outstandingFeeTransaction.AdmissionFee = null;
                outstandingFeeTransaction.FeePaid = null;
                outstandingFeeTransaction.FeePayable = null;
                outstandingFeeTransaction.Fine = null;
                outstandingFeeTransaction.NextArrears = null;
                outstandingFeeTransaction.StationaryCharges = null;
                //outstandingFeeTransaction.Student = _context.Students.Find(id) ?? new Student();
                //TempData["Message"] = "Outstanding fee bill found!";
                return outstandingFeeTransaction;
            }
            else
            {
                outstandingFeeTransaction = new();
                //TempData["Message"] = "No outstanding fee bill found!";
                return outstandingFeeTransaction;
            }
        }
    }
}
