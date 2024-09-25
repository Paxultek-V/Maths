using TMPro;
using UnityEngine;

public class UI_Text_ControlsType : MonoBehaviour
{
    private TMP_Text m_controlsTypeText;


    private void OnEnable()
    {
        m_controlsTypeText = GetComponent<TMP_Text>();
        InputsDialController.OnSendControlsMode += OnSendControlsMode;
    }

    private void OnDisable()
    {
        InputsDialController.OnSendControlsMode -= OnSendControlsMode;
    }

    private void OnSendControlsMode(ControlMode controlMode)
    {
        m_controlsTypeText.text = "Controls type : \n" + controlMode.ToString();
    }
}
