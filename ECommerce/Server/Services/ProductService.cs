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
    }
}
