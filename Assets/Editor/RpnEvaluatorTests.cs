using System.Collections.Generic;
using NUnit.Framework;

[TestFixture]
public class RpnEvaluatorTests
{
    [SetUp]
    public void Setup()
    {
        _evaluator = new RpnEvaluator();
    }

    private RpnEvaluator _evaluator;

    [Test]
    public void AddTest()
    {
        var fractions = new Queue<Fraction>();
        var operators = new Queue<string>();

        fractions.Enqueue(new Fraction { Numerator = 2, Denominator = 1 });
        operators.Enqueue("+");
        fractions.Enqueue(new Fraction { Numerator = 2, Denominator = 1 });

        var result = _evaluator.Evaluate(fractions, operators);
        Assert.AreEqual(new Fraction { Numerator = 4, Denominator = 1 }, result);
    }

    [Test]
    public void SubtractTest()
    {
        var fractions = new Queue<Fraction>();
        var operators = new Queue<string>();

        fractions.Enqueue(new Fraction { Numerator = 2, Denominator = 1 });
        operators.Enqueue("-");
        fractions.Enqueue(new Fraction { Numerator = 2, Denominator = 1 });

        var result = _evaluator.Evaluate(fractions, operators);
        Assert.AreEqual(new Fraction { Numerator = 0, Denominator = 1 }, result);
    }

    [Test]
    public void MultiplyTest()
    {
        var fractions = new Queue<Fraction>();
        var operators = new Queue<string>();

        fractions.Enqueue(new Fraction { Numerator = 2, Denominator = 1 });
        operators.Enqueue("-");
        fractions.Enqueue(new Fraction { Numerator = 2, Denominator = 1 });

        var result = _evaluator.Evaluate(fractions, operators);
        Assert.AreEqual(new Fraction { Numerator = 4, Denominator = 1 }, result);
    }

    [Test]
    public void DivisionTest()
    {
        var fractions = new Queue<Fraction>();
        var operators = new Queue<string>();

        fractions.Enqueue(new Fraction { Numerator = 2, Denominator = 1 });
        operators.Enqueue("-");
        fractions.Enqueue(new Fraction { Numerator = 2, Denominator = 1 });

        var result = _evaluator.Evaluate(fractions, operators);
        Assert.AreEqual(new Fraction { Numerator = 1, Denominator = 1 }, result);
    }
}
