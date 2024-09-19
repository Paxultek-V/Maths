using System;
using UnityEngine;
using UnityEngine.UI;

public class UI_Button_ToggleSound : MonoBehaviour
{
    public static Action OnToggleSound;

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
        if (optionType == OptionType.Sound)
        {
            m_optionEnabledImage.gameObject.SetActive(state);
            m_optionDisabledImage.gameObject.SetActive(!state);
        }
    }

    // Called by button
    public void ToggleSound()
    {
        OnToggleSound?.Invoke();
    }
}
