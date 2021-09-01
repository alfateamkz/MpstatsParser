using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.IO;
using RestSharp;
using MpstatsParser.Exceptions;
using MpstatsParser.Models.API;


namespace MpstatsParser.Services
{
    public static class MpstatsAPI
    {
        static RestRequest request;
        static IRestResponse response;
        static JsonSerializer serializer = new JsonSerializer();
        public static string APIKey { get; set; }

    

        public static List<SubcategoryModel> GetSubcategoryInfo(string path,DateTime d1,DateTime d2)
        {
            if (!CheckNet())
            {
                throw new NoInternetConnectionException();
            }

            if (!string.IsNullOrEmpty(APIKey))
            {
                RestClient client = new RestClient("https://mpstats.io/api/wb/get/category/subcategories" +
                   $"?d1={d1.ToString("yyyy-MM-dd")}&d1={d2.ToString("yyyy-MM-dd")}" +
                   $"&path={path}");
                request = new RestRequest(Method.GET);
                request.AddHeader("Accept", "application/json");
                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("X-Mpstats-TOKEN", APIKey);
                response = client.Execute(request);

                StatusCodeChecker.CheckStatusCode((int)response.StatusCode);
                while (((int)response.StatusCode) == 202)
                {         
                    response = client.Execute(request);
                }
                dynamic resp = response.Content;
                List<SubcategoryModel> items = new List<SubcategoryModel>();
                // foreach (var i in resp.)


                System.Windows.MessageBox.Show(response.Content);
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
                   $"?d1={d1.ToString("yyyy-MM-dd")}&d1={d2.ToString("yyyy-MM-dd")}" +
                   $"&path={path}");
                request = new RestRequest(Method.GET);
                request.AddHeader("Accept", "application/json");
                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("X-Mpstats-TOKEN", APIKey);
                response = client.Execute(request);

                StatusCodeChecker.CheckStatusCode((int)response.StatusCode);
                while (((int)response.StatusCode) == 202)
                {
                    response = client.Execute(request);
                }
                dynamic resp = response.Content;
                List<CategorySellerModel> items = new List<CategorySellerModel>();
                // foreach (var i in resp.)


                System.Windows.MessageBox.Show(response.Content);
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
                RestClient client = new RestClient("https://mpstats.io/api/wb/get/category/sellers" +
                   $"?d1={d1.ToString("yyyy-MM-dd")}&d1={d2.ToString("yyyy-MM-dd")}" +
                   $"&path={path}");
                request = new RestRequest(Method.GET);
                request.AddHeader("Accept", "application/json");
                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("X-Mpstats-TOKEN", APIKey);
                
                response = client.Execute(request);

                StatusCodeChecker.CheckStatusCode((int)response.StatusCode);
                while (((int)response.StatusCode) == 202)
                {
                    response = client.Execute(request);
                }
                dynamic resp = response.Content;
                List<CategoryProductModel> items = new List<CategoryProductModel>();
                // foreach (var i in resp.)


                System.Windows.MessageBox.Show(response.Content);
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
