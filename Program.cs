using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using L1a.code;
using L1a.Code;
using L1a.Models;

namespace L1a
{
    class Program
    {
        private const string FilenameTemplate = "Data/IFF8-1_PetrauskasV_L1_dat_{0}.json";
        private const int DatasetCount = 3;
        private const int SamplesCount = 50;
        private const decimal Threshold = 5000;

        private Random _rnd;

        public Program()
        {
            _rnd = new Random();
        }

        private void GenerateData()
        {
            string f1 = String.Format(FilenameTemplate, 1);
            string f2 = String.Format(FilenameTemplate, 2);
            string f3 = String.Format(FilenameTemplate, 3);
            var ds1a = DataManager<Car>.CreateDataset(SamplesCount / 2, Threshold, Criteria.LessThan);
            var ds1b = DataManager<Car>.CreateDataset(SamplesCount / 2, Threshold, Criteria.GreaterThan);
            var ds1 = ds1a.Concat(ds1b).OrderBy(x => _rnd.Next()).ToArray();
            var ds2 = DataManager<Car>.CreateDataset(SamplesCount, Threshold, Criteria.LessThan);
            var ds3 = DataManager<Car>.CreateDataset(SamplesCount, Threshold, Criteria.GreaterThan);
            DataManager<Car>.SerializeArray(ds1, f1);
            DataManager<Car>.SerializeArray(ds2, f2);
            DataManager<Car>.SerializeArray(ds3, f3);
        }

        private void GenerateRandomData()
        {
            for (int i = 1; i <= DatasetCount; i++)
            {
                string filename = String.Format(FilenameTemplate, i);
                var objects = DataManager<Car>.CreateDataset(SamplesCount);
                DataManager<Car>.SerializeArray(objects, filename);
            }
        }

        private void Execute(Car[] cars, int threadCount = -1)
        {
            if (threadCount <= 0 || !ValidateThreadCount(threadCount, cars.Length))
            {
                threadCount = _rnd.Next(2, cars.Length / 4);
            }

            System.Console.WriteLine($"Running with {threadCount} threads");

            var queue = new Queue<Car>(cars);
            var dataMonitor = new DataMonitor<Car>(cars.Length / 2);
            var resultsMonitor = new SortedDataMonitor<Car>(cars.Length);
            var runner = new Runner<Car>(threadCount);

            runner.PrepareThreads(dataMonitor, resultsMonitor, Threshold);
            runner.StartThreads();

            while (queue.Count > 0)
            {
                var item = queue.Dequeue();
                try
                {
                    dataMonitor.AddItem(item);
                    if (queue.Count == 0) dataMonitor.IsFinal = true;
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine(ex.Message);
                    queue.Enqueue(item);
                }
            }

            runner.FinishThreads();

            var sortedItems = resultsMonitor.getItems();

            System.Console.WriteLine($"Sorted: {sortedItems.Length}");
            System.Console.WriteLine();
        }

        private bool ValidateThreadCount(int count, int length)
        {
            return count >= 2 && count <= (length / 4);
        }

        private void Run()
        {
            for (int i = 1; i <= DatasetCount; i++)
            {
                string filename = String.Format(FilenameTemplate, i);
                var cars = DataManager<Car>.DeserializeArray(filename);
                Execute(cars);
                System.Console.WriteLine("------------------------------");
            }
        }

        static void Main(string[] args)
        {
            var p = new Program();
            //p.GenerateData();
            p.Run();

            Console.WriteLine("Program finished execution");
        }
    }
}
