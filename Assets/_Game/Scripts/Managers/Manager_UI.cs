using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

public class Manager_UI : SerializedMonoBehaviour
{
    [SerializeField] private Dictionary<GameState, CanvasGroup> m_uiDictionary;

    [SerializeField] private float m_fadeDuration = 0.5f;

    private void Awake()
    {
        foreach (var element in m_uiDictionary)
        {
            ToggleCanvas(element.Value, false, 0f);
        }
    }

    private void OnEnable()
    {
        Manager_GameState.OnSendCurrentGameState += OnSendCurrentGameState;
    }

    private void OnDisable()
    {
        Manager_GameState.OnSendCurrentGameState -= OnSendCurrentGameState;
    }

    private void OnSendCurrentGameState(GameState gameState)
    {
        foreach (var element in m_uiDictionary)
        {
            ToggleCanvas(element.Value, element.Key == gameState, m_fadeDuration);
        }
    }

    private void ToggleCanvas(CanvasGroup canvas, bool state, float duration)
    {
        canvas.DOFade(state == true ? 1 : 0, duration)
            .OnComplete(() => ToggleCanvasGroupInteractionState(canvas, state));
    }

    private void ToggleCanvasGroupInteractionState(CanvasGroup canvas, bool state)
    {
        canvas.interactable = state;
        canvas.blocksRaycasts = state;
    }
}
