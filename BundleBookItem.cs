using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ninelivesbooks
{
    internal class BundleBookItem
    {
        public string BookId { get; set; }
        public string TitleId { get; set; }

        public string Title { get; set; }
        public string Author { get; set; }
        public string Condition { get; set; }

        public decimal Price { get; set; }
        public decimal ApproxWeight { get; set; }
    }
}
