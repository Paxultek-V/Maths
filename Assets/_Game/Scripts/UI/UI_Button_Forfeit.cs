using System;
using UnityEngine;

public class UI_Button_Forfeit : MonoBehaviour
{
    public static Action OnForfeitButtonPressed;

    // called by button
    public void Forfeit()
    {
        OnForfeitButtonPressed?.Invoke();
    }
}
