using System;
using UnityEngine;

public class UI_Button_Retry : MonoBehaviour
{
    public static Action OnRetryButtonClicked; 
    
    //Called by button
    public void ClickOnRetryButton()
    {
        OnRetryButtonClicked?.Invoke();
    }
}
