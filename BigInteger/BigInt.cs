
using System;
using System.Collections.Generic;
namespace bigInteger
{
    public class BigInt
    {
        public LinkedList<int> arr;
        private static Random rand = new Random();

        //O(n)
        public BigInt(string val)
        {
            arr = new LinkedList<int>();

            foreach (char c in val)
            {
                arr.AddLast(Int32.Parse(c.ToString()));
            }
        }

        //O(1)
        public BigInt()
        {
            arr = new LinkedList<int>();
        }
        public bool isEven()//Function IsEven  O(1)

        {

            return (this.arr.Last != null && this.arr.Last.Value % 2 == 0); //O(1)
        }
        //O(n) RemoveFrontZeros
        //public static void RemoveFrontZeros( BigInt n)
        //{
        //    LinkedListNode<int> p = n.arr.First;//O(1)
        //    while (n.arr.Count > 1 && n.arr.First.Value == 0) ////O(n)
        //    {
        //        n.arr.RemoveFirst(); //O(1)
                
        //    }

        //}

        public static BigInt Power(BigInt x, BigInt n) // O(n^1.58)
        {
            if (n.arr.Count == 0 || n.ToString() == "0") // we need to make sure of this condition
            {
                return new BigInt("1");//O(1)
            }
            if (n.isEven())//O(1)
            {

                BigInt div = divide(n, new BigInt("2")).Quotient; //O(n)
                BigInt half = Power(x, div); //T(N)= T(N/2) + O(n^1.58) O(n^1.58) .5<c 0.5>E>0
                half.removeLeadingZeroes();//O(n)

                BigInt result = (Multiplication(half, half));//O(n^1.58)
                return result;
            }
            else
            {
                BigInt result = Multiplication(x, Power(x, subtract(n, new BigInt("1"))));
                result.removeLeadingZeroes();

                return result;
            }

        }
      
        public static BigInt Multiplication(BigInt f, BigInt s)  // O(n^1.58)
        {
            //O(1)
            if (f.arr.Count == 1 && f.arr.First.Value == 0 ||
           s.arr.Count == 1 && s.arr.First.Value == 0)
            {
                return new BigInt("0");//O(1)
            }
            //O(1)
            if (f.arr.Count == 1 && s.arr.Count == 1)
            {
                Int128 product = f.arr.First.Value * s.arr.First.Value;//O(1)
                return new BigInt(product.ToString());//O(1)
            }


            int n = Math.Max(f.arr.Count, s.arr.Count);//O(1)

            if (f.arr.Count < s.arr.Count)
                AddZerosFirst(f, s.arr.Count - f.arr.Count);//O(n)
            else if (s.arr.Count < f.arr.Count)
                AddZerosFirst(s, f.arr.Count - s.arr.Count);//O(n)

            BigInt a = new BigInt();//O(1)
            BigInt b = new BigInt();//O(1)
            SplitBigInt(f,  a, b);//O(n)
            BigInt c = new BigInt();//O(1)
            BigInt d = new BigInt();//O(1)
            SplitBigInt(s,c,d);//O(n)

            BigInt ac = Multiplication(a, c);// T(N)= 3T(N/2)+O(n)  a =3 b =2  0.58>E>0   case 1 
            BigInt bd = Multiplication(b, d);
            BigInt a_plus_b = sum(a, b);//O(n)
            BigInt c_plus_d = sum(c, d);//O(n)
            BigInt a_plus_b_Mult_c_plus_d = Multiplication(a_plus_b, c_plus_d);

            BigInt y = subtract(a_plus_b_Mult_c_plus_d, ac);//O(n)
            BigInt ad_Plus_bc = subtract(y, bd);//O(n)



            BigInt result1 = AddZerosLast( ac, (n / 2) * 2);//O(n)
            BigInt result2 = AddZerosLast(ad_Plus_bc, n / 2);//O(n)
            // ac* 10 ^ n + bd * 10 ^ n/2  +ad_Plus_bc

            BigInt finalResult = sum(result1, result2);//O(n)
            finalResult = sum(finalResult, bd);//O(n)
            finalResult.removeLeadingZeroes();//O(n)
            return finalResult;//O(1)

        }
        static void AddZerosFirst( BigInt n, int count)  // O(n)
        {
            for (int i = 0; i < count; i++)  // O(n)
            {
                n.arr.AddFirst(0);  // O(1)
            }
        }
        static BigInt AddZerosLast(BigInt n, int count)//O(n)
        {
            for (int i = 0; i < count; i++)//O(n)
            {
                n.arr.AddLast(0);//O(1)
            }
            return n;//O(1)
        }
        static void SplitBigInt(BigInt n,  BigInt a, BigInt b)//O(n)
        {
            int mid = n.arr.Count - n.arr.Count / 2;//O(1)
            LinkedListNode<int> p = n.arr.First;//O(1)
            for (Int128 i = 0; i < mid; i++)//O(n)
            {
                a.arr.AddLast(p.Value);//O(1)
                p = p.Next;//O(1)
            }
            while (p != null)//O(n)
            {
                b.arr.AddLast(p.Value);//O(1)
                p = p.Next;//O(1)
            }
        }

