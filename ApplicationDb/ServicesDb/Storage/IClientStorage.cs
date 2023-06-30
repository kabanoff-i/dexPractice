using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelsDb;

namespace ServicesDb.Storage
{
    public interface IClientStorage<T>: IStorage<T>
    {
        Dictionary<Client, List<Account>> Data { get; set; }
        void AddAccount() { }
        void UpdateAccount() { }
        void DeleteAccount() { }
    }
}
