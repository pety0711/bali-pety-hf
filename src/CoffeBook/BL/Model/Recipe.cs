using GalaSoft.MvvmLight;

namespace BL.Model
{
    public class Recipe : ObservableObject
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
        private Coffee coffeType;

        public Coffee CoffeType
        {
            get { return coffeType; }
            set
            {
                Set<Coffee>(() => this.CoffeType, ref coffeType, value);
            }
        }

        private byte[] picture;

        public byte[] Picture
        {
            get { return picture; }
            set { picture = value; }
        }

        private string description;

        public string Description
        {
            get { return description; }
            set { description = value; }
        }

    }
}