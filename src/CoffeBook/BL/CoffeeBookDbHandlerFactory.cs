using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.Interfaces;

namespace BL
{
    public class CoffeeBookDbHandlerFactory : IDbHandlerFactory<CoffeeBookDbHandler>
    {
        public CoffeeBookDbHandler GetDbHandler()
        {
            return new CoffeeBookDbHandler();
        }
    }
}
