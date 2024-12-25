using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using meo.DataAccess.Data;
using meo.DataAccess.Repository.IRepository;
using meo.Models;

namespace meo.DataAccess.Repository
{
    public class CategoryReposition : Repository<Category>, ICategoryRepository
    {
        private ApplicationDbContext _db;
       
        public CategoryReposition(ApplicationDbContext db) : base(db)
        {
            _db = db;
           
        }

        public void Update(Category entity)
        {
            _db.Update(entity);
        }
    }
}
