using ExportTool;
using Newtonsoft.Json;
using ServicesDb;
using Xunit;
using ModelsDb;

namespace ServicesdbTests.Tests
{
    public class SerializedImportExportTests
    {
        [Fact]
        public async Task ExportDBEmployeeToJson_WritesJsonToFile()
        {
            var employees = new List<Employee>
        {
            DataGenerator.GetEmployee(), 
            DataGenerator.GetEmployee()
        };

            var exportService = new ExportService(Environment.CurrentDirectory, "serializedToJson.json");   

            await exportService.ExportDBToJson(employees);

            var fullPath = Path.Combine(Environment.CurrentDirectory, "serializedToJson.json");

            Assert.True(File.Exists(fullPath)); 

            string jsonFromFile = await File.ReadAllTextAsync(fullPath);
            var deserializedEmployees = JsonConvert.DeserializeObject<List<Employee>>(jsonFromFile);

            Assert.NotNull(deserializedEmployees);
            Assert.Equal(employees.Count, deserializedEmployees.Count);
            Assert.Equal(employees.First(), deserializedEmployees.First());
        }

        [Fact]
        public async Task ImportJsonToDBEmployee_ReadsJsonFromFile()
        {
            var employees = new List<Employee>
        {
            DataGenerator.GetEmployee(), 
            DataGenerator.GetEmployee()
        };

            var exportService = new ExportService(Environment.CurrentDirectory, "testSerializedToJson.json"); 

            string json = JsonConvert.SerializeObject(employees);
            await File.WriteAllTextAsync(Path.Combine(Environment.CurrentDirectory, "testSerializedToJson.json"), json);

            var importedEmployees = await exportService.ImportJsonToDB<Employee>();

            Assert.NotNull(importedEmployees);
            Assert.Equal(employees.Count, importedEmployees.Count);
            Assert.Equal(employees.First(), importedEmployees.First());
        }
        [Fact]
        public async Task ImportJsonToDBClient_ReadsJsonFromFile()
        {
            var clients = new List<Client>
        {
            DataGenerator.GetClient(),
            DataGenerator.GetClient()
        };

            var exportService = new ExportService(Environment.CurrentDirectory, "testSerializedToJson.json");

            string json = JsonConvert.SerializeObject(clients);
            await File.WriteAllTextAsync(Path.Combine(Environment.CurrentDirectory, "testSerializedToJson.json"), json);

            var importedClients = await exportService.ImportJsonToDB<Client>();

            Assert.NotNull(importedClients);
            Assert.Equal(clients.Count, importedClients.Count);
            Assert.Equal(clients.First(), importedClients.First());
        }
        [Fact]
        public async Task ExportDBClientToJson_WritesJsonToFile()
        {
            var clients = new List<Client>
        {
            DataGenerator.GetClient(), 
            DataGenerator.GetClient()
        };
            var fullPath = Path.Combine(Environment.CurrentDirectory, "serializedToJson.json");

            var exportService = new ExportService(Environment.CurrentDirectory, "serializedToJson.json"); 

            await exportService.ExportDBToJson(clients);

            Assert.True(File.Exists(fullPath)); 

            string jsonFromFile = await File.ReadAllTextAsync(fullPath);
            List<Client> deserializedClients = JsonConvert.DeserializeObject<List<Client>>(jsonFromFile);

            Assert.NotNull(deserializedClients);
            Assert.Equal(clients.Count, deserializedClients.Count);
            Assert.Equal(clients.First(), deserializedClients.First());
        }
        [Fact]
        public async Task ExportDBClientWithAccountToJson_WritesJsonToFile()
        {
            List<Client> clients = new List<Client>
        {
            DataGenerator.GetClient(),
            DataGenerator.GetClient()
        };
            foreach (Client client in clients)
            {
                client.accounts = new List<Account>
                {
                    DataGenerator.GetAccount(client, new Currency("USD", '$'))
                };
            }
            var fullPath = Path.Combine(Environment.CurrentDirectory, "serializedToJson.json");

            var exportService = new ExportService(Environment.CurrentDirectory, "serializedToJson.json");

            await exportService.ExportDBToJson(clients);

            Assert.True(File.Exists(fullPath)); 

            string jsonFromFile = await File.ReadAllTextAsync(fullPath);
            List<Client> deserializedClients = JsonConvert.DeserializeObject<List<Client>>(jsonFromFile);

            Assert.NotNull(deserializedClients);
            Assert.Equal(clients.Count, deserializedClients.Count);
            Assert.Equal(clients.First(), deserializedClients.First());
        }
    }
}
