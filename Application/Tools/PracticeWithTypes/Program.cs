using Models;

namespace PracticeWithTypes
{
    class Program
    {
        static void UpdateContract(Employee employee)
        {
            employee.Contract = $"Name: {employee.Name}\nSurname: {employee.Surname}\n Date of birth: {employee.DateOfBirth.ToString("yyyy/mm/dd")}\n\n Job title: {employee.JobTitle}\n Salary: {employee.Salary}\n Date of hire: {employee.DateOfHire.ToString("yyyy/mm/dd")}\n";
            Console.WriteLine("Contract was successfully updated\n\n" + employee.Contract);
        }
        static void ChangeExchangeRate(Currency currency)
        {
            Console.WriteLine("Enter new rate");
            double rate = double.Parse(Console.ReadLine());
            currency.exchangeRateToUSD = rate;
            Console.WriteLine($"Currency Information:\nName: {currency.name}\nSymbol: {currency.symbol}\nCode: {currency.code}\nCountry:{currency.country}\nExhange rate to USD:{currency.exchangeRateToUSD}");
        }

        //static void Checkspeed()
        //{
        //    Stopwatch sw = new Stopwatch();
        //    int[] a = new int[10000];
        //    for (int i = 0; i < a.Length; i++)
        //    {
        //        a[i] = new Random().Next(100);
        //    }
        //    object[] b = new object[a.Length];
        //    sw.Start();
        //    for (int i = 0; i < a.Length; i++)
        //    {
        //        b[i] = a[i];
        //    }
        //    sw.Stop();
        //    // Get the elapsed time as a TimeSpan value.
        //    TimeSpan ts = sw.Elapsed;

        //    // Format and display the TimeSpan value.
        //    string elapsedTime = String.Format("{0:00}.{1:00}", ts.Seconds, ts.Milliseconds);
        //    Console.WriteLine("RunTime " + elapsedTime);
        //}
        static void Main()
        {
            //update contract
            Employee employee = new Employee("Igor", "Fedorov", new DateTime(1991, 7, 11), "", "HR", 2300, new DateTime(2023, 2, 6));
            UpdateContract(employee);

            //change properties of currency
            Currency EUR = new Currency("Euro", "€", "EUR", "Austria", 1.07);
            ChangeExchangeRate(EUR);

            //client to employee
            //BankService a = new BankService();
            //employee = a.TurnIntoEmployee(new Client("Vladislav", "Yudaev", new DateTime(1987, 12, 19), "184018", "778-2-32-31", "VladislavYudaev@gmail.com"));

        }
    }
}
