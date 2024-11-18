using FreelancerPlatform.Application.Abstraction.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreelancerPlatform.Infratructure.Entityframework.Implementions.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Cancel()
        {
            _context.ChangeTracker.Clear();
        }

        public async Task<int> SaveChangeAsync()
        {
            //try
            //  {
            return await _context.SaveChangesAsync();
            //     }
            //    catch (Exception ex) { }
            //    return 1;
        }
    }
}
