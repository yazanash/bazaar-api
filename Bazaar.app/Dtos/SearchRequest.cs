using Bazaar.Entityframework.Filters;

namespace Bazaar.app.Dtos
{
    public class SearchRequest
    {
        public GeneralFilter General { get; set; } = new();
        public ISpecsFilter? Specs { get; set; }
        public int pageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}
