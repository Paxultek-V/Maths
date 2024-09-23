using System;
using System.Collections.Generic;
using UnityEngine;

public class LocksController : MonoBehaviour
{
    public static Action<int> OnAskRandomDialNumbersCombination;
    public static Action OnAllLocksDestroyed;

    private List<Lock> m_lockList;

    private void Awake()
    {
        m_lockList = new List<Lock>(GetComponentsInChildren<Lock>());
    }

    private void OnEnable()
    {
        Dials_Controller.OnDialsInitialized += OnDialsInitialized;
        Dials_Controller.OnSendRandomCombinationList += OnSendRandomCombinationList;
        Lock.OnLockDestroyed += OnLockDestroyed;
    }

    private void OnDisable()
    {
        Dials_Controller.OnDialsInitialized -= OnDialsInitialized;
        Dials_Controller.OnSendRandomCombinationList -= OnSendRandomCombinationList;
        Lock.OnLockDestroyed -= OnLockDestroyed;
    }

    private void OnDialsInitialized()
    {
        OnAskRandomDialNumbersCombination?.Invoke(m_lockList.Count);
    }

    private void OnSendRandomCombinationList(List<int> randomCombinationList)
    {
        for (int i = 0; i < randomCombinationList.Count; i++)
        {
            m_lockList[i].Initialize(randomCombinationList[i]);
        }
    }

    private void OnLockDestroyed(Lock lockDestroyed)
    {
        m_lockList.Remove(lockDestroyed);

        if (m_lockList.Count == 0)
        {
            OnAllLocksDestroyed?.Invoke();
        }
    }
}
