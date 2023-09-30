using System;
using System.Collections.Generic;

public class RpnEvaluator
{
    public Fraction Evaluate(List<Fraction> fractions, Stack<string> operators)
    {
        var stack = new Stack<Fraction>();

        foreach (var fraction in fractions)
        {
            stack.Push(fraction);

            if (stack.Count >= 2 && operators.Count > 0)
            {
                var operatorSymbol = operators.Pop();
                var b = stack.Pop();
                var a = stack.Pop();

                switch (operatorSymbol)
                {
                    case "+":
                        stack.Push(a + b);
                        break;
                    case "-":
                        stack.Push(a - b);
                        break;
                    case "*":
                        stack.Push(a * b);
                        break;
                    case "/":
                        if (b.Numerator == 0)
                            throw new DivideByZeroException("Denominator cannot be zero");
                        stack.Push(a / b);
                        break;
                    default:
                        throw new InvalidOperationException($"Unknown operator: {operatorSymbol}");
                }
            }
        }

        if (stack.Count != 1 || operators.Count != 0) throw new InvalidOperationException("Invalid expression");

        return stack.Pop();
    }
}
