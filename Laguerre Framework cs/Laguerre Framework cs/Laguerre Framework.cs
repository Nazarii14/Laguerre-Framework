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
        NumOfPoints = _numOfPoints;
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
    
    //@WeaRD276
    public double _laguerre()
    {
        return t;
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
    public double _integral_with_rectangles()
    {
        return t;
    }

    //@volodia_tech
    public double _laguerre_transformation()
    {
        return t;
    }

    //@zbyrachnosochkiv
    public double _reverse_laguerre_transformation() 
    {
        return t;
    }
}