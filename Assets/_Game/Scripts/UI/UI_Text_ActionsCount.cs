using TMPro;
using UnityEngine;

public class UI_Text_ActionsCount : MonoBehaviour
{
    private TMP_Text m_actionsCountText;


    private void OnEnable()
    {
        m_actionsCountText = GetComponent<TMP_Text>();
        ActionsController.OnUpdateActionCount += OnUpdateActionCount;
    }

    private void OnDisable()
    {
        ActionsController.OnUpdateActionCount -= OnUpdateActionCount;
    }


    private void OnUpdateActionCount(int actionsCount)
    {
        m_actionsCountText.text = "Actions Left :\n" + actionsCount.ToString();
    }
}
