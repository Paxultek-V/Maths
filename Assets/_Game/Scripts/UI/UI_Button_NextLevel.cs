using System;
using UnityEngine;

public class UI_Button_NextLevel : MonoBehaviour
{
    public static Action OnNextLevelButtonPressed;

    // called by button
    public void NextLevel()
    {
        OnNextLevelButtonPressed?.Invoke();
    }
}
