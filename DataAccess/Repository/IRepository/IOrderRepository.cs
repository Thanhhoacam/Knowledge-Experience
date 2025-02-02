using BusinessObject.Object;
using System.Linq.Expressions;

namespace DataAccess.Repository.IRepository
{
    public interface IOrderRepository
    {
        //IEnumerable<Order> GetOrders();

        //void InsertOrder(Order order);
        //void DeleteOrder(Order order);
        //void UpdateOrder(Order order);


        //-----------------v2 async-----------------//

        Task<List<Order>> GetAllAsync(Expression<Func<Order, bool>>? filter = null, string? includeProperties = null,
          int pageSize = 0, int pageNumber = 1);
        Task<Order> GetAsync(Expression<Func<Order, bool>> filter = null, bool tracked = true, string? includeProperties = null);
        Task CreateAsync(Order entity);
        Task RemoveAsync(Order entity);
        Task<Order> UpdateAsync(Order entity);
    }
}
