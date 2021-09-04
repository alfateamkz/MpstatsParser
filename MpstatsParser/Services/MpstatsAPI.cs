using MpstatsParser.Exceptions;
using MpstatsParser.Models.API;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;

namespace MpstatsParser.Services
{
    public static class MpstatsAPI
    {
        static RestRequest request;
        static IRestResponse response;
        static JsonSerializer serializer = new JsonSerializer();
        public static string APIKey { get; set; }

        public static List<RubricatorItemModel> GetRubricator()
        {
            if (!CheckNet())
            {
                throw new NoInternetConnectionException();
            }

            if (!string.IsNullOrEmpty(APIKey))
            {
                RestClient client = new RestClient("https://mpstats.io/api/wb/get/categories");
                request = new RestRequest(Method.GET);
                request.AddHeader("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*" +
                    "/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
                request.AddHeader("Content-Type", "application/json; charset=utf-8");
                request.AddHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 " +
                    "(KHTML, like Gecko) Chrome/92.0.4515.159 Safari/537.36 OPR/78.0.4093.184 (Edition utorrent)");
                request.AddHeader("X-Mpstats-TOKEN", APIKey);

              //  request.AddParameter("application/json; charser=utf-8", JsonConvert.SerializeObject(new { startRow = 1, endRow = 2}), ParameterType.RequestBody);
                response = client.Execute(request);
                System.Windows.MessageBox.Show(response.Content.ToString());
                StatusCodeChecker.CheckStatusCode((int)response.StatusCode);
         
                    while (((int)response.StatusCode) == 202)
                {
                    response = client.Execute(request);
                }
               
                dynamic resp = JsonConvert.DeserializeObject(response.Content);
                List<RubricatorItemModel> items = new List<RubricatorItemModel>();
                foreach (var i in resp)
                {
                    RubricatorItemModel item = new RubricatorItemModel
                    {
                        Name = i.name,
                        Path = i.path,
                        Url = i.url,
                    };
                    items.Add(item);
                }
         
                return items;
            }
            else
            {
                throw new NoAPIKeyException();
            }
        }

        public static List<SubcategoryModel> GetSubcategoryInfo(string path, DateTime d1, DateTime d2)
        {
            if (!CheckNet())
            {
                throw new NoInternetConnectionException();
            }

            if (!string.IsNullOrEmpty(APIKey))
            {
                RestClient client = new RestClient("https://mpstats.io/api/wb/get/category/subcategories" +
                   $"?d1={d1.ToString("yyyy-MM-dd")}&d2={d2.ToString("yyyy-MM-dd")}" +
                   $"&path={path}");
                request = new RestRequest(Method.GET);
                request.AddHeader("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*" +
                    "/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
                request.AddHeader("Content-Type", "application/json; charset=utf-8");
                request.AddHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 " +
                    "(KHTML, like Gecko) Chrome/92.0.4515.159 Safari/537.36 OPR/78.0.4093.184 (Edition utorrent)");
                request.AddHeader("X-Mpstats-TOKEN", APIKey);
                // request.AddHeader("content-length", "196");
                response = client.Execute(request);

                StatusCodeChecker.CheckStatusCode((int)response.StatusCode);
                while (((int)response.StatusCode) == 202)
                {
                    response = client.Execute(request);
                }
                List<SubcategoryModel> items = new List<SubcategoryModel>();
                try
                {
                    dynamic resp = JsonConvert.DeserializeObject(response.Content);

                    bool isFirstElem = true;
                    foreach (var i in resp)
                    {
                        if (isFirstElem) { isFirstElem = false; continue;  }
                        SubcategoryModel item = new SubcategoryModel
                        {
                            Name = i.name,
                            ItemsNumber = (int?)i.items,
                            CommentsAverage = (double?)i.comments,
                            Rating = (double?)i.rating,
                            Revenue = (double?)i.revenue,
                            SalesNumber = (int?)i.sales
                        };
                        items.Add(item);
                    }
                }
                catch
                {

                }
                
                // System.Windows.MessageBox.Show(response.StatusCode.ToString());
                return items;
            }
            else
            {
                throw new NoAPIKeyException();
            }
        }

