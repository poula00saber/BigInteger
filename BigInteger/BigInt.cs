using System;
using System.Collections.Generic;
using System.Text;
namespace bigInteger
{
    public class BigInt
    {
        public LinkedList<int> arr;//O(1) 
        private static Random rand = new Random();//O(1)

        //O(n)
        public BigInt(string val)
        {
            arr = new LinkedList<int>(); //O(1)

            foreach (char c in val) //O(N)
            {
                int num; //O(1)
                num = c - '0'; //O(1)
                arr.AddLast(num); //O(1)
            }

        }
        public BigInt(string val, string mode) //O(N)
        {
            arr = new LinkedList<int>(); //O(1)

            foreach (char c in val) //O(N)
            {
                string str = (c - '0').ToString(); //O(1), at most 2 to 3 digits so it tends to O(1)
                arr.AddLast(str.Length);//O(1)
                foreach (char n in str) //O(1), The length of the string is very small at most 3 digits so it tends to O(1)
                {
                    int num = n - '0'; //O(1)
                    arr.AddLast(num); //O(1)
                }
            }
        }


        public BigInt()//O(1)
        {
            arr = new LinkedList<int>();//O(1)
        }

        public bool isEven()//Function IsEven  O(1)

        {
            return (this.arr.Last != null && this.arr.Last.Value % 2 == 0); //O(1)
        }
 
        public static BigInt Power(BigInt x, BigInt n) // O(n^1.58)
        {
            if (n.arr.Count == 0 || n.arr.Last.Value == 0)
            {
                return new BigInt("1");//O(1)
            }

            if (n.isEven())//O(1)
            {

                BigInt div = divide(n, new BigInt("2")).Quotient; //O(n)
                BigInt half = Power(x, div); //T(N)= T(N/2) + O(n^1.58),Using master method case 3 =>  O(n^1.58), (0.5)^1.58 <c AND 0.58>E>0
                half.removeLeadingZeroes();//O(n)

                BigInt result = (Multiplication(half, half));//O(n^1.58)
                return result;//O(1)
            }
            else
            {
                BigInt result = Multiplication(x, Power(x, subtract(n, new BigInt("1")))); // T(N) = T(N-1) + O(N^1.58), T(1) = 1, Using tree method Number of levels = N, Complexity = Sum from 1 to N-1 (N^1.58 - i) = O(N^2.58)
                result.removeLeadingZeroes();//O(N) 

                return result;//O(1)
            }

        }

