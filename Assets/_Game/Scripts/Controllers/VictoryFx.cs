using System.Collections.Generic;
using UnityEngine;

public class VictoryFx : MonoBehaviour
{
    [SerializeField] private List<ParticleSystem> m_victoryFxList = null;


    private void OnEnable()
    {
        Manager_GameState.OnSendCurrentGameState += OnSendCurrentGameState;
    }

    private void OnDisable()
    {
        Manager_GameState.OnSendCurrentGameState -= OnSendCurrentGameState;
    }


    private void OnSendCurrentGameState(GameState state)
    {
        if (state == GameState.Victory)
        {
            PlayVictoryFx();
        }
    }

    private void PlayVictoryFx()
    {
        for (int i = 0; i < m_victoryFxList.Count; i++)
        {
            m_victoryFxList[i].Play();
        }
    }
}
