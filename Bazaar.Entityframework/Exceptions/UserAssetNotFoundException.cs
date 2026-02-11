using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bazaar.Entityframework.Exceptions
{
    public class UserAssetNotFoundException : Exception
    {
        public string? UserId { get; }

        public UserAssetNotFoundException() : base("The requested assets was not found.") { }

        public UserAssetNotFoundException(string message) : base(message) { }

        public UserAssetNotFoundException(string message, Exception innerException)
            : base(message, innerException) { }

        public UserAssetNotFoundException(string id, string message) : base(message)
        {
            UserId = id;
        }
    }
}
