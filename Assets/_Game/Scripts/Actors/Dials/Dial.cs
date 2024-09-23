using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

public class Dial : MonoBehaviour
{
    public static Action OnDialUpdated;

    private const float WSUI_POSITION_RADIUS_OFFSET = 0.5f;
    private const float DIAL_SCALE_OFFSET = 1f;

    [Header("Game Params")] [SerializeField]
    private int m_dialOrder = 1;
    [SerializeField]
    private bool m_isInteractable = true;

    [SerializeField] private bool m_canOverrideDialsControllerParam = false;

    [ShowIf("m_canOverrideDialsControllerParam")] [SerializeField]
    private int m_minRangeOverride = 1;

    [ShowIf("m_canOverrideDialsControllerParam")] [SerializeField]
    private int m_maxRangeOverride = 6;

    [Header("Separator Params")] [SerializeField]
    private GameObject m_separatorPrefab = null;

    [SerializeField] private Transform m_separatorsParent = null;

    [Header("UI Numbers Params")] [SerializeField]
    private WSUI_DialNumber m_wsuiDialNumberPrefab = null;

    [SerializeField] private GameObject m_anchorPrefab = null;

    [SerializeField] private Transform m_anchorParent = null;

    [SerializeField] private Transform m_wsuiParent = null;

    //[SerializeField] private float m_wsuiPositionRadius = 1f;

    [Header("Scale Params")] [SerializeField]
    private Transform m_scaleController = null;

    //[SerializeField] private float m_dialScale = 1f;

    [Header("Rotation Params")] [SerializeField]
    private Transform m_rotationController = null;

    [SerializeField] private Collider m_collider;


    private List<int> m_dialNumberWithOffsetList = new List<int>();

    public List<int> DialNumberWithOffsetList
    {
        get => m_dialNumberWithOffsetList;
    }

    private List<Transform> m_wsuiAnchorsList;
    private List<int> m_dialNumberList;
    private WSUI_DialNumber m_wsuiNumberBuffer;
    private Quaternion m_startRotationQuaternion;
    private Coroutine m_snapRotationCoroutine;
    private Vector3 m_screenStartDirection;
    private Vector3 m_screenCurrentDirection;
    private Vector3 m_dialCenterScreenPosition => Camera.main.WorldToScreenPoint(transform.position);

    private float m_rotationAngle;
    private float m_angleRotationDiff;
    private float m_angleStep => 360f / m_dialNumberList.Count;
    private bool m_isSelected;


    private void OnEnable()
    {
        if (m_isInteractable == false)
            return;

        InputsDialController.OnSelectDial += OnSelectDial;
        InputsDialController.OnSendRotationInfos += OnSendRotationInfos;
        InputsDialController.OnNoDialSelected += OnNoDialSelected;
        InputsDialController.OnReleaseDial += OnReleaseDial;
    }

    private void OnDisable()
    {
        if (m_isInteractable == false)
            return;

        InputsDialController.OnSelectDial -= OnSelectDial;
        InputsDialController.OnSendRotationInfos -= OnSendRotationInfos;
        InputsDialController.OnNoDialSelected -= OnNoDialSelected;
        InputsDialController.OnReleaseDial -= OnReleaseDial;
    }

    #region INPUTS

    private void OnSelectDial(Collider selectedDialCollider, Vector3 cursorPosition)
    {
        m_isSelected = m_collider == selectedDialCollider;

        if (m_isSelected)
        {
            if (m_snapRotationCoroutine != null)
                StopCoroutine(m_snapRotationCoroutine);

            m_screenStartDirection = cursorPosition - m_dialCenterScreenPosition;
            m_screenStartDirection.z = 0;
            m_screenStartDirection.Normalize();

            m_startRotationQuaternion = m_rotationController.rotation;
        }
    }

    private void OnSendRotationInfos(Vector3 cursorPosition, float rotationAngle)
    {
        if (!m_isSelected)
            return;

        m_screenCurrentDirection = cursorPosition - m_dialCenterScreenPosition;
        m_screenCurrentDirection.z = 0;
        m_screenCurrentDirection.Normalize();
        m_rotationAngle = Vector3.SignedAngle(m_screenStartDirection, m_screenCurrentDirection, transform.forward);

        Quaternion targetRotation =
            m_startRotationQuaternion * Quaternion.AngleAxis(m_rotationAngle, transform.forward);

        m_rotationController.rotation = targetRotation;
    }

    private void OnNoDialSelected()
    {
        m_isSelected = false;
    }