        public static List<CategorySellerModel> GetCategorySellers(string path, DateTime d1, DateTime d2)
        {
            if (!CheckNet())
            {
                throw new NoInternetConnectionException();
            }

            if (!string.IsNullOrEmpty(APIKey))
            {
                RestClient client = new RestClient("https://mpstats.io/api/wb/get/category/sellers" +
                   $"?d1={d1.ToString("yyyy-MM-dd")}&d2={d2.ToString("yyyy-MM-dd")}" +
                   $"&path={path}");
                request = new RestRequest(Method.GET);
                request.AddHeader("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*" +
                        "/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
                request.AddHeader("Content-Type", "application/json; charset=utf-8");
                request.AddHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 " +
                    "(KHTML, like Gecko) Chrome/92.0.4515.159 Safari/537.36 OPR/78.0.4093.184 (Edition utorrent)");
                request.AddHeader("X-Mpstats-TOKEN", APIKey);
                response = client.Execute(request);

                StatusCodeChecker.CheckStatusCode((int)response.StatusCode);
                while (((int)response.StatusCode) == 202)
                {
                    response = client.Execute(request);
                }
                dynamic resp = JsonConvert.DeserializeObject(response.Content);
                List<CategorySellerModel> items = new List<CategorySellerModel>();
                foreach (var i in resp)
                {
                    CategorySellerModel item = new CategorySellerModel
                    {
                        Name = i.name,
                        CommentsAverage = i.comments,
                        ItemsNumber = i.items,
                        SalesNumber = i.sales,
                        Position = i.position,
                        Rating = i.rating,
                        Revenue = i.revenue
                    };
                    items.Add(item);
                }
                //      System.Windows.MessageBox.Show(response.StatusCode.ToString());
                return items;
            }
            else
            {
                throw new NoAPIKeyException();
            }
        }

        public static List<CategoryProductModel> GetCategoryProducts(string path, DateTime d1, DateTime d2)
        {
            if (!CheckNet())
            {
                throw new NoInternetConnectionException();
            }

            if (!string.IsNullOrEmpty(APIKey))
            {
                RestClient client = new RestClient("https://mpstats.io/api/wb/get/category" +
                   $"?d1={d1.ToString("yyyy-MM-dd")}&d2={d2.ToString("yyyy-MM-dd")}" +
                   $"&path={path}");
                request = new RestRequest(Method.GET);
                request.AddHeader("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*" +
               "/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
                request.AddHeader("Content-Type", "application/json; charset=utf-8");
                request.AddHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 " +
                    "(KHTML, like Gecko) Chrome/92.0.4515.159 Safari/537.36 OPR/78.0.4093.184 (Edition utorrent)");
                request.AddHeader("X-Mpstats-TOKEN", APIKey);

                response = client.Execute(request);

                StatusCodeChecker.CheckStatusCode((int)response.StatusCode);
                while (((int)response.StatusCode) == 202)
                {
                    response = client.Execute(request);
                }
                dynamic resp = response.Content;
                List<CategoryProductModel> items = new List<CategoryProductModel>();
                foreach (var i in resp.data)
                {
                    CategoryProductModel item = new CategoryProductModel
                    {
                        Id = i.id,
                        Name = i.name,
                        Brand = i.brand,
                        Seller = i.seller,
                        Category = i.category,
                        Color = i.color,
                        Balance = i.balance,
                        Comments = i.comments,
                        Rating = i.rating,
                        FinalPrice = i.final_price,
                        FinalPriceMax = i.final_price_max,
                        FinalPriceMin = i.final_price_min,
                        FinalPriceAverage = i.final_price_average,
                        BasicSale = i.basic_sale,
                        BasicPrice = i.basic_price,
                        Sales = i.sales,
                        Revenue = i.revenue,
                        RevenuePotential = i.revenue_potential,
                        LostProfit = i.lost_profit,
                        DaysInStock = i.days_in_stock,
                        DaysWithSales = i.days_with_sales,
                        AverageIfInStock = i.average_if_in_stock,
                        Thumb = i.thumb,
                        ClientPrice = i.client_price,
                        ClientSale = i.client_sale,
                        PromoSale = i.promo_sale,
                        StartPrice = i.start_price
                    };
                }
                //     System.Windows.MessageBox.Show(response.StatusCode.ToString());
                return items;
            }
            else
            {
                throw new NoAPIKeyException();
            }
        }












        private static bool CheckNet()
        {
            bool stats;
            if (System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable() == true)
            {
                stats = true;
            }
            else
            {
                stats = false;
            }
            return stats;
        }
    }
}
