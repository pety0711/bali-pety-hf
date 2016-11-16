using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class CoffeeHelper
    {
        public static Model.Coffee coffeeConverter(DB.Entities.Coffee coffee)
        {
            if (coffee == null)
            {
                return null;
            }
            Model.Coffee c = new Model.Coffee
            {
                Id = coffee.Id,
                Name = coffee.Name,
                Picture = coffee.Picture
            };
            return c;
        }
    }
}
