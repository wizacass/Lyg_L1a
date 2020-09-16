using System.IO;
using L1a.Code.Interfaces;
using Newtonsoft.Json;

namespace L1a.code
{
    static class DataManager<T> where T : IParsable, new()
    {
        public static T[] CreateDataset(int count)
        {
            var objects = new T[count];
            for (int i = 0; i < count; i++)
            {
                var obj = new T();
                obj.InitializeRandom();
                objects[i] = obj;
            }
            return objects;
        }

        public static void SerializeArray(T[] objects, string filename)
        {
            string data = JsonConvert.SerializeObject(objects);
            using var writer = new StreamWriter(filename);
            writer.WriteLine(data);
        }

        public static T[] DeserializeArray(string filename)
        {
            string text = File.ReadAllText(filename);
            var data = JsonConvert.DeserializeObject<T[]>(text);
            return data;
        }
    }
}
