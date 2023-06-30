using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using ModelsDb;
using Bogus;
using System.Reflection;
using ServicesDb.Exceptions;
using ServicesDb.Storage;

namespace ServicesDb
{
    public class TestDataGenerator
    {
        public static Client GetClient()
        {
            var person = new Bogus.Person();
            return new Client(person.FirstName, person.LastName, person.dateOfBirth, Guid.NewGuid(), person.Phone, person.Email);
        }
        public static Employee GetEmployee()
        {
            var person = new Bogus.Person();
            return new Employee(person.FirstName, person.LastName, person.dateOfBirth, Guid.NewGuid(), person.Company.name, person.Company.name, new Random().Next(3000), person.dateOfBirth);
        }
        public static List<Client> CreateClientList()
        {
            List<Client> clients = new List<Client>();
            for (int i = 0; i < 1000; i++)
            {
                var person = new Bogus.Person();
                clients.Add(new Client(person.FirstName, person.LastName, person.dateOfBirth, Guid.NewGuid(), person.Phone, person.Email));
            }
            return clients;
        }
        public static Dictionary<string, Client> CreateClientDictionary(List<Client> clients)
        {
            Dictionary<string, Client> clientsDictionary = new Dictionary<string, Client>();
            foreach (Client client in clients)
            {
                clientsDictionary.Add(client.phoneNumber, client);
            }
            return clientsDictionary;
        }
        public static List<Employee> CreateEmployeeList()
        {
            List<Employee> employees = new List<Employee>();
            for (int i = 0; i < 1000; i++)
            {
                var person = new Bogus.Person();

                employees.Add(new Employee(person.FirstName, person.LastName, person.dateOfBirth, Guid.NewGuid(), person.Company.name, person.Company.name, new Random().Next(3000), person.dateOfBirth));
            }
            return employees;
        }
        public static Dictionary<Client, Account> CreateAccountsDictionary(List<Client> clients)
        {
            Dictionary<Client, Account> accountsDictionary = new Dictionary<Client, Account>();
            foreach (Client client in clients)
            {
                Faker faker = new Faker();
                accountsDictionary.Add(client, new Account(faker.Finance.Account(), faker.Finance.Currency().Symbol, (int)faker.Finance.amount()));
            }
            return accountsDictionary;
        }
        public static Dictionary<Client, List<Account>> CreateListOfAccountsDictionary(List<Client> clients)
        {
            Dictionary<Client, List<Account>> accountsDictionary = new Dictionary<Client, List<Account>>();
            foreach (Client client in clients)
            {
                Faker faker = new Faker();
                accountsDictionary.Add(client, new List<Account>() { new Account(faker.Finance.Account(), faker.Finance.Currency().Symbol, (int)faker.Finance.amount()), new Account(faker.Finance.Account(), faker.Finance.Currency().Symbol, (int)faker.Finance.amount()) });
            }
            return accountsDictionary;
        }
    }

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
    public class EmployeeService
    {
        private Dictionary<Employee, List<Account>> employeesAccounts = new Dictionary<Employee, List<Account>>();
        private EmployeeStorage employeeStorage;

        public List<Account> GetAccounts(Employee employee)
        {
            if (employeesAccounts.ContainsKey(employee))
            {
                return employeesAccounts[employee];
            }

            return new List<Account>();
        }

        public void AddEmployee(Employee employee)
        {
            if (employee.dateOfBirth > DateTime.Now.AddYears(-18))
            {
                throw new EmployeeUnderageException("Employee must be at least 18 years old");
            }

            if (string.IsNullOrEmpty(employee.contract))
            {
                throw new MissingContractDataException("Employee must provide contract data");
            }

            employeesAccounts.Add(employee, new List<Account> { CreateDefaultAccount() });
        }

        public void AddAccount(Employee employee, Account account)
        {
            if (!employeesAccounts.ContainsKey(employee))
            {
                throw new EmployeeNotFoundException("Employee not found");
            }

            employeesAccounts[employee].Add(account);
        }

        public void EditAccount(Employee employee, Account account)
        {
            if (!employeesAccounts.ContainsKey(employee))
            {
                throw new EmployeeNotFoundException("Employee not found");
            }

            var employeeAccounts = employeesAccounts[employee];
            var existingAccount = employeeAccounts.FirstOrDefault(a => a.accountNumber == account.accountNumber);

            if (existingAccount == null)
            {
                throw new AccountNotFoundException("Account not found.");
            }

            existingAccount.amount = account.amount;
        }

