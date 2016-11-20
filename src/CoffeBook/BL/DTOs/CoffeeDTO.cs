using DB.Entities;

namespace BL.DTOs
{
    public class CoffeeDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public byte[] Picture { get; set; }

        public CoffeeDto()
        {
            Id = -1;
        }

        public CoffeeDto(Coffee coffeEntity)
        {
            Id = coffeEntity.Id;
            Name = coffeEntity.Name;
            Picture = coffeEntity.Picture;
        }
    }
}
