using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Calculator : MonoBehaviour
{
    public TextMeshProUGUI inputField;
    public TextMeshProUGUI outputField;
    private readonly RpnEvaluator _rpnEvaluator = new();
    private Fraction _currentFraction = new();
    private InputState _currentState = InputState.Numerator;
    private List<string> _equationParts = new();
    private List<Fraction> _fractionList = new();
    private Stack<string> _operatorStack = new();

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
            case "C":
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
        UpdateInputField();
    }

    private void CalculateResult()
    {
        try
        {
            _fractionList.Add(_currentFraction);
            var result = _rpnEvaluator.Evaluate(_fractionList, _operatorStack);
            inputField.text =
                $"<align=left>{inputField.text} =</align>\n<align=right>{result}</align>";
            outputField.text += inputField.text;
        }
        catch (Exception e)
        {
            inputField.text = $"Error: {e.Message}";
        }
    }

    private void AddCurrentFractionAndOperator(string operatorText)
    {
        if (_currentFraction.IsNonEmpty())
        {
            _fractionList.Add(_currentFraction);
            _equationParts.Add(_currentFraction.ToString());
            _currentFraction = new Fraction();
        }

        _equationParts.Add(operatorText);
        _operatorStack.Push(operatorText);

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
        _fractionList = new List<Fraction>();
        _equationParts = new List<string>();
        _operatorStack = new Stack<string>();

        inputField.text = "";
        _currentState = InputState.Numerator;
    }

    private void ClearLastCharacter()
    {
    }

    private void UpdateInputField()
    {
        var displayText = string.Join(" ", _equationParts);
        if (_currentFraction.IsNonEmpty()) displayText += " " + _currentFraction;
        inputField.text = displayText.Trim();
    }

    private enum InputState
    {
        Numerator,
        Denominator,
        Operator
    }
}
