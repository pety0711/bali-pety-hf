using GalaSoft.MvvmLight;

namespace CoffeBook.Model
{
    public class Coffee : ObservableObject
    {
        private long id;

        public long Id
        {
            get { return id; }
            set {
                Set<long>(() => this.Id, ref id, value);
            }
        }

        private string name;

        public string Name
        {
            get { return name; }
            set {
                Set<string>(() => this.Name, ref name, value);
            }
        }

        private byte[] picture;

        public byte[] Picture
        {
            get { return picture; }
            set
            {
                Set<byte[]>(() => this.Picture, ref picture, value);
            }
        }

    }
}