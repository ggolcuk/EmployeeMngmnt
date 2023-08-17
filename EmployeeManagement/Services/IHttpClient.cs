using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Services
{
    public interface IHttpClient
    {
        Task<HttpResponseMessage> GetAsync(string requestUri);
        Task<HttpResponseMessage> PostAsync(string requestUri, HttpContent content);
        Task<HttpResponseMessage> PutAsync(string requestUri, HttpContent content);
        Task<HttpResponseMessage> DeleteAsync(string requestUri);
    }

    public class DefaultHttpClient : IHttpClient
    {
        private readonly HttpClient _httpClient;

        public DefaultHttpClient()
        {
            _httpClient = CreateHttpClient();
        }

        public Task<HttpResponseMessage> GetAsync(string requestUri) =>
            _httpClient.GetAsync(requestUri);

        public Task<HttpResponseMessage> PostAsync(string requestUri, HttpContent content) =>
            _httpClient.PostAsync(requestUri, content);

        public Task<HttpResponseMessage> PutAsync(string requestUri, HttpContent content) =>
            _httpClient.PutAsync(requestUri, content);

        public Task<HttpResponseMessage> DeleteAsync(string requestUri) =>
            _httpClient.DeleteAsync(requestUri);

        private HttpClient CreateHttpClient()
        {
            var httpClient = new HttpClient
            {
                BaseAddress = new Uri(Properties.Settings.Default.BaseUrl)
            };
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {Properties.Settings.Default.ApiKey}");

            return httpClient;
        }
    }


}
