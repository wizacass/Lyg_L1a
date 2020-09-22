using System;
using L1a.Code.Interfaces;
using Newtonsoft.Json;

namespace L1a.Models
{
    public class Car : IParsable, IComputable, IComparable
    {
        public string Model { get; set; }
        public decimal Price { get; set; }
        public int Period { get; set; }
        public decimal InitialPayment { get; set; }

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

        public void InitializeRandom()
        {
            var random = new Random();
            Model = $"Car{random.Next(1, 1000)}";
            Price = random.Next(50, 500) * 100;
            Period = random.Next(1, 5);
            InitialPayment = random.Next(0, (int)Price);
        }

        public decimal ComputedValue()
        {
            decimal realPrice = Price - InitialPayment;
            decimal fullPrice = realPrice * (Decimal)Math.Pow((1 + _interestRate), Period) + _contractPrice;
            decimal monthlyPayment = fullPrice / Period;

            return Math.Round(monthlyPayment);
        }

        public int CompareTo(object obj)
        {
            var other = obj as Car;
            return this.ComputedValue().CompareTo(other.ComputedValue());
        }


        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
