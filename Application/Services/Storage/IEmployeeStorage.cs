using Models;

namespace Services.Storage
{
    public interface IEmployeeStorage<T>: IStorage<T>
    {
        Dictionary<Employee, List<Account>> Data { get; set; }
        void AddAccount() { }
        void UpdateAccount() { }
        void DeleteAccount() { }
    }
}
