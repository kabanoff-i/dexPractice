using ModelsDb;
using ServicesDb.Exceptions;

namespace ServicesDb
{
    public class EmployeeService
    {
        BankServiceContext _dbContext;
        public EmployeeService()
        {
            _dbContext = new BankServiceContext();
        }
        //employee
        public Employee GetEmployee(Guid employeeId)
        {
            return _dbContext.employee.FirstOrDefault(c => c.id == employeeId);
        }
        public List<Employee> GetEmployees()
        {
            return _dbContext.employee.ToList();
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
