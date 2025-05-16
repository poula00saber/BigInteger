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

            BigInt x = new BigInt("123");
            BigInt y = new BigInt("456");
            BigInt z = BigInt.subtract(y, x);

            Console.WriteLine(z.ToString());

            int after = System.Environment.TickCount;
            Console.WriteLine("After run: " + after);

            Console.WriteLine("Difference in ticks: " + (after - before));


        }
    }
}