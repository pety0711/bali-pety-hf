using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.DTOs;
using CoffeBook.Model;
using BL;
using System.Collections.ObjectModel;

namespace CoffeBook.Helpers
{
    public class CoffeeHelper
    {
        public static async Task<ICollection<Coffee>> GetCoffees()
        {
            var dbHandler = new CoffeeBookDbHandlerFactory().GetDbHandler();
            var coffees = await dbHandler.GetAllCoffeesAsync();
            return ConvertToCoffees(coffees);
        }

        public static void RemoveCoffee(Coffee coffee)
        {
            var dbHandler = new CoffeeBookDbHandlerFactory().GetDbHandler();
            dbHandler.DeleteCoffeeAsync(coffee.Id);
        }

        public static async Task<Coffee> AddCoffee(Coffee coffee)
        {
            var dbHandler = new CoffeeBookDbHandlerFactory().GetDbHandler();
            var coffeeDto = ConvertFromCoffee(coffee);
            return ConvertToCoffee(await dbHandler.AddCoffeeAsync(coffeeDto));
        }

        private static ICollection<Coffee> ConvertToCoffees(IList<CoffeeDto> coffeeDtos)
        {
            ObservableCollection<Coffee> coffees = new ObservableCollection<Coffee>();
            foreach (var coffeeDto in coffeeDtos)
            {
                coffees.Add(ConvertToCoffee(coffeeDto));
            }
            return coffees;
        }

        public static Coffee ConvertToCoffee(CoffeeDto coffeeType)
        {
            Coffee coffee = new Coffee
            {
                Id = coffeeType.Id,
                Name = coffeeType.Name,
                Picture = coffeeType.Picture
            };
            return coffee;
        }

        public static CoffeeDto ConvertFromCoffee(Coffee coffee)
        {
            CoffeeDto coffeeDto = new CoffeeDto
            {
                Id = coffee.Id,
                Name = coffee.Name,
                Picture = coffee.Picture
            };
            return coffeeDto;
        }
    }
}
