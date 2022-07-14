using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dedy.Models;
using Dedy.ViewModels;

namespace Dedy.Models
{
    public class ListProductView
    {
        public ListProductView(List<Product> En, PagingInfo Pa, Filter fil)
        {
            Entryes = En;
            PageINFO = Pa;
            filter = fil;
        }
        public List<Product> Entryes { set; get; }

        public PagingInfo PageINFO { set; get; }
        public Filter filter { set; get; }
    }
}
