using BusinessObject;
using BusinessObject.Object;
using DataAccess.GenericDAO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DataAccess.Dao
{
    public class ProductDAOv2 : GenericDAO<Product>
    {
        private readonly ApplicationDbContext _db;
        public ProductDAOv2(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<Product> UpdateAsync(Product entity)
        {
            _db.Products.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

    }

}