    private void OnReleaseDial(Collider dialCollider)
    {
        if (dialCollider != m_collider)
            return;

        if (m_snapRotationCoroutine != null)
            StopCoroutine(m_snapRotationCoroutine);

        m_snapRotationCoroutine = StartCoroutine(SnapDialRotation());
    }

    #endregion

    #region SNAPING AND NUMBERS MANAGEMENTS

    private IEnumerator SnapDialRotation()
    {
        float delta = m_rotationController.rotation.eulerAngles.z % m_angleStep;
        int stepCount = (int)m_rotationController.rotation.eulerAngles.z / (int)m_angleStep;

        if (Mathf.Abs(delta) >= m_angleStep / 2f)
            stepCount += (int)Mathf.Sign(delta);

        UpdateNumberListOrder(stepCount);

        float targetAngle = m_angleStep * stepCount;

        Vector3 targetRotation = m_rotationController.rotation.eulerAngles;
        targetRotation.z = targetAngle;

        Quaternion targetRotationQuaternion = Quaternion.Euler(targetRotation);

        while (Quaternion.Angle(m_rotationController.rotation, targetRotationQuaternion) > 1)
        {
            m_rotationController.rotation =
                Quaternion.RotateTowards(m_rotationController.rotation, targetRotationQuaternion,
                    Time.deltaTime * 360f);

            yield return new WaitForEndOfFrame();
        }

        m_rotationController.rotation = targetRotationQuaternion;

        OnDialUpdated?.Invoke();
    }

    private void UpdateNumberListOrder(int stepCount)
    {
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

    #endregion

    #region INITIALIZATION

    public void Initialize(int numbersCount, int minRange, int maxRange)
    {
        m_dialNumberList = new List<int>();

        if (m_canOverrideDialsControllerParam)
            GenerateRandomNumbersInList(numbersCount, m_minRangeOverride, m_maxRangeOverride);
        else
            GenerateRandomNumbersInList(numbersCount, minRange, maxRange);

        GenerateWSUIAnchors();

        GenerateWSUI();

        UpdateDialScale();

        GenerateSeparators();

        m_dialNumberWithOffsetList = new List<int>(m_dialNumberList);
    }


    private void GenerateRandomNumbersInList(int numbersCount, int minRange, int maxRange)
    {
        for (int i = 0; i < numbersCount; i++)
        {
            m_dialNumberList.Add(Random.Range(minRange, maxRange));
        }
    }

    private void GenerateWSUIAnchors()
    {
        m_wsuiAnchorsList = new List<Transform>();

        for (int i = 0; i < m_dialNumberList.Count; i++)
        {
            Transform anchor = Instantiate(m_anchorPrefab, m_anchorParent).transform;

            anchor.position = new Vector3(
                Mathf.Cos(Mathf.Deg2Rad * (m_angleStep * i)) * (m_dialOrder + WSUI_POSITION_RADIUS_OFFSET) + transform.position.x,
                Mathf.Sin(Mathf.Deg2Rad * (m_angleStep * i)) * (m_dialOrder + WSUI_POSITION_RADIUS_OFFSET) + transform.position.y,
                transform.position.z - 0.3f
            );

            m_wsuiAnchorsList.Add(anchor);
        }
    }

    private void GenerateWSUI()
    {
        for (int i = 0; i < m_dialNumberList.Count; i++)
        {
            m_wsuiNumberBuffer = Instantiate(m_wsuiDialNumberPrefab, m_wsuiParent);
            m_wsuiNumberBuffer.Initialize(m_dialNumberList[i], m_wsuiAnchorsList[i]);
        }
    }

    private void UpdateDialScale()
    {
        m_scaleController.transform.localScale = new Vector3((m_dialOrder + DIAL_SCALE_OFFSET), (m_dialOrder + DIAL_SCALE_OFFSET), 1f);
    }

    private void GenerateSeparators()
    {
        GameObject separatorBuffer;
        for (int i = 0; i < m_dialNumberList.Count; i++)
        {
            separatorBuffer = Instantiate(m_separatorPrefab, transform.position - Vector3.forward, Quaternion.identity,
                m_separatorsParent);
            Vector3 direction = m_wsuiAnchorsList[i].position - transform.position;
            direction.z = 0f;
            separatorBuffer.transform.up = Quaternion.Euler(0,0, m_angleStep / 2f) * direction;
            separatorBuffer.transform.localScale = new Vector3(1f, (m_dialOrder + DIAL_SCALE_OFFSET), 1f);
        }
    }

    #endregion

    public int GetRandomValueInNumberList()
    {
        return m_dialNumberList[Random.Range(0, m_dialNumberList.Count)];
    }
}