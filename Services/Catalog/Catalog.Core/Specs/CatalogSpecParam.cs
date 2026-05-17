using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Core.Specs
{
    public class CatalogSpecParam
    {
        private const int MaxPageSize = 80;
        private int _pageSize = 10;
        public int PageIndex { get; set; } = 1;
        public int PageSize
        { 
            get=>_pageSize; 
            set => _pageSize = (value>MaxPageSize)? MaxPageSize : value  ;
        }
        public string? BrabdId { get; set; }
        public string? TypeId { get; set; }
        public string? Sort { get; set; }
        public string? search { get; set; }
        

    }
}
