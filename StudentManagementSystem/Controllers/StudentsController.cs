﻿using Microsoft.AspNetCore.Mvc;
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
            return View(_context.Students.ToList());
        }

        public IActionResult AddOrEdit(int? id)
        {
            if (id != null || id > 0)
            {
                //return view to edit already existing record
                return View(_context.Students.Find(id));
            }
            else
            {
                //return view to create new record
                return View();
            }
        }

        [HttpPost]
        public IActionResult AddOrEdit(Student student)
        {
            if (student != null)
            {
                if (student.StdId > 0)
                {
                    try
                    {
                        Student _student = _context.Students.Find(student.StdId);
                        _student.StdId = student.StdId;
                        _student.Standard = student.Standard;
                        _student.Name = student.Name;
                        _student.Age = student.Age;
                        _student.Gender = student.Gender;
                        _context.Students.Update(_student);
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
                        _context.Students.Add(student);
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
            _context.Students.Remove(_context.Students.Find(id));
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Details(int id)
        {
            return View(_context.Students.Find(id));
        }
    }
}