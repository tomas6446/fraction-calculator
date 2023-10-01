using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Calculator : MonoBehaviour
{
    private const int ERROR_FONT_SIZE = 20;
    private const int FONT_SIZE = 40;
    public TextMeshProUGUI inputField;
    public TextMeshProUGUI outputField;
    private readonly RpnEvaluator _rpnEvaluator = new();
    private Fraction _currentFraction = new();
    private InputState _currentState = InputState.Numerator;
    private List<string> _equationParts = new();
    private Queue<Fraction> _fractionQueue = new();
    private Queue<string> _operatorQueue = new();

    public void UpdateInput(string newText)
    {
        switch (newText)
        {
            case "=":
                CalculateResult();
                return;
            case "AC":
                ClearText();
                return;
            case "CLR":
                ClearLastCharacter();
                return;
            case "<-":
                MoveToPreviousState();
                return;
            case "->":
                MoveToNextState();
                return;
        }

        if ("+-:*".Contains(newText))
        {
            AddCurrentFractionAndOperator(newText);
            UpdateInputField();
            return;
        }

        HandleText(newText);
    }

    private void CalculateResult()
    {
        try
        {
            _fractionQueue.Enqueue(_currentFraction);
            var result = _rpnEvaluator.Evaluate(_fractionQueue, _operatorQueue);
            string res;
            switch (result.Numerator)
            {
                case < 0:
                    result.Numerator *= -1;
                    res = "-" + result.Simplify();
                    break;
                default:
                {
                    if (result.Denominator < 0)
                    {
                        result.Denominator *= -1;
                        res = "-" + result.Simplify();
                    }
                    else
                    {
                        res = result.Simplify().ToString();
                    }

                    break;
                }
            }

            if (!_currentFraction.IsNonEmpty()) return;
            inputField.text =
                $"<align=left>{inputField.text}</align>\n<align=right>= {res}</align>";
            outputField.text = inputField.text;
        }
        catch (Exception e)
        {
            inputField.fontSize = ERROR_FONT_SIZE;
            inputField.text = $"<align=left>Error: {e.Message}</align>";
        }
    }

    private void AddCurrentFractionAndOperator(string operatorText)
    {
        if (_currentFraction.IsNonEmpty())
        {
            _fractionQueue.Enqueue(_currentFraction);
            _equationParts.Add(_currentFraction.ToString());
            _currentFraction = new Fraction();
        }

        _equationParts.Add(operatorText);
        _operatorQueue.Enqueue(operatorText);

        _currentFraction = new Fraction();
        _currentState = InputState.Numerator;
    }


    private void MoveToNextState()
    {
        _currentState = _currentState switch
        {
            InputState.Numerator => InputState.Denominator,
            InputState.Denominator => InputState.Operator,
            InputState.Operator => InputState.Numerator,
            _ => _currentState
        };
    }

    private void MoveToPreviousState()
    {
        _currentState = _currentState switch
        {
            InputState.Numerator => InputState.Operator,
            InputState.Denominator => InputState.Numerator,
            InputState.Operator => InputState.Denominator,
            _ => _currentState
        };
    }

    private void HandleText(string newText)
    {
        if (int.TryParse(newText, out var newDigit))
            switch (_currentState)
            {
                case InputState.Numerator:
                    _currentFraction.Numerator = _currentFraction.Numerator * 10 + newDigit;
                    break;
                case InputState.Denominator:
                    _currentFraction.Denominator = _currentFraction.Denominator * 10 + newDigit;
                    break;
                case InputState.Operator:
                    _currentFraction = new Fraction { Numerator = newDigit };
                    _currentState = InputState.Numerator;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

        UpdateInputField();
    }


    private void ClearText()
    {
        _currentFraction = new Fraction();
        _fractionQueue = new Queue<Fraction>();
        _equationParts = new List<string>();
        _operatorQueue = new Queue<string>();

        inputField.text = "";
        _currentState = InputState.Numerator;
    }

    private void ClearLastCharacter()
    {
        ClearText();
        outputField.text = "";
    }

    private void UpdateInputField()
    {
        var displayText = string.Join(" ", _equationParts);
        if (_currentFraction.IsNonEmpty() || _currentState == InputState.Numerator)
            displayText += " " + _currentFraction;
        inputField.text = displayText.Trim();
        inputField.fontSize = FONT_SIZE;
    }

    private enum InputState
    {
        Numerator,
        Denominator,
        Operator
    }
}
