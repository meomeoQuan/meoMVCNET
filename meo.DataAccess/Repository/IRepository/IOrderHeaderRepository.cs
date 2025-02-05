using meo.Models;





namespace meo.DataAccess.Repository.IRepository
{
    public interface IOrderHeaderRepository : IRepository<OrderHeader>
    {


        void Update(OrderHeader entity);
    }
}
