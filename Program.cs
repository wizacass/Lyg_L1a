using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using L1a.code;
using L1a.Interfaces;
using L1a.Models;

namespace L1a
{
    class Program
    {
        private const string FilenameTemplate = "Data/IFF8-1_PetrauskasV_L1_dat_{0}.json";
        private const int DatasetCount = 3;
        private const int SamplesCount = 25;

        private Random _rnd;
        private IDataMonitor<Car> _dataMonitor;
        private ISortedResultMonitor<Car> _resultsMonitor;

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

            if (threadCount < 0 || !ValidateThreadCount(threadCount, cars.Length))
            {
                threadCount = _rnd.Next(2, cars.Length / 4);
            }

            System.Console.WriteLine($"Running with {threadCount} threads");
            System.Console.WriteLine(cars.Length);
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
            }
        }

        static void Main(string[] args)
        {
            var p = new Program();
            //p.GenerateData();
            p.Run();

            Console.WriteLine("Hello World!");
        }
    }
}
