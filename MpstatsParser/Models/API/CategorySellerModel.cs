using System;
using System.Collections.Generic;
using System.Text;

namespace MpstatsParser.Models.API
{
    public class CategorySellerModel
    {
        public string Name { get; set; }
        public int? Position { get; set; }
        public int? ItemsNumber { get; set; }
        public int? SalesNumber { get; set; }
        public double? Revenue { get; set; }
        public double? CommentsAverage { get; set; }
        public double? Rating { get; set; }

        public override string ToString()
        {
            return $"{Name}  Место : {Position} ";
        }
    }
}
