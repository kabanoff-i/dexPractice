using ServicesDb;
using Xunit;
using Xunit.Abstractions;
using ModelsDb;
using Microsoft.Data.SqlClient;

namespace ServicesdbTests.Tests
{
    public class AsyncTests
    {
        private readonly ITestOutputHelper output;
        public AsyncTests(ITestOutputHelper output)
        {
            this.output = output;
        }
        [Fact]
        public async Task Test()
        {
            if (!ThreadPool.SetMaxThreads(Environment.ProcessorCount, Environment.ProcessorCount))
                throw new Exception("Error setting max threads");
            List<Task> tasks = new();
            for (int i = 0; i < 15; i++)
            {
                tasks.Add(ProcessTaskAsync(i+1));
                await Task.Delay(1000);
            }
            await Task.WhenAll(tasks);
        }
        private async Task ProcessTaskAsync(int taskNumber)
        {
            output.WriteLine($"Запуск {taskNumber} задачи");
            ThreadPool.GetAvailableThreads(out int avThreads, out int avIOThreads);
            output.WriteLine($"Свободных потоков {avThreads} {avIOThreads}");
            await Task.Delay(2000);
            output.WriteLine($"Завершение {taskNumber} задачи");
        }
        [Fact]
        public async Task UpdateRate_ShouldUpdateClientRates()
        {
            //Arrange
            RateUpdater rateUpdater = new RateUpdater();
            List<Client> clients = new List<Client> { DataGenerator.GetClient(), DataGenerator.GetClient(), DataGenerator.GetClient() };
            foreach (Client client in clients) 
            {
                client.accounts.Add(new Account() { amount = 1000 });
            }
            
            //Act
            await rateUpdater.UpdateRateAsync( clients );

            //assert
            foreach (Client client in clients)
            {
                foreach (Account account in client.accounts)
                {
                    Assert.NotEqual(1000, account.amount);
                }
            }
        }
        [Fact]
        public async Task DispenseCash_ShouldDispenceAllAccounts()
        {
            //Arrange
            CashDispenserService cashDispenserService = new CashDispenserService();
            List<Client> clients = new List<Client>() 
            {
                DataGenerator.GetClient(),
                DataGenerator.GetClient(),
                DataGenerator.GetClient(),
                DataGenerator.GetClient(),
                DataGenerator.GetClient(),
                DataGenerator.GetClient(),
                DataGenerator.GetClient(),
            };
            foreach (Client client in clients)
            {
                client.accounts.Add(new Account());
                client.accounts.First().amount = 1000;
            }

            //Act
            await cashDispenserService.DispenseCash(4, clients );


            //assert
            foreach (Client client in clients)
            {
                foreach (Account account in client.accounts)
                {
                    Assert.NotEqual(1000, account.amount);
                }
            }
        }
    }
}
