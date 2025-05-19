using System;
using System.Diagnostics;

namespace bigInteger
{
    class Program
    {
        static void Main(string[] args)
        {
            int n = Convert.ToInt32(Console.ReadLine());
            BigInt[] mod = new BigInt[n];
            BigInt[] key = new BigInt[n];
            BigInt[] Message = new BigInt[n];
            int[] mode = new int[n];
            for (int i = 0; i < n; i++)
            {
                mod[i] = new BigInt(Console.ReadLine());
                key[i] = new BigInt(Console.ReadLine());
                Message[i] = new BigInt(Console.ReadLine());
                mode[i] = Convert.ToInt32(Console.ReadLine());
            }

            for (int i = 0; i < n; i++)
            {
                BigInt result = new BigInt();
                Console.WriteLine("Test Case: " + (i + 1));
                int before = System.Environment.TickCount;
                if (mode[i] == 0)
                {
                    result = BigInt.encrypt(Message[i], key[i], mod[i]);
                }
                else
                {
                    result = BigInt.decrypt(Message[i], key[i], mod[i]);
                }
                int after = System.Environment.TickCount;
                Console.WriteLine("Execution Time:" + (after - before));
                Console.WriteLine("----------------");
            }
        }
    }
}