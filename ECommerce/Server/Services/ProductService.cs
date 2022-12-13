using ECommerce.Shared;

namespace ECommerce.Server.Services
{
    public class ProductService : IProductService
    {
        private readonly DataContext _context;

        public ProductService(DataContext context)
        {
            _context = context;
        }

        public async Task<ServiceResponse<Product>> GetProductAsync(int productId)
        {
            var result = new ServiceResponse<Product>();
            var product = await _context.Set<Product>()
                .Include(p => p.Variants)
                .ThenInclude(v => v.ProductType)
                .FirstOrDefaultAsync(p => p.Id == productId);
            if (product is null)
            {
                result.Success = false;
                result.Message = "Sorry, the product was not found";
            }
            else
                result.Data = product;
            return result;
        }

        public async Task<ServiceResponse<List<Product>>> GetProductsAsync()
        {
            return new ServiceResponse<List<Product>>
            {
                Data = await _context.Set<Product>()
                .Include(p => p.Variants)
                .ToListAsync()
            };
        }

        public async Task<ServiceResponse<List<Product>>> GetProductsByCategoryAsync(string categoryUrl)
        {
            return new ServiceResponse<List<Product>>
            {
                Data = await _context.Set<Product>()
            .Where(p => p.Category.Url.ToLower().Equals(categoryUrl.ToLower()))
            .Include(p => p.Variants)
            .ToListAsync()
            };
        }

        public async Task<ServiceResponse<List<string>>> GetProductSearchSuggestions(string searchText)
        {
            var products = await FindProductsBySearchText(searchText);
            var result = new List<string>();
            foreach (var product in products)
            {
                if (product.Title.Contains(searchText, StringComparison.OrdinalIgnoreCase))
                    result.Add(product.Title);
                if (product.Description != null)
                {
                    var punctuation = product.Description.Where(char.IsPunctuation).Distinct().ToArray();
                    var words = product.Description.Split()
                        .Select(s => s.Trim(punctuation));
                    foreach (var word in words)
                        if (word.Contains(searchText, StringComparison.OrdinalIgnoreCase) && !result.Contains(word))
                            result.Add(word);
                }
            }
            return new ServiceResponse<List<string>>
            {
                Data = result
            };
        }

        public async Task<ServiceResponse<List<Product>>> SearchProducts(string seacrhText)
        {
            return new ServiceResponse<List<Product>>
            {
                Data = await FindProductsBySearchText(seacrhText)
            };
        }

        private async Task<List<Product>> FindProductsBySearchText(string seacrhText)
        {
            return await _context.Products
                            .Where(p => p.Title.ToLower().Contains(seacrhText.ToLower()) || p.Description.ToLower().Contains(seacrhText.ToLower()))
                            .Include(p => p.Variants)
                            .ToListAsync();
        }
    }
}