        public static BigInt sum(BigInt n, BigInt m)//O(n)
        {
            LinkedListNode<int> lastN = n.arr.Last;//O(1)
            LinkedListNode<int> lastM = m.arr.Last;//O(1)
            int carry = 0;//O(1)
            BigInt sum = new BigInt();//O(1)
            while (lastN != null || lastM != null || carry > 0)//O(Max(n,m))
            {
                int s = carry;//O(1)
                if (lastN != null)//O(1)
                {
                    s += lastN.Value;//O(1)
                    lastN = lastN.Previous;//O(1)
                }

                if (lastM != null)//O(1)
                {
                    s += lastM.Value;//O(1)
                    lastM = lastM.Previous;//O(1)
                }

                carry = s / 10;//O(1)
                sum.arr.AddFirst(s % 10);//O(1)
            }
            return sum;//O(1)
        }

        
        public static (BigInt Quotient, BigInt Remainder) divide(BigInt num, BigInt divisor)//O(n)
        {
            if (compare(num, divisor) == -1)//O(Max(n1,n2))
                return (new BigInt("0"), num);//O(1)

            else
            {
                BigInt quotient = new BigInt();//O(1)
                BigInt remainder = new BigInt();//O(1)
                BigInt s = sum(divisor, divisor);//O(n)

                (quotient, remainder) = divide(num,s); //T(N) = (N/2) + //O(n)  n/2< cn case 3  // 2* divisor = n/2
                quotient = sum(quotient, quotient);//O(n)
                if (compare(remainder, divisor) == -1)//O(n)
                {
                    return (quotient, remainder);//O(1)
                }
                else
                {
                    BigInt d = sum(quotient, new BigInt("1"));//O(n)
                    BigInt sub = subtract(remainder, divisor);//O(n)
                    return (d, sub);//O(1)
                }
            }



        }
        public static int compare(BigInt a, BigInt b)//O(Max(n1,n2))
        {

            if (a.arr.Count > b.arr.Count)//O(1)
                return 1;//O(1)

            else if (a.arr.Count < b.arr.Count) return -1;//O(1)
            else//O(n)
            {

                LinkedListNode<int> n1 = a.arr.First;//O(1)
                LinkedListNode<int> n2 = b.arr.First;//O(1)
                while (n1 != null || n2 != null)//O(Max(n1,n2))
                {
                    if (n1.Value > n2.Value) return 1;//O(1)
                    if (n1.Value < n2.Value) return -1;//O(1)
                    n1 = n1.Next;//O(1)
                    n2 = n2.Next;//O(1)
                }
                return 0;//O(1)

            }
        }
        public static BigInt subtract(BigInt x, BigInt y)//O(n)
        {

            if (compare(x, y) == 0) return new BigInt("0");//O(Max(y,x))


            LinkedListNode<int> num1 = x.arr.Last;//O(1)
            LinkedListNode<int> num2 = y.arr.Last;//O(1)
            BigInt result = new BigInt();//O(1)
            int borrow = 0;//O(1)
            while (num1 != null || num2 != null)//O(Max(n1,n2))
            {
                int a, b;//O(1)
                if (num1 != null)//O(1)
                {
                    a = num1.Value;//O(1)
                }
                else
                {
                    a = 0;//O(1)
                }
                if (num2 != null)//O(1)
                {
                    b = num2.Value;//O(1)
                }
                else
                {
                    b = 0;//O(1)
                }
                int difference = a - b - borrow;//O(1)
                if (difference < 0)//O(1)
                {
                    difference = difference + 10;//O(1)
                    borrow = 1;//O(1)
                }
                else
                {
                    borrow = 0;//O(1)
                }
                result.arr.AddFirst(difference);//O(1)
                if (num1 != null)//O(1)
                {
                    num1 = num1.Previous;//O(1)
                }
                if (num2 != null)//O(1)
                {
                    num2 = num2.Previous;//O(1)
                }


            }
            return result.removeLeadingZeroes();//O(n)
        }


