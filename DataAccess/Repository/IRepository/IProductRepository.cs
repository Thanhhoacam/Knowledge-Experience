using BusinessObject.Object;
using System.Linq.Expressions;

namespace DataAccess.Repository.IRepository
{
    public interface IProductRepository
    {
        //---------v1----------//
        //IEnumerable<Product> GetProducts();
        //void InsertProduct(Product product);
        //void DeleteProduct(Product product);
        //void UpdateProduct(Product product);
        //Product GetProductById(int id);
        //---------v2 async----------//
        //public Task<Product> GetAsync(Expression<Func<Product, bool>> filter = null, bool tracked = true);
        //public Task<List<Product>> GetAllAsync(Expression<Func<Product, bool>> filter = null);
        //public Task RemoveAsync(Product entity);
        //public Task UpdateAsync(Product entity);
        //public Task CreateAsync(Product entity);
        //---------v3 async----------//
        Task<List<Product>> GetAllAsync(Expression<Func<Product, bool>>? filter = null, string? includeProperties = null,
           int pageSize = 0, int pageNumber = 1);
        Task<Product> GetAsync(Expression<Func<Product, bool>> filter = null, bool tracked = true, string? includeProperties = null);
        Task CreateAsync(Product entity);
        Task RemoveAsync(Product entity);
        Task<Product> UpdateAsync(Product entity);
        //Task SaveAsync();
    }
}
