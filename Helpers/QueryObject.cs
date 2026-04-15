using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sadkah.Backend.Helpers
{
    public class QueryObject
    {
        public string? Title { get; set; } = null;
        public string? Description { get; set; } = null;
        public string? SortBy { get; set; } = null;
        public bool IsSortDescending { get; set; } = false;
    }
}