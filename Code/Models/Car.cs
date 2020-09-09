using System;
using L1a.code.Interfaces;
using Newtonsoft.Json;

namespace L1a.Models
{
    public class Car : IParsable
    {
        public string Model { get; private set; }
        public decimal Price { get; private set; }
        public int Period { get; private set; }
        public decimal InitialPayment { get; private set; }
        //public decimal MonthlyPayment => CalculateMonthlyPayment();

        private const decimal _contractPrice = 300;
        private const float _interestRate = 0.03f;

        public Car() { }

        public Car(string model, decimal fullPrice, int period, decimal initialPayment)
        {
            Model = model;
            Price = fullPrice;
            Period = period;
            InitialPayment = initialPayment;
        }

        public decimal CalculateMonthlyPayment()
        {
            decimal realPrice = Price - InitialPayment;
            decimal fullPrice = realPrice * (Decimal)Math.Pow((1 + _interestRate), Period) + _contractPrice;
            decimal monthlyPayment = fullPrice / Period;

            return Math.Round(monthlyPayment);
        }

        public void InitializeRandom()
        {
            var random = new Random();
            Model = $"Car{random.Next(1, 1000)}";
            Price = random.Next(50, 500) * 100;
            Period = random.Next(1, 5);
            InitialPayment = random.Next(0, (int)Price);
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