        public static BigInt Multiplication(BigInt f, BigInt s)  // O(n^1.58)
        {
          
            if (f.arr.Count == 1 && f.arr.First.Value == 0 ||
                s.arr.Count == 1 && s.arr.First.Value == 0)//O(1)
            {
                return new BigInt("0");//O(N)
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
            SplitBigInt(f, a, b);//O(n)
            BigInt c = new BigInt();//O(1)
            BigInt d = new BigInt();//O(1)
            SplitBigInt(s, c, d);//O(n)

            BigInt ac = Multiplication(a, c);// T(N)= 3T(N/2)+O(n)  a =3 b =2  0.58>E>0   case 1 
            BigInt bd = Multiplication(b, d);
            BigInt a_plus_b = sum(a, b);//O(n)
            BigInt c_plus_d = sum(c, d);//O(n)
            BigInt a_plus_b_Mult_c_plus_d = Multiplication(a_plus_b, c_plus_d);

            BigInt y = subtract(a_plus_b_Mult_c_plus_d, ac);//O(n)
            BigInt ad_Plus_bc = subtract(y, bd);//O(n)



            BigInt result1 = AddZerosLast(ac, (n / 2) * 2);//O(n)
            BigInt result2 = AddZerosLast(ad_Plus_bc, n / 2);//O(n)
            // ac* 10 ^ n + bd * 10 ^ n/2  +ad_Plus_bc

            BigInt finalResult = sum(result1, result2);//O(n)
            finalResult = sum(finalResult, bd);//O(n)
            finalResult.removeLeadingZeroes();//O(n)
            return finalResult;//O(1)

        }
        static void AddZerosFirst(BigInt n, int count)  // O(n)
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
        static void SplitBigInt(BigInt n, BigInt a, BigInt b)//O(n)
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


        public static (BigInt Quotient, BigInt Remainder) divide(BigInt num, BigInt divisor)//O(nlog(n))
        {

            if (compare(num, divisor) == -1)//O(Max(n1,n2))
                return (new BigInt("0"), num);//O(1)
            else
            {
                BigInt quotient = new BigInt();//O(1)
                BigInt remainder = new BigInt();//O(1)
                BigInt s = sum(divisor, divisor);//O(n)

                //T(N) = T(N/2) + O(N) ,  Using tree method => Number of Levels = log(N) , Complexity per level = O(N), BaseCase complexity = O(N)
                //Sum(i => Log(N)-1) (N/(2^i)) = Nlog(N)
                (quotient, remainder) = divide(num, s); 
               
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
        public static BigInt subtract(BigInt x, BigInt y)//O(N)
        {

            if (compare(x, y) == 0) //O(Max(n1,n2))
                return new BigInt("0");//O(N)


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
        //O(n)
        override public String ToString()
        {
            string ans = "";//O(1)
            //O(N)
            foreach (int n in arr)
            {
                ans += n;//O(1)
            }
            return ans;//O(1)
        }


        //O(nlog(n))
        public String ToLetters()
        {
            LinkedList<int>.Enumerator it = arr.GetEnumerator();//O(log(N))
            string ans = "";//O(1)

            //Body * Iterations = log(n) * n  = O(nlog(n))
            while (it.MoveNext())//O(n)
            {
                int size = it.Current;//O(1)
                it.MoveNext();//O(1)
                string letter = "";//O(1)

                for (int i = 0; i < size; i++)//O(n)
                {
                    letter += it.Current;//O(1)
                    if (i != size - 1)//O(1)
                    {
                        it.MoveNext();//O(1)
                    }
                }
                int letterASCII = 0;//O(1)

                //O(log n) 
                for (int i = 0; i < letter.Length; i++)//O(1), letter size can't go above 3 , N <=3 so it tends to O(1)
                {
                    //O(logn) built in function
                    letterASCII += (int)((letter[i] - '0') * (Math.Pow(10, letter.Length - i - 1))); //Ascii code of the letter, Multiplied by its unit (i.e Hundred and things like that guys smh)
                }

                letterASCII += '0';//O(1)
                ans += (char)letterASCII;//O(1)
            }
            return ans;//O(1)
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

        public static BigInt encrypt(BigInt num, BigInt key, BigInt mod)//O(n ^ 2.58)
        {
            if (num.arr.Count == 1 && num.arr.Last.Value == 0)//O(1)
            {
                return new BigInt("0");//O(N)
            }
            if (key.arr.Count == 1 && key.arr.Last.Value == 0)//O(1)
            {
                return new BigInt("1");//O(N)
            }
            BigInt result = new BigInt();//O(1)

            if (key.isEven())//O(!)
            {
                BigInt div = divide(key, new BigInt("2")).Quotient;//nLog(n)

                result = encrypt(num, div, mod);//T(N)= T(N/2) + O(n^1.58), Using master method Case 3 => O(n^1.58) (0.5)^1.58 < c < 1 and 0.58 > E > 0

                BigInt temp = Multiplication(result, result);//O(n^1.58)
                result = divide(temp, mod).Remainder;//O(n)
            }
            else
            {
                result = divide(num, mod).Remainder;//O(n)
                BigInt middleValue = encrypt(num, subtract(key, new BigInt("1")), mod); // T(N) = T(N-1) + O(N^1.58), T(1) = 1, Using tree method Number of levels = N, Complexity = Sum from 1 to N-1 (N^1.58 - i) = O(N^2.58)
                middleValue = Multiplication(result, middleValue);//O(n^1.58)
                middleValue = divide(middleValue, mod).Remainder;//O(n)
                result = divide(middleValue, mod).Remainder;//O(n)
            }
            return (divide(sum(result, mod), mod).Remainder);//O(nLog(N)))
        }

        public static BigInt decrypt(BigInt num, BigInt key, BigInt mod)
        {
            if (num.arr.Count == 1 && num.arr.Last.Value == 0)//O(1)
            {
                return new BigInt("0");//O(N)
            }
            if (key.arr.Count == 1 && key.arr.Last.Value == 0)//O(1)
            {
                return new BigInt("1");//O(N)
            }
            BigInt result = new BigInt();//O(1)

            if (key.isEven())//O(!)
            {
                BigInt div = divide(key, new BigInt("2")).Quotient;//nLog(n)

                result = encrypt(num, div, mod);//T(N)= T(N/2) + O(n^1.58), Using master method Case 3 => O(n^1.58) (0.5)^1.58 < c < 1 and 0.58 > E > 0

                BigInt temp = Multiplication(result, result);//O(n^1.58)
                result = divide(temp, mod).Remainder;//O(n)
            }
            else 
            {
                result = divide(num, mod).Remainder;//O(n)
                BigInt middleValue = encrypt(num, subtract(key, new BigInt("1")), mod); // T(N) = T(N-1) + O(N^1.58), T(1) = 1, Using tree method Number of levels = N, Complexity = Sum from 1 to N-1 (N^1.58 - i) = O(N^2.58)
                middleValue = Multiplication(result, middleValue);//O(n^1.58)
                middleValue = divide(middleValue, mod).Remainder;//O(n)
                result = divide(middleValue, mod).Remainder;//O(n)
            }
            return (divide(sum(result, mod), mod).Remainder);//O(nLog(N)))
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


        // O(n^2.08)
        public static BigInt generateprime()
        {
            Dictionary<BigInt, bool> isChecked = new Dictionary<BigInt, bool>();//O(1)
            int digits = 3;//O(1)
            // Define the range for the prime number
            BigInt lowerBound = lowerbound(digits);//O(1)
            BigInt upperBound = upperbound(digits);//O(1)
            //Loop iterations can't be properly determined but since the input is so small (n = 3) it tends to O(1)
            //Iteratons * Body = 1 * n^2.08 = O(n^2.08)
            while (true)
            {
                BigInt num = GenerateRandomBigInt(lowerBound, upperBound);//O(n) 
                if (isChecked.ContainsKey(num)) //O(1)
                {
                    continue;//O(1)
                }
                else
                {
                    isChecked.Add(num,true); //O(1)
                    if (num.isEven()) //O(1)
                        num = BigInt.sum(num, new BigInt("1"));//O(n)

                    if (IsPrime(num))//O(n^2.08) 
                        return num;//O(1)
                }
            }
        }

        private static BigInt GenerateRandomBigInt(BigInt min, BigInt max)//O(n)
        {


            string maxStr = max.ToString();//O(N)
            int maxDigits = maxStr.Length;//O(1)
            while (true)
            {
                string randomValue = "";//O(1)
                randomValue += (char)('0' + rand.Next(1, 10));//O(1)


                for (int i = 1; i < maxDigits; i++)//O(1) max digit is constant max= 999
                {
                    randomValue += (char)('0' + rand.Next(10));//O(1)
                }

                BigInt result = new BigInt(randomValue);//O(N)

               // if (compare(result, min) >= 0 && compare(result, max) <= 0)//O(n)
                    return result;//O(1)
            }
        }


        //O(n^2.08)
        public static (BigInt n, BigInt e) GeneratePublicKey(int digitSize)
        {
            BigInt p = generateprime();//O(n^2.08)
            BigInt q = generateprime();//O(n^2.08)
            while (BigInt.Equals(p, q))//O(N)
            {
                q = generateprime();//O(n^2.08)
            }
            BigInt n = Multiplication(p, q);//O(n^1.58)
            BigInt x = subtract(p, new BigInt("1"));//O(n)
            BigInt y = subtract(q, new BigInt("1"));//O(n)
            BigInt phi = Multiplication(x, y);//O(n^1.58)
            BigInt e = SelectPublicExponent(phi);//O(n^2(log(n))^2)
            return (n, e);//O(1)
        }


        public static BigInt lowerbound(int digits)//O(1), digits is a constant of 3
        {
            string value = "1";//O(1)
            for (int i = 1; i < digits; i++)//O(1)
                value += "0";//O(1)
            return new BigInt(value);//O(1)
        }

        public static BigInt upperbound(int digits)//O(1), digits is a constant of 3 
        {
            string value = "";//O(1)
            for (int i = 0; i < digits; i++)//O(1)
                value += "9";//O(1)
            return new BigInt(value);//O(1)
        }
        //O(n^2.08) 
        static bool IsPrime(BigInt number)
        {

            if (compare(number, new BigInt("2")) < 0) //O(N)
                return false;//O(1)
            if (BigInt.Equals(number, new BigInt("2")) || BigInt.Equals(number, new BigInt("3"))) //O(N)
                return true; //O(1)
            if (number.isEven()) //O(1)
                return false;//O(1)

            //O(n^(1.58 + 0.5 )) = //O(n^2.08) 
            // root n (num of iterations) * O(n^1.58) (body)
            for (BigInt i = new BigInt("3"); Multiplication(i, i) <= number; i = sum(i, new BigInt("1")))
            {

                // multiplication >  divide -> O(n^1.58) 
                if (divide(number, i).Remainder.arr.Last.Value == 0)//O(nlog(n))
                    return false;//O(1)
            }

            return true;
        }
        //for phi and e

        //O(n (log(n))^2)
        private static BigInt GCD(BigInt a, BigInt b)
        {
            while (b.ToString() != "0")//O(log(n))
            {
                // to string -> O(n)

                BigInt temp = b; //O(1)
                b = BigInt.divide(a, b).Remainder;//O(nlog(n))
                a = temp;//O(1)
            }
            return a;//O(1)
        }


        //O(N^2 (Log N)^2)
        private static BigInt SelectPublicExponent(BigInt phi)
        {

            BigInt e = new BigInt("3");
            //Number of iterations = N, Body = Nlog(n)^2
            //O(N^2Log(n)^2)
            while (BigInt.compare(e, phi) < 0)//O(n)
            {
                if (GCD(e, phi).ToString() == "1") //O(n (log(n))^2)
                    return e; //O(1)

                e = BigInt.sum(e, new BigInt("1"));//O(n)
            }
            throw new Exception("Failed to find a suitable public exponent.");//O(1);
        }
    }
}