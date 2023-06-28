using Xunit;
using Services;
using Models;

namespace ServiceTests
{
    public class EquivalenceTests
    {
        [Fact]
        void GetHashCodeNecessityPositivTest()
        {
            List<Client> clients = TestDataGenerator.CreateClientList();
            Dictionary<Client, Account> accounts = TestDataGenerator.CreateAccountsDictionary(clients);

            Client testClient = clients[new Random().Next(clients.Count)];
            Account testAccount = accounts[testClient];

            Client newClient = new Client(testClient.Name, testClient.Surname, testClient.DateOfBirth, testClient.ClientID, testClient.PhoneNumber, testClient.Email);


            Assert.Equal(testAccount, accounts[newClient]);
        }
        [Fact]
        void GetHashCodeNecessityPositivTestMoreAccounts()
        {
            List<Client> clients = TestDataGenerator.CreateClientList();
            Dictionary<Client, List<Account>> accounts = TestDataGenerator.CreateListOfAccountsDictionary(clients);

            Client testClient = clients[new Random().Next(clients.Count)];
            List<Account> testAccount = accounts[testClient];

            Client newClient = new Client(testClient.Name, testClient.Surname, testClient.DateOfBirth, testClient.ClientID, testClient.PhoneNumber, testClient.Email);


            Assert.Equal(testAccount, accounts[newClient]);
        }
        [Fact]
        void GetHashCodeNecessityPositivTestEmployees()
        {
            List<Employee> employees = TestDataGenerator.CreateEmployeeList();

            Employee testEmployee = employees[5];

            Employee newEmployee = new Employee(testEmployee.Name, testEmployee.Surname, testEmployee.DateOfBirth, testEmployee.Contract, testEmployee.JobTitle, testEmployee.Salary, testEmployee.DateOfHire);


            Assert.Equal(newEmployee, employees[5]);
        }
    }
}