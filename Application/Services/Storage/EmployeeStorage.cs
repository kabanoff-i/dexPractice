using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Storage
{
    public class EmployeeStorage : IEmployeeStorage<Employee>
    {
        public readonly List<Employee> employees;
        public Dictionary<Employee, List<Account>> Data { get; set; }

        public void Add(Employee employee) => employees.Add(employee);
        public void Update(Employee employee) { }
        public void Delete(Employee employee) => employees.Remove(employee);

        public void AddAccount(Employee employee, Account account)
        {
            if (Data.ContainsKey(employee))
                Data[employee].Add(account);
            else
                Data[employee] = new List<Account> { account };
        }

        public void UpdateAccount(Employee employee, Account account)
        {
            if (Data.ContainsKey(employee))
            {
                var employeeAccounts = Data[employee];
                var existingAccount = employeeAccounts.FirstOrDefault(a => a.Currency == account.Currency);
                if (existingAccount != null)
                    existingAccount.Amount = account.Amount;
            }
        }

        public void DeleteAccount(Employee employee, Account account)
        {
            if (Data.ContainsKey(employee))
            {
                var employeeAccounts = Data[employee];
                employeeAccounts.Remove(account);
            }
        }

        public EmployeeStorage()
        {
            employees = new List<Employee>();
            Data = new Dictionary<Employee, List<Account>>();
        }
    }

}
