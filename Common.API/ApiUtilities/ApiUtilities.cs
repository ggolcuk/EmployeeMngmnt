using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;


namespace Common.API.ApiUtilities
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
                var statusCode = response.StatusCode;

                switch (statusCode)
                {
                    case HttpStatusCode.OK:
                    case HttpStatusCode.Created:
                        var content = await response.Content.ReadAsStringAsync();
                        return JsonConvert.DeserializeObject<T>(content);

                    case HttpStatusCode.NoContent:
                        if (typeof(T) == typeof(bool))
                        {
                            // Return true for bool type for successful delete
                            return (T)(object)true;
                        }
                        break;

                    case HttpStatusCode.Conflict:
                        apiErrorOccurred?.Invoke(errorMessage + ": Conflict");
                        break;

                    case HttpStatusCode.BadRequest:
                    case HttpStatusCode.Unauthorized:
                    case HttpStatusCode.Forbidden:
                    case HttpStatusCode.NotFound:
                    case HttpStatusCode.MethodNotAllowed:
                    case HttpStatusCode.UnsupportedMediaType:
                    case HttpStatusCode.UnprocessableEntity:
                    case HttpStatusCode.TooManyRequests:
                    case HttpStatusCode.InternalServerError:
                        apiErrorOccurred?.Invoke(errorMessage + ": Error (" + statusCode + ")");
                        break;

                    // You can add more cases for other status codes if needed

                    default:
                        apiErrorOccurred?.Invoke(errorMessage + ": Unknown error (" + statusCode + ")");
                        break;
                }

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
