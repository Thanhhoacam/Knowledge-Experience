using BusinessObject;
using BusinessObject.Object;
using DataAccess.GenericDAO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DataAccess.Dao
{
    public class OrderDAOv2 : GenericDAO<Order>
    {
        private readonly ApplicationDbContext _db;
        public OrderDAOv2(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<Order> UpdateAsync(Order entity)
        {
            _db.Orders.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

    }

}
