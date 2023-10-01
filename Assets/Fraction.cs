using System;

public class Fraction
{
    public int Numerator { get; set; }
    public int Denominator { get; set; }

    public Fraction Simplify()
    {
        var gcd = Gcd(Numerator, Denominator);
        return new Fraction
        {
            Numerator = Numerator / gcd,
            Denominator = Denominator / gcd
        };
    }

    private int Gcd(int a, int b)
    {
        while (b != 0)
        {
            var temp = b;
            b = a % b;
            a = temp;
        }

        return a;
    }

    public bool IsNonEmpty()
    {
        return Numerator != 0 || Denominator != 0;
    }

    public override string ToString()
    {
        return Numerator == 0 && Denominator == 0
            ? "<sup>\u25a1</sup>/<sup>\u25a1</sup>"
            : $"<sup>{Numerator}</sup>/<sub>{IsZero()}</sub>";
    }

    private string IsZero()
    {
        return Denominator == 0 ? "\u25A1" : Denominator.ToString();
    }

    public static Fraction operator +(Fraction a, Fraction b)
    {
        return new Fraction
        {
            Numerator = a.Numerator * b.Denominator + b.Numerator * a.Denominator,
            Denominator = a.Denominator * b.Denominator
        };
    }

    public static Fraction operator -(Fraction a, Fraction b)
    {
        return new Fraction
        {
            Numerator = a.Numerator * b.Denominator - b.Numerator * a.Denominator,
            Denominator = a.Denominator * b.Denominator
        };
    }

    public static Fraction operator *(Fraction a, Fraction b)
    {
        return new Fraction
        {
            Numerator = a.Numerator * b.Numerator,
            Denominator = a.Denominator * b.Denominator
        };
    }

    public static Fraction operator /(Fraction a, Fraction b)
    {
        if (b.Numerator == 0) throw new DivideByZeroException();
        return new Fraction
        {
            Numerator = a.Numerator * b.Denominator,
            Denominator = a.Denominator * b.Numerator
        };
    }
}
