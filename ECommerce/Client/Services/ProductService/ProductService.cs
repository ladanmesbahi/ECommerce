using ECommerce.Shared;
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
        public string Message { get; set; }

        public event Action ProductsChanged;

        public async Task<ServiceResponse<Product>> GetProduct(int productId)
        {
            return (await _http.GetFromJsonAsync<ServiceResponse<Product>>($"api/product/{productId}"));
        }

        public async Task GetProducts(string? categoryUrl = null)
        {
            var result = categoryUrl is null ? await _http.GetFromJsonAsync<ServiceResponse<List<Product>>>("api/Product") :
            await _http.GetFromJsonAsync<ServiceResponse<List<Product>>>($"api/Product/Category/{categoryUrl}");
            if (result != null && result.Data != null)
                Products = result.Data;

            ProductsChanged.Invoke();
        }

        public async Task<List<string>> GetProductSearchSuggestions(string searchText)
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<List<String>>>($"api/product/searchSuggestion/{searchText}");
            return result.Data;
        }

        public async Task SearchProducts(string searchText)
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<List<Product>>>($"api/product/search/{searchText}");
            if (result != null && result.Data != null)
                Products = result.Data;
            if (Products.Count == 0)
                Message = "No products found!";

            ProductsChanged.Invoke();
        }
    }
}
