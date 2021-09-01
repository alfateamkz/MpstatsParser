using System;
using System.Collections.Generic;
using System.Text;

namespace MpstatsParser.Models.API
{
    public class CategoryProductModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Brand { get; set; }
        public string Seller { get; set; }
        public string Category { get; set; }
        public string Color { get; set; }
        public int Balance { get; set; }
        public int Comments { get; set; }
        public double Rating { get; set; }
        public double FinalPrice { get; set; }
        public double FinalPriceMax { get; set; }
        public double FinalPriceMin { get; set; }
        public double FinalPriceAverage { get; set; }
        public double BasicSale { get; set; }
        public double BasicPrice { get; set; }
        public double PromoSale { get; set; }
        public double ClientSale { get; set; }
        public double ClientPrice { get; set; }
        public double StartPrice { get; set; }
        public int Sales { get; set; }
        public double Revenue { get; set; }
        public double RevenuePotential { get; set; }
        public double LostProfit { get; set; }
        public int DaysInStock { get; set; }
        public int DaysWithSales { get; set; }
        public double AverageIfInStock { get; set; }
        public string Thumb { get; set; }

        public override string ToString()
        {
            return $"{Name}  Продажи на {Revenue} рублей";
        }
    }
}
