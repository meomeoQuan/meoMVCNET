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
    public class ProductReposition : Repository<Product>,IProductRepository
    {
        private ApplicationDbContext _db;
       
        public ProductReposition(ApplicationDbContext db) : base(db)
        {
            _db = db;
           
        }

        public void Update(Product entity)
        {
            _db.Update(entity);
        }
    }
}
