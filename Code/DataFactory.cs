using System.IO;
using L1a.code.Interfaces;
using Newtonsoft.Json;

namespace L1a.code
{
    static class DataFactory<T> where T : IParsable, new()
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
    }
}
