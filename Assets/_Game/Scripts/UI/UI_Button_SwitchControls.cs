using System;
using UnityEngine;

public class UI_Button_SwitchControls : MonoBehaviour
{
    public static Action OnSwitchControlButtonPressed;

    public void SwitchControls()
    {
        OnSwitchControlButtonPressed?.Invoke();
    }
}
