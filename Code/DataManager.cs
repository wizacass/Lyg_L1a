using System.IO;
using System.Linq;
using System.Text;
using L1a.Code.Interfaces;
using Newtonsoft.Json;

namespace L1a.code
{
    internal enum Criteria
    {
        LessThan = -1,
        GreaterThan = 1
    }

    internal static class DataManager<T> where T : IParsable, IComputable, new()
    {
        private static readonly int[] Lenghts = {8, 10, 4, 10, 9};

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

        public static T[] CreateDataset(int count, decimal threshold, Criteria criteria)
        {
            var objects = new T[count];
            for (int i = 0; i < count; i++)
            {
                T obj;
                do
                {
                    obj = new T();
                    obj.InitializeRandom();
                } while (obj.ComputedValue().CompareTo(threshold) != (int) criteria);

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

        public static void ToFile(T[] data, string filename, string header = null)
        {
            using var writer = new StreamWriter(filename);
            if (header != null)
            {
                writer.WriteLine(TableSeparator());
                writer.WriteLine(header);
            }

            writer.WriteLine(TableSeparator());
            if (data.Length == 0)
            {
                int len = Lenghts.Sum() / 2 - 4;
                writer.WriteLine($"|{new string(' ', len + 1)}No elements!{new string(' ', len)}|");
                writer.WriteLine($"+{new string('-', Lenghts.Sum() + Lenghts.Count() - 1)}+");
                return;
            }

            foreach (var item in data)
            {
                writer.WriteLine(item.ToTableRow());
            }

            writer.WriteLine(TableSeparator());
        }

        private static string TableSeparator()
        {
            var sb = new StringBuilder();

            sb.Append('+');
            foreach (int amount in Lenghts)
            {
                sb.Append('-', amount);
                sb.Append('+');
            }

            return sb.ToString();
        }
    }
}
