using System;
using System.Collections.Generic;
using UnityEngine;

public class Dials_Controller : MonoBehaviour
{
    public static Action OnDialsInitialized;
    public static Action<int, List<int>> OnSendCodeSequence;
    public static Action<List<int>> OnSendRandomCombinationList;
    public static Action<int> OnSendTotalDialCount;

    [SerializeField] private int m_numbersCount = 6;
    [SerializeField] private int m_minRange = 1;
    [SerializeField] private int m_maxRange = 6;

    public int NumbersCount
    {
        get => m_numbersCount;
    }

    private List<Dial> m_dialList = null;
    private List<int> m_codeSequenceListBuffer = new List<int>();

    public List<Dial> DialList
    {
        get => m_dialList;
    }

    private void OnEnable()
    {
        Dial.OnDialUpdated += OnDialUpdated;
        LocksController.OnAskRandomDialNumbersCombination += OnAskRandomDialNumbersCombination;
    }

    private void OnDisable()
    {
        Dial.OnDialUpdated -= OnDialUpdated;
        LocksController.OnAskRandomDialNumbersCombination -= OnAskRandomDialNumbersCombination;
    }

    private void Start()
    {
        Initialize();
    }

    private void OnDialUpdated()
    {
        for (int i = 0; i < m_numbersCount; i++)
        {
            m_codeSequenceListBuffer.Clear();

            for (int j = 0; j < m_dialList.Count; j++)
            {
                m_codeSequenceListBuffer.Add(m_dialList[j].DialNumberWithOffsetList[i]);
            }

            OnSendCodeSequence?.Invoke(i, m_codeSequenceListBuffer);
        }
    }

    private void Initialize()
    {
        m_dialList = new List<Dial>(GetComponentsInChildren<Dial>());

        Dial_VisualController.SetRandomIndexOffset();
        
        for (int i = 0; i < m_dialList.Count; i++)
        {
            m_dialList[i].Initialize(m_numbersCount, m_minRange, m_maxRange);
        }

        OnDialsInitialized?.Invoke();
        OnSendTotalDialCount?.Invoke(m_dialList.Count);
    }

    private void OnAskRandomDialNumbersCombination(List<Lock> lockList)
    {
        List<int> randomCombinationList = new List<int>();

        for (int i = 0; i < lockList.Count; i++)
        {
            randomCombinationList.Add(GetValidCombinationSum(lockList, i));
        }

        OnSendRandomCombinationList?.Invoke(randomCombinationList);
    }

    private int GetValidCombinationSum(List<Lock> lockList, int lockIndex)
    {
        int combination = GetRandomDialCombinationSum();
        int sumAtIndex = 0;

        foreach (var dial in m_dialList)
        {
            sumAtIndex += dial.DialNumberList[lockList[lockIndex].IndexOnDial];
        }

        return combination != sumAtIndex ? combination : GetValidCombinationSum(lockList, lockIndex);
    }
    
    private int GetRandomDialCombinationSum()
    {
        int sum = 0;

        for (int i = 0; i < m_dialList.Count; i++)
        {
            sum += m_dialList[i].GetRandomValueInNumberList();
        }

        return sum;
    }

}