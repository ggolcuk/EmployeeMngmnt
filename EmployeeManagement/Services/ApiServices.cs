using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Common.API.ApiUtilities;
using EmployeeManagement.Models;
using Newtonsoft.Json;

namespace EmployeeManagement.Services
{
    // Service class responsible for employee-related operations
    public class ApiService : IApiService
    {
        private readonly IHttpClient _httpClient;

        // Event that triggers when an API error occurs
        public event Action<string>? ApiErrorOccurred;



        public ApiService(IHttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        // Retrieve a list of employees based on parameters
        public async Task<List<Employee>> GetEmployeesAsync(SearchParameters searchParameters)
        {
            string apiUrl = $"users?page={searchParameters.Page}&per_page={searchParameters.PerPage}";

            if (!string.IsNullOrEmpty(searchParameters.Name))
            {
                apiUrl += $"&name={Uri.EscapeDataString(searchParameters.Name)}";
            }
            if (!string.IsNullOrEmpty(searchParameters.Email))
            {
                apiUrl += $"&email={Uri.EscapeDataString(searchParameters.Email)}";
            }
            if (!string.IsNullOrEmpty(searchParameters.Gender))
            {
                apiUrl += $"&gender={Uri.EscapeDataString(searchParameters.Gender)}";
            }
            if (!string.IsNullOrEmpty(searchParameters.Status))
            {
                apiUrl += $"&status={Uri.EscapeDataString(searchParameters.Status)}";
            }

            return await ApiUtilities.HandleApiCallAsync<List<Employee>>(
                async () => await _httpClient.GetAsync(apiUrl),
                "Error getting employees",
                ApiErrorOccurred
            ) ?? new List<Employee>();
        }


        public async Task<List<Employee>> GetEmployeesAsync()
        {
            string apiUrl = "users";

            return await ApiUtilities.HandleApiCallAsync<List<Employee>>(
                async () => await _httpClient.GetAsync(apiUrl),
                "Error getting employees",
                ApiErrorOccurred
            ) ?? new List<Employee>();
        }


        // Retrieve a single employee by ID
        public async Task<Employee> GetEmployeeAsync(int id)
        {
            return await ApiUtilities.HandleApiCallAsync<Employee>(
                async () => await _httpClient.GetAsync($"users/{id}"),
                "Error getting employee",
                ApiErrorOccurred
            );
        }

        // Create a new employee
        public async Task<Employee> CreateEmployeeAsync(Employee employee)
        {
            var json = JsonConvert.SerializeObject(employee);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            return await ApiUtilities.HandleApiCallAsync<Employee>(
                async () => await _httpClient.PostAsync("users", content),
                "Error creating employee",
                ApiErrorOccurred
            );
        }

        // Update an existing employee by ID
        public async Task<Employee> UpdateEmployeeAsync(int id, Employee employee)
        {
            var json = JsonConvert.SerializeObject(employee);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            return await ApiUtilities.HandleApiCallAsync<Employee>(
                async () => await _httpClient.PutAsync($"users/{id}", content),
                "Error updating employee",
                ApiErrorOccurred
            );
        }

        // Delete an employee by ID
        public async Task<bool> DeleteEmployeeAsync(int id)
        {
            return await ApiUtilities.HandleApiCallAsync<bool>(
                 async () =>
                 {
                     var response = await _httpClient.DeleteAsync($"users/{id}");
                     return response;
                 },
                 "Error deleting employee",
                 ApiErrorOccurred
             );
        }
    }

}


