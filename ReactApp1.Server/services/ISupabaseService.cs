using ReactApp1.Server.Models;

namespace ReactApp1.Server.services
{
    public interface ISupabaseService
    {
        Task<List<Product>> GetProductsAsync();
    }
}