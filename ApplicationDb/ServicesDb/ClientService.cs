using Microsoft.EntityFrameworkCore;
using ModelsDb;
using ServicesDb.Exceptions;

namespace ServicesDb
{
    public class ClientService
    {
        BankServiceContext _dbContext;
        public ClientService(BankServiceContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }
        public ClientService()
        {
            _dbContext = new BankServiceContext();
        }
        //client
        public async Task<Client?> GetClientAsync(Guid clientId)
        {
            return await _dbContext.client.FirstOrDefaultAsync(c => c.id == clientId);
        }
        public async Task AddClientAsync(Client client)
        {
            await _dbContext.client.AddAsync(client);
            await _dbContext.SaveChangesAsync();
            await _dbContext.account.AddAsync(CreateDefaultAccount(client));
            await _dbContext.SaveChangesAsync();
        }
        public async Task ChangeClientAsync(Guid clientId)
        {
            Client? client = _dbContext.client.FirstOrDefaultAsync(c => c.id == clientId).Result;
            if (client == null)
            {
                throw new ClientNotFoundException("Client not found");
            }

            client.surname += " changed";
            await _dbContext.SaveChangesAsync();
        }
        public async Task DeleteClientAsync(Guid clientId)
        {
            Client? client = _dbContext.client.FirstOrDefaultAsync(c => c.id == clientId).Result;
            if (client == null)
            {
                throw new ClientNotFoundException("Client not found");
            }

            _dbContext.client.Remove(client);
            await _dbContext.SaveChangesAsync();

        }
        //account
        public async Task AddAccountAsync(Guid clientId)
        {
            if (_dbContext.client.FindAsync(clientId).Result != null)
            {
                throw new ClientNotFoundException("Client not found");
            }

            await _dbContext.account.AddAsync(CreateDefaultAccount(_dbContext.client.FindAsync(clientId).Result));
            await _dbContext.SaveChangesAsync();
        }
        public async Task DeleteAccountAsync(Guid AccountId)
        {
            Account? account = _dbContext.account.FirstOrDefaultAsync(c => c.accountNumber == AccountId).Result;
            if (account == null)
                throw new AccountNotFoundException("Account not found");

            _dbContext.account.Remove(account);
            await _dbContext.SaveChangesAsync();
        }
        private Account CreateDefaultAccount(Client client)
        {
            return new Account(Guid.NewGuid(), "USDA", client.id, 0, client, new Currency("USDA", '$'));
        }
    }
}