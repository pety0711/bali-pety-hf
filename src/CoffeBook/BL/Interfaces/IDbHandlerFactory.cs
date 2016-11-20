using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Interfaces
{
    interface IDbHandlerFactory<DbHandler> where DbHandler : IDbHandler
    {
        DbHandler GetDbHandler();
    }
}
