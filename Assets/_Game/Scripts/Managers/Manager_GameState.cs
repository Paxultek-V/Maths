using System;
using System.Collections;
using UnityEngine;


public enum GameState
{
    MainMenu,
    Gameplay,
    Victory,
    Gameover
}

public class Manager_GameState : MonoBehaviour
{
    public static Action<GameState> OnSendCurrentGameState;


    private GameState m_currentGameState;
    private Coroutine m_enterGamestateWithDelayCoroutine;
    
    private void OnEnable()
    {
        Manager_Level.OnLevelInstantiated += OnLevelInstantiated;

        UI_Button_Forfeit.OnForfeitButtonPressed += OnForfeitButtonPressed;

        UI_Button_NextLevel.OnNextLevelButtonPressed += Initialize;
        UI_Button_Retry.OnRetryButtonClicked += Initialize;

        GameoverCondition.OnGameover += OnGameover;
        VictoryCondition.OnVictoryConditionMet += OnVictory;
    }

    private void OnDisable()
    {
        Manager_Level.OnLevelInstantiated -= OnLevelInstantiated;

        UI_Button_Forfeit.OnForfeitButtonPressed -= OnForfeitButtonPressed;

        UI_Button_NextLevel.OnNextLevelButtonPressed -= Initialize;
        UI_Button_Retry.OnRetryButtonClicked -= Initialize;

        GameoverCondition.OnGameover -= OnGameover;
        VictoryCondition.OnVictoryConditionMet -= OnVictory;
    }

    private void Start()
    {
        Initialize();
    }


    private void Initialize()
    {
        m_currentGameState = GameState.MainMenu;
        BroadcastCurrentGameState();
    }

    private void OnLevelInstantiated()
    {
        if(m_enterGamestateWithDelayCoroutine != null)
            StopCoroutine(m_enterGamestateWithDelayCoroutine);

        m_enterGamestateWithDelayCoroutine = StartCoroutine(EnterStateWithDelay(GameState.Gameplay, 0f, 10));
    }

    private void OnVictory()
    {
        m_currentGameState = GameState.Victory;
        BroadcastCurrentGameState();
    }

    private void OnGameover()
    {
        m_currentGameState = GameState.Gameover;
        BroadcastCurrentGameState();
    }

    private void OnForfeitButtonPressed()
    {
        m_currentGameState = GameState.Gameover;
        BroadcastCurrentGameState();
    }

    private IEnumerator EnterStateWithDelay(GameState targetGameState, float delay = 0f, int framesToWait = 0)
    {
        if (delay > 0)
            yield return new WaitForSeconds(delay);

        if (framesToWait > 0)
        {
            for (int i = 0; i < framesToWait; i++)
                yield return new WaitForEndOfFrame();
        }

        m_currentGameState = targetGameState;
        BroadcastCurrentGameState();
    }

    private void BroadcastCurrentGameState()
    {
        OnSendCurrentGameState?.Invoke(m_currentGameState);
    }
}