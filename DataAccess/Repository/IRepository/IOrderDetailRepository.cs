using BusinessObject.Object;
using System.Linq.Expressions;

namespace DataAccess.Repository.IRepository
{
    public interface IOrderDetailRepository
    {
        //IEnumerable<OrderDetail> GetOrderDetails();

        //void InsertOrderDetail(OrderDetail orderDetail);
        //void DeleteOrderDetail(OrderDetail orderDetail);
        //void UpdateOrderDetail(OrderDetail orderDetail);

        //-----------------v2 async-----------------//
        Task<List<OrderDetail>> GetAllAsync(Expression<Func<OrderDetail, bool>>? filter = null, string? includeProperties = null,
          int pageSize = 0, int pageNumber = 1);
        Task<OrderDetail> GetAsync(Expression<Func<OrderDetail, bool>> filter = null, bool tracked = true, string? includeProperties = null);
        Task CreateAsync(OrderDetail entity);
        Task RemoveAsync(OrderDetail entity);
        Task<OrderDetail> UpdateAsync(OrderDetail entity);
        Task RemoveAllAsync(List<OrderDetail> listEntity);
    }
}
