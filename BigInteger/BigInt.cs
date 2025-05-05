
using System;

namespace bigInteger 
{
   public class BigInt
    {
        public LinkedList<int> arr;
       

        public BigInt(string val)
        {
            arr = new LinkedList<int>();
           
            foreach(char c in val)
            {     
                arr.AddLast(Int32.Parse(c.ToString()));
            }
        }


        public BigInt()
        {
            arr = new LinkedList<int>();
        }

        public static bool isEven(BigInt n)
        {

            LinkedListNode<int> lastN = n.arr.Last;
            try
            {
                if (lastN == null)
                {
                    return false;
                }
                return (lastN.Value % 2 == 0);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static void RemoveFrontZeros(ref BigInt n)
        {
            LinkedListNode<int> p = n.arr.First;
            while (n.arr.Count > 1 && n.arr.First.Value== 0)
            {
                n.arr.RemoveFirst();   
            }
           
        }
        
        public static BigInt Power(BigInt x, BigInt n)
        {
            if (n.arr.Count == 0 || n.ToString() == "0") // we need to make sure of this condition
            {
                return new BigInt("1");
            }
            if (isEven(n))
            {
                BigInt half = Power(x, divide(n, new BigInt("2")).Quotient);
                RemoveFrontZeros(ref half);

                return (Multiplication(half, half));
            }
            else
            {
                BigInt result = Multiplication(x, Power(x, subtract(n, new BigInt("1"))));
                RemoveFrontZeros(ref result);

                return result;
            }

        }
        public static  BigInt Multiplication(BigInt f, BigInt s)
        {
            if (f.arr.Count == 1 && f.arr.First.Value== 0 ||
           s.arr.Count == 1 && s.arr.First.Value == 0)
            {
                return new BigInt("0");
            }

            if (f.arr.Count == 1 && s.arr.Count == 1)
            {
                Int128 product = f.arr.First.Value * s.arr.First.Value;
                return new BigInt(product.ToString());
            }


            int n = Math.Max(f.arr.Count, s.arr.Count);

            if (f.arr.Count < s.arr.Count)
                AddZerosFirst(ref f, s.arr.Count - f.arr.Count);
            else if (s.arr.Count < f.arr.Count)
                AddZerosFirst(ref s, f.arr.Count - s.arr.Count);

            BigInt a = new BigInt();
            BigInt b = new BigInt();
            SplitBigInt(f, ref a, ref b);
            BigInt c = new BigInt();
            BigInt d = new BigInt();
            SplitBigInt(s, ref c, ref d);

            BigInt ac= Multiplication(a, c);
            BigInt bd = Multiplication(b, d);
            BigInt a_plus_b = sum(a, b);
            BigInt c_plus_d = sum(c, d);
            BigInt a_plus_b_Mult_c_plus_d = Multiplication(a_plus_b, c_plus_d);
            BigInt ad_Plus_bc = subtract( subtract(a_plus_b_Mult_c_plus_d, ac),bd);

            BigInt result1 = AddZerosLast(ref ac, (n / 2) * 2); // shift ac by 2 * (n/2)
            BigInt result2 = AddZerosLast(ref ad_Plus_bc, n / 2); // shift ad+bc by (n/2)


            BigInt finalResult = sum(result1, result2);
            finalResult = sum(finalResult, bd);
            RemoveFrontZeros(ref finalResult);
            return finalResult;

        }


        static void AddZerosFirst(ref BigInt n, int count)
        {
            for (int i = 0; i < count; i++)
            {
                n.arr.AddFirst(0);
            }
        }
        static BigInt AddZerosLast(ref BigInt n, int count)
        {
            for (int i = 0; i < count; i++)
            {
                n.arr.AddLast(0);
            }
            return n;
        }
        static void SplitBigInt(BigInt n, ref BigInt a, ref BigInt b)
        {
            int mid = n.arr.Count- n.arr.Count / 2;
            LinkedListNode<int> p = n.arr.First;
            for (Int128 i = 0; i < mid; i++)
            {
                a.arr.AddLast(p.Value);
                p = p.Next;
            }
            while (p != null)
            {
                b.arr.AddLast(p.Value);
                p = p.Next;
            }
        }

        public static BigInt sum(BigInt n, BigInt m)
        {
            LinkedListNode<int> lastN = n.arr.Last;
            LinkedListNode<int> lastM = m.arr.Last;
            int carry = 0;
            BigInt sum = new BigInt();
            while(lastN != null || lastM != null || carry > 0)
            {
                int s = carry;
                if (lastN != null)
                {
                    s += lastN.Value;
                    lastN = lastN.Previous;
                }

                if(lastM != null)
                {
                    s += lastM.Value;
                    lastM = lastM.Previous;
                }

                carry = s / 10;
                sum.arr.AddFirst(s % 10);
            }
            return sum;
        }

         public static BigInt subtract(BigInt n, BigInt m){
            LinkedListNode<int> lastN = n.arr.Last;
            LinkedListNode<int> lastM = m.arr.Last;
            int carry = 0;
            BigInt sum = new BigInt();

            while (lastN != null || lastM != null)
            {
                int a = (lastN != null) ? lastN.Value : 0;
                int b = (lastM != null) ? lastM.Value : 0;

                int s = a - b - carry; //correct logic

                if (s < 0)//edited insted of multiplying it to return positive value, you should borrow
                {
                    s += 10;
                    carry = 1;
                }
                else
                {
                    carry = 0;
                }

                sum.arr.AddFirst(s);//add number after sub

                if (lastN != null) lastN = lastN.Previous;
                if (lastM != null) lastM = lastM.Previous;
            }

            sum.removeLeadingZeros(); //to remove unnessecary leading zeroes
            return sum;
        }

        public static (BigInt Quotient, BigInt Remainder) divide(BigInt dividend, BigInt divisor)
        {
            // Handle division by zero
            if (divisor.arr.Count == 0 || divisor.ToString() == "0")
                throw new DivideByZeroException("Cannot divide by zero.");

            // Early exit cases
            if (compare(dividend, divisor) < 0)
                return (new BigInt("0"), new BigInt(dividend.ToString()));
            if (compare(dividend, divisor) == 0)
                return (new BigInt("1"), new BigInt("0"));

            BigInt quotient = new BigInt();
            BigInt remainder = new BigInt("0");

            // Iterate over each digit in the dividend (left to right)
            var currentDigitNode = dividend.arr.First;
            while (currentDigitNode != null)
            {
                // Bring down the next digit
                remainder.arr.AddLast(currentDigitNode.Value);
                remainder.removeLeadingZeros();

                // Skip if remainder is still smaller than divisor
                if (compare(remainder, divisor) < 0)
                {
                    quotient.arr.AddLast(0); // Append 0 to quotient
                    currentDigitNode = currentDigitNode.Next;
                    continue;
                }

                // Subtract divisor from remainder as many times as possible
                int count = 0;
                while (compare(remainder, divisor) >= 0)
                {
                    remainder = subtract(remainder, divisor);
                    count++;
                }

                quotient.arr.AddLast(count); // Append count to quotient
                currentDigitNode = currentDigitNode.Next;
            }

            quotient.removeLeadingZeros();
            return (quotient,remainder);
        }

public static int compare(BigInt a, BigInt b)
{
    a.removeLeadingZeros();
    b.removeLeadingZeros();

    if (a.arr.Count > b.arr.Count) return 1;
    if (a.arr.Count < b.arr.Count) return -1;

    var n1 = a.arr.First;
    var n2 = b.arr.First;
    while (n1 != null)
    {
        if (n1.Value > n2.Value) return 1;
        if (n1.Value < n2.Value) return -1;
        n1 = n1.Next;
        n2 = n2.Next;
    }
    return 0;
}
public void removeLeadingZeros()
{
    while (arr.Count > 1 && arr.First.Value == 0)
    {
        arr.RemoveFirst();
    }
}


    override public String ToString(){
        return string.Join("", arr);
    }
        public override bool Equals(object obj)
        {
            LinkedListNode<int> p1 = this.arr.Last;
            BigInt secondObj = (BigInt) obj;
            LinkedListNode<int> p2 = secondObj.arr.Last;
            while (p1 != null && p2 != null)
            {
                if (p1.Value != p2.Value)
                {
                    return false;
                }
                else
                {
                    p1 = p1.Previous;
                    p2 = p2.Previous;
                }
            }
            return true;
        }

        public static BigInt encrypt(BigInt num, BigInt key, BigInt mod)
        {
             return divide( Power(num, key), mod).Remainder ;
        }
        public static BigInt decrypt(BigInt fnum, BigInt key, BigInt mod)
        {
            return divide(Power(fnum, key), mod).Remainder;
        }

    }

}


