using System;

namespace L1a.Models
{
    public class Car
    {
        public string Model { get; }
        public decimal Price { get; }
        public int Period { get; }
        public decimal InitialPayment { get; }
        public decimal MonthlyPayment => CalculateMonthlyPayment();

        private const decimal _contractPrice = 300;
        private const float _interestRate = 0.03f;

        public Car(string model, decimal fullPrice, int period, decimal initialPayment)
        {
            Model = model;
            Price = fullPrice;
            Period = period;
            InitialPayment = initialPayment;
        }

        private decimal CalculateMonthlyPayment()
        {
            decimal realPrice = Price - InitialPayment;
            decimal fullPrice = realPrice * (Decimal)Math.Pow((1 + _interestRate), Period) + _contractPrice;
            decimal monthlyPayment = fullPrice / Period;

            return Math.Round(monthlyPayment);
        }
    }
}
