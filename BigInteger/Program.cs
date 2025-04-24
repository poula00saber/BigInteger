using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bigInteger
{
    class Program
    {
        static void Main(string[] args)
        {

            BigInt n = new BigInt("57329817389217483219843341245621641542354235423543254325");
            BigInt m = new BigInt("55435432543729845789275437297438547543254325432543254344");
            BigInt sum = new BigInt();



            Console.Write("\n Before function: " + System.Environment.TickCount + "\n");
            sum = BigInt.sum(n, m);
            Console.Write("\n After function: " + System.Environment.TickCount + "\n");
            Console.WriteLine("\n Sum array\n");
            foreach (int x in sum.arr)
                Console.Write(x + " ");
            Console.WriteLine("\n");



        }





    }
}
