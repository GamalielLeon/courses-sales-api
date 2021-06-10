using Domain.Contracts.UnitOfWork;
using Infrastructure.Data;
using System.Threading.Tasks;

namespace Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly OnlineCoursesContext _context;
        public UnitOfWork(OnlineCoursesContext context)
        {
            _context = context;
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
