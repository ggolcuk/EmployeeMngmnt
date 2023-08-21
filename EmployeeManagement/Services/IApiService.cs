using EmployeeManagement.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmployeeManagement.Services
{
    public interface IApiService
    {
        event Action<string>? ApiErrorOccurred;

        Task<Employee> CreateEmployeeAsync(Employee employee);
        Task<bool> DeleteEmployeeAsync(int id);
        Task<Employee> GetEmployeeAsync(int id);
        Task<List<Employee>> GetEmployeesAsync();
        Task<List<Employee>> GetEmployeesAsync(SearchParameters sp);
        Task<Employee> UpdateEmployeeAsync(int id, Employee employee);
    }
}