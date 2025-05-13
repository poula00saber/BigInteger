using System;
using System.Diagnostics;

namespace bigInteger
{
    class Program
    {
        static void Main(string[] args)
        {

            int before = System.Environment.TickCount;
            Console.WriteLine("Before run: " + before);

            BigInt Prime = new BigInt();
            Prime = BigInt.generateprime();

            Console.WriteLine(Prime.ToString());

            int after = System.Environment.TickCount;
            Console.WriteLine("After run: " + after);

            Console.WriteLine("Difference in ticks: " + (after - before));

        }
    }
}