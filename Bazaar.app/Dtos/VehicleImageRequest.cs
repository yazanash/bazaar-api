namespace Bazaar.app.Dtos
{
    public class VehicleImageRequest
    {
        public int Id { get; set; }
        public string ImageUrl{ get; set; } = string.Empty;
        public int Order { get; set; }
    }
}
