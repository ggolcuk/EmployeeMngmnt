using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using EmployeeManagement.Models;
using EmployeeManagement.Utilities;
using Newtonsoft.Json;

namespace EmployeeManagement.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;

        public event Action<string>? ApiErrorOccurred;

        public ApiService()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(Properties.Settings.Default.BaseUrl)
            };
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {Properties.Settings.Default.ApiKey}");
        }

        public async Task<List<Employee>> GetEmployeesAsync(string firstName = null, int page = 1)
        {
            string apiUrl = "public/v2/users";

            if (!string.IsNullOrEmpty(firstName))
            {
                apiUrl += $"?first_name={Uri.EscapeDataString(firstName)}";
            }

            apiUrl += $"?page={page}";

            return await ApiUtilities.HandleApiCallAsync<List<Employee>>(
                async () => await _httpClient.GetAsync(apiUrl),
                "Error getting employees"
            ) ?? new List<Employee>();
        }

        public async Task<Employee> GetEmployeeAsync(int id)
        {
            return await ApiUtilities.HandleApiCallAsync<Employee>(
                async () => await _httpClient.GetAsync($"public/v2/users/{id}"),
                "Error getting employee"
            );
        }

        public async Task<Employee> CreateEmployeeAsync(Employee employee)
        {
            var json = JsonConvert.SerializeObject(employee);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            return await ApiUtilities.HandleApiCallAsync<Employee>(
                async () => await _httpClient.PostAsync("public/v2/users", content),
                "Error creating employee"
            );
        }

        public async Task<Employee> UpdateEmployeeAsync(int id, Employee employee)
        {
            var json = JsonConvert.SerializeObject(employee);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            return await ApiUtilities.HandleApiCallAsync<Employee>(
                async () => await _httpClient.PutAsync($"public/v2/users/{id}", content),
                "Error updating employee"
            );
        }

        public async Task<bool> DeleteEmployeeAsync(int id)
        {
            return await ApiUtilities.HandleApiCallAsync<bool>(
                async () => await _httpClient.DeleteAsync($"public/v2/users/{id}"),
                "Error deleting employee"
            );
        }

    }

    // Create a class to represent the response structure from the API
    public class EmployeeResponse
    {
        public List<Employee> Data { get; set; }
    }
}
