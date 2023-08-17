namespace Laptop.Models
{
    public class EquipmentModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string ManufactureDate { get; set; } = null!;
        public string ManufactureTime { get; set; } = null!;
    }
}
