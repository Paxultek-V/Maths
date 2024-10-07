using System;
using System.Collections.Generic;
using UnityEngine;

public class LocksController : MonoBehaviour
{
    public static Action<List<Lock>> OnAskRandomDialNumbersCombination;
    public static Action OnAllLocksDestroyed;
    public static Action<bool> OnSendAllLocksDestroyedState;

    [SerializeField] private Dials_Controller m_dialsController;
    
    private List<Lock> m_lockList;

    public Dials_Controller DialsController
    {
        get => m_dialsController;
    }

    private void Awake()
    {
        m_lockList = new List<Lock>(GetComponentsInChildren<Lock>());
    }

    private void OnEnable()
    {
        Dials_Controller.OnDialsInitialized += OnDialsInitialized;
        Dials_Controller.OnSendRandomCombinationList += OnSendRandomCombinationList;
        Lock.OnLockDestroyed += OnLockDestroyed;
        ActionsController.OnCheckLocksState += OnCheckLocksState;
    }

    private void OnDisable()
    {
        Dials_Controller.OnDialsInitialized -= OnDialsInitialized;
        Dials_Controller.OnSendRandomCombinationList -= OnSendRandomCombinationList;
        Lock.OnLockDestroyed -= OnLockDestroyed;
        ActionsController.OnCheckLocksState -= OnCheckLocksState;
    }

    private void OnDialsInitialized()
    {
        OnAskRandomDialNumbersCombination?.Invoke(m_lockList);
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

    private void OnCheckLocksState()
    {
        OnSendAllLocksDestroyedState?.Invoke(m_lockList.Count == 0);
    }
}
