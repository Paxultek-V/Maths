using System;
using UnityEngine;

public class Manager_Score : GameflowBehavior
{
    public static Action<float> OnSendCurrentScore;
    public static Action<Vector3, float, float> OnSendGainedScoreInfos;

    private float m_currentScore;
    private float m_currentCombo;
    private bool m_hasDestroyedCoinsThisTurn;

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
    }

    protected override void OnGameplay()
    {
        base.OnGameplay();

        m_currentScore = 0;
        m_currentCombo = 1;

        OnSendCurrentScore?.Invoke(m_currentScore);
    }


}