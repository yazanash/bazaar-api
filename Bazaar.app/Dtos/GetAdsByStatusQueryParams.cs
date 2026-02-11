using Bazaar.Entityframework.Models;

namespace Bazaar.app.Dtos
{
    public class GetAdsByStatusQueryParams
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public PubStatus PubStatus { get; set; } = PubStatus.Pending;
    }
}