        public BigInt removeLeadingZeroes()//O(n)
        {
            while (this.arr.First != null && this.arr.First.Value == 0)//O(n)
            {
                this.arr.RemoveFirst();//O(1)
            }
            return this;//O(1)
        }

        override public String ToString()//O(n)
        {
            return string.Join("", arr);//O(1)
        }
        public override bool Equals(object obj)//O(Max(n1,n2))
        {
            if (obj is BigInt secondObj)//O(Max(n1,n2))
            {
                LinkedListNode<int> p1 = this.arr.Last;//O(1)
                LinkedListNode<int> p2 = secondObj.arr.Last;//O(1)
                while (p1 != null && p2 != null)//O(Max(n1,n2))
                {
                    if (p1.Value != p2.Value)//O(1)
                    {
                        return false;//O(1)
                    }
                    else
                    {
                        p1 = p1.Previous;//O(1)
                        p2 = p2.Previous;//O(1)
                    }
                }
                return p1 == null && p2 == null;//O(1) // Both lists fully traversed
            }
            return false;//O(1)
        }

        public static BigInt encrypt(BigInt num, BigInt key, BigInt mod)//O(n ^ 1.58)
        {
            if (num.arr.Last.Value == 0) //O(1)
            {
                return new BigInt("0");//O(1)
            }

            if(key.arr.Last.Value == 0)//O(1)
            {
                return new BigInt("1");//O(1)
            }
            BigInt result = new BigInt();//O(1)
            if (key.isEven()) //O(n ^ 1.58)
            {
                BigInt div = divide(key, new BigInt("2")).Quotient;

                result = encrypt(num, div, mod);//T(N)= T(N/2) + O(n^1.58) O(n^1.58) .5<c 0.5>E>0

                BigInt temp = Multiplication(result, result);//O(n^1.58)
                result = divide(temp, mod).Remainder;//O(n)
            }
            else //O(n ^ 1.58)
            {
                result = divide(num, mod).Remainder;//O(n)
                BigInt middleValue = encrypt(num, subtract(key, new BigInt("1")), mod);
                middleValue = Multiplication(result, middleValue);//O(n^1.58)
                middleValue = divide(middleValue, mod).Remainder;//O(n)
                result = divide(middleValue, mod).Remainder;//O(n)
            }


            return (divide(sum(result, mod), mod).Remainder);//O(n)+O(n) = //O(n)
        }

        //O(n)
        public static bool operator <=(BigInt left, BigInt right)
        {
            if (compare(left, right) <= 0)//O(n)
            {
                return true;//O(1)
            }
            return false;//O(1)
        }
        //O(n)
        public static bool operator >=(BigInt left, BigInt right)
        {
            if (compare(left, right) >= 0)//O(n)
            {
                return true;//O(1)
            }
            return false;//O(1)
        }
        //O(n)
        public static bool operator <(BigInt left, BigInt right)
        {
            if (compare(left, right) < 0)//O(n)
            {
                return true;//O(1)
            }
            return false;//O(1)
        }
        //O(n)
        public static bool operator >(BigInt left, BigInt right)
        {
            if (compare(left, right) > 0)//O(n)
            {
                return true;//O(1)
            }
            return false;//O(1)
        }

