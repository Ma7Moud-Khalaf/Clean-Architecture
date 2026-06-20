using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Common
{
    public interface ICurrentUserService
    {
        Guid? UserId { get; }
        bool IsAuthenticated { get; }
    }
}
