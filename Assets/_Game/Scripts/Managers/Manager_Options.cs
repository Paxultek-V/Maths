using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public enum OptionType
{
    Sound, 
    Vibration
}

[Serializable]
public class Option
{
    public static Action<OptionType, bool> OnSendOptionState;
    
    public string saveKey = "";

    public bool state;

    public void LoadOption()
    {
        if (PlayerPrefs.HasKey(saveKey))
            state = PlayerPrefs.GetInt(saveKey) == 1;
        else
        {
            state = true;
            SaveOption();
        }
    }
    
    public void SaveOption()
    {
        PlayerPrefs.SetInt(saveKey, state == true ? 1 : 0);
    }

    public void ToggleOption()
    {
        state = !state;
        SaveOption();
    }
}

public static class OptionListExtention
{
    public static void LoadOptions(this Dictionary<OptionType, Option> optionDictionary)
    {
        foreach (var option in optionDictionary)
        {
            option.Value.LoadOption();
        }
    }
    
    public static void SaveOptions(this Dictionary<OptionType, Option> optionDictionary)
    {
        foreach (var option in optionDictionary)
        {
            option.Value.SaveOption();
        }
    }
}

public class Manager_Options : SerializedMonoBehaviour
{
    [SerializeField] private Dictionary<OptionType, Option> m_optionDictionary = null;

    private void Awake()
    {
        m_optionDictionary.LoadOptions();
    }

    private void OnEnable()
    {
        UI_Button_ToggleSound.OnToggleSound += OnToggleSound;
        UI_Button_ToggleVibration.OnToggleVibration += OnToggleVibration;
    }

    private void OnDisable()
    {
        UI_Button_ToggleSound.OnToggleSound -= OnToggleSound;
        UI_Button_ToggleVibration.OnToggleVibration -= OnToggleVibration;
    }

    private void Start()
    {
        foreach (var option in m_optionDictionary)
        {
            Option.OnSendOptionState?.Invoke(option.Key, option.Value.state);
        }
    }

    private void OnToggleSound()
    {
        ToggleOption(OptionType.Sound);
    }

    private void OnToggleVibration()
    {
        ToggleOption(OptionType.Vibration);
    }

    private void ToggleOption(OptionType optionType)
    {
        Option option = m_optionDictionary[optionType];
        option.ToggleOption();
        Option.OnSendOptionState?.Invoke(optionType, option.state);
    }
    
    private void OnApplicationPause(bool pauseStatus)
    {
        m_optionDictionary.SaveOptions();
    }
}
