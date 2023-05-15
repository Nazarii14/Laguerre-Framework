using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
//using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;

namespace Program
{
    public static class Program
    {
        public static List<double> Linspace(double startval, double endval, int steps)
        {
            double interval = (endval / (Math.Abs(endval)) * Math.Abs(endval - startval) / (steps - 1));
            return (from val in Enumerable.Range(0, steps) select startval + (val * interval)).ToList();
        }
        public static double f(double t)
        {
            if (0 <= t && t <= 2 * Math.PI) return Math.Sin(t - Math.PI / 2) + 1;
            else if (t > 2 * Math.PI) return 0;
            return 0;
        }
        public static double gaussian_function(double t, double nu, double lamd)
        {
            return 1 / (lamd * Math.Sqrt(Math.PI * 2)) * (Math.Exp(-Math.Pow((t - nu), 2) / (2 * Math.Pow(lamd, 2))));
        }
        public static void WriteTabulateLaguerre(Tuple<List<double>, List<double>> t, string path)
        {
            using (var writer = new StreamWriter(path))
            {
                writer.WriteLine("Tabulate laguerre");
                foreach (var i in t.Item1) writer.WriteLine(i);
                writer.WriteLine("\n");
                foreach (var i in t.Item2) writer.WriteLine(i);
            }
        }
        public static void WriteExperiment(LaguerreFramework lag, string path)
        {
            using (var writer = new StreamWriter(path))
            {
                writer.WriteLine("Experiment");
                var t = lag.Experiment().Item1;
                var arr = lag.Experiment().Item2 as List<double>;
                lag.T = t;
                writer.WriteLine(t);
                for (int i = 0; i < 21; i++)
                {
                    lag.N = i;
                    var arrT = lag.TabulateLaguerre().Item1;
                    var arrLag = lag.TabulateLaguerre().Item2;
                    foreach (var item in arrT) { writer.WriteLine(item); }
                    writer.WriteLine();
                    foreach (var item in arrLag) { writer.WriteLine(item); }
                    writer.WriteLine("\n");
                }
            }
        }
        public static void WriteLagTrans(LaguerreFramework lag, string path)
        {
            using (var writer = new StreamWriter(path))
            {
                writer.WriteLine("Laguerre Transformation");
                foreach (var item in lag.LaguerreTransformation(f))
                {
                    writer.WriteLine(item);
                }
            }
        }
        public static void WriteRevLagTrans(LaguerreFramework lag, List<double> lst, string path)
        {
            using (var writer = new StreamWriter(path))
            {
                writer.WriteLine("Reverse Laguerre Transformation");
                writer.WriteLine(lag.ReverseLaguerreTransformation(lst).Item1);
            }
        }
        public static void WriteSimpleAndReverseTransformation(LaguerreFramework lag, List<double> lst, string path)
        {
            using (StreamWriter writer = new StreamWriter(path))
            {
                writer.WriteLine("Simple and Reverse Laguerre Transformation");
                var arr = lag.LaguerreTransformation(f);
                var res = new List<double>();
                var steps = Linspace(0, 2 * Math.PI, 1000);
                foreach (var i in steps)
                {
                    lag.T = i;
                    res.Add(lag.ReverseLaguerreTransformation(arr).Item1);
                }
                foreach (var item in res)
                {
                    writer.WriteLine(item);
                }
            }
        }
        public static void WriteSimpleAndReverseGausse(LaguerreFramework lag, double a, double b, string path)
        {
            using (StreamWriter writer = new StreamWriter(path))
            {
                writer.WriteLine("Simple and Reverse Gausse Transformation");
                var arr = lag.LaguerreTransformation(gaussian_function, a, b);
                var arrX = new List<double>();
                var arrY = new List<double>();
                var steps = Linspace(0, lag.T, 1000);
                foreach (var i in steps)
                {
                    lag.T = i;
                    arrX.Add(i);
                    arrY.Add(lag.ReverseLaguerreTransformation(arr).Item1);
                }
                foreach (var item in arrX)
                {
                    writer.WriteLine(item);
                }
                writer.WriteLine("\n");
                foreach (var item in arrY)
                {
                    writer.WriteLine(item);
                }
            }
        }

