using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Models;
using Bogus;
using System.Reflection;
using Services.Exceprtions;
using Services.Storage;
using System.

namespace Services
{
    public class TestDataGenerator
    {
        public static Client GetClient()
        {
            var person = new Bogus.Person();
            return new Client(person.FirstName, person.LastName, person.DateOfBirth, new Random().Next(1000).ToString(), person.Phone, person.Email);
        }
        public static List<Client> CreateClientList()
        {
            List<Client> clients = new List<Client>();
            for (int i = 0; i < 1000; i++)
            {
                var person = new Bogus.Person();
                clients.Add(new Client(person.FirstName, person.LastName, person.DateOfBirth, new Random().Next(1000).ToString(), person.Phone, person.Email));
            }
            return clients;
        }
        public static Dictionary<string, Client> CreateClientDictionary(List<Client> clients)
        {
            Dictionary<string, Client> clientsDictionary = new Dictionary<string, Client>();
            foreach (Client client in clients)
            {
                clientsDictionary.Add(client.PhoneNumber, client);
            }
            return clientsDictionary;
        }
        public static List<Employee> CreateEmployeeList()
        {
            List<Employee> employees = new List<Employee>();
            for (int i = 0; i < 1000; i++)
            {
                var person = new Bogus.Person();

                employees.Add(new Employee(person.FirstName, person.LastName, person.DateOfBirth, person.Company.Name, person.Company.Name, new Random().Next(3000), person.DateOfBirth));
            }
            return employees;
        }
        public static Dictionary<Client, Account> CreateAccountsDictionary(List<Client> clients)
        {
            Dictionary < Client, Account> accountsDictionary = new Dictionary<Client, Account>();
            foreach (Client client in clients)
            {
                Faker faker = new Faker();
                accountsDictionary.Add(client, new Account(faker.Finance.Account(), faker.Finance.Currency().Symbol, (int)faker.Finance.Amount()));
            }
            return accountsDictionary;
        }
        public static Dictionary<Client, List<Account>> CreateListOfAccountsDictionary(List<Client> clients)
        {
            Dictionary<Client, List<Account>> accountsDictionary = new Dictionary<Client, List<Account>>();
            foreach (Client client in clients)
            {
                Faker faker = new Faker();
                accountsDictionary.Add(client, new List<Account>() { new Account(faker.Finance.Account(), faker.Finance.Currency().Symbol, (int)faker.Finance.Amount()), new Account(faker.Finance.Account(), faker.Finance.Currency().Symbol, (int)faker.Finance.Amount()) });
            }
            return accountsDictionary;
        }
    }

    public class BankService
    {
        private long income = 1340000;
        private int expenses = 43300;
        private int numberOfEmployee = 180;
        List<Models.Person> BlackList = new List<Models.Person>();
        public void CalcSalary(Employee employee)
        {
            employee.Salary = (int)(income - expenses) / numberOfEmployee;
        }
        public Employee TurnIntoEmployee(Client client)
        {
            Models.Person a = client;
            return (Employee)a;
        }
        public void AddBonus<T>(T person) where T: Models.Person
        {
            person.Bonus = income * 0.05;
        }
        public void AddToBlackList<T> (T person) where T: Models.Person
        {
            BlackList.Add(person);
        }
        public bool IsPersonInBlackList<T>(T person) where T: Models.Person
        {
            return BlackList.Contains(person);
        }
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
            if (client.DateOfBirth > DateTime.Now.AddYears(-18))
            {
                throw new ClientUnderageException("Client must be at least 18 years old");
            }

            if (string.IsNullOrEmpty(client.PassportNumber))
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
            var existingAccount = clientAccounts.FirstOrDefault(a => a.AccountNumber == account.AccountNumber);

            if (existingAccount == null)
            {
                throw new AccountNotFoundException("Account not found.");
            }


            existingAccount.Amount = account.Amount;
        }

        private Account CreateDefaultAccount()
        {
            string accountNumber = Guid.NewGuid().ToString();
            return new Account(accountNumber,"$", 0);
        }

        //IEnumerable
        public List<Client> GetFilteredClients(string nameFilter, string phoneFilter, string passportFilter, DateTime? minDateOfBirth, DateTime? maxDateOfBirth)
        {
            var filteredClients = clientStorage.clients;

            if (!string.IsNullOrEmpty(nameFilter))
            {
                filteredClients = filteredClients.Where(c => c.Name.Contains(nameFilter)).ToList();
            }

            if (!string.IsNullOrEmpty(phoneFilter))
            {
                filteredClients = filteredClients.Where(c => c.PhoneNumber == phoneFilter).ToList();
            }

            if (!string.IsNullOrEmpty(passportFilter))
            {
                filteredClients = filteredClients.Where(c => c.PassportNumber == passportFilter).ToList();
            }

            if (minDateOfBirth.HasValue)
            {
                filteredClients = filteredClients.Where(c => c.DateOfBirth >= minDateOfBirth.Value).ToList();
            }

            if (maxDateOfBirth.HasValue)
            {
                filteredClients = filteredClients.Where(c => c.DateOfBirth <= maxDateOfBirth.Value).ToList();
            }

            return filteredClients;
        }
        public Client GetYoungestClient()
        {
            var youngestClient = clientStorage.clients.OrderByDescending(c => c.DateOfBirth).FirstOrDefault();
            return youngestClient;
        }

        public Client GetOldestClient()
        {
            var oldestClient = clientStorage.clients.OrderBy(c => c.DateOfBirth).FirstOrDefault();
            return oldestClient;
        }

        public double CalculateAverageAge()
        {
            var totalAge = clientStorage.clients.Sum(c => (DateTime.Now - c.DateOfBirth).TotalDays / 365);
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
            Stopwatch sw = new Stopwatch();
            TestDataGenerator test = new TestDataGenerator();
            List<Client> clients = TestDataGenerator.CreateClientList();
            Dictionary<string, Client> clientsDictionary = TestDataGenerator.CreateClientDictionary(clients);
            List<Employee> employees = TestDataGenerator.CreateEmployeeList();
            Dictionary<Client, Account> accounts = TestDataGenerator.CreateAccountsDictionary(clients);

            //a
            int randIndex = new Random().Next(clients.Count);
            string randNumber = clients[randIndex].PhoneNumber;

            sw.Start();
            Client client = clients.Find((Client) => Client.PhoneNumber == randNumber);
            sw.Stop();
            Console.WriteLine("a) " + sw.Elapsed.TotalMilliseconds.ToString());

            sw.Reset();
            //b
            sw.Start();
            client = clientsDictionary[randNumber];
            sw.Stop();
            Console.WriteLine("b) " + sw.Elapsed.TotalMilliseconds.ToString());

            sw.Reset();
            //C
            List<Client> clientsAge = clients.FindAll((Client) => DateTime.Now.Subtract(Client.DateOfBirth).Days / 365 < 30);//возраст меньше 30
            foreach (var item in clientsAge)
                Console.WriteLine($"c) name: {item.Name}, date of birth: {item.DateOfBirth}");

            //D
            employees.Sort((a, b) => a.Salary.CompareTo(b.Salary));
            Console.WriteLine($"D) name: {employees[0].Name} salary: {employees[0].Salary}");

        }
    }
}
