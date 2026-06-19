using Crud_Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Crud_Core.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public HomeController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
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

        public IActionResult Details(int id)
        {
            var data=_context.Employees.FirstOrDefault(x=>x.id == id);
            return View(data);
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

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(Employee employee)
        {
            var data= _context.Employees.FirstOrDefault(s=>s.Email == employee.Email && s.Role == employee.Role);
            if (employee == null)
                return Unauthorized();

            var token = GenerateToken(employee);

            return Ok(new
            {
                Token = token
            });
        }


        private string GenerateToken(Employee user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

            var creds = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler()
                .WriteToken(token);
        }



        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
