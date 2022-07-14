using Dedy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dedy.ViewModel
{
    public class CatalogViewModel
    {
        public FilterViewModel Filter { get; set; }
        public IEnumerable<Product> Products { get; set; }
    }
}
