using BusinessObject;
using BusinessObject.Object;
using DataAccess.GenericDAO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DataAccess.Dao
{
    public class OrderDetailDAOv2 : GenericDAO<OrderDetail>
    {
        private readonly ApplicationDbContext _db;
        public OrderDetailDAOv2(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<OrderDetail> UpdateAsync(OrderDetail entity)
        {
            _db.OrderDetails.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task RemoveAllAsync(List<OrderDetail> listEntity)
        {
            _db.OrderDetails.RemoveRange(listEntity);
            await _db.SaveChangesAsync();
        }

    }

}
