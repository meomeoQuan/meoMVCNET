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
    public class OrderDetailReposition : Repository<OrderDetail>, IOrderDetailRepository
    {
        private ApplicationDbContext _db;
       
        public OrderDetailReposition(ApplicationDbContext db) : base(db)
        {
            _db = db;
           
        }

        public void Update(OrderDetail entity)
        {
            _db.Update(entity);
        }
    }
}
