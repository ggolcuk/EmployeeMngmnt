using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace EmployeeManagement.Utilities
{

    public static class ApiUtilities
    {
        public static async Task<T> HandleApiCallAsync<T>(Func<Task<HttpResponseMessage>> apiCall, string errorMessage)
        {
            try
            {
                var response = await apiCall.Invoke();
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(content);
            }
            catch (Exception ex)
            {
                // Handle error here or propagate it as needed
                // You could log the error, raise an event, etc.
                return default(T);
            }
        }
    }
    
}
