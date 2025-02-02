using BusinessObject;
using BusinessObject.Object;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class OrderDAO
    {
        private static OrderDAO instance = null;
        private static readonly object instanceLock = new object();

        public OrderDAO()
        {
        }

        public static OrderDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new OrderDAO();
                    }
                    return instance;
                }
            }
        }

        public void DeleteOrder(Order order)
        {
            try
            {
                using ApplicationDbContext context = new ApplicationDbContext();
                context.Orders.Remove(order);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public IEnumerable<Order> GetOrders()
        {
            List<Order> orders;
            try
            {
                using ApplicationDbContext context = new ApplicationDbContext();
                orders = context.Orders.Include(o=>o.OrderDetails ).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return orders;
        }

        public void InsertOrder(Order order)
        {
            try
            {
                Console.WriteLine("InsertOrder");
                using ApplicationDbContext context = new ApplicationDbContext();
                context.Orders.Add(order);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error InsertOrder");
                throw new Exception(ex.Message);
            }
        }

        public void UpdateOrder(Order order)
        {
            try
            {
                using ApplicationDbContext context = new ApplicationDbContext();
                context.Entry<Order>(order).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error UpdateOrder");
                throw new Exception(ex.Message);
            }
        }

    }
}
