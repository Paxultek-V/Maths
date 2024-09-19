using System;
using UnityEngine;

public class UI_ButtonDebug_NextLevel : MonoBehaviour
{
    public static Action OnDebugNextLevelButtonPressed;

    public void PressNextLevelButton()
    {
        OnDebugNextLevelButtonPressed?.Invoke();
    }
}
