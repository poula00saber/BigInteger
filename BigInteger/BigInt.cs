
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




        public  BigInt Multiplication(BigInt f, BigInt s)
        {
            int n = Math.Max(f.arr.Count, s.arr.Count);

            if (f.arr.Count< s.arr.Count)
                AddZerosFirst(ref f, s.arr.Count - f.arr.Count);
            else
                AddZerosFirst(ref s, f.arr.Count - s.arr.Count);

            BigInt a = new BigInt();
            BigInt b = new BigInt();
            SplitBigInt(f, ref a, ref b);
            BigInt c = new BigInt();
            BigInt d = new BigInt();
            SplitBigInt(s, ref c, ref d);

            //BigInt  power= new BigInt((Math.Pow(10, n / 2)).ToString());

            BigInt x =AddZerosLast(a,n/2);
            x= sum(x,b);

            BigInt y = AddZerosLast(c, n / 2);
            y= sum (y,d);


            BigInt ac = Multiplication(a, c);
            BigInt bd = Multiplication(b, d);
            BigInt a_plus_b = sum(a, b);
            BigInt c_plus_d = sum(c, d);
            BigInt ad_Mult_bc = Multiplication(a_plus_b,c_plus_d);



            return x;
        }


        void AddZerosFirst(ref BigInt n, int count)
        {
            for (int i = 0; i < count; i++)
            {
                n.arr.AddFirst(0);
            }
        }
        BigInt AddZerosLast(BigInt n, int count)
        {
            for (int i = 0; i < count; i++)
            {
                n.arr.AddLast(0);
            }
            return n;
        }
        void SplitBigInt(BigInt n, ref BigInt a, ref BigInt b)
        {
            Int128 mid = n.arr.Count / 2;
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
            while(lastN != null || lastM != null || carry > 0)
            {
                int s = 0;
                if (lastN != null && lastM != null)
                {
                    s = carry + lastN.Value - lastM.Value;
                    lastN = lastN.Previous;
                    lastM = lastM.Previous;
                }

                else if(lastN == null)
                {
                    s = 0;
                    lastM = lastM.Previous;
                }
                else if(lastM == null){
                    s = lastN.Value - 0;
                    lastN = lastN.Previous;
                }

                if(s < 0){
                    s *= -1;
                }
                carry = s/10;
                sum.arr.AddFirst(s % 10 );
            }
            return sum;




        return null;
       }



    override public String ToString(){
        return string.Join("", arr);
    }
        public override bool Equals(object obj)
        {
            LinkedListNode<int> p1 = this.arr.Last;
            BigInt secondObj = (BigInt) obj;
            LinkedListNode<int> p2 = secondObj.arr.Last;
            while(p1 != null && p2 != null){
            if(p1.Value != p2.Value){
                return false;
            }else{
                p1 = p1.Previous;
                p2 = p2.Previous;
            }
            }
            return true;
        }

    }
}


