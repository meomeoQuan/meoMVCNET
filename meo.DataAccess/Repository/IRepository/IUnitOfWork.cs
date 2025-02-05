using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace meo.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        public ICategoryRepository Category { get;  }
        public IProductRepository Product { get; }

        public IShoppingCartRepository ShoppingCart { get; }
        public IApplicationUserRepository ApplicationUser { get; }
        public ICompanyRepository Company { get; }

        public IOrderHeaderRepository OrderHeader { get; }
        public IOrderDetailRepository OrderDetails { get; }
        void Save();
    }
}
