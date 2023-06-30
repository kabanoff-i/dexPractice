using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
