using System;

public class Fraction
{
    public int Numerator { get; set; }
    public int Denominator { get; set; }

    public bool IsNonEmpty()
    {
        return Numerator != 0 || Denominator != 0;
    }

    public override string ToString()
    {
        if (Numerator == 0)
            return string.Empty;

        return Denominator == 0
            ? $"<sup>{Numerator}</sup>/<sub></sup>"
            : $"<sup>{Numerator}</sup>/<sub>{Denominator}</sub>";
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
