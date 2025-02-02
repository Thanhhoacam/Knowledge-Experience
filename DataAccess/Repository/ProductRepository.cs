using BusinessObject.Object;
using DataAccess.Dao;
using DataAccess.Repository.IRepository;
using System.Linq.Expressions;

namespace DataAccess.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ProductDAOv2 productDAOv2;

        public ProductRepository(ProductDAOv2 productDAOv2)
        {
            this.productDAOv2 = productDAOv2;
        }
        #region

        //---------v1----------//
        public void DeleteProduct(Product product)
        {
            ProductDAO.Instance.DeleteProduct(product);
        }
        public Product GetProductById(int id)
        {
           return ProductDAO.Instance.GetProductById(id);
        }

        public IEnumerable<Product> GetProducts()
        {
           return ProductDAO.Instance.GetProducts();
        }

        public void InsertProduct(Product product)
        {
            ProductDAO.Instance.InsertProduct(product);
        }

        public void UpdateProduct(Product product)
        {
           ProductDAO.Instance.UpdateProduct(product);
        }

        //---------v2 async----------//
        //public async Task CreateAsync(Product entity)
        //{
        //    await ProductDAO.Instance.CreateAsync(entity);
        //}


        //public async Task<List<Product>> GetAllAsync(Expression<Func<Product, bool>> filter = null)
        //{
        //    return await ProductDAO.Instance.GetAllAsync(filter);
        //}

        //public async Task<Product> GetAsync(Expression<Func<Product, bool>> filter = null, bool tracked = true)
        //{
        //   return await ProductDAO.Instance.GetAsync(filter, tracked);
        //}

        //public async Task RemoveAsync(Product entity)
        //{
        //    await ProductDAO.Instance.RemoveAsync(entity);
        //}

        //public async Task UpdateAsync(Product entity)
        //{
        //    await ProductDAO.Instance.UpdateAsync(entity);
        //}
        #endregion
        //---------v2 DAOv2----------//
        public async Task CreateAsync(Product entity)
        {
            await productDAOv2.CreateAsync(entity);
        }


        public async Task<List<Product>> GetAllAsync(Expression<Func<Product, bool>>? filter = null, string? includeProperties = null,
            int pageSize = 0, int pageNumber = 1)
        {
            return await productDAOv2.GetAllAsync(filter: filter, pageSize: pageSize, pageNumber: pageNumber, includeProperties: includeProperties);
        }


        public async Task RemoveAsync(Product entity)
        {
            await productDAOv2.RemoveAsync(entity);
        }

        public async Task UpdateAsync(Product entity)
        {
            await productDAOv2.UpdateAsync(entity);
        }

        public async Task<Product> GetAsync(Expression<Func<Product, bool>> filter = null, bool tracked = true, string includeProperties = null)
        {
            return await productDAOv2.GetAsync(filter, tracked, includeProperties);
        }

        async Task<Product> IProductRepository.UpdateAsync(Product entity)
        {
            return await productDAOv2.UpdateAsync(entity);
        }

        //public Task SaveAsync()
        //{
        //    throw new NotImplementedException();
        //}
    }
}
