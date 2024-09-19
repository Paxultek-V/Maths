using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MovementPointAPointB : MonoBehaviour
{
    [SerializeField] private Transform m_controlledTransform = null;
    
    public Vector3 m_startPosition = Vector3.zero;
    public Vector3 m_endPosition = Vector3.zero;

    [SerializeField] private float m_progressionSpeed = 1f;

    [SerializeField] private float m_gizmosSize = 0.5f;
    
    private float m_progression;
    private float m_timer;

    private void Update()
    {
        UpdatePosition();
    }


    private void UpdatePosition()
    {
        m_timer += Time.deltaTime * m_progressionSpeed;
        m_progression = Mathf.PingPong(m_timer, 1f);

        m_controlledTransform .position = Vector3.Lerp(m_startPosition, m_endPosition, m_progression);
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        
        Gizmos.DrawSphere(m_startPosition, m_gizmosSize);
        Gizmos.DrawSphere(m_endPosition, m_gizmosSize);
    }
}


#if UNITY_EDITOR
[CustomEditor(typeof(MovementPointAPointB))]
public class Module_Movement_Editor : Editor
{
    public void OnSceneGUI()
    {
        var linkedObject = target as MovementPointAPointB;

        linkedObject.m_startPosition = Handles.PositionHandle(linkedObject.m_startPosition, Quaternion.identity);
        linkedObject.m_endPosition = Handles.PositionHandle(linkedObject.m_endPosition, Quaternion.identity);
    }
}

#endif
