using ModelsDb;
using ServicesDb.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesDb
{
    public class EmployeeService
    {
        ApplicationContext _dbContext;
        public EmployeeService()
        {
            _dbContext = new ApplicationContext();
        }
        //employee
        public Employee GetEmployee(Guid employeeId)
        {
            return _dbContext.employee.FirstOrDefault(c => c.id == employeeId);
        }
        public void AddEmployee(Employee employee)
        {
            _dbContext.employee.Add(employee);
            _dbContext.SaveChanges();
        }
        public void ChangeEmployee(Guid employeeId)
        {
            Employee employee = _dbContext.employee.FirstOrDefault(c => c.id == employeeId);
            if (employee == null)
            {
                throw new EmployeeNotFoundException("Employee not found");
            }
            employee.surname += " changed";
            _dbContext.SaveChanges();
        }
        public void DeleteEmployee(Guid employeeId)
        {
            Employee employee = _dbContext.employee.FirstOrDefault(c => c.id == employeeId);
            if (employee == null)
            {
                throw new EmployeeNotFoundException("Employee not found");
            }

            _dbContext.employee.Remove(employee);

        }
    }
}
