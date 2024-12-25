using meo.Models;





namespace meo.DataAccess.Repository.IRepository
{
    public interface ICategoryRepository : IRepository<Category>
    {


        void Update(Category entity);
    }
}
