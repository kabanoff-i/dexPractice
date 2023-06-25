using System;

namespace OOP
{
    public abstract class Car
    {
        public string make { get; set; }
        public string model { get; set; }
        public abstract void ChangeGear();
        public Car(string make, string model)
        {
            this.make = make;
            this.model = model;
        }
    }
    public class ManualTransmission : Car
    {
        private static int n = 0;
        private static string[] gears = new string[] { "N", "1", "2", "3", "4", "5", "R" };
        private string gear = gears[n];
        private void SqueezeClutch()
        {
            Console.WriteLine("Clutch squeezed");

        }
        public override void ChangeGear()
        {
            SqueezeClutch();
            gear = gears[(n + 1) % gears.Length];
            Console.WriteLine("The gear was changed, current gear: " + gear);
        }
        public ManualTransmission(string make, string model) : base(make, model)
        {
        }

    }
    public class AutomaticTransmission : Car
    {
        private static int n = 0;
        private static string[] gears = new string[] { "N", "D", "P", "R" };
        private string gear = gears[n];
        public override void ChangeGear()
        {
            gear = gears[(n + 1) % gears.Length];
            Console.WriteLine("The gear was changed, current gear: " + gear);
        }
        public AutomaticTransmission(string make, string model) : base(make, model)
        {
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            var cars = new Car[] { new ManualTransmission("Honda", "Civic"), new AutomaticTransmission("Lexus", "RX450h") };
            foreach (var car in cars)
            {
                car.ChangeGear();
            }
            Console.ReadKey();
        }
    }
}
