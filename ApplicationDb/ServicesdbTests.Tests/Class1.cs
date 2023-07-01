using Xunit;
using ModelsDb;
using ServicesDb;
using Microsoft.EntityFrameworkCore;
using ServicesDb.Exceptions;


namespace ServicesdbTests.Tests
{
    public class ClientServiceTests
    {
        [Fact]
        public void AddClient_ShouldAddClientAndDefaultAccount()
        {
            // Arrange
            var dbContext = GetTestDbContext();
            var clientService = new ClientService(dbContext);
            var client = DataGenerator.GetClient();

            // Act
            clientService.AddClient(client);

            // Assert
            Assert.Contains(client, dbContext.client);
            Assert.Single(dbContext.account, a => a.clientId == client.id);
        }

        [Fact]
        public void ChangeClient_ExistingClient_ShouldUpdateClientSurname()
        {
            // Arrange
            var dbContext = GetTestDbContext();
            var clientService = new ClientService(dbContext);
            var client = DataGenerator.GetClient();
            dbContext.client.Add(client);
            dbContext.SaveChanges();

            // Act
            clientService.ChangeClient(client.id);

            // Assert
            Assert.Equal(client.surname + " changed", dbContext.client.Find(client.id).surname);
        }

        [Fact]
        public void ChangeClient_NonexistentClient_ShouldThrowClientNotFoundException()
        {
            // Arrange
            var dbContext = GetTestDbContext();
            var clientService = new ClientService(dbContext);
            var clientId = Guid.NewGuid();

            // Act and Assert
            var exception = Assert.Throws<ClientNotFoundException>(() => clientService.ChangeClient(clientId));
            Assert.Equal("Client not found", exception.Message);
        }


        private BankServiceContext GetTestDbContext()
        {
            var options = new DbContextOptionsBuilder<BankServiceContext>().UseInMemoryDatabase(databaseName: "TestDatabase").Options;

            return new BankServiceContext(options);
        }
    }
}