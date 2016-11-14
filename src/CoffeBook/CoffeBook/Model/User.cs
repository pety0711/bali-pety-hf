using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeBook.Model
{
    public class User : ObservableObject
    {
        private long id;

        public long Id
        {
            get { return id; }
            set
            {
                Set<long>(() => this.Id, ref id, value);
            }
        }

        private string name;

        public string Name
        {
            get { return name; }
            set
            {
                Set<string>(() => this.Name, ref name, value);
            }
        }

        private string password;

        public string Password
        {
            get { return password; }
            set
            {
                Set<string>(() => this.Password, ref password, value);
            }
        }

        private ICollection<RecipeBook> recipeBooks;

        public ICollection<RecipeBook> RecipeBooks
        {
            get { return recipeBooks; }
            set
            {
                Set<ICollection<RecipeBook>>(() => this.RecipeBooks, ref recipeBooks, value);
            }
        }
    }
}
