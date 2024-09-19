using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class Manager_Debug : MonoBehaviour
{
    [SerializeField] private bool m_isDebugEnabled = false;

    [SerializeField] private List<GameObject> m_debugObjectList = null;

    private void Start()
    {
        UpdateDebugObjectState();
    }

    private void Update()
    {
#if UNITY_EDITOR
        UpdateDebugObjectState();
#endif
    }

    private void UpdateDebugObjectState()
    {
        if (m_debugObjectList != null && m_debugObjectList.Count > 0)
        {
            if(m_debugObjectList[0].activeInHierarchy == m_isDebugEnabled)
                return;
        }
        
        for (int i = 0; i < m_debugObjectList.Count; i++)
        {
            m_debugObjectList[i].SetActive(m_isDebugEnabled);
        }
    }
}