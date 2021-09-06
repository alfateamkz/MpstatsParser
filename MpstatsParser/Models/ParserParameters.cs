using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using MpstatsParser.Models.API;
using MpstatsParser.Models.Excel;
using System.IO;
using System.Threading.Tasks;

namespace MpstatsParser.Models
{
    public static class ParserParameters
    {
        static JsonSerializer serializer = new JsonSerializer();
        public static string FilePath = Path.Combine(Environment.CurrentDirectory, "params.json");
        public static ThisParameters Params { get; set; } = new ThisParameters();
        public static ThisParameters GetParserParameters()
        {
            return Params;
        }

        public static void SaveParameters()
        {
            Services.MpstatsAPI.APIKey = Params.APIKey;
            using (StreamWriter sw = new StreamWriter(FilePath))
            {
                using (JsonWriter writer = new JsonTextWriter(sw))
                {
                    serializer.Serialize(writer, Params);
                }
            }
        }
        public static void RestoreParameters()
        {
            if (File.Exists(FilePath))
            {
                using (StreamReader file = File.OpenText(FilePath))
                {
                    Params = (ThisParameters)serializer.Deserialize(file, typeof(ThisParameters));
                }
                Services.MpstatsAPI.APIKey = Params.APIKey;
            }
        }

        public static async Task ResetParserParameters()
        {
            ParserParameters.Params.CurrentCategoryIndex = -1;
            ParserParameters.Params.IsSuspended = false;
            ParserParameters.Params.IsStarted = false;

            ParserParameters.Params.Categories = new List<SubcategoryModel>();
            ParserParameters.Params.ExcelReportRows = new List<ExcelRowModel>();
            ParserParameters.Params.GetCategoriesIteration = default;
            ParserParameters.Params.GetSubcategoryInfoIteration = default;
            ParserParameters.Params.IsCategoriesGot = default;
            ParserParameters.Params.IsSubcategoryInfoGot = default;
            ParserParameters.Params.Rubricator = new List<RubricatorItemModel>();
        }

        public class ThisParameters
        {
            //Настройка парсера в интерфейсе программы
            public string FileResultPath { get; set; }
            public string APIKey { get; set; }
            public double SKUPriceFrom { get; set; }

            //Промежуточная информация
            public List<RubricatorItemModel> Rubricator { get; set;}
            public List<SubcategoryModel> Categories { get; set; }
            public List<ExcelRowModel> ExcelReportRows { get; set; }
   

            //Управление работой парсера
            public bool IsStarted { get; set; }
            public bool IsSuspended { get; set; }
            

            //Переменнные для возобновления работы парсера при повторном запуске программы
            public int GetCategoriesIteration { get; set; }
            public bool IsCategoriesGot { get; set; }
            public int GetSubcategoryInfoIteration { get; set; }
            public bool IsSubcategoryInfoGot { get; set; }

            [Obsolete]
            public int CurrentCategoryIndex { get; set; }

        }
    }
}
