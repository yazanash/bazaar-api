namespace Bazaar.app.Dtos
{
    public class VehicleImageRequest
    {
        public int Id { get; set; }
        public string ImagePath { get; set; } = string.Empty;
        public int Order { get; set; }
    }
}
