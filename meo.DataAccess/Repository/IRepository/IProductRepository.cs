using meo.Models;





namespace meo.DataAccess.Repository.IRepository
{
    public interface IProductRepository : IRepository<Product>
    {


        void Update(Product entity);
    }
}
