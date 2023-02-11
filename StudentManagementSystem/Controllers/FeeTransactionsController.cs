using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
            return View(_context.FeeTransactions.ToList());
        }

        public IActionResult AddOrEdit(int? id)
        {
            var students = _context.Students.ToList();
            ViewBag.StudentId = new SelectList(students, "StdId", "Name");
            if (id != null || id > 0)
            {
                return View(_context.FeeTransactions.Find(id));
            }
            else
            {
                FeeTransaction _feeTransaction = new FeeTransaction();
                return View(_feeTransaction);
            }
        }

        [HttpPost]
        public IActionResult AddOrEdit(FeeTransaction feeTransaction)
        {
            if (feeTransaction != null)
            {
                if (feeTransaction.FeeId > 0)
                {
                    try
                    {
                        FeeTransaction? _feeTransaction = _context.FeeTransactions.Find(feeTransaction.FeeId);
                        //_feeTransaction.FeeId = feeTransaction.FeeId;
                        //_feeTransaction.StudentId = feeTransaction.StudentId;
                        _feeTransaction.TutionFee = feeTransaction.TutionFee;
                        _feeTransaction.AdmissionFee = feeTransaction.AdmissionFee;
                        _feeTransaction.StationaryCharges = feeTransaction.StationaryCharges;
                        _feeTransaction.Fine = feeTransaction.Fine;
                        _feeTransaction.PreviousArrears = feeTransaction.PreviousArrears;
                        _feeTransaction.NextArrears = feeTransaction.NextArrears;
                        _feeTransaction.FeePayable = (
                            _feeTransaction.TutionFee +
                            _feeTransaction.AdmissionFee +
                            _feeTransaction.StationaryCharges +
                            _feeTransaction.Fine +
                            _feeTransaction.PreviousArrears
                        );
                        _feeTransaction.FeePaid = feeTransaction.FeePaid;

                        _context.FeeTransactions.Update(_feeTransaction);
                        _context.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    catch (Exception e)
                    {
                        return NotFound(e.Message);
                    }
                }
                else
                {
                    try
                    {
                        feeTransaction.FeePayable = (
                            feeTransaction.TutionFee +
                            feeTransaction.AdmissionFee +
                            feeTransaction.StationaryCharges +
                            feeTransaction.Fine +
                            feeTransaction.PreviousArrears
                        );
                        
                        _context.FeeTransactions.Add(feeTransaction);
                        _context.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    catch (Exception e)
                    {
                        return NotFound(e.Message);
                    }
                }
            }
            return View();
        }

        public IActionResult Delete(int id)
        {
            _context.FeeTransactions.Remove(_context.FeeTransactions.Find(id));
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Details(int id)
        {
            FeeTransaction? feeTransaction= _context.FeeTransactions.Find(id);
            feeTransaction.Student = _context.Students.Find(feeTransaction.StudentId);
            return View(feeTransaction);
        }
    }
}
