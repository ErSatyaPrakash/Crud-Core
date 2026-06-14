using Crud_Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Diagnostics;

namespace Crud_Core.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            ViewBag.Countries = _context.Countries.ToList();
            //var data =_context.Employees.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult Create(Employee employee) 
        {

            if (ModelState.IsValid) 
            {
                _context.Employees.Add(employee);
                _context.SaveChanges();
                return RedirectToAction("List");
            }
            return View();
        }

        public IActionResult List()
        {
            var data = _context.Employees.ToList();

            ViewBag.Countries = _context.Countries.ToList();
            ViewBag.States = _context.States.ToList();
            ViewBag.Cities = _context.Cities.ToList();

            return View(data);
        }

        [HttpGet]
        public JsonResult getstate(int id)
        {
            var data=_context.States.Where(x=>x.CountryId == id).ToList();
            return Json(data);
        }

        [HttpGet]
        public JsonResult getcity(int id)
        {
            var data = _context.Cities.Where(x => x.StateId == id).ToList();
            return Json(data);
        }


        [HttpGet]
        public IActionResult Edit(int id)
        {
            var data = _context.Employees.FirstOrDefault(x => x.id == id);

            ViewBag.Countries = _context.Countries.ToList();

            ViewBag.States = _context.States
                .Where(x => x.CountryId == data.CountryId)
                .ToList();

            ViewBag.Cities = _context.Cities
                .Where(x => x.StateId == data.StateId)
                .ToList();

            return View(data);
        }


        [HttpPost]
        public IActionResult Edit(Employee employee)
        {
            if (ModelState.IsValid)
            {
                _context.Employees.Update(employee);
                _context.SaveChanges();
                return RedirectToAction("List");
            }
            return View(employee);
        }

        public IActionResult Delete(int id)
        {
            var data = _context.Employees.Where(x => x.id == id).FirstOrDefault();
            if (data != null)
            {
                _context.Employees.Remove(data);
                _context.SaveChanges();
            }
            return RedirectToAction("List");
        }




        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
