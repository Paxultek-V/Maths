using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Lock : MonoBehaviour
{
    public static Action<Lock> OnLockDestroyed;

    [SerializeField] private int m_lockValue;
    
    [SerializeField] private int m_indexOnDial = 0;
    
    [SerializeField] private float m_radius = 4.5f;
    
    [SerializeField] private TMP_Text m_text;

    [SerializeField] private GameObject m_lockDestructionFxPrefab = null;

    [SerializeField] private Transform m_fxSpawnPosition = null;

    [SerializeField] private bool m_debug = false;
    
    private void OnEnable()
    {
        Dials_Controller.OnSendCodeSequence += OnSendCodeSequence;
    }

    private void OnDisable()
    {
        Dials_Controller.OnSendCodeSequence -= OnSendCodeSequence;
    }

    public void Initialize(int lockValue)
    {
        m_lockValue = lockValue;
        m_text.text = m_lockValue.ToString();
    }

    private void OnSendCodeSequence(int index, List<int> codeSequenceList)
    {
        if (m_indexOnDial != index)
            return;

        int sum = 0;
        
        for (int i = 0; i < codeSequenceList.Count; i++)
        {
            sum += codeSequenceList[i];
        }
        
        if(m_debug)
            Debug.Log(sum);
        
        if (sum == m_lockValue)
        {
            Instantiate(m_lockDestructionFxPrefab, m_fxSpawnPosition.position, Quaternion.identity);
            OnLockDestroyed?.Invoke(this);
            Destroy(gameObject);
        }
    }

    public void IncreaseIndexOnDial()
    {
        LocksController locksController = GetComponentInParent<LocksController>();
        
        m_indexOnDial++;
        if (m_indexOnDial >= locksController.DialsController.NumbersCount)
            m_indexOnDial = 0;

        PositionLock(locksController);
    }
    
    public void DecreaseIndexOnDial()
    {
        LocksController locksController = GetComponentInParent<LocksController>();
        
        m_indexOnDial--;
        if (m_indexOnDial < 0)
            m_indexOnDial = locksController.DialsController.NumbersCount - 1;

        PositionLock(locksController);
    }
    
    private void PositionLock(LocksController locksController)
    {
        if(m_indexOnDial >= locksController.DialsController.NumbersCount)
            return;
        
        float angleStep = 360f / locksController.DialsController.NumbersCount;
        
        transform.position = new Vector3(
            Mathf.Cos(Mathf.Deg2Rad * (angleStep * m_indexOnDial)) * m_radius,
            Mathf.Sin(Mathf.Deg2Rad * (angleStep * m_indexOnDial)) * m_radius,
            0
        );
        
    }
}