        public static void Main(string[] args)
        {
            string pathRead = "MyFile.txt";

            var parameters = new string[] { };

            using (var reader = new StreamReader(pathRead))
            {
                parameters = reader.ReadLine().Split(';');

                foreach (var i in parameters) { Console.WriteLine(i); }
            }

            var lag1 = new LaguerreFramework(Convert.ToDouble(parameters[0]), Convert.ToInt32(parameters[1]),
                                             Convert.ToInt32(parameters[2]), Convert.ToDouble(parameters[3]),
                                             Convert.ToInt32(parameters[4]), Convert.ToInt32(parameters[5]));
            double nu = Convert.ToDouble(parameters[6]);
            double gamma = Convert.ToDouble(parameters[7]);

            var lst1 = new List<double>() { -5, -2, -1, 0, 3, 5, 7 };
            var lst2 = new List<double>() { 20, 10, 5, 1 };

            if (parameters[8] == "Tabulate laguerre")
                WriteTabulateLaguerre(lag1.TabulateLaguerre(), pathRead);
            else if (parameters[8] == "Experiment")
                WriteExperiment(lag1, pathRead);
            else if (parameters[8] == "Laguerre Transformation")
                WriteLagTrans(lag1, pathRead);
            else if (parameters[8] == "Reverse Laguerre Transformation")
                WriteRevLagTrans(lag1, lst1, pathRead);
            else if (parameters[8] == "Simple and Reverse transformation")
                WriteSimpleAndReverseTransformation(lag1, lst2, pathRead);
            else if (parameters[8] == "Simple and Reverse Gausse Transformation")
                WriteSimpleAndReverseGausse(lag1, nu, gamma, pathRead);
        }
    }
}

public class LaguerreFramework
{
    public LaguerreFramework(double _t = 7, int _n = 5, int _numOfPoints = 100, double _eps = 0.001, int _beta = 2, int _sigma = 4)
    {
        T = _t;
        N = _n;
        NumOfPoints = _numOfPoints;
        Eps = _eps;
        Beta = _beta;
        Sigma = _sigma;
    }
    public double T { get; set; }
    public int N { get; set; }
    public int NumOfPoints { get; set; }
    public double Eps { get; set; }
    public int Beta { get; set; }
    public int Sigma { get; set; }
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

