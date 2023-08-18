using Models;

namespace Services.Storage
{
    public interface IClientStorage<T>: IStorage<T>
    {
        Dictionary<Client, List<Account>> Data { get; set; }
        void AddAccount() { }
        void UpdateAccount() { }
        void DeleteAccount() { }
    }
}
