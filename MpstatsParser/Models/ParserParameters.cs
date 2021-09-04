using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using MpstatsParser.Models.API;
using System.IO;

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
        public class ThisParameters
        {
            public string FileResultPath { get; set; }
            public string APIKey { get; set; }
            public double SKUPriceFrom { get; set; }
            public List<RubricatorItemModel> Rubricator { get; set;}
            public List<SubcategoryModel> Categories { get; set; }
            public int CurrentCategoryIndex { get; set; }
            public bool IsStarted { get; set; }
            public bool IsSuspended { get; set; }
        }
    }
}
