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
        private const int DatasetCount = 1;
        private const int SamplesCount = 50;
        private const decimal Threshold = 5000;

        private Random _rnd;

        public Program()
        {
            _rnd = new Random();
        }

        private void GenerateData()
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

            while (!dataMonitor.IsFull)
            {
                var item = queue.Dequeue();
                try
                {
                    dataMonitor.AddItem(item);
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine(ex.Message);
                    queue.Enqueue(item);
                    //break;
                }
            }

            runner.PrepareThreads(dataMonitor, resultsMonitor, Threshold);
            runner.StartThreads();
            runner.FinishThreads();

            //System.Console.WriteLine($"Monitor is {(dataMonitor.IsEmpty ? "Empty" : "Full")}.");
            //System.Console.WriteLine(cars.Length);
            var sortedItems = resultsMonitor.getItems();

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
                Execute(cars, 2);
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