        private Account CreateDefaultAccount()
        {
            string accountNumber = Guid.NewGuid().ToString();
            return new Account(accountNumber, "$", 0);
        }

        public List<Employee> GetFilteredEmployees(string nameFilter, string jobTitleFilter, DateTime? minDateOfBirth, DateTime? maxDateOfBirth)
        {
            var filteredEmployees = employeeStorage.employees;

            if (!string.IsNullOrEmpty(nameFilter))
            {
                filteredEmployees = filteredEmployees.Where(e => e.name.Contains(nameFilter)).ToList();
            }

            if (!string.IsNullOrEmpty(jobTitleFilter))
            {
                filteredEmployees = filteredEmployees.Where(e => e.jobTitle == jobTitleFilter).ToList();
            }

            if (minDateOfBirth.HasValue)
            {
                filteredEmployees = filteredEmployees.Where(e => e.dateOfBirth >= minDateOfBirth.Value).ToList();
            }

            if (maxDateOfBirth.HasValue)
            {
                filteredEmployees = filteredEmployees.Where(e => e.dateOfBirth <= maxDateOfBirth.Value).ToList();
            }

            return filteredEmployees;
        }

        public Employee GetYoungestEmployee()
        {
            var youngestEmployee = employeeStorage.employees.OrderByDescending(e => e.dateOfBirth).FirstOrDefault();
            return youngestEmployee;
        }

        public Employee GetOldestEmployee()
        {
            var oldestEmployee = employeeStorage.employees.OrderBy(e => e.dateOfBirth).FirstOrDefault();
            return oldestEmployee;
        }

        public double CalculateAverageAge()
        {
            var totalAge = employeeStorage.employees.Sum(e => (DateTime.Now - e.dateOfBirth).TotalDays / 365);
            var averageAge = totalAge / employeeStorage.employees.Count;
            return averageAge;
        }

        public EmployeeService(EmployeeStorage employeeStorage)
        {
            this.employeeStorage = employeeStorage;
        }

        public EmployeeService() { }
    }
    public class ClientService
    {
        Dictionary<Client, List<Account>> clientsAccounts = new Dictionary<Client, List<Account>>();
        ClientStorage clientStorage;

        public List<Account> GetAccounts(Client client)
        {
            if (clientsAccounts.ContainsKey(client))
            {
                return clientsAccounts[client];
            }

            return new List<Account>();
        }
        public void AddClient(Client client)
        {
            if (client.dateOfBirth > DateTime.Now.AddYears(-18))
            {
                throw new ClientUnderageException("Client must be at least 18 years old");
            }

            if (string.IsNullOrEmpty(client.passportNumber))
            {
                throw new MissingPassportDataException("Client must provide passport data");
            }

            clientsAccounts.Add(client, new List<Account> { CreateDefaultAccount() });
        }

        public void AddAccount(Client client, Account account)
        {
            if (!clientsAccounts.ContainsKey(client))
            {
                throw new ClientNotFoundException("Client not found");
            }


            clientsAccounts[client].Add(account);
        }

        public void EditAccount(Client client, Account account)
        {
            if (!clientsAccounts.ContainsKey(client))
            {
                throw new ClientNotFoundException("Client not found");
            }
            var clientAccounts = clientsAccounts[client];
            var existingAccount = clientAccounts.FirstOrDefault(a => a.accountNumber == account.accountNumber);

            if (existingAccount == null)
            {
                throw new AccountNotFoundException("Account not found.");
            }


            existingAccount.amount = account.amount;
        }

        private Account CreateDefaultAccount()
        {
            string accountNumber = Guid.NewGuid().ToString();
            return new Account(accountNumber, "$", 0);
        }

        //IEnumerable
        public List<Client> GetFilteredClients(string nameFilter, string phoneFilter, string passportFilter, DateTime? minDateOfBirth, DateTime? maxDateOfBirth)
        {
            var filteredClients = clientStorage.clients;

            if (!string.IsNullOrEmpty(nameFilter))
            {
                filteredClients = filteredClients.Where(c => c.name.Contains(nameFilter)).ToList();
            }

            if (!string.IsNullOrEmpty(phoneFilter))
            {
                filteredClients = filteredClients.Where(c => c.phoneNumber == phoneFilter).ToList();
            }

            if (!string.IsNullOrEmpty(passportFilter))
            {
                filteredClients = filteredClients.Where(c => c.passportNumber == passportFilter).ToList();
            }

            if (minDateOfBirth.HasValue)
            {
                filteredClients = filteredClients.Where(c => c.dateOfBirth >= minDateOfBirth.Value).ToList();
            }

            if (maxDateOfBirth.HasValue)
            {
                filteredClients = filteredClients.Where(c => c.dateOfBirth <= maxDateOfBirth.Value).ToList();
            }

            return filteredClients;
        }
        public Client GetYoungestClient()
        {
            var youngestClient = clientStorage.clients.OrderByDescending(c => c.dateOfBirth).FirstOrDefault();
            return youngestClient;
        }

