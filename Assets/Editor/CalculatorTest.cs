using NUnit.Framework;

[TestFixture]
public class CalculatorTest
{
    [SetUp]
    public void Setup()
    {
        _calculator = new Calculator();
    }

    private Calculator _calculator;

    [Test]
    public void AddTest()
    {
    }
}
