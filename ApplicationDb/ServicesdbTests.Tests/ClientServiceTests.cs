using Xunit;
using ModelsDb;
using ServicesDb;
using ServicesDb.Exceptions;


namespace ServicesdbTests.Tests
{
    public class ClientServiceTests
    {
        [Fact]
        public async Task AddClient_ShouldAddClientAndDefaultAccount()
        {
            // Arrange
            using (BankServiceContext dbContext = new())
            {
                var clientService = new ClientService(dbContext);
                var client = DataGenerator.GetClient();

                // Act
                await clientService.AddClientAsync(client);

                // Assert
                Assert.Contains(client, dbContext.client);
                Assert.Single(dbContext.account, a => a.clientId == client.id);
            }
        }

        [Fact]
        public async Task ChangeClient_ExistingClient_ShouldUpdateClientSurname()
        {
            // Arrange
            using (BankServiceContext dbContext = new())
            {
                var clientService = new ClientService(dbContext);
                var client = DataGenerator.GetClient();
                await dbContext.client.AddAsync(client);
                await dbContext.SaveChangesAsync();

                // Act
                await clientService.ChangeClientAsync(client.id);

                // Assert
                Assert.Equal(client.surname, dbContext.client.FindAsync(client.id).Result.surname);
            }
        }

        [Fact]
        public async Task ChangeClient_NonexistentClient_ShouldThrowClientNotFoundException()
        {
            // Arrange
            using (BankServiceContext dbContext = new())
            {
                var clientService = new ClientService(dbContext);
                var clientId = Guid.NewGuid();

                // Act and Assert
                var exception = await Assert.ThrowsAsync<ClientNotFoundException>(() => clientService.ChangeClientAsync(clientId));
                Assert.Equal("Client not found", exception.Message);
            }
        }
        [Fact]
        public async Task GetClient_ShouldGetClientById()
        {
            // Arrange
            using (BankServiceContext dbContext = new())
            {
                var clientService = new ClientService(dbContext);
                var client = DataGenerator.GetClient();
                await clientService.AddClientAsync(client);

                // Act
                Client newClient = await clientService.GetClientAsync(client.id);

                // Assert
                Assert.Equal(client, newClient);
            }
        }
        [Fact]
        public async Task DeleteClient_ShouldDeleteClientById()
        {
            // Arrange
            using (BankServiceContext dbContext = new())
            {
                var clientService = new ClientService(dbContext);
                var client = DataGenerator.GetClient();
                await clientService.AddClientAsync(client);

                // Act
                await clientService.DeleteClientAsync(client.id);

                // Assert
                Assert.Null(clientService.GetClientAsync(client.id).Result);
            }
        }
        [Fact]
        public async Task DeleteAccount_ShouldDeleteAccountById()
        {
            // Arrange
            using (BankServiceContext dbContext = new())
            {
                var clientService = new ClientService(dbContext);
                var client = DataGenerator.GetClient();
                await clientService.AddClientAsync(client);
                Account account = client.accounts.FirstOrDefault();

                // Act
                await clientService.DeleteAccountAsync(account.accountNumber);

                // Assert
                Assert.Null(dbContext.account.FirstOrDefault(a => a.accountNumber == account.accountNumber));
            }
        }
    }
}