using BusinessObject.Object;
using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class CategoryDAO
    {
        private static CategoryDAO instance;
        private static readonly object instanceLock = new object();
        private CategoryDAO() { }
        public static CategoryDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new CategoryDAO();
                    }
                    return instance;
                }
            }
        }
        public Category GetCategoryById(int id)
        {
            try
            {
                using ApplicationDbContext context = new ApplicationDbContext();
                return context.Categories.Find(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error GetCategoryById");
                throw new Exception(ex.Message);
            }
        }
        public void DeleteCategory(Category Category)
        {
            try
            {
                using ApplicationDbContext context = new ApplicationDbContext();
                context.Categories.Remove(Category);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error DeleteCategory");
                throw new Exception(ex.Message);
            }
        }

        public IEnumerable<Category> GetCategories()
        {
            List<Category> Categories;
            try
            {
                using ApplicationDbContext context = new ApplicationDbContext();
                Categories = context.Categories.ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error GetCategories");
                throw new Exception(ex.Message);
            }
            return Categories;
        }

        public void InsertCategory(Category Category)
        {
            try
            {
                using ApplicationDbContext context = new ApplicationDbContext();
                context.Categories.Add(Category);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error InsertCategory");
                throw new Exception(ex.Message);
            }
        }

        public void UpdateCategory(Category Category)
        {
            try
            {
                using ApplicationDbContext context = new ApplicationDbContext();
                context.Entry<Category>(Category).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error UpdateCategory");
                throw new Exception(ex.Message);
            }
        }
    }
}
