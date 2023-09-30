using Models;

namespace Services.Storage
{
    public class ClientStorage : IClientStorage<Client>
    {
        public readonly List<Client> clients;
        public Dictionary<Client, List<Account>> Data { get; set; }
        public void Add(Client client) => clients.Add(client);
        public void Update(Client client) { }
        public void Delete(Client client) => clients.Remove(client);
        public void AddAccount(Client client, Account account)
        {
            if (Data.ContainsKey(client))
                Data[client].Add(account);
            else
                Data[client] = new List<Account> { account };
        }
        public void UpdateAccount(Client client, Account account)
        {
            if (Data.ContainsKey(client))
            {
                var clientAccounts = Data[client];
                var existingAccount = clientAccounts.FirstOrDefault(a => a.Currency == account.Currency);
                if (existingAccount != null)
                    existingAccount.Amount = account.Amount;
            }
        }
        public void DeleteAccount(Client client, Account account)
        {
            if (Data.ContainsKey(client))
            {
                var clientAccounts = Data[client];
                clientAccounts.Remove(account);
            }
        }
        public ClientStorage()
        {
            clients = new List<Client>();
            Data = new Dictionary<Client, List<Account>>();
        }
    }
}
