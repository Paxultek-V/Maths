using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_SagaMap : MonoBehaviour
{
    [SerializeField] private List<TMP_Text> m_levelTextList = new List<TMP_Text>();


    private void OnEnable()
    {
        Manager_Level.OnSendTotalLevelCleared += OnSendTotalLevelCleared;
    }

    private void OnDisable()
    {
        Manager_Level.OnSendTotalLevelCleared -= OnSendTotalLevelCleared;
    }

    private void OnSendTotalLevelCleared(int currentLevelIndex)
    {
        for (int i = 0; i < m_levelTextList.Count; i++)
        {
            m_levelTextList[i].text = (currentLevelIndex + i + 1).ToString();
        }
    }
}
