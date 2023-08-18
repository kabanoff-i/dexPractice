using ModelsDb;

namespace ServicesDb
{
    public class CashDispenserService
    {
        async Task DispenseCash(int n, List<Client> clients)
        {
            SemaphoreSlim semaphore = new SemaphoreSlim(n);
            List<Task> tasks = new();
            
            foreach (Client client in clients) {
                Task task = DispenseCashAsync(client, semaphore);
                tasks.Add(task);
            }

            await Task.WhenAll(tasks);
        }
        async Task DispenseCashAsync(Client client, SemaphoreSlim semaphore)
        {
            await semaphore.WaitAsync();
            try
            {
                foreach (var account in client.accounts)
                {
                    account.amount = 0;
                }
            }
            finally 
            { 
                semaphore.Release(); 
            }
        }
    }
}
