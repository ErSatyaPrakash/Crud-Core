using Crud_Core.Models;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class ValuesController : ControllerBase
{
    private readonly ApplicationDbContext context;

    public ValuesController(ApplicationDbContext context)
    {
        this.context = context;
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
    }s

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
}