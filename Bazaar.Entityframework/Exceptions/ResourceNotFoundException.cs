using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bazaar.Entityframework.Exceptions
{
    public class ResourceNotFoundException : Exception
    {
        public int? ResourceId { get; }

        public ResourceNotFoundException() : base("The requested resource was not found.") { }

        public ResourceNotFoundException(string message) : base(message) { }

        public ResourceNotFoundException(string message, Exception innerException)
            : base(message, innerException) { }

        public ResourceNotFoundException(int id, string message) : base(message)
        {
            ResourceId = id;
        }
    }
}
