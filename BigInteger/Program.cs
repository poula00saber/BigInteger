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

            BigInt mod= new BigInt("47594980475625417724408267823112764463863576918685226363032787239910118740004860624166859668486833021538759738968887527");
            BigInt key = new BigInt("22397637870882549517368596622641300924171095020557753582603446902846197377658196974714575577237681892436409853219169457");
            BigInt Message = new BigInt("29569328982587913530120188733526859818541063119729703526373445395974444779004599794422891381390520178237801160060744398");

            BigInt z = BigInt.encrypt(Message,key,mod);

            Console.WriteLine(z.ToLetters());

            int after = System.Environment.TickCount;
            Console.WriteLine("After run: " + after);

            Console.WriteLine("Difference in ticks: " + (after - before));


        }
    }
}