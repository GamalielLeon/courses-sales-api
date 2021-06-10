using System.Threading.Tasks;

namespace Domain.Contracts.UnitOfWork
{
    public interface IUnitOfWork
    {
        void Save();
        Task SaveAsync();
    }
}
