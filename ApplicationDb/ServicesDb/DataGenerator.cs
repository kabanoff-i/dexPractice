using ModelsDb;

namespace ServicesDb
{
    public class DataGenerator
    {
        public static Client GetClient()
        {
            var person = new Bogus.Person();
            return new Client(person.FirstName, person.LastName, person.DateOfBirth, Guid.NewGuid(), person.Phone, person.Email);
        }
        public static Employee GetEmployee()
        {
            var person = new Bogus.Person();
            return new Employee(person.FirstName, person.LastName, person.DateOfBirth, Guid.NewGuid(), person.Company.Name, person.Company.Name, new Random().Next(3000), person.DateOfBirth);
        }
    }
}
