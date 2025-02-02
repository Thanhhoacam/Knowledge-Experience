using BusinessObject.Object;
using DataAccess.Dao;
using DataAccess.Repository.IRepository;
using System.Linq.Expressions;

namespace DataAccess.Repository
{
    public class OrderDetailRepository : IOrderDetailRepository
    {
        private readonly OrderDetailDAOv2 orderDetailDAOv2;

        public OrderDetailRepository(OrderDetailDAOv2 orderDetailDAOv2)
        {
            this.orderDetailDAOv2 = orderDetailDAOv2;
        }

        //---------v2 DAOv2----------//
        public async Task CreateAsync(OrderDetail entity)
        {
            await orderDetailDAOv2.CreateAsync(entity);
        }


        public async Task<List<OrderDetail>> GetAllAsync(Expression<Func<OrderDetail, bool>>? filter = null, string? includeProperties = null,
            int pageSize = 0, int pageNumber = 1)
        {
            return await orderDetailDAOv2.GetAllAsync(filter: filter, pageSize: pageSize, pageNumber: pageNumber, includeProperties: includeProperties);
        }


        public async Task RemoveAsync(OrderDetail entity)
        {
            await orderDetailDAOv2.RemoveAsync(entity);
        }

        public async Task RemoveAllAsync(List<OrderDetail> listEntity)
        {
            await orderDetailDAOv2.RemoveAllAsync(listEntity);
        }

        public async Task UpdateAsync(OrderDetail entity)
        {
            await orderDetailDAOv2.UpdateAsync(entity);
        }

        public async Task<OrderDetail> GetAsync(Expression<Func<OrderDetail, bool>> filter = null, bool tracked = true, string includeProperties = null)
        {
            return await orderDetailDAOv2.GetAsync(filter, tracked, includeProperties);
        }

        async Task<OrderDetail> IOrderDetailRepository.UpdateAsync(OrderDetail entity)
        {
            return await orderDetailDAOv2.UpdateAsync(entity);
        }

        //public Task SaveAsync()
        //{
        //    throw new NotImplementedException();
        //}
    }
}
