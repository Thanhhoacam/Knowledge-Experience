using BusinessObject;
using BusinessObject.Object;

namespace DataAccess
{
    public class OrderDetailDAO
    {
        private static OrderDetailDAO instance=null;
        private static readonly object instanceLock = new object();
        

        private OrderDetailDAO() { }
        public static OrderDetailDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new OrderDetailDAO();
                    }
                    return instance;
                }
            }
        }

        public void DeleteOrderDetail(OrderDetail orderDetail)
        {
            try
            {
                using ApplicationDbContext context = new ApplicationDbContext();
                context.OrderDetails.Remove(orderDetail);
                context.SaveChanges();

            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }



        public IEnumerable<OrderDetail> GetOrderDetails()
        {
            List<OrderDetail> orderDetails = new List<OrderDetail>();
            try
            {
                using ApplicationDbContext context = new ApplicationDbContext();
                orderDetails = context.OrderDetails.ToList();
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            return orderDetails;
        }

        public void InsertOrderDetail(OrderDetail orderDetail)
        {
            try
            {
                using ApplicationDbContext context = new ApplicationDbContext();
                context.OrderDetails.Add(orderDetail);
                context.SaveChanges();
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public void UpdateOrderDetail(OrderDetail orderDetail)
        {
            try
            {
                using ApplicationDbContext context = new ApplicationDbContext();
                context.Entry<OrderDetail>(orderDetail).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                context.SaveChanges();
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
    }
}
