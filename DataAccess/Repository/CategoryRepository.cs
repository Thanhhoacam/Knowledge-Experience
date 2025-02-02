using BusinessObject.Object;
using DataAccess.Dao;
using DataAccess.Repository.IRepository;
using System.Linq.Expressions;

namespace DataAccess.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly CategoryDAOv2 categoryDAOv2;

        public CategoryRepository(CategoryDAOv2 categoryDAOv2)
        {
            this.categoryDAOv2 = categoryDAOv2;
        }

        //---------v2 DAOv2----------//
        public async Task CreateAsync(Category entity)
        {
            await categoryDAOv2.CreateAsync(entity);
        }


        public async Task<List<Category>> GetAllAsync(Expression<Func<Category, bool>>? filter = null, string? includeProperties = null,
            int pageSize = 0, int pageNumber = 1)
        {
            return await categoryDAOv2.GetAllAsync(filter: filter, pageSize: pageSize, pageNumber: pageNumber, includeProperties: includeProperties);
        }


        public async Task RemoveAsync(Category entity)
        {
            await categoryDAOv2.RemoveAsync(entity);
        }

        public async Task UpdateAsync(Category entity)
        {
            await categoryDAOv2.UpdateAsync(entity);
        }

        public async Task<Category> GetAsync(Expression<Func<Category, bool>> filter = null, bool tracked = true, string includeProperties = null)
        {
            return await categoryDAOv2.GetAsync(filter, tracked, includeProperties);
        }

        async Task<Category> ICategoryRepository.UpdateAsync(Category entity)
        {
            return await categoryDAOv2.UpdateAsync(entity);
        }

        //public Task SaveAsync()
        //{
        //    throw new NotImplementedException();
        //}
    }
}
