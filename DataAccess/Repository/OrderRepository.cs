using BusinessObject.Object;
using DataAccess.Dao;
using DataAccess.Repository.IRepository;
using System.Linq.Expressions;

namespace DataAccess.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly OrderDAOv2 orderDAOv2;

        public OrderRepository(OrderDAOv2 orderDAOv2)
        {
            this.orderDAOv2 = orderDAOv2;
        }

        //---------v2 DAOv2----------//
        public async Task CreateAsync(Order entity)
        {
            await orderDAOv2.CreateAsync(entity);
        }


        public async Task<List<Order>> GetAllAsync(Expression<Func<Order, bool>>? filter = null, string? includeProperties = null,
            int pageSize = 0, int pageNumber = 1)
        {
            return await orderDAOv2.GetAllAsync(filter: filter, pageSize: pageSize, pageNumber: pageNumber, includeProperties: includeProperties);
        }


        public async Task RemoveAsync(Order entity)
        {
            await orderDAOv2.RemoveAsync(entity);
        }

        public async Task UpdateAsync(Order entity)
        {
            await orderDAOv2.UpdateAsync(entity);
        }

        public async Task<Order> GetAsync(Expression<Func<Order, bool>> filter = null, bool tracked = true, string includeProperties = null)
        {
            return await orderDAOv2.GetAsync(filter, tracked, includeProperties);
        }

        async Task<Order> IOrderRepository.UpdateAsync(Order entity)
        {
            return await orderDAOv2.UpdateAsync(entity);
        }

        //public Task SaveAsync()
        //{
        //    throw new NotImplementedException();
        //}
    }
}
