using Crud_Core.Models;
using Crud_Core.Repository;
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
        private readonly IEmployee _employeeRepository;
        private readonly IConfiguration _configuration;

        public HomeController(IEmployee employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public IActionResult Index()
        {
            ViewBag.Countries = _employeeRepository.GetCountries();

            return View();
        }

        [HttpPost]
        public IActionResult Create(Employee employee) 
        {

            if (ModelState.IsValid) 
            {
                _employeeRepository.addEmployee(employee);
                return RedirectToAction("index");
            }
            return View();
        }

        public IActionResult List()
        {
            var data = _employeeRepository.getemployeelist();

            ViewBag.Countries = _employeeRepository.GetCountries();
            ViewBag.States = _employeeRepository.GetStates();
            ViewBag.Cities = _employeeRepository.GetCities();

            return View(data);
        }
        [HttpGet]
        public JsonResult GetStatesByCountry(int countryId)
        {
            return Json(_employeeRepository.GetStatesByCountry(countryId));
        }

        [HttpGet]
        public JsonResult GetCitiesByState(int stateId)
        {
            return Json(_employeeRepository.GetCitiesByState(stateId));
        }

        public IActionResult delete(int id)
        {
            _employeeRepository.DeleteEmployee(id);
            return RedirectToAction("index");
        }


        [HttpGet]
        public IActionResult Edit(int id)
        {
            var data=_employeeRepository.getemployeebyid(id);
            return View(data);
        }


        [HttpPost]
        public IActionResult Edit(Employee employee)
        {
            _employeeRepository.UpdateEmployee(employee);
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            _employeeRepository.DeleteEmployee(id);
            return RedirectToAction("List");
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(Employee employee)
        {
            var data = _employeeRepository.Login(employee.Email,employee.Role);

            if (data == null)
            {
                return Unauthorized();
            }

            var token = GenerateToken(data);

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
