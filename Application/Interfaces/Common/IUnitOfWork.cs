using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Common
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync();

        Task ExecuteInTransactionAsync(Func<Task> action, CancellationToken cancellationToken = default);
    }


}
