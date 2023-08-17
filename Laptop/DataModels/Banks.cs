namespace Laptop.DataModels
{
    public partial class Banks
    {
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Bank { get; set; } = null!;
        public bool? IsActive { get; set; } 
    }
}
