
using System;
using System.Collections.Generic;
using System.Text;
namespace bigInteger
{
    public class Digits
    {
        public int x;
        public int repetitions;

        public Digits(int x, int repetitions)
        {
            this.x = x;
            this.repetitions = repetitions;
        }
    }

    public class BigInt
    {
        public LinkedList<Digits> arr;
        private static Random rand = new Random();

        public BigInt(string val)
        {
            arr = new LinkedList<Digits>();

            foreach (char c in val)
            {
                int num;
                if (int.TryParse(c.ToString(), out num))
                {
                    arr.AddLast(new Digits(num, 1));
                    continue;
                }
                num = c;
                int counter = 0;
                while (num > 0)
                {
                    arr.AddFirst(new Digits(num%10, 0));
                    num /= 10;
                    counter++;
                }
                arr.First.Value.repetitions = counter;
            }
        }


        public BigInt()
        {
            arr = new LinkedList<Digits>();
        }

        public bool isEven()
        {

            return (this.arr.Last != null && this.arr.Last.Value.x % 2 == 0);
        }

        public static void RemoveFrontZeros(ref BigInt n)
        {
            LinkedListNode<Digits> p = n.arr.First;
            while (n.arr.Count > 1 && n.arr.First.Value.x == 0)
            {
                n.arr.RemoveFirst();
            }

        }

