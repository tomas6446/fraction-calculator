using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Calculator : MonoBehaviour
{
    public TextMeshProUGUI inputField;
    public TextMeshProUGUI outputField;
    private readonly RpnEvaluator _rpnEvaluator = new();
    private readonly int ERROR_FONT_SIZE = 20;
    private readonly int FONT_SIZE = 40;
    private Fraction _currentFraction = new();
    private InputState _currentState = InputState.Numerator;
    private List<string> _equationParts = new();
    private Stack<Fraction> _fractionStack = new();
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

        if ("+-/*".Contains(newText))
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
            _fractionStack.Push(_currentFraction);
            var result = _rpnEvaluator.Evaluate(_fractionStack, _operatorQueue);
            if (!_currentFraction.IsNonEmpty()) return;
            inputField.text =
                $"<align=left>{inputField.text}</align>\n<align=right>= {result.Simplify()}</align>";
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
            _fractionStack.Push(_currentFraction);
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
        _fractionStack = new Stack<Fraction>();
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
