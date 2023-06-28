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


namespace Services
{
    class TestDataGenerator
    {
        public List<Client> clients = new List<Client>();
        public Dictionary<string, Client> clientsDictionary = new Dictionary<string, Client>();
        public List<Employee> employees = new List<Employee>();

        void CreateClientList()
        {
            for (int i = 0; i < 1000; i++)
            {
                var person = new Bogus.Person();
                clients.Add(new Client(person.FirstName, person.LastName, person.DateOfBirth, new Random().Next(1000).ToString(), person.Phone, person.Email));
            }
        }
        void CreateClientDictionary()
        {
            foreach (Client client in clients)
            {
                clientsDictionary.Add(client.PhoneNumber, client);
            }
        }
        void CreateEmployeeList()
        {
            for (int i = 0; i < 1000; i++)
            {
                var person = new Bogus.Person();

                employees.Add(new Employee(person.FirstName, person.LastName, person.DateOfBirth, person.Company.Name, person.Company.Name, new Random().Next(3000), person.DateOfBirth));
            }

        }

        public TestDataGenerator()
        {
            CreateClientList();
            CreateClientDictionary();
            CreateEmployeeList();
        }
    }

    public class BankService
    {
        private long income = 1340000;
        private int expenses = 43300;
        private int numberOfEmployee = 180;
        public void CalcSalary(Employee employee)
        {
            employee.Salary = (int)(income - expenses) / numberOfEmployee;
        }
        public Employee TurnIntoEmployee(Client client)
        {
            Models.Person a = client;
            return (Employee)a;
        }
    }
    public class Program
    {
        static void Main()
        {
            Stopwatch sw = new Stopwatch();
            TestDataGenerator test = new TestDataGenerator();

            //a
            int randIndex = new Random().Next(test.clients.Count);
            string randNumber = test.clients[randIndex].PhoneNumber;

            sw.Start();
            Client client = test.clients.Find((Client) => Client.PhoneNumber == randNumber);
            sw.Stop();
            Console.WriteLine("a) " + sw.Elapsed.TotalMilliseconds.ToString());

            sw.Reset();
            //b
            sw.Start();
            client = test.clientsDictionary[randNumber];
            sw.Stop();
            Console.WriteLine("b) " + sw.Elapsed.TotalMilliseconds.ToString());

            sw.Reset();
            //C
            List<Client> clientsAge = test.clients.FindAll((Client) => DateTime.Now.Subtract(Client.DateOfBirth).Days / 365 < 30);//возраст меньше 30
            foreach (var item in clientsAge)
                Console.WriteLine($"c) name: {item.Name}, date of birth: {item.DateOfBirth}");

            //D
            test.employees.Sort((a, b) => a.Salary.CompareTo(b.Salary));
            Console.WriteLine($"D) name: {test.employees[0].Name} salary: {test.employees[0].Salary}");

        }
    }
}