        public Client GetOldestClient()
        {
            var oldestClient = clientStorage.clients.OrderBy(c => c.dateOfBirth).FirstOrDefault();
            return oldestClient;
        }

        public double CalculateAverageAge()
        {
            var totalAge = clientStorage.clients.Sum(c => (DateTime.Now - c.dateOfBirth).TotalDays / 365);
            var averageAge = totalAge / clientStorage.clients.Count;
            return averageAge;
        }
        public ClientService(ClientStorage clientStorage)
        {
            this.clientStorage = clientStorage;
        }
        public ClientService() { }
    }
    public class Program
    {
        static void Main()
        {
            //Stopwatch sw = new Stopwatch();
            //TestDataGenerator test = new TestDataGenerator();
            //List<Client> clients = TestDataGenerator.CreateClientList();
            //Dictionary<string, Client> clientsDictionary = TestDataGenerator.CreateClientDictionary(clients);
            //List<Employee> employees = TestDataGenerator.CreateEmployeeList();
            //Dictionary<Client, Account> accounts = TestDataGenerator.CreateAccountsDictionary(clients);

            ////a
            //int randIndex = new Random().Next(clients.Count);
            //string randNumber = clients[randIndex].phoneNumber;

            //sw.Start();
            //Client client = clients.Find((Client) => Client.phoneNumber == randNumber);
            //sw.Stop();
            //Console.WriteLine("a) " + sw.Elapsed.TotalMilliseconds.ToString());

            //sw.Reset();
            ////b
            //sw.Start();
            //client = clientsDictionary[randNumber];
            //sw.Stop();
            //Console.WriteLine("b) " + sw.Elapsed.TotalMilliseconds.ToString());

            //sw.Reset();
            ////C
            //List<Client> clientsAge = clients.FindAll((Client) => DateTime.Now.Subtract(Client.dateOfBirth).Days / 365 < 30);//возраст меньше 30
            //foreach (var item in clientsAge)
            //    Console.WriteLine($"c) name: {item.name}, date of birth: {item.dateOfBirth}");

            ////D
            //employees.Sort((a, b) => a.salary.CompareTo(b.salary));
            //Console.WriteLine($"D) name: {employees[0].name} salary: {employees[0].salary}");

            var person = new Bogus.Person();
            Console.WriteLine($"\"FirstName\": \"{person.FirstName}\",\r\n  \"LastName\": \"{person.LastName}\",\r\n  \"UserName\": \"{person.UserName}\",\r\n  \"Avatar\": \"https://s3.amazonaws.com/uifaces/faces/twitter/ccinojasso1/128.jpg\",\r\n  \"Email\": \"{person.Email}\",\r\n  \"dateOfBirth\": \"{person.dateOfBirth.ToString("yyyy-MM-dd")}\",\r\n  \"City\": \"{person.Address.City}\",\r\n    \"ZipCode\": \"78425-0411\",\r\n    \"Geo\": {{\r\n      \"Lat\": -35.8154,\r\n      \"Lng\": -140.2044\r\n    }}\r\n  }},\r\n  \"Phone\": \"{person.Phone}\",\r\n  \"Website\": \"javier.biz\",\r\n  \"Company\": {{\r\n    \"name\": \"{person.Company.name}\",\r\n    \"CatchPhrase\": \"Organic even-keeled monitoring\",\r\n    \"Bs\": \"open-source brand e-business\"\r\n  }}");

            string accountNumber = Guid.NewGuid().ToString();
            Console.WriteLine(accountNumber);
            var empl = TestDataGenerator.GetEmployee();
            Console.WriteLine($"{empl.id}\r\n{empl.name}\r\n{empl.surname}\r\n{empl.dateOfBirth.ToString("yyyy-MM-dd")}\r\n{empl.contract}\r\n{empl.salary}\r\n{empl.DateOfHire.ToString("yyyy-MM-dd")}\r\n{empl.jobTitle}\r\n{empl.passportNumber}\r\n");
            


        }
    }
}
