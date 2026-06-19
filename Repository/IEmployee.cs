using Crud_Core.Models;

namespace Crud_Core.Repository
{
    public interface IEmployee
    {
        public List<Employee> getemployeelist();
        public List<Country> GetCountries();
        public List<State> GetStatesByCountry(int countryId);
        public List<City> GetCitiesByState(int stateId);

        List<State> GetStates();
        List<City> GetCities();

        Employee Login(string email, string role);


        Employee getemployeebyid(int id);
        
        void addEmployee(Employee employee);
        void UpdateEmployee(Employee employee);
        void DeleteEmployee(int id);


    }
}
