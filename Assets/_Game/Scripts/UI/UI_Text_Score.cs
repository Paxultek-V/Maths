using DG.Tweening;
using TMPro;
using UnityEngine;

public class UI_Text_Score : MonoBehaviour
{
    [SerializeField] private TMP_Text m_scoreText = null;

    [SerializeField] private Color m_transparentColor = Color.white;
    
    [SerializeField] private Color m_fullColor = Color.white;

    [SerializeField] private float m_animationDuration = 0.5f;
    
    private void OnEnable()
    {
        Manager_GameState.OnSendCurrentGameState += OnSendCurrentGameState;
        Manager_Score.OnSendCurrentScore += UpdateScoreText;
    }

    private void OnDisable()
    {
        Manager_GameState.OnSendCurrentGameState -= OnSendCurrentGameState;
        Manager_Score.OnSendCurrentScore -= UpdateScoreText;
    }

    private void UpdateScoreText(float currentScore)
    {
        m_scoreText.text = "Score\n" + currentScore;
    }

    private void OnSendCurrentGameState(GameState state)
    {
        if (state == GameState.Gameplay)
        {
            m_scoreText.color = m_transparentColor;
            m_scoreText.DOColor(m_fullColor, m_animationDuration).SetEase(Ease.Linear);
        }
    }
    
}
