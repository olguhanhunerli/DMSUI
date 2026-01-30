using DMSUI.Business.Interfaces;
using DMSUI.Entities.DTOs.Common;
using DMSUI.Entities.DTOs.Customers;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DMSUI.Business
{
    public class CustomerApiClient : ICustomerApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CustomerApiClient(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }
        private void AttachToken()
        {
            var token = _httpContextAccessor.HttpContext?
                .Request.Cookies["access_token"];

            _httpClient.DefaultRequestHeaders.Remove("Authorization");

            if (!string.IsNullOrWhiteSpace(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }
        }

        public async Task<bool> CreateCustomer(CustomerCreateDTO createCustomerDTO)
        {
            AttachToken();
            var response = await _httpClient.PostAsJsonAsync(
                "api/Customer",
                createCustomerDTO
            );
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteCustomer(int id)
        {
            AttachToken();
            var response = await _httpClient.DeleteAsync(
                $"api/Customer/{id}"
            );
            return response.IsSuccessStatusCode;
        }

        public async Task<CustomersItemDTO> GetCustomerById(int id)
        {
            AttachToken();
            var response = await _httpClient.GetAsync(
                $"api/Customer/{id}"
            );
            if(!response.IsSuccessStatusCode)
            {
                return new CustomersItemDTO();
            }
            var body = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<CustomersItemDTO>(
                body,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            ) ?? new CustomersItemDTO();
        }

        public async Task<PagedResultDTO<CustomersItemDTO>> GetCustomersPaging(int page, int pageSize)
        {
            AttachToken();
            var response = await _httpClient.GetAsync(
                $"api/Customer?page={page}&pageSize={pageSize}"
            );
            if(!response.IsSuccessStatusCode)
            {
                return new PagedResultDTO<CustomersItemDTO>();
            }
            var body = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<PagedResultDTO<CustomersItemDTO>>(
                body,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            ) ?? new PagedResultDTO<CustomersItemDTO>();
        }

        public async Task<bool> UpdateCustomer(int id, UpdateCustomerDTO updateCustomerDTO)
        {
            AttachToken();

            var response = await _httpClient.PutAsJsonAsync($"api/Customer/{id}", updateCustomerDTO);

            if (!response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"UpdateCustomer failed. Status={(int)response.StatusCode} Body={body}");
            }

            return response.IsSuccessStatusCode;
        }

        public async Task<List<CustomerMiniDTO>> GetAllCustomersMini(int id)
        {
            AttachToken();
            var response = await _httpClient.GetAsync(
                $"api/Customer/lookup?id={id}"
            );
            if(!response.IsSuccessStatusCode)
            {
                return new List<CustomerMiniDTO>();
            }
            var body = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<CustomerMiniDTO>>(
                body,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            ) ?? new List<CustomerMiniDTO>();
        }
    }
}
