using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows;
using EmployeeManagement.Models;


namespace EmployeeManagement.Services
{
    public class ApiService
    {
        private readonly string baseUrl = "https://gorest.co.in/public/v2/";
        private readonly string apiToken = "0bf7fb56e6a27cbcadc402fc2fce8e3aa9ac2b40d4190698eb4e8df9284e2023";
        private readonly HttpClient httpClient;

        public ApiService()
        {
            httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiToken}");
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            try
            {
                HttpResponseMessage response = await httpClient.GetAsync($"{baseUrl}users");
                if (response.IsSuccessStatusCode)
                {
                    List<User> users = await response.Content.ReadFromJsonAsync<List<User>>();
                    return users;
                }
                else
                {
                    ShowErrorMessage("Error", $"Error: {response.StatusCode}");
                    return new List<User>();
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage("Error", $"Error: {ex.Message}");
                return new List<User>();
            }
        }
        public async Task<User> CreateUserAsync(User newUser)
        {
            try
            {
                HttpResponseMessage response = await httpClient.PostAsJsonAsync($"{baseUrl}users", newUser);

                if (response.IsSuccessStatusCode)
                {
                    User createdUser = await response.Content.ReadFromJsonAsync<User>();
                    return createdUser;
                }
                else
                {
                    ShowErrorMessage("Error", $"Error: {response.StatusCode}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage("Error", $"Error: {ex.Message}");
                return null;
            }
        }

        public async Task<User> UpdateUserAsync(User updatedUser)
        {
            try
            {
                HttpResponseMessage response = await httpClient.PutAsJsonAsync($"{baseUrl}users/{updatedUser.Id}", updatedUser);

                if (response.IsSuccessStatusCode)
                {
                    User user = await response.Content.ReadFromJsonAsync<User>();
                    return user;
                }
                else
                {
                    ShowErrorMessage("Error", $"Error: {response.StatusCode}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage("Error", $"Error: {ex.Message}");
                return null;
            }
        }

        public async Task<User> UpdateUserPatchAsync(int userId, Dictionary<string, object> patchValues)
        {
            try
            {
                var patchContent = new StringContent(JsonSerializer.Serialize(patchValues), Encoding.UTF8, "application/json");

                HttpResponseMessage response = await httpClient.PatchAsync($"{baseUrl}users/{userId}", patchContent);

                if (response.IsSuccessStatusCode)
                {
                    User user = await response.Content.ReadFromJsonAsync<User>();
                    return user;
                }
                else
                {
                    ShowErrorMessage("Error", $"Error: {response.StatusCode}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage("Error", $"Error: {ex.Message}");
                return null;
            }
        }

        // Other methods and error handling

        

        public async Task<bool> DeleteUserAsync(int userId)
        {
            try
            {
                HttpResponseMessage response = await httpClient.DeleteAsync($"{baseUrl}users/{userId}");

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    ShowErrorMessage("Error", $"Error: {response.StatusCode}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage("Error", $"Error: {ex.Message}");
                return false;
            }
        }

        private void ShowErrorMessage(string title, string message)
        {
            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Error);
        }



    }
}
