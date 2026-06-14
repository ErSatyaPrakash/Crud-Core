using Crud_Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

[Route("api/[controller]")]
[ApiController]
public class ValuesController : ControllerBase
{
    private readonly ApplicationDbContext context;
    private readonly IConfiguration _configuration;
    public ValuesController(ApplicationDbContext context, IConfiguration configuration)
    {
        this.context = context;
        this._configuration = configuration;
    }

    [HttpGet]
    public IActionResult Get()
    {
        return Ok(context.Employees.ToList());
    }

    [HttpPost]
    public IActionResult Post(Employee employee)
    {
        context.Employees.Add(employee);
        context.SaveChanges();
        return Ok();
    }

    [HttpPut("{id}")]
    public IActionResult Put(int id, Employee employee)
    {
        var data = context.Employees.Find(id);

        if (data == null)
            return NotFound();

        data.Name = employee.Name;
        data.Email = employee.Email;
        data.Role = employee.Role;

        context.SaveChanges();

        return Ok(data);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var data = context.Employees.Find(id);

        if (data == null)
            return NotFound();

        context.Employees.Remove(data);
        context.SaveChanges();

        return Ok("Deleted Successfully");
    }

    public IActionResult GetById(int id)
    {
        var data = context.Employees.Find(id);
        if (data == null)
            return NotFound();
        return Ok(data);
    }

    [HttpPost]
    public IActionResult Login(Employee employee)
    {
        var data = context.Employees.FirstOrDefault(s => s.Email == employee.Email && s.Role == employee.Role);
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
}