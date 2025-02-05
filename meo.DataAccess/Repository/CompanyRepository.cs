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
    public class CompanyRepository : Repository<Company>, ICompanyRepository
    {
        private readonly ApplicationDbContext _db;
        public CompanyRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Company company)
        {
            _db.Update<Company>(company);
        }
    }
}
