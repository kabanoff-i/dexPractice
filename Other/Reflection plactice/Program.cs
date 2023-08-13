using System;
using System.Reflection;

class Triangle
{
    private double sideA;
    private double sideB;
    private double sideC;

    public Triangle(double a, double b, double c)
    {
        if (!IsValidTriangle(a, b, c))
        {
            throw new ArgumentException("Invalid triangle sides");
        }

        sideA = a;
        sideB = b;
        sideC = c;
    }

    public double SideA
    {
        get { return sideA; }
        set
        {
            if (IsValidSide(value))
            {
                sideA = value;
            }
            else
            {
                throw new ArgumentException("Invalid side length");
            }
        }
    }

    public double SideB
    {
        get { return sideB; }
        set
        {
            if (IsValidSide(value))
            {
                sideB = value;
            }
            else
            {
                throw new ArgumentException("Invalid side length");
            }
        }
    }

    public double SideC
    {
        get { return sideC; }
        set
        {
            if (IsValidSide(value))
            {
                sideC = value;
            }
            else
            {
                throw new ArgumentException("Invalid side length");
            }
        }
    }

    public double CalculatePerimeter()
    {
        return sideA + sideB + sideC;
    }

    public double CalculateArea()
    {
        double s = CalculatePerimeter() / 2;
        return Math.Sqrt(s * (s - sideA) * (s - sideB) * (s - sideC));
    }

    private bool IsValidTriangle(double a, double b, double c)
    {
        return a + b > c && a + c > b && b + c > a;
    }

    private bool IsValidSide(double side)
    {
        return side > 0;
    }
}

class Program
{
    static string GetTypePrivateFields(object myObject)
    {
        if (myObject == null)
            throw new Exception(nameof(myObject));

        Type type = myObject.GetType();
        var properties = type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
        var values = properties.Select(x => $"{x.Name} : {x.GetValue(myObject)}");
        
        return string.Join("\n" , string.Join("\n", values));
        
    }
    static void Main()
    {
        Triangle triangle = new Triangle(3, 4, 5);
        Console.WriteLine(GetTypePrivateFields(triangle));
    }
}