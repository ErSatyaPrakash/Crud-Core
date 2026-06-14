namespace Crud_Core.Models
{
    public class Employee
    {
        public int id { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string Role { get; set; }

        public int CountryId { get; set; }

        public int StateId { get; set; }

        public int CityId { get; set; }
    }


    public class Country
    {
        public int CountryId { get; set; }

        public required string CountryName { get; set; }
    }

    public class State
    {
        public int StateId { get; set; }

        public required string StateName { get; set; }

        public int CountryId { get; set; }
    }

    public class City
    {
        public int CityId { get; set; }

        public required string CityName { get; set; }

        public int StateId { get; set; }
    }


}


