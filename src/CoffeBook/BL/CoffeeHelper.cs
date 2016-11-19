using DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.Model;
using System.Collections.ObjectModel;

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

        public static ICollection<Model.Coffee> GetCoffees()
        {
            using (var ctx = new CoffeBookContext())
            {
                var coffes = ctx.Coffes.ToList();
                ObservableCollection<Model.Coffee> cfs = new ObservableCollection<Model.Coffee>();
                foreach (var coffe in coffes)
                {
                    cfs.Add(coffeeConverter(coffe));
                }

                return cfs;
            }
        }

        public static void RemoveCoffee(Coffee coffee)
        {
            throw new NotImplementedException();
        }

        public static void AddCoffee(Coffee coffee)
        {
            throw new NotImplementedException();
        }
    }
}
