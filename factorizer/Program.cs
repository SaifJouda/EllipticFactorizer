//Saif Jouda

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace factorizer
{
    class Program
    {
        //Euclidean Method
        private static long GCD(long a, long b)
        {
            while(a != 0 && b!=0)
            {
                if(a>b) a%=b;
                else b%=a;
            }
            return a|b;
        }

        static List<long> primeSieve(long n)
        {
            bool[] primeMark = new bool[n];
            Array.Fill(primeMark, true);

            List<long> primes = new List<long>();

            for(long i=2; i<n;i++)
            {
                if(primeMark[i]==true)
                {
                    primes.Add(i);
                    for(long j=i;j<n;j+=i) primeMark[j]=false;
                }
            }
            return primes;
        }

        static long[] modInverse(long a, long b)
        {
            if(b==0) return new long[] {(long)1,(long)0,a};
            long q=a/b;
            long r=a%b;
            long[] rec = modInverse(b,r);
            return new long[] {rec[1],rec[0]-q*rec[1],rec[2]};
        }

        static long[] elpAdd(long x1, long y1, long inf1, long x2, long y2, long inf2, long a, long m)
        {
            if (inf1==0) return new long[]{x2,y2,inf2};
            if (inf2==0) return new long[]{x1,y1,inf1};
            long diffTop,diffBot;
            if(x1==x2 && y1==y2)
            {
                diffTop=(3*x1*x1+a)%m;
                diffBot=(2*y1)%m;
            }
            else
            {
                diffTop=(y2-y1)%m;
                diffBot=(x2-x1)%m;
            }
            if(diffBot==0) return new long[]{0,1,0};
            if(diffBot<0) diffBot=m+diffBot;
            long[] diffInv=modInverse(diffBot,m);
        
            if(diffInv[2]>1) return new long[]{0,0,diffBot};
    
    
            long diff=diffTop*diffInv[0];
            long x3=(diff*diff-x1-x2)%m;
            if(x3<0)x3=m+x3;
            long y3=((diff*(x1-x3)-y1))%m;
            if(y3<0)y3=m+y3;
            return new long[]{x3,y3,1};
        }

        static long[] elpMult(long k,long[] p, long a, long m)
        {
            long[] result = new long[]{0,1,0};
            while(k>0)
            {
                if(p[2]>1) return p;
                if(k%2==1) result=elpAdd(p[0],p[1],p[2],result[0],result[1],result[2],a,m);
                k=(long)Math.Floor((double)k/2);
                p=elpAdd(p[0],p[1],p[2],p[0],p[1],p[2],a,m);    
            }
            return result;
        }

        static long randLong(long n)
        {
            Random randLongNumGen = new Random();
            long result = randLongNumGen.Next((Int32)(0 >> 32), (Int32)(n >> 32));
            result = (result << 32);
            result = result | (long)randLongNumGen.Next((Int32)0, (Int32)n);
            return result;
        }

        static long lenstraFactor(long n)
        {
            //Create random elliptic curve
            long limit=1000;
            long gcd=n;
            long x0=randLong(n-1);
            long y0=randLong(n-1);
            long a=randLong(n-1);
            long b=0;
            Random randNumGen = new Random();
            while(gcd==n)
            {
                long anti=-1;
                while(anti<0)
                {
                    x0=randLong(n-1);
                    y0=randLong(n-1);
                    a=randLong(n-1);
                    b=(y0*y0-x0*x0*x0-a*x0)%n;
                    anti=4*a*a*a+27*b*b;
                }
                gcd = GCD(4*a*a*a+27*b*b,n);
        
            }
            //If we are lucky individuals
            if(gcd>1) return gcd;
            //Create a list of primes
            List<long> primes = primeSieve(limit);
            //
            long[] q = new long[]{x0,y0,1};
            for(int i=0; i<primes.Count;i++)
            {
                long pp = primes[i];
                while(pp<limit)
                {
                    q=elpMult(primes[i], q,a,n);
                    if(q[2]>1) return GCD(q[2],n);
                    pp=primes[i]+pp;
                }
            } 
            return 0;
        }

        static void returnFactors(long n)
        {
            int i=1;
            long p = lenstraFactor(n);
            while(p==0 & i<100)
            {
                p = lenstraFactor(n);
                i++;
            }
            if(p==0) Console.WriteLine("No factors found");
            else 
            {
                Console.WriteLine("p= "+p+" , q= "+n/p);  
                Console.WriteLine("Attempts: "+i);
            }
        }

        static bool menu()
        {
            Console.Write("Factor: ");
            long entered=(long)Convert.ToDouble(Console.ReadLine());
            if(entered<3) return false;
            Stopwatch sw;
            sw = Stopwatch.StartNew();
            returnFactors(entered);
            Console.WriteLine("Time: "+sw.ElapsedMilliseconds+" ms");
            sw.Stop();
            return true;
        }

        static void Main(string[] args)
        {
            bool inCommand=true;
            while(inCommand) inCommand=menu();
            
        }
    }
}
