using ModelsDb;
using ServicesDb.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public Client GetClient(Guid clientId)
        {
            return _dbContext.client.FirstOrDefault(c => c.id == clientId);
        }
        public void AddClient(Client client)
        {
            _dbContext.client.Add(client);
            _dbContext.account.Add(CreateDefaultAccount(client));
            _dbContext.SaveChanges();
        }
        public void ChangeClient(Guid clientId)
        {
            Client client = _dbContext.client.FirstOrDefault(c => c.id == clientId);
            if (client == null)
            {
                throw new ClientNotFoundException("Client not found");
            }

            client.surname += " changed";
            _dbContext.SaveChanges();
        }
        public void DeleteClient(Guid clientId)
        {
            Client client = _dbContext.client.FirstOrDefault(c => c.id == clientId);
            if (client == null)
            {
                throw new ClientNotFoundException("Client not found");
            }

            _dbContext.client.Remove(client);
            _dbContext.SaveChanges();

        }
        //account
        public void AddAccount(Guid clientId)
        {
            if (_dbContext.client.Find(clientId) != null)
            {
                throw new ClientNotFoundException("Client not found");
            }

            _dbContext.account.Add(CreateDefaultAccount(_dbContext.client.Find(clientId)));
            _dbContext.SaveChanges();
        }
        public void DeleteAccount(Guid AccountId)
        {
            var account = _dbContext.account.FirstOrDefault(c => c.accountNumber == AccountId);
            if (account == null)
                throw new AccountNotFoundException("Account not found");

            _dbContext.account.Remove(account);
            _dbContext.SaveChanges();
        }
        private Account CreateDefaultAccount(Client client)
        {
            return new Account(Guid.NewGuid(), "USD", client.id, 0, client, new Currency("USD", '$'));
        }
    }
}