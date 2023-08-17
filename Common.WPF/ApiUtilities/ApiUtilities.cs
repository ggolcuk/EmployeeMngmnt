using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;


namespace Common.WPF.ApiUtilities
{
    // Utility class for common API-related functionality
    public static class ApiUtilities
    {
        public static async Task<T> HandleApiCallAsync<T>(
            Func<Task<HttpResponseMessage>> apiCall,
            string errorMessage,
            Action<string>? apiErrorOccurred = null)
        {
            try
            {
                var response = await apiCall.Invoke();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<T>(content);
                }
                else if (response.StatusCode == HttpStatusCode.NoContent)
                {
                    if (typeof(T) == typeof(bool))
                    {
                        // Return true for bool type for successful delete
                        return (T)(object)true;
                    }

                    // Return default value for other types
                    return default(T);
                }
                else if (response.StatusCode == HttpStatusCode.Conflict)
                {
                    apiErrorOccurred?.Invoke(errorMessage + ": Conflict");
                }
                else
                {
                    apiErrorOccurred?.Invoke(errorMessage + ": Error (" + response.StatusCode + ")");
                }

                // Return default value for T in case of non-OK status code
                return default(T);
            }
            catch (Exception ex)
            {
                apiErrorOccurred?.Invoke(errorMessage + ": " + ex.Message);
                return default(T);
            }
        }


    }
}
