using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bazaar.Entityframework.Models
{
    public class City
    {
        public int Id { get; set; }
        public string? ArabicName { get; set; }
        public string? EnglishName { get; set; }
    }
}
