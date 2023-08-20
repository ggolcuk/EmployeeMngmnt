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
                        apiErrorOccurred?.Invoke(errorMessage + ": Conflict - The requested operation could not be completed due to a conflict with the current state of the resource.");
                        break;

                    case HttpStatusCode.BadRequest:
                        apiErrorOccurred?.Invoke(errorMessage + ": Bad Request - The request was invalid or could not be understood by the server.");
                        break;

                    case HttpStatusCode.Unauthorized:
                        apiErrorOccurred?.Invoke(errorMessage + ": Unauthorized - Authentication credentials are missing or invalid.");
                        break;

                    case HttpStatusCode.Forbidden:
                        apiErrorOccurred?.Invoke(errorMessage + ": Forbidden - The server understood the request, but it refuses to authorize it.");
                        break;

                    case HttpStatusCode.NotFound:
                        apiErrorOccurred?.Invoke(errorMessage + ": Not Found - The requested resource could not be found.");
                        break;

                    case HttpStatusCode.MethodNotAllowed:
                        apiErrorOccurred?.Invoke(errorMessage + ": Method Not Allowed - The requested method is not supported for the requested resource.");
                        break;

                    case HttpStatusCode.UnsupportedMediaType:
                        apiErrorOccurred?.Invoke(errorMessage + ": Unsupported Media Type - The server does not support the media type that the requested resource requires.");
                        break;

                    case HttpStatusCode.UnprocessableEntity:
                        apiErrorOccurred?.Invoke(errorMessage + ": Unprocessable Entity - The server understands the content type of the request entity, but it was unable to process the contained instructions.");
                        break;

                    case HttpStatusCode.TooManyRequests:
                        apiErrorOccurred?.Invoke(errorMessage + ": Too Many Requests - The user has sent too many requests in a given amount of time.");
                        break;

                    case HttpStatusCode.InternalServerError:
                        apiErrorOccurred?.Invoke(errorMessage + ": Internal Server Error - The server encountered an unexpected condition that prevented it from fulfilling the request.");
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