        Eps = _eps;
        Beta = _beta;
        Sigma = _sigma;
        NumOfPoints = _numOfPoints;
        N = _n;
        T = _t;
    }
    public double Laguerre()
    {
        double l0 = Math.Sqrt(Sigma) * Math.Pow(Math.E, -Beta * T / 2);
        double l1 = Math.Sqrt(Sigma) * (1 - Sigma * T) * Math.Pow(Math.E, -Beta * T / 2);
        if (N == 0) return l0;
        else if (N == 1) return l1;
        else
        {
            double l2 = (2 * 2 - 1 - Sigma * T) * l1 / 2 - (2 - 1) * l0 / 2;
            for (var i = 3; i < N + 1; i++)
            {
                l0 = l1;
                l1 = l2;
                l2 = (2 * i - 1 - Sigma * T) * l1 / i - (i - 1) * l0 / i;
            }
            return l2;
        }
    }
    public Tuple<List<double>, List<double>> TabulateLaguerre()
    {
        var steps = Program.Program.Linspace(0, T, NumOfPoints);
        var yS = new List<double>();

        foreach (var i in steps)
        {
            T = i;
            yS.Add(Laguerre());
        }
        var to_return = new Tuple<List<double>, List<double>>(steps, yS);
        return to_return;
    }
    public Tuple<double, List<double>> Experiment()
    {
        T = 0;
        N = 20;
        while (true)
        {
            T += 0.01;
            var res = new List<double>();
            for (var i = 0; i < N + 1; i++)
            {
                var x = Math.Abs(Laguerre());
                if (x < Eps)
                {
                    res.Add(x);
                    if (i == N) return Tuple.Create(T, res);

                }
                else break;
            }
        }
    }
    public double IntegralWithRectangles(Func<double, double> f)
    {
        int alpha = Sigma - Beta;
        NumOfPoints = 100;

        var steps = Program.Program.Linspace(0, T, NumOfPoints);
        var help1 = new List<double>();

        foreach (var i in steps)
        {
            T = i;
            help1.Add(f(i) * Laguerre() * Math.Exp(-alpha * i));
        }
        double res1 = help1.Sum() * T / NumOfPoints;


        NumOfPoints *= 2;
        steps = Program.Program.Linspace(0, T, NumOfPoints);


        var help2 = new List<double>();
        foreach (var i in steps)
        {
            T = i;
            help2.Add(f(i) * Laguerre() * Math.Exp(-alpha * i));
        }
        double res2 = help2.Sum() * T / NumOfPoints;


        while (Math.Abs(res2 - res1) >= Eps)
        {
            NumOfPoints *= 2;
            res1 = res2;

            var help3 = new List<double>();
            foreach (var i in steps)
            {
                T = i;
                help3.Add(f(i) * Laguerre() * Math.Exp(-alpha * i));
            }
            res2 = help3.Sum() * T / NumOfPoints;
        }
        return res2;
    }
    public double IntegralWithRectangles(Func<double, double, double, double> f, double a, double b)
    {
        int alpha = Sigma - Beta;
        NumOfPoints = 100;

        var steps = Program.Program.Linspace(0, T, NumOfPoints);
        var help1 = new List<double>();

        foreach (var i in steps)
        {
            T = i;
            help1.Add(f(i, a, b) * Laguerre() * Math.Exp(-alpha * i));
        }
        double res1 = help1.Sum() * T / NumOfPoints;


        NumOfPoints *= 2;
        steps = Program.Program.Linspace(0, T, NumOfPoints);


        var help2 = new List<double>();
        foreach (var i in steps)
        {
            T = i;
            help2.Add(f(i, a, b) * Laguerre() * Math.Exp(-alpha * i));
        }
        double res2 = help2.Sum() * T / NumOfPoints;


        while (Math.Abs(res2 - res1) >= Eps)
        {
            NumOfPoints *= 2;
            res1 = res2;

            var help3 = new List<double>();
            foreach (var i in steps)
            {
                T = i;
                help3.Add(f(i, a, b) * Laguerre() * Math.Exp(-alpha * i));
            }
            res2 = help3.Sum() * T / NumOfPoints;
        }
        return res2;
    }
    public List<double> LaguerreTransformation(Func<double, double> f)
    {
        var to_return = new List<double>();
        var upperBound = N + 1;

        for (var i = 0; i < upperBound; i++)
        {
            N = i;
            to_return.Add(IntegralWithRectangles(f));
        }
        return to_return;
    }
    public List<double> LaguerreTransformation(Func<double, double, double, double> f, double a, double b)
    {
        var to_return = new List<double>();
        var upperBound = N + 1;

        for (var i = 0; i < upperBound; i++)
        {
            N = i;
            to_return.Add(IntegralWithRectangles(f, a, b));
        }
        return to_return;
    }
    public Tuple<double, List<double>> ReverseLaguerreTransformation(List<double> lst)
    {
        var to_return = new List<double>();
        for (int i = 0; i < lst.Count; i++)
        {
            N = i;
            to_return.Add(lst[i] * Laguerre());
        }
        return Tuple.Create(to_return.Sum(), lst);
    }
}