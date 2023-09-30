using ModelsDb;
using ServicesDb.Exceptions;
using Microsoft.EntityFrameworkCore;

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
        public  async Task<Employee?> GetEmployeeAsync(Guid employeeId)
        {
            return await _dbContext.employee.FirstOrDefaultAsync(c => c.id == employeeId);
        }
        public async Task<List<Employee>> GetEmployeesAsync()
        {
            return await _dbContext.employee.ToListAsync();
        }
        public async Task AddEmployeeAsync(Employee employee)
        {
            await _dbContext.employee.AddAsync(employee);
            await _dbContext.SaveChangesAsync();
        }
        public async Task ChangeEmployeeAsync(Guid employeeId)
        {
            Employee? employee = await _dbContext.employee.FirstOrDefaultAsync(c => c.id == employeeId);
            if (employee == null)
            {
                throw new EmployeeNotFoundException("Employee not found");
            }
            employee.surname += " changed";
            await _dbContext.SaveChangesAsync();
        }
        public async Task DeleteEmployeeAsync(Guid employeeId)
        {
            Employee? employee = await _dbContext.employee.FirstOrDefaultAsync(c => c.id == employeeId);
            if (employee == null)
            {
                throw new EmployeeNotFoundException("Employee not found");
            }

            _dbContext.employee.Remove(employee);
            await _dbContext.SaveChangesAsync();
        }
    }
}