        public static BigInt Power(BigInt x, BigInt n)
        {
            if (n.arr.Count == 0 || n.arr.Last.Value.x == 0) ; 
            {
                return new BigInt("1");
            }
            if (n.isEven())
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
        public static BigInt Multiplication(BigInt f, BigInt s)
        {
            if (f.arr.Count == 1 && f.arr.First.Value.x == 0 ||
           s.arr.Count == 1 && s.arr.First.Value.x == 0)
            {
                return new BigInt("0");
            }

            if (f.arr.Count == 1 && s.arr.Count == 1)
            {
                Int128 product = f.arr.First.Value.x * s.arr.First.Value.x;
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

            BigInt ac = Multiplication(a, c);
            BigInt bd = Multiplication(b, d);
            BigInt a_plus_b = sum(a, b);
            BigInt c_plus_d = sum(c, d);
            BigInt a_plus_b_Mult_c_plus_d = Multiplication(a_plus_b, c_plus_d);
            BigInt ad_Plus_bc = subtract(subtract(a_plus_b_Mult_c_plus_d, ac), bd);

            BigInt result1 = AddZerosLast(ref ac, (n / 2) * 2);
            BigInt result2 = AddZerosLast(ref ad_Plus_bc, n / 2);


            BigInt finalResult = sum(result1, result2);
            finalResult = sum(finalResult, bd);
            RemoveFrontZeros(ref finalResult);
            return finalResult;

        }


        static void AddZerosFirst(ref BigInt n, int count)
        {
            for (int i = 0; i < count; i++)
            {
                n.arr.AddFirst(new Digits(0, 1));
            }
        }
        static BigInt AddZerosLast(ref BigInt n, int count)
        {
            for (int i = 0; i < count; i++)
            {
                n.arr.AddLast(new Digits(0, 1));
            }
            return n;
        }
        static void SplitBigInt(BigInt n, ref BigInt a, ref BigInt b)
        {
            int mid = n.arr.Count - n.arr.Count / 2;
            LinkedListNode<Digits> p = n.arr.First;
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
            LinkedListNode<Digits> lastN = n.arr.Last;
            LinkedListNode<Digits> lastM = m.arr.Last;
            int carry = 0;
            BigInt sum = new BigInt();
            while (lastN != null || lastM != null || carry > 0)
            {
                int s = carry;
                if (lastN != null)
                {
                    s += lastN.Value.x;
                    lastN = lastN.Previous;
                }

                if (lastM != null)
                {
                    s += lastM.Value.x;
                    lastM = lastM.Previous;
                }

                carry = s / 10;
                sum.arr.AddFirst(new Digits(s % 10,1));
            }
            return sum;
        }
        public static (BigInt Quotient, BigInt Remainder) divide(BigInt num, BigInt divisor)
        {
            if (compare(num, divisor) == -1)
                return (new BigInt("0"), num);
            else
            {
                BigInt quotient = new BigInt();
                BigInt remainder = new BigInt();
                (quotient, remainder) = divide(num, sum(divisor, divisor));
                quotient = sum(quotient, quotient);
                if (compare(remainder, divisor) == -1)
                {
                    return (quotient, remainder);
                }
                else
                {
                    return (sum(quotient, new BigInt("1")), subtract(remainder, divisor));
                }
            }
        }
        public static int compare(BigInt a, BigInt b)
        {

            if (a.arr.Count > b.arr.Count)
                return 1;

            else if (a.arr.Count < b.arr.Count) return -1;
            else
            {

                LinkedListNode<Digits> n1 = a.arr.First;
                LinkedListNode<Digits> n2 = b.arr.First;
                while (n1 != null || n2 != null)
                {
                    if (n1.Value.x > n2.Value.x) return 1;
                    if (n1.Value.x < n2.Value.x) return -1;
                    n1 = n1.Next;
                    n2 = n2.Next;
                }
                return 0;

            }
        }
        public static BigInt subtract(BigInt x, BigInt y)
        {

            if (compare(x, y) == 0) return new BigInt("0");


            LinkedListNode<Digits> num1 = x.arr.Last;
            LinkedListNode<Digits> num2 = y.arr.Last;
            BigInt result = new BigInt();
            int borrow = 0;
            while (num1 != null || num2 != null)
            {
                int a, b;
                if (num1 != null)
                {
                    a = num1.Value.x;
                }
                else
                {
                    a = 0;
                }
                if (num2 != null)
                {
                    b = num2.Value.x;
                }
                else
                {
                    b = 0;
                }
                int difference = a - b - borrow;
                if (difference < 0)
                {
                    difference = difference + 10;
                    borrow = 1;
                }
                else
                {
                    borrow = 0;
                }
                result.arr.AddFirst(new Digits(difference,1));
                if (num1 != null)
                {
                    num1 = num1.Previous;
                }
                if (num2 != null)
                {
                    num2 = num2.Previous;
                }


            }
            return result.removeLeadingZeroes();
        }

        public BigInt removeLeadingZeroes()
        {
            while (this.arr.First != null && this.arr.First.Value.x == 0)
            {
                this.arr.RemoveFirst();
            }
            return this;
        }

        override public String ToString()
        {
            string ans = "";

           foreach(Digits n in arr)
            {
                ans += n.x;
            }

            return ans;

            
        }

        public String ToLetters()
        {
            string ans = "";


            LinkedList<Digits>.Enumerator it = arr.GetEnumerator();
            while (it.MoveNext())
            {
                
                string asciiCode = "";
                int repetitions = it.Current.repetitions;

                if(repetitions == 1)
                {
                    ans += it.Current.x.ToString();
                    continue;
                }

                for (int j = 0; j < repetitions; j++)
                {
                    asciiCode += (it.Current.x);
                    if(j != repetitions-1)
                        it.MoveNext();
                }
                int code; 
                int.TryParse(asciiCode, out code);
                ans += (char)code;
            }
            return ans;
        }


        public static BigInt encrypt(BigInt num, BigInt key, BigInt mod)
        {
            if (num.arr.Count == 1 && num.arr.Last.Value.x == 0)
            {
                return new BigInt("0");
            }

            if (key.arr.Count == 1 && key.arr.Last.Value.x == 0)
            {
                return new BigInt("1");
            }
            BigInt result = new BigInt();
            if (key.isEven())
            {
                result = encrypt(num, divide(key, new BigInt("2")).Quotient, mod);
                result = divide(Multiplication(result, result), mod).Remainder;
            }
            else
            {
                result = divide(num, mod).Remainder;
                BigInt middleValue = encrypt(num, subtract(key, new BigInt("1")), mod);
                middleValue = Multiplication(result, middleValue);
                middleValue = divide(middleValue, mod).Remainder;
                result = divide(middleValue, mod).Remainder;
            }
            return (divide(sum(result, mod), mod).Remainder);
        }


        public static bool operator <=(BigInt left, BigInt right)
        {
            if (compare(left, right) <= 0)
            {
                return true;
            }
            return false;
        }

        public static bool operator >=(BigInt left, BigInt right)
        {
            if (compare(left, right) >= 0)
            {
                return true;
            }
            return false;
        }

        public static bool operator <(BigInt left, BigInt right)
        {
            if (compare(left, right) < 0)
            {
                return true;
            }
            return false;
        }

        public static bool operator >(BigInt left, BigInt right)
        {
            if (compare(left, right) > 0)
            {
                return true;
            }
            return false;
        }

        public static BigInt generateprime(int digits)
        {

            BigInt lowerBound = lowerbound(digits);
            BigInt upperBound = upperbound(digits);

            while (true)
            {
                BigInt num = GenerateRandomBigInt(lowerBound, upperBound);
                if (num.isEven())
                    num = BigInt.sum(num, new BigInt("1"));

                if (IsPrime(num))
                    return num;
            }
        }

        private static BigInt GenerateRandomBigInt(BigInt min, BigInt max)
        {

            string maxStr = max.ToString();
            int maxDigits = maxStr.Length;

            while (true)
            {
                string randomValue = "";

                randomValue += (char)('0' + rand.Next(1, 9));

                for (int i = 1; i < maxDigits; i++)
                {
                    randomValue += (char)('0' + rand.Next(9));
                }

                BigInt result = new BigInt(randomValue);

                if (compare(result, min) >= 0 && compare(result, max) <= 0)
                    return result;
            }
        }

        public static (BigInt n, BigInt e) GeneratePublicKey(int digitSize)
        {
            BigInt p = generateprime(digitSize);
            BigInt q = generateprime(digitSize);
            while (BigInt.Equals(p, q))
            {
                q = generateprime(digitSize);
            }
            BigInt n = Multiplication(p, q);
            BigInt phi = Multiplication(subtract(p, new BigInt("1")), subtract(q, new BigInt("1")));
            BigInt e = SelectPublicExponent(phi);
            return (n, e);
        }


        private static BigInt lowerbound(int digits)
        {
            string value = "1";
            for (int i = 1; i < digits; i++)
                value += "0";
            return new BigInt(value);
        }

        private static BigInt upperbound(int digits)
        {
            string value = "";
            for (int i = 0; i < digits; i++)
                value += "9";
            return new BigInt(value);
        }

        static bool IsPrime(BigInt number)
        {
            if (compare(number, new BigInt("2")) < 0)
                return false;
            if (BigInt.Equals(number, new BigInt("2")) || BigInt.Equals(number, new BigInt("3")))
                return true;
            if (number.isEven())
                return false;

            for (BigInt i = new BigInt("3"); Multiplication(i, i) <= number; i = sum(i, new BigInt("1")))
            {
                if (divide(number, i).Remainder.arr.Last.Value.x == 0)
                    return false;
            }

            return true;
        }
        //for phi and e
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

            BigInt e = new BigInt("3");

            while (BigInt.compare(e, phi) < 0)
            {
                if (GCD(e, phi).ToString() == "1")
                    return e;

                e = BigInt.sum(e, new BigInt("1"));
            }

            throw new Exception("Failed to find a suitable public exponent.");
        }
    }
}
