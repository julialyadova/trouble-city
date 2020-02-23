using System;
using System.IO;
using System.Text;

namespace trouble_city
{
    [Serializable]
    class VisualizedPrefab
    {
        public string Name = "fi";
        public string PNGName;
        public int MinSize;
        public int MaxSize;
        public int Speed;
        public int Destiny;

        public static VisualizedPrefab[] LoadFromJSON(string fileName)
        {
            var json = File.ReadAllText(@"../../Prefabs/" + fileName + ".json");
            Console.WriteLine("success");
            return Json.Net.JsonNet.Deserialize<VisualizedPrefab[]>(json);
        }
    }
}
