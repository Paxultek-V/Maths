using System;
using UnityEngine;
using UnityEngine.UI;

public class UI_Button_ToggleVibration : MonoBehaviour
{
    public static Action OnToggleVibration;

    [SerializeField] private Image m_optionEnabledImage = null;
    
    [SerializeField] private Image m_optionDisabledImage = null; 
    
    private void OnEnable()
    {
        Option.OnSendOptionState += OnSendOptionState;
    }

    private void OnDisable()
    {
        Option.OnSendOptionState -= OnSendOptionState;
    }

    private void OnSendOptionState(OptionType optionType, bool state)
    {
        if (optionType == OptionType.Vibration)
        {
            m_optionEnabledImage.gameObject.SetActive(state);
            m_optionDisabledImage.gameObject.SetActive(!state);
        }
    }
    
    // Called by button
    public void ToggleVibration()
    {
        OnToggleVibration?.Invoke();
    }
}
