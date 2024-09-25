using System;
using System.Collections.Generic;
using UnityEngine;

public class GameoverCondition : MonoBehaviour
{
    public static Action OnGameover;

    private void OnEnable()
    {
        ActionsController.OnNoActionsLeft += Gameover;
    }

    private void OnDisable()
    {
        ActionsController.OnNoActionsLeft -= Gameover;
    }

    private void Gameover()
    {
        OnGameover?.Invoke();
    }

}