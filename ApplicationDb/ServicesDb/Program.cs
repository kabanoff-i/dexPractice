using ModelsDb;

namespace ServicesDb
{
    public class BankService
    {
        private long income = 1340000;
        private int expenses = 43300;
        private int numberOfEmployee = 180;
        List<ModelsDb.Person> BlackList = new List<ModelsDb.Person>();
        public void CalcSalary(Employee employee)
        {
            employee.salary = (int)(income - expenses) / numberOfEmployee;
        }
        public Employee TurnIntoEmployee(Client client)
        {
            ModelsDb.Person a = client;
            return (Employee)a;
        }
    }
    public class Program
    {
        static void Main()
        {
            Console.WriteLine(Guid.NewGuid());
        }
    }
}
