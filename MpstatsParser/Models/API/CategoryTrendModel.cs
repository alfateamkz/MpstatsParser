using System;
using System.Collections.Generic;
using System.Text;

namespace MpstatsParser.Models.API
{
    public class CategoryTrendModel
    {
        public DateTime Week { get; set; }
        public int? Sales { get; set; }
        public double? Revenue { get; set; }
        public double? ProductRevenue { get; set; }
        public double? ProductWithSells { get; set; }
        public int? Items { get; set; }
        public int? Brands { get; set; }
        public int? Sellers { get; set; }
    }
}
