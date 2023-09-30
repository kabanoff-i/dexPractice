using System;

namespace DelegatesPractice
{
    internal class Program
    {
        delegate void Message();
        static void Main(string[] args)
        {
            Action<int, int> action = (int a, int b) => Console.WriteLine(a+b);
            action(2, 3);

            Predicate<int> predicate = (int a) => a >= 20;
            Console.WriteLine(predicate(10));

            int SquareNumber(int n) => n * n;

            void DoOperation(int n, Func<int, int> op) => Console.WriteLine(op(n));

            DoOperation(5, SquareNumber);

            int x = 7;
            Console.WriteLine(x.GetHashCode());
            Console.ReadLine();

        }
    }
}
