using System;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;

namespace Program
{
    public static class Program
    {
        public static double f(double t)
        {
            if (0 <= t && t <= 2 * Math.PI)
            {
                return Math.Sin(t - Math.PI / 2) + 1;
            }

            else if (t > 2 * Math.PI)
            {
                return 0;
            }
            return 0;
        }
            
        public static void Main(string[] args)
        {
            var lag1 = new Laguerre(7, 5, 100, 0.1, 2, 4);
            //Console.WriteLine(lag1.tabulate_laguerre());

            var lag2 = new Laguerre(5, 20, 100, 0.01);
            //Console.WriteLine(lag2.laguerre_transformation(f));

            var lag3 = new Laguerre(10, 20, 100, 0.01);
            //Console.WriteLine(lag3.reverse_laguerre_transformation(np.array([2, 5, 10, 0, -1])));
        }
    }


}

public class Laguerre
{
    private double t;
    private int n;
    private int numOfPoints;
    private double eps;
    private int beta;
    private int sigma;

    public Laguerre(double _t = 20, int _n = 10, int _numOfPoints = 100, double _eps = 0.1, int _beta = 2, int _sigma = 4)
    {
        t = _t;
        n = _n;
        numOfPoints = _numOfPoints;
        eps = _eps;
        beta = _beta;
        sigma = _sigma;
    }

    public double GetT => t;
    public int GetN => n;
    public double GetEps => eps;
    public int GetBeta => beta;
    public int GetSigma => sigma;
    public int GetNumOfPoints => numOfPoints;

    public void SetT(double _t) { t = _t; }
    public void SetN(int _n) { n = _n; }
    public void SetEps(double _eps) { eps = _eps; }
    public void SetBeta(int _beta) { beta = _beta; }
    public void SetSigma(int _sigma) { sigma = _sigma; }
    public void SetNumOfPoints(int _numOfPoints) { numOfPoints = _numOfPoints; }


    public void InputData()
    {
        Console.WriteLine("Enter t: ");
        double _t = Convert.ToDouble(Console.ReadLine());
        
        Console.WriteLine("Enter n: ");
        int _n = Convert.ToInt32(Console.ReadLine());

        Console.WriteLine("Enter number of points: ");
        int _numOfPoints = Convert.ToInt32(Console.ReadLine());

        Console.WriteLine("Enter epsilon: ");
        double _eps = Convert.ToDouble(Console.ReadLine());

        Console.WriteLine("Enter beta: ");
        int _beta = Convert.ToInt32(Console.ReadLine());

        Console.WriteLine("Enter sigma: ");
        int _sigma = Convert.ToInt32(Console.ReadLine());

        eps = _eps;
        beta = _beta;
        sigma = _sigma;
        numOfPoints = _numOfPoints;
        n = _n;
        t = _t;
    }

    public static double[] linspace(double startval, double endval, int steps)
    {
        double interval = (endval / (Math.Abs(endval)) * Math.Abs(endval - startval) / (steps - 1));
        return (from val in Enumerable.Range(0, steps)
                select startval + (val * interval)).ToArray();
    }
    //@WeaRD276
    public double _laguerre()
    {
        double l0 = Math.Sqrt(sigma) * Math.Pow(Math.E, -beta * t / 2);
        double l1 = Math.Sqrt(sigma) * (1 - sigma * t) * Math.Pow(Math.E, -beta * t / 2);
        if (n == 0)
            return l0;
        else if (n == 1)
            return l1;
        else
        {
            double l2 = (2 * 2 - 1 - sigma * t) * l1 / 2 - (2 - 1) * l0 / 2;
            for (var i = 3; i < n + 1; i++)
            {
                l0 = l1;
                l1 = l2;
                l2 = (2 * j - 1 - sigma * t) * l1 / j - (j - 1) * l0 / j;
            }
            return l2;
        }
    }

    //@ayatsuliak
    public double _tabulate_laguerre() 
    {
        return t;
    }

    //@roman_borovets
    public double _experiment()
    {
        return t;
    }

    //@zbyrachnosochkiv
    public double _integral_with_rectangles(Func<double, double> f)
    {
        int alpha = sigma - beta;
        SetNumOfPoints(100);

        double[] steps = linspace(0, t, numOfPoints);

        double[] help1 = { };
        foreach (var i in steps) 
        {
            t = i;
            help1.Append(f(i) * _laguerre() * Math.Exp(-alpha * i));
        }
        double res1 = help1.Sum() * t / numOfPoints;


        numOfPoints *= 2;
        steps = linspace(0, t, numOfPoints);


        double[] help2 = { };
        foreach (var i in steps)
        {
            t = i;
            help2.Append(f(i) * _laguerre() * Math.Exp(-alpha * i));
        }
        double res2 = help2.Sum() * t / numOfPoints;


        while (Math.Abs(res2-res1) >= eps)
        {
            numOfPoints *= 2;
            res1 = res2;

            double[] help3 = { };
            foreach (var i in steps)
            {
                t = i;
                help3.Append(f(i) * _laguerre() * Math.Exp(-alpha * i));
            }
            res2 = help3.Sum() * t / numOfPoints;
        }
        return res2;
    }

    //@volodia_tech
    public double _laguerre_transformation()
    {
        return t;
    }

    //@zbyrachnosochkiv
    public double _reverse_laguerre_transformation(double[] lst) 
    {
        double[] to_return = { };
        for(int i = 0; i < lst.Length; i++)
        {
            n = i;
            to_return.Append(lst[i] * _laguerre());
        }
        return to_return.Sum();
    }
}