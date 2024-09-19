using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameflowBehavior : MonoBehaviour
{
    protected virtual void OnEnable()
    {
        Manager_GameState.OnSendCurrentGameState += OnSendCurrentGameState;
    }

    protected virtual void OnDisable()
    {
        Manager_GameState.OnSendCurrentGameState -= OnSendCurrentGameState;
    }

    private void OnSendCurrentGameState(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.MainMenu:
                OnMainMenu();
                break;
            case GameState.Gameplay:
                OnGameplay();
                break;
            case GameState.Victory:
                OnVictory();
                break;
            case GameState.Gameover:
                OnGameover();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(gameState), gameState, null);
        }
    }

    protected virtual void OnMainMenu()
    {
        
    }
    
    protected virtual void OnGameplay()
    {
        
    }
    
    protected virtual void OnVictory()
    {
        
    }
    
    protected virtual void OnGameover()
    {
        
    }
}
