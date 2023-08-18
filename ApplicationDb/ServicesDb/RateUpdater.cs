using ModelsDb;

namespace ServicesDb
{
    public class RateUpdater
    {
         async Task UpdateRateAsync(List<Client> clients, CancellationToken token)
        {
            Timer timer = new Timer(UpdateRate, clients, TimeSpan.Zero, TimeSpan.FromDays(30));
            await Task.Delay(Timeout.Infinite, token);
        }
         void UpdateRate(object clients)
        {
            Parallel.ForEachAsync((List<Client>)clients, UpdateClientRate);
        }

         ValueTask UpdateClientRate(Client client, CancellationToken token)
        {
            if (!client.accounts.Any())
                client.accounts.ToList().Add(new Account());
            client.accounts.ToList().First().amount += (int)(client.accounts.ToList().First().amount * 0.1);
            return ValueTask.CompletedTask;
        }
    }
}
