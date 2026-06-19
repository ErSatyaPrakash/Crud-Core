using Crud_Core.Models;

namespace Crud_Core.Repository
{
    public class EmployeeRepository : IEmployee
    {
        private readonly ApplicationDbContext _context;

        public EmployeeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Addemployee(Employee employee) 
        {
            _context.Employees.Add(employee);
            _context.SaveChanges();
        
        }

        public List<Employee> getemployeelist()
        {
            return _context.Employees.ToList();
        }


        public Employee getemployeebyid(int id)
        {
            return _context.Employees.FirstOrDefault(x => x.id == id);
        }



        public void UpdateEmployee(Employee employee) 
        {
            _context.Employees.Update(employee);
            _context.SaveChanges();
        
        }

        public void DeleteEmployee(int id)
        {
            var emp=_context.Employees.FirstOrDefault(x=>x.id==id);
            _context.Employees.Remove(emp);
            _context.SaveChanges();
        }

        public List<Country> GetCountries()
        {
            return _context.Countries.ToList();
        }

        public List<State> GetStatesByCountry(int countryId)
        {
            return _context.States
                           .Where(x => x.CountryId == countryId)
                           .ToList();
        }

        public List<City> GetCitiesByState(int stateId)
        {
            return _context.Cities
                           .Where(x => x.StateId == stateId)
                           .ToList();
        }


        public List<State> GetStates()
        {
            return _context.States.ToList();
        }

        public List<City> GetCities()
        {
            return _context.Cities.ToList();
        }


        public Employee Login(string email, string role)
        {
            return _context.Employees.FirstOrDefault(x =>x.Email == email && x.Role == role);
        }


        public void addEmployee(Employee employee)
        {
            throw new NotImplementedException();
        }
    }
}
