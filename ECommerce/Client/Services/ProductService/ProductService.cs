﻿using ECommerce.Shared;
using System.Net.Http.Json;

namespace ECommerce.Client.Services.ProductService
{
    public class ProductService : IProductService
    {
        private readonly HttpClient _http;

        public ProductService(HttpClient http)
        {
            _http = http;
        }
        public List<Product> Products { get; set; } = new List<Product>();

        public async Task<ServiceResponse<Product>> GetProduct(int productId)
        {
            return (await _http.GetFromJsonAsync<ServiceResponse<Product>>($"api/product/{productId}"));
        }

        public async Task GetProducts()
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<List<Product>>>("api/Product");
            if (result != null && result.Data != null)
                Products = result.Data;
        }
    }
}
