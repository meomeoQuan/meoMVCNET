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
    public class OrderHeaderReposition : Repository<OrderHeader>, IOrderHeaderRepository
    {
        private ApplicationDbContext _db;
       
        public OrderHeaderReposition(ApplicationDbContext db) : base(db)
        {
            _db = db;
           
        }

        public void Update(OrderHeader entity)
        {
            _db.Update(entity);
        }
    }
}
