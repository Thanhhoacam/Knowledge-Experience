using BusinessObject;
using BusinessObject.Object;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DataAccess
{
    public class ProductDAO
    {
        private static ProductDAO instance;
        private static readonly object instanceLock = new object();
        private ProductDAO() { }
        public static ProductDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new ProductDAO();
                    }
                    return instance;
                }
            }
        }
        //----------------------------------CRUD V1----------------------------------------------

        public Product GetProductById(int id)
        {
            try
            {
                using ApplicationDbContext context = new ApplicationDbContext();
                return context.Products.Find(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error GetProductById");
                throw new Exception(ex.Message);
            }
        }
        public void DeleteProduct(Product product)
        {
            try
            {
                using ApplicationDbContext context = new ApplicationDbContext();
                context.Products.Remove(product);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error DeleteProduct");
                throw new Exception(ex.Message);
            }
        }

        public IEnumerable<Product> GetProducts()
        {
            List<Product> products;
            try
            {
                using ApplicationDbContext context = new ApplicationDbContext();
                products = context.Products.Include(p=>p.Category).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error GetProducts");
                throw new Exception(ex.Message);
            }
            return products;
        }

        public void InsertProduct(Product product)
        {
            try
            {
                using ApplicationDbContext context = new ApplicationDbContext();
                context.Products.Add(product);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error InsertProduct");
                throw new Exception(ex.Message);
            }
        }

        public void UpdateProduct(Product product)
        {
            try
            {
                using ApplicationDbContext context = new ApplicationDbContext();
                context.Entry<Product>(product).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error UpdateProduct");
                throw new Exception(ex.Message);
            }
        }

        //----------------------------------CRUD V2 async----------------------------------------------

        public async Task CreateAsync(Product entity)
        {
            try
            {
                using ApplicationDbContext context = new ApplicationDbContext();
                await context.Products.AddAsync(entity);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n\n---------------------------------Error CreateAsync() ProductDAO--------------------------------------\n\n");
                throw new Exception(ex.Message);
            }
        }

        public async Task UpdateAsync(Product entity)
        {
            try
            {
                using ApplicationDbContext context = new ApplicationDbContext();
                await context.Products.AddAsync(entity);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n\n---------------------------------Error UpdateAsync() ProductDAO--------------------------------------\n\n");
                throw new Exception(ex.Message);
            }
        }

        public async Task RemoveAsync(Product entity)
        {
            try
            {
                using ApplicationDbContext context = new ApplicationDbContext();
                context.Products.Remove(entity);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n\n---------------------------------Error RemoveAsync() ProductDAO--------------------------------------\n\n");
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<Product>> GetAllAsync(Expression<Func<Product,bool>> filter = null)
        {
           
            try
            {
                using ApplicationDbContext context = new ApplicationDbContext();
                IQueryable<Product> query = context.Products;
                if (filter != null)
                {
                    query = query.Where(filter);
                }
                return await query.ToListAsync(); // at this point, the query is executed, this is deferred execution, to list causes immidiate execution
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n\n---------------------------------Error GetAllAsync() ProductDAO--------------------------------------\n\n");
                throw new Exception(ex.Message);
            }
            
        }

        public async Task<Product> GetAsync(Expression<Func<Product,bool>> filter = null, bool tracked = true)
        {
            try
            {
                using ApplicationDbContext context = new ApplicationDbContext();
                IQueryable<Product> query = context.Products;
                if (!tracked)
                {
                    query = query.AsNoTracking();
                }
                if (filter != null)
                {
                    query = query.Where(filter);
                }
                return await query.FirstOrDefaultAsync(); // at this point, the query is executed, this is deferred execution, to list causes immidiate execution
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n\n---------------------------------Error GetAsync() ProductDAO--------------------------------------\n\n");
                throw new Exception(ex.Message);
            }
        }

        //Nếu singleton thì khi gọi hàm này thì instance bị gì không và có save được không ?
        //public async Task SaveAsync()
        //{
        //    try
        //    {
        //        using ApplicationDbContext context = new ApplicationDbContext();
        //        await context.SaveChangesAsync();
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("\n\n---------------------------------Error SaveAsync() ProductDAO --------------------------------------\n\n");
        //        throw new Exception(ex.Message);
        //    }
        //}
    }
}
