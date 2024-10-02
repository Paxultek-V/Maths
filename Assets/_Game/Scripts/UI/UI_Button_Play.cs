using System;
using UnityEngine;

public class UI_Button_Play : MonoBehaviour
{
    public static Action OnPlayButtonClicked; 
    
    //Called by button
    public void ClickOnPlayButton()
    {
        OnPlayButtonClicked?.Invoke();
    }
}
