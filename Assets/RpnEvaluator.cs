using System;
using System.Collections.Generic;

public class RpnEvaluator
{
    public Fraction Evaluate(Queue<Fraction> fractions, Queue<string> operators)
    {
        var resultStack = new Stack<Fraction>();

        while (fractions.Count > 0)
        {
            var current = fractions.Dequeue();
            if (current.Numerator == 0 || current.Denominator == 0)
                throw new DivideByZeroException("Denominator cannot be zero");
            if (operators.Count > 0 && resultStack.Count > 0)
            {
                var operatorSymbol = operators.Dequeue();
                var a = resultStack.Pop();
                current = operatorSymbol switch
                {
                    "+" => a + current,
                    "-" => a - current,
                    "*" => a * current,
                    "/" => a / current,
                    _ => throw new InvalidOperationException($"Unknown operator: {operatorSymbol}")
                };
            }

            resultStack.Push(current);
        }

        if (resultStack.Count != 1 || operators.Count != 0)
            throw new InvalidOperationException("Invalid expression");

        return resultStack.Pop();
    }
}
