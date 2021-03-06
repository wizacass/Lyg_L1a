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

        private const decimal ContractPrice = 300;
        private const float InterestRate = 0.03f;

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
            decimal fullPrice = realPrice * (decimal)Math.Pow((1 + InterestRate), Period) + ContractPrice;
            decimal monthlyPayment = fullPrice / Period;

            return Math.Round(monthlyPayment);
        }

        public int CompareTo(object obj)
        {
            var other = obj as Car;
            return ComputedValue().CompareTo(other.ComputedValue());
        }

        public string ToTableRow()
        {
            return $"| {Model,-6} | {Price,8} | {Period,2} | {InitialPayment,8} | {ComputedValue(),7} |";
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
