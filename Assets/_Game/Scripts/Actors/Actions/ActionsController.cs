using System;
using UnityEngine;

public class ActionsController : MonoBehaviour
{
    public static Action OnNoActionsLeft;
    public static Action<int> OnUpdateActionCount;
    public static Action OnCheckLocksState;

    [SerializeField] private int m_initialActionCount = 20;

    private int m_currentActionCount;

    private void OnEnable()
    {
        Dial.OnPlayAction += DecreaseCurrentActionCount;
        LocksController.OnSendAllLocksDestroyedState += OnSendAllLocksDestroyedState;
    }

    private void OnDisable()
    {
        Dial.OnPlayAction -= DecreaseCurrentActionCount;
        LocksController.OnSendAllLocksDestroyedState -= OnSendAllLocksDestroyedState;
    }

    private void Start()
    {
        Initalize();
    }

    private void Initalize()
    {
        m_currentActionCount = m_initialActionCount;
        OnUpdateActionCount?.Invoke(m_currentActionCount);
    }

    private void DecreaseCurrentActionCount()
    {
        m_currentActionCount--;
        OnUpdateActionCount?.Invoke(m_currentActionCount);

        if (m_currentActionCount <= 0)
            OnCheckLocksState?.Invoke();
    }

    private void OnSendAllLocksDestroyedState(bool isAllLocksDestroyed)
    {
        if (isAllLocksDestroyed == false)
            OnNoActionsLeft?.Invoke();
    }
}