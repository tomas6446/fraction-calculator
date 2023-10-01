using UnityEngine;

public class ButtonHandler : MonoBehaviour
{
    public Calculator calculator;

    public void OnClick(string val)
    {
        calculator.UpdateInput(val);
    }
}
