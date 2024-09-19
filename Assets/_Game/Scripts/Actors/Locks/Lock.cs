using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class Lock : MonoBehaviour
{
    public static Action<Lock> OnLockDestroyed;

    [SerializeField] private int m_lockValue;

    [SerializeField] private int m_indexOnDial = 0;

    [SerializeField] private TMP_Text m_text;

    [SerializeField] private GameObject m_lockDestructionFxPrefab = null;

    [SerializeField] private Transform m_fxSpawnPosition = null;

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
        
        if (sum == m_lockValue)
        {
            Instantiate(m_lockDestructionFxPrefab, m_fxSpawnPosition.position, Quaternion.identity);
            OnLockDestroyed?.Invoke(this);
            Destroy(gameObject);
        }
    }
}