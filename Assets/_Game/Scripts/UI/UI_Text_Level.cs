using DG.Tweening;
using TMPro;
using UnityEngine;

public class UI_Text_Level : MonoBehaviour
{
    [SerializeField] private TMP_Text m_levelText = null;

    [SerializeField] private Color m_transparentColor = Color.white;
    
    [SerializeField] private Color m_fullColor = Color.white;

    [SerializeField] private float m_animationDuration = 0.5f;
    
    private void OnEnable()
    {
        Manager_GameState.OnSendCurrentGameState += OnSendCurrentGameState;
        Manager_Level.OnSendTotalLevelCleared += OnSendTotalLevelCleared;
    }

    private void OnDisable()
    {
        Manager_GameState.OnSendCurrentGameState -= OnSendCurrentGameState;
        Manager_Level.OnSendTotalLevelCleared -= OnSendTotalLevelCleared;
    }


    private void OnSendTotalLevelCleared(int currentLevel)
    {
        m_levelText.text = "Level\n" + (currentLevel + 1);
    }
    
    private void OnSendCurrentGameState(GameState state)
    {
        if (state == GameState.Gameplay)
        {
            m_levelText.color = m_transparentColor;
            m_levelText.DOColor(m_fullColor, m_animationDuration).SetEase(Ease.Linear);
        }
    }
}
