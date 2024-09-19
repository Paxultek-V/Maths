using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Dial : MonoBehaviour
{
    public static Action OnDialUpdated;

    [SerializeField] private GameObject m_separatorPrefab = null;

    [SerializeField] private Transform m_separatorsParent = null;

    [SerializeField] private WSUI_DialNumber m_wsuiDialNumberPrefab = null;

    [SerializeField] private GameObject m_anchorPrefab = null;

    [SerializeField] private Transform m_anchorParent = null;

    [SerializeField] private Transform m_wsuiParent = null;

    [SerializeField] private Transform m_rotationController = null;

    [SerializeField] private float m_wsuiPositionRadius = 1f;

    [SerializeField] private Collider m_collider;

    public List<int> m_dialNumberWithOffsetList = new List<int>();

    private List<Transform> m_wsuiAnchorsList;
    private List<int> m_dialNumberList;
    private WSUI_DialNumber m_wsuiNumberBuffer;
    private Quaternion m_startRotationQuaternion;
    private Coroutine m_snapRotationCoroutine;
    private float m_angleRotationDiff;
    private float m_angleStep => 360f / m_dialNumberList.Count;
    private bool m_isSelected;
    private int m_offset;


    private void OnEnable()
    {
        InputsDialController.OnSelectDial += OnSelectDial;
        InputsDialController.OnSendRotationAngle += OnSendRotationAngle;
        InputsDialController.OnNoDialSelected += OnNoDialSelected;
        InputsDialController.OnReleaseDial += OnReleaseDial;
    }

    private void OnDisable()
    {
        InputsDialController.OnSelectDial -= OnSelectDial;
        InputsDialController.OnSendRotationAngle -= OnSendRotationAngle;
        InputsDialController.OnNoDialSelected -= OnNoDialSelected;
        InputsDialController.OnReleaseDial -= OnReleaseDial;
    }

    private void OnReleaseDial(Collider dialCollider)
    {
        if (dialCollider != m_collider)
            return;

        if (m_snapRotationCoroutine != null)
            StopCoroutine(m_snapRotationCoroutine);

        m_snapRotationCoroutine = StartCoroutine(SnapDialRotation());
    }

    private void OnNoDialSelected()
    {
        m_isSelected = false;
    }

    private void UpdateNumberListOrder(int stepCount)
    {
        m_offset = stepCount;
        m_dialNumberWithOffsetList = new List<int>(m_dialNumberList);

        if (stepCount > 0)
            for (int i = 0; i < stepCount; i++)
                MoveLeft();
        else
            for (int i = 0; i < Mathf.Abs(stepCount); i++)
                MoveRight();
    }

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

    private void OnSendRotationAngle(float rotationAngle)
    {
        if (!m_isSelected)
            return;

        Quaternion targetRotation = m_startRotationQuaternion * Quaternion.AngleAxis(rotationAngle, Vector3.forward);
        m_rotationController.rotation = targetRotation;
    }

    private void OnSelectDial(Collider selectedDialCollider)
    {
        m_isSelected = m_collider == selectedDialCollider;

        if (m_isSelected)
        {
            if (m_snapRotationCoroutine != null)
                StopCoroutine(m_snapRotationCoroutine);

            m_startRotationQuaternion = m_rotationController.rotation;
        }
    }

    public void Initialize(int numbersCount, int minRange, int maxRange)
    {
        m_dialNumberList = new List<int>();

        GenerateRandomNumbersInList(numbersCount, minRange, maxRange);

        GenerateWSUIAnchors();

        GenerateWSUI();

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

    private void GenerateSeparators()
    {
        GameObject separatorBuffer;
        for (int i = 0; i < m_dialNumberList.Count; i++)
        {
            separatorBuffer = Instantiate(m_separatorPrefab, transform.position - Vector3.forward, Quaternion.identity,
                m_separatorsParent);
            separatorBuffer.transform.rotation =
                Quaternion.Euler(new Vector3(0, 0, m_angleStep * i ));
        }
    }

    private void GenerateWSUIAnchors()
    {
        m_wsuiAnchorsList = new List<Transform>();

        for (int i = 0; i < m_dialNumberList.Count; i++)
        {
            Transform anchor = Instantiate(m_anchorPrefab, m_anchorParent).transform;

            anchor.position = new Vector3(
                Mathf.Cos(Mathf.Deg2Rad * (m_angleStep * i)) * m_wsuiPositionRadius + transform.position.x,
                Mathf.Sin(Mathf.Deg2Rad * (m_angleStep * i)) * m_wsuiPositionRadius + transform.position.y,
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

    public void MoveLeft()
    {
        m_dialNumberWithOffsetList.MoveItemAtIndexToFront(m_dialNumberWithOffsetList.Count - 1);
    }

    public void MoveRight()
    {
        m_dialNumberWithOffsetList.MoveItemAtIndexToLast(0);
    }

    public int GetRandomValueInNumberList()
    {
        return m_dialNumberList[Random.Range(0, m_dialNumberList.Count)];
    }
}