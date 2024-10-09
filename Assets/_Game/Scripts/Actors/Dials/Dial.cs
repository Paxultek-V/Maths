using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class Dial : MonoBehaviour
{
    public static Action OnDialUpdated;
    public static Action OnPlayAction;

    private Dial_ScaleController m_dialScaleController;
    private Dial_SeparatorsController m_dialSeparatorsController;
    private Dial_WSUI m_dialWSUI;
    private Dial_RotationController m_dialRotationController;
    private Dial_VisualController m_dialVisualController;


    [FormerlySerializedAs("m_dialOrder")] [Header("Game Params")] [SerializeField]
    private int m_dialOrderNumber = 1;

    [SerializeField] private bool m_canOverrideDialsControllerParam = false;

    [ShowIf("m_canOverrideDialsControllerParam")] [SerializeField]
    private int m_minRangeOverride = 1;

    [ShowIf("m_canOverrideDialsControllerParam")] [SerializeField]
    private int m_maxRangeOverride = 6;


    private List<int> m_dialNumberWithOffsetList = new List<int>();
    public List<int> DialNumberWithOffsetList => m_dialNumberWithOffsetList;

    private List<int> m_dialNumberList;
    public List<int> DialNumberList => m_dialNumberList;
    private float m_angleStep => 360f / m_dialNumberList.Count;
    private int m_numbersCountOnDial => m_dialNumberList.Count;


    private void Awake()
    {
        m_dialScaleController = GetComponent<Dial_ScaleController>();
        m_dialSeparatorsController = GetComponent<Dial_SeparatorsController>();
        m_dialWSUI = GetComponent<Dial_WSUI>();
        m_dialRotationController = GetComponent<Dial_RotationController>();
        m_dialVisualController = GetComponent<Dial_VisualController>();
    }

    private void OnEnable()
    {
        m_dialRotationController.OnApplyRotation += OnApplyRotation;
        m_dialRotationController.OnRotationComplete += OnRotationComplete;
    }

    private void OnDisable()
    {
        m_dialRotationController.OnApplyRotation -= OnApplyRotation;
        m_dialRotationController.OnRotationComplete -= OnRotationComplete;
    }

    public void Initialize(int numbersCount, int minRange, int maxRange)
    {
        m_dialNumberList = new List<int>();

        if (m_canOverrideDialsControllerParam)
            GenerateRandomNumbersInList(numbersCount, m_minRangeOverride, m_maxRangeOverride);
        else
            GenerateRandomNumbersInList(numbersCount, minRange, maxRange);

        m_dialRotationController.Initialize(m_angleStep);

        m_dialWSUI.Initialize(m_dialNumberList, m_dialOrderNumber, m_dialNumberList.Count, m_angleStep);

        m_dialScaleController.Initialize(m_dialOrderNumber);

        m_dialSeparatorsController.Initialize(m_dialOrderNumber, m_numbersCountOnDial, m_angleStep);

        m_dialVisualController.Initialize(m_dialOrderNumber);

        m_dialNumberWithOffsetList = new List<int>(m_dialNumberList);
    }


    private void GenerateRandomNumbersInList(int numbersCount, int minRange, int maxRange)
    {
        for (int i = 0; i < numbersCount; i++)
        {
            m_dialNumberList.Add(Random.Range(minRange, maxRange));
        }
    }

    private void OnRotationComplete()
    {
        OnDialUpdated?.Invoke();
        OnPlayAction?.Invoke();
    }

    private void OnApplyRotation(int stepCount, bool makeOffsetFromZero)
    {
        UpdateNumberListOrder(stepCount, makeOffsetFromZero);
    }

    private void UpdateNumberListOrder(int stepCount, bool makeOffsetFromZero = false)
    {
        if (makeOffsetFromZero)
            m_dialNumberWithOffsetList = new List<int>(m_dialNumberList);

        if (stepCount > 0)
            for (int i = 0; i < stepCount; i++)
                MoveLeft();
        else
            for (int i = 0; i < Mathf.Abs(stepCount); i++)
                MoveRight();
    }

    private void MoveLeft()
    {
        m_dialNumberWithOffsetList.MoveItemAtIndexToFront(m_dialNumberWithOffsetList.Count - 1);
    }

    private void MoveRight()
    {
        m_dialNumberWithOffsetList.MoveItemAtIndexToLast(0);
    }

    public int GetRandomValueInNumberList()
    {
        return m_dialNumberList[Random.Range(0, m_dialNumberList.Count)];
    }
}