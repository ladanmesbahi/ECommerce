using ECommerce.Shared;

namespace ECommerce.Server.Services
{
    public interface ICategoryService
    {
        Task<ServiceResponse<List<Category>>> GetCategories();
    }
}
