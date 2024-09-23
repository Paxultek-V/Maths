using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelBase : MonoBehaviour, I_Initializable
{
    public static Action OnLevelCleared;

    [SerializeField] private List<GameObject> m_subLevelList = null;

    private GameObject m_currentLevel;
    private int m_currentIndex;

    private void OnEnable()
    {
        LocksController.OnAllLocksDestroyed += OnAllLocksDestroyed;
    }

    private void OnDisable()
    {
        LocksController.OnAllLocksDestroyed -= OnAllLocksDestroyed;
    }

    public void Initialize()
    {
        m_currentIndex = 0;

        m_currentLevel = Instantiate(m_subLevelList[m_currentIndex], transform);
    }


    private void OnAllLocksDestroyed()
    {
        Invoke(nameof(LoadNextSubLevel), 1f);
    }

    private void LoadNextSubLevel()
    {
        m_currentIndex++;

        Destroy(m_currentLevel);

        if (m_currentIndex >= m_subLevelList.Count)
        {
            OnLevelCleared?.Invoke();
        }
        else
        {
            m_currentLevel = Instantiate(m_subLevelList[m_currentIndex], transform);
        }
    }
    
    
}
