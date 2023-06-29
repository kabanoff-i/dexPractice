using Xunit;
using Services;
using Models;
using Services.Exceprtions;
using Services.Storage;

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
    public class ClientServiceTests
    {
        [Fact]
        public void AddClient_ValidClient_CreatesDefaultAccount()
        {

            var clientService = new ClientService();
            var client = TestDataGenerator.GetClient();
            client.PassportNumber = "AB123456";

            clientService.AddClient(client);

            Assert.Single(clientService.GetAccounts(client));
        }

        [Fact]
        public void AddClient_ClientUnderage_ThrowsClientUnderageException()
        {
            var clientService = new ClientService();
            var underageClient = TestDataGenerator.GetClient();
            underageClient.DateOfBirth = DateTime.Now;
            underageClient.PassportNumber = "CD789012";

            Assert.Throws<ClientUnderageException>(() => clientService.AddClient(underageClient));
        }

        [Fact]
        public void AddClient_MissingPassportData_ThrowsMissingPassportDataException()
        {

            var clientService = new ClientService();
            var clientWithoutPassport = TestDataGenerator.GetClient();
            clientWithoutPassport.DateOfBirth = new DateTime(1991, 7, 12);


            Assert.Throws<MissingPassportDataException>(() => clientService.AddClient(clientWithoutPassport));
        }

        [Fact]
        public void AddAccount_ExistingClient_AddsAccount()
        {

            var clientService = new ClientService();
            var client = TestDataGenerator.GetClient();
            client.DateOfBirth = new DateTime(1991, 7, 12);
            client.PassportNumber = "AB123456";
            var account = new Account("123456789", "$", 1000);


            clientService.AddClient(client);
            clientService.AddAccount(client, account);


            Assert.Equal(2, clientService.GetAccounts(client).Count);
        }

        [Fact]
        public void AddAccount_NonExistingClient_ThrowsClientNotFoundException()
        {

            var clientService = new ClientService();
            var client = TestDataGenerator.GetClient();
            client.DateOfBirth = new DateTime(1991, 7, 12);
            client.PassportNumber = "AB123456";
            var account = new Account("123456789", "$", 1000);


            Assert.Throws<ClientNotFoundException>(() => clientService.AddAccount(client, account));
        }

        [Fact]
        public void EditAccount_ExistingAccount_UpdatesBalance()
        {

            var clientService = new ClientService();
            var client = TestDataGenerator.GetClient();
            client.DateOfBirth = new DateTime(1991, 7, 12);
            client.PassportNumber = "AB123456";
            var account = new Account("123456789", "$", 1000);
            var updatedAccount = new Account("123456789", "$", 2000);


            clientService.AddClient(client);
            clientService.AddAccount(client, account);
            clientService.EditAccount(client, updatedAccount);


            var editedAccount = clientService.GetAccounts(client).FirstOrDefault(a => a.AccountNumber == "123456789");
            Assert.NotNull(editedAccount);
            Assert.Equal(2000, editedAccount.Amount);
        }

        [Fact]
        public void EditAccount_NonExistingClient_ThrowsClientNotFoundException()
        {

            var clientService = new ClientService();
            var client = TestDataGenerator.GetClient();
            client.DateOfBirth = new DateTime(1991, 7, 12);
            client.PassportNumber = "AB123456";
            var account = new Account("123456789", "$", 1000);


            Assert.Throws<ClientNotFoundException>(() => clientService.EditAccount(client, account));
        }

        [Fact]
        public void EditAccount_NonExistingAccount_ThrowsAccountNotFoundException()
        {

            var clientService = new ClientService();
            var client = TestDataGenerator.GetClient();
            client.DateOfBirth = new DateTime(1991, 7, 12);
            client.PassportNumber = "AB123456";
            var account = new Account("123456789", "$", 1000);
            var updatedAccount = new Account("987654321", "$", 2000);

            clientService.AddClient(client);
            clientService.AddAccount(client, account);

            Assert.Throws<AccountNotFoundException>(() => clientService.EditAccount(client, updatedAccount));
        }
    }
    public class ClientServiceFiltersTests
    {
        [Fact]
        public void GetYoungestClient_ReturnsYoungestClient()
        {
            // Arrange
            var clientStorage = new ClientStorage();
            ClientService clientService;

            var client1 = TestDataGenerator.GetClient();
            client1.DateOfBirth = new DateTime(1990, 1, 1);
            client1.PassportNumber = "AB123456";
            var client2 = TestDataGenerator.GetClient();
            client2.DateOfBirth = new DateTime(1985, 5, 10);
            client2.PassportNumber = "AB123456";
            var client3 = TestDataGenerator.GetClient();
            client3.DateOfBirth = new DateTime(1995, 12, 20);
            client3.PassportNumber = "AB123456";


            clientStorage.Add(client1);
            clientStorage.Add(client2);
            clientStorage.Add(client3);
            clientService = new ClientService(clientStorage);

            // Act
            var youngestClient = clientService.GetYoungestClient();

            // Assert
            Assert.Equal(client3, youngestClient);
        }

        [Fact]
        public void GetOldestClient_ReturnsOldestClient()
        {
            // Arrange
            var clientStorage = new ClientStorage();
            ClientService clientService;

            var client1 = TestDataGenerator.GetClient();
            client1.DateOfBirth = new DateTime(1990, 1, 1);
            client1.PassportNumber = "AB123456";
            var client2 = TestDataGenerator.GetClient();
            client2.DateOfBirth = new DateTime(1985, 5, 10);
            client2.PassportNumber = "AB123456";
            var client3 = TestDataGenerator.GetClient();
            client3.DateOfBirth = new DateTime(1995, 12, 20);
            client3.PassportNumber = "AB123456";

            clientStorage.Add(client1);
            clientStorage.Add(client2);
            clientStorage.Add(client3);
            clientService = new ClientService(clientStorage);

            // Act
            var oldestClient = clientService.GetOldestClient();

            // Assert
            Assert.Equal(client2, oldestClient);
        }

        [Fact]
        public void CalculateAverageAge_ReturnsAverageAge()
        {
            // Arrange
            var clientStorage = new ClientStorage();
            ClientService clientService;

            var client1 = TestDataGenerator.GetClient();
            client1.DateOfBirth = new DateTime(1990, 1, 1);
            client1.PassportNumber = "AB123456";
            var client2 = TestDataGenerator.GetClient();
            client2.DateOfBirth = new DateTime(1985, 5, 10);
            client2.PassportNumber = "AB123456";
            var client3 = TestDataGenerator.GetClient();
            client3.DateOfBirth = new DateTime(1995, 12, 20);
            client3.PassportNumber = "AB123456";

            clientStorage.Add(client1);
            clientStorage.Add(client2);
            clientStorage.Add(client3);
            clientService = new ClientService(clientStorage);

            // Act
            var averageAge = clientService.CalculateAverageAge();

            // Assert
            Assert.Equal(33, (int)averageAge);
        }
    }
}