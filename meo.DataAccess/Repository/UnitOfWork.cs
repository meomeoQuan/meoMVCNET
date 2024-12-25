using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using meo.DataAccess.Data;
using meo.DataAccess.Repository.IRepository;

namespace meo.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        public ICategoryRepository Category {  get; private set; }
        public IProductRepository Product { get; private set; }
        private ApplicationDbContext _db;
        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Category = new CategoryReposition(_db);
            Product = new ProductReposition(_db);
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
