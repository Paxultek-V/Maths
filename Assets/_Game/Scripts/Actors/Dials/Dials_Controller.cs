using System;
using System.Collections.Generic;
using UnityEngine;

public class Dials_Controller : MonoBehaviour
{
    public static Action OnDialsInitialized;
    public static Action<int, List<int>> OnSendCodeSequence;
    public static Action<List<int>> OnSendRandomCombinationList;

    [SerializeField] private List<Dial> m_dialList = null;

    [SerializeField] private int m_numbersCount = 6;
    [SerializeField] private int m_minRange = 1;
    [SerializeField] private int m_maxRange = 6;

    private List<int> m_codeSequenceListBuffer = new List<int>();

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
                m_codeSequenceListBuffer.Add(m_dialList[j].m_dialNumberWithOffsetList[i]);
            }
            
            OnSendCodeSequence?.Invoke(i, m_codeSequenceListBuffer);
        }
    }
    
    private void Initialize()
    {
        for (int i = 0; i < m_dialList.Count; i++)
        {
            m_dialList[i].Initialize(m_numbersCount, m_minRange, m_maxRange);
        }
        
        OnDialsInitialized?.Invoke();
    }

    private void OnAskRandomDialNumbersCombination(int combinationCount)
    {
        List<int> randomCombinationList = new List<int>();

        for (int i = 0; i < combinationCount; i++)
        {
            randomCombinationList.Add(GetRandomDialCombinationSum());
        }
        
        OnSendRandomCombinationList?.Invoke(randomCombinationList);
    }

    public int GetRandomDialCombinationSum()
    {
        int sum = 0;

        for (int i = 0; i < m_dialList.Count; i++)
        {
            sum += m_dialList[i].GetRandomValueInNumberList();
        }

        return sum;
    }
    
}
