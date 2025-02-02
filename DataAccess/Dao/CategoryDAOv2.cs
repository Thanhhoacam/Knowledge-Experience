using BusinessObject;
using BusinessObject.Object;
using DataAccess.GenericDAO;

namespace DataAccess.Dao
{
    public class CategoryDAOv2 : GenericDAO<Category>
    {
        private readonly ApplicationDbContext _db;
        public CategoryDAOv2(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<Category> UpdateAsync(Category entity)
        {
            _db.Categories.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

    }

}