        //public static BigInt ModulusFactor(BigInt n)
        //{
        //    BigInt p;
        //    BigInt q;
        //    BigInt phi;
        //    for (BigInt i = new BigInt("2"); Multiplication(i, i) <= n; i = sum(i, new BigInt("1")))
        //    {
        //        if (divide(n, i).Remainder.arr.Last.Value == 0)
        //        {
        //            q = i;
        //            p = divide(n, i).Quotient;
        //            phi = Multiplication(subtract(q, new BigInt("1")), subtract(p, new BigInt("1")));
        //            RemoveFrontZeros(ref phi);
        //            return phi;
        //        }
        //    }
        //    return new BigInt("1");
        //}

        public static BigInt generateprime()

        {
            int digits = 3;
            // Define the range for the prime number
            BigInt lowerBound = lowerbound(digits);//O(1)
            BigInt upperBound = upperbound(digits);//O(1)

            while (true)
            {
                // Generate a random odd number
                BigInt candidate = GenerateRandomBigInt(lowerBound, upperBound);

                // Make sure it's odd
                if (candidate.isEven())
                    candidate = BigInt.sum(candidate, new BigInt("1"));

                // Perform primality test
                if (IsPrime(candidate))
                    return candidate;
            }
        }

        private static BigInt GenerateRandomBigInt(BigInt min, BigInt max)//O(n)
        {
        
            string maxStr = max.ToString();//O(1)
            int maxDigits = maxStr.Length;//O(1)

            while (true)
            {
                string randomValue = "";//O(1)

                randomValue += (char)('0' + rand.Next(1,10));//O(1)

                for (int i = 1; i < maxDigits; i++)//O(1) max digit is constant max= 999
                {
                    randomValue += (char)('0' + rand.Next(10));//O(1)
                }

                BigInt result = new BigInt(randomValue);//O(1)

                if (compare(result, min) >= 0 && compare(result, max) <= 0)//O(n)
                    return result;//O(1)
            }
        }

        public static (BigInt n, BigInt e) GeneratePublicKey(int digitSize)
        {
            BigInt p = generateprime();
            BigInt q = generateprime();
            while (BigInt.Equals(p, q))
            {
                q = generateprime();
            }

            BigInt n = Multiplication(p, q);
            BigInt phi = Multiplication(subtract(p, new BigInt("1")), subtract(q, new BigInt("1")));
            BigInt e = SelectPublicExponent(phi);
    //        BigInt d = ModInverse(e, phi);

            return (n, e);
        }


        public static BigInt lowerbound(int digits)//O(1)
        {
            string value = "1";//O(1)
            for (int i = 1; i < digits; i++)//O(1)
                value += "0";//O(1)
            return new BigInt(value);//O(1)
        }

        public static BigInt upperbound(int digits)//O(1)
        {
            string value = "";//O(1)
            for (int i = 0; i < digits; i++)//O(1)
                value += "9";//O(1)
            return new BigInt(value);//O(1)
        }

        static bool IsPrime(BigInt number)
        {
            if (compare(number, new BigInt("2")) < 0) return false;//O(n)
            if (BigInt.Equals(number, new BigInt("2")) || BigInt.Equals(number, new BigInt("3"))) return true;
            if (number.isEven()) return false;


            for (BigInt i = new BigInt("3"); Multiplication(i, i) <= number; i = sum(i, new BigInt("2")))
            {
                if (divide(number, i).Remainder.arr.Last.Value == 0)
                    return false;
            }

            return true;
        }
        //phi&&e
        private static BigInt GCD(BigInt a, BigInt b)
        {
            while (b.ToString() != "0")
            {
                BigInt temp = b;
                b = BigInt.divide(a, b).Remainder;
                a = temp;
            }
            return a;
        }

        private static BigInt SelectPublicExponent(BigInt phi)
        {
            // Common choices for e include 3, 17, and 65537
            BigInt[] commonValues = {
                new BigInt("3"),
                new BigInt("17"),
                new BigInt("65537")
            };

            // Try common values first
            foreach (BigInt candidate in commonValues)
            {
                if (BigInt.compare(candidate, phi) < 0 && GCD(candidate, phi).ToString() == "1")
                    return candidate;
            }

            // If none work, find another suitable value
            BigInt e = new BigInt("65537");
            while (BigInt.compare(e, phi) < 0)
            {
                if (GCD(e, phi).ToString() == "1")
                    return e;

                e = BigInt.sum(e, new BigInt("2"));
            }

            throw new Exception("Failed to find a suitable public exponent.");
        }
    }
}
