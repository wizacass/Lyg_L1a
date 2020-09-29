using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using L1a.code;
using L1a.Code;
using L1a.Models;

namespace L1a
{
    internal class Program
    {
        private const string DataDirectory = "Data";

        private const int DatasetCount = 3;
        private const int SamplesCount = 50;
        private const decimal Threshold = 5000;

        private readonly string _datafileTemplate = $"{DataDirectory}/IFF8-1_PetrauskasV_L1_dat_{0}.json";
        private readonly string _resultsFileTemplate = $"{DataDirectory}/IFF8-1_PetrauskasV_L1_rez_{0}.txt";

        private readonly Random _rnd;

        private Program()
        {
            _rnd = new Random();
        }

        private void GenerateData()
        {
            string f1 = string.Format(_datafileTemplate, 1);
            string f2 = string.Format(_datafileTemplate, 2);
            string f3 = string.Format(_datafileTemplate, 3);
            var ds1A = DataManager<Car>.CreateDataset(SamplesCount / 2, Threshold, Criteria.LessThan);
            var ds1B = DataManager<Car>.CreateDataset(SamplesCount / 2, Threshold, Criteria.GreaterThan);
            var ds1 = ds1A.Concat(ds1B).OrderBy(x => _rnd.Next()).ToArray();
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
                string filename = string.Format(_datafileTemplate, i);
                var objects = DataManager<Car>.CreateDataset(SamplesCount);
                DataManager<Car>.SerializeArray(objects, filename);
            }
        }

        private Car[] Execute(Car[] cars, int threadCount = -1)
        {
            if (threadCount <= 0 || !ValidateThreadCount(threadCount, cars.Length))
            {
                threadCount = _rnd.Next(2, cars.Length / 4);
            }

            Console.WriteLine($"Running with {threadCount} threads");

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
                    Console.WriteLine(ex.Message);
                    queue.Enqueue(item);
                }
            }

            runner.FinishThreads();

            return resultsMonitor.GetItems();
        }

        private static bool ValidateThreadCount(int count, int length)
        {
            return count >= 2 && count <= (length / 4);
        }

        private void Run(int threadCount = -1)
        {
            for (int i = 1; i <= DatasetCount; i++)
            {
                string datafile = string.Format(_datafileTemplate, i);
                string resultsFile = string.Format(_resultsFileTemplate, i);
                var cars = DataManager<Car>.DeserializeArray(datafile);
                var sortedCars = Execute(cars, threadCount);
                string header = $"| {"Model",-6} | {"Price",-8} | P. | {"Initial",-8} | Monthly |";
                DataManager<Car>.ToFile(sortedCars, resultsFile, header);

                Console.WriteLine();
            }
        }

        private static void Main()
        {
            Directory.CreateDirectory(DataDirectory);

            var p = new Program();
            p.GenerateData();
            p.Run();

            Console.WriteLine("Program finished execution");
        }
    }
}
