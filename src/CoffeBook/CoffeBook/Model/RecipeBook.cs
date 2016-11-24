using GalaSoft.MvvmLight;
using System.Collections.Generic;

namespace CoffeBook.Model
{
    public class RecipeBook : ObservableObject
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
        private string description;

        public string Description
        {
            get { return description; }
            set {
                Set<string>(() => this.Description, ref description, value);
            }
        }

        private ICollection<Recipe> recipes = new List<Recipe>();

        public ICollection<Recipe> Recipes
        {
            get { return recipes; }
            set {
                Set<ICollection<Recipe>>(() => this.Recipes, ref recipes, value);
            }
        }
    }
}