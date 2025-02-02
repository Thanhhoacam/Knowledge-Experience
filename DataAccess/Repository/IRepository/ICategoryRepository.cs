using BusinessObject.Object;
using System.Linq.Expressions;

namespace DataAccess.Repository.IRepository
{
    public interface ICategoryRepository
    {
        //Task<IEnumerable<Category>> GetCategoriesAsync();
        //Task InsertCategoryAsync(Category category);
        //Task DeleteCategoryAsync(Category category);
        //Task UpdateCategoryAsync(Category category);
        //Task<Category> GetCategoryByIdAsync(int id);

        //---------v2----------//
        //public Task<Category> GetAsync(Expression<Func<Category, bool>> filter = null, bool tracked = true);
        //public Task<List<Category>> GetAllAsync(Expression<Func<Category, bool>> filter = null);
        //public Task RemoveAsync(Category entity);
        //public Task UpdateAsync(Category entity);
        //public Task CreateAsync(Category entity);

        //---------v3----------//
        Task<List<Category>> GetAllAsync(Expression<Func<Category, bool>>? filter = null, string? includeProperties = null,
          int pageSize = 0, int pageNumber = 1);
        Task<Category> GetAsync(Expression<Func<Category, bool>> filter = null, bool tracked = true, string? includeProperties = null);
        Task CreateAsync(Category entity);
        Task RemoveAsync(Category entity);
        Task<Category> UpdateAsync(Category entity);

    }
}
