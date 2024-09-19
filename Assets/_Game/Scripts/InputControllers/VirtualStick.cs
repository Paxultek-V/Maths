using System;
using UnityEngine;

public class VirtualStick : MonoBehaviour
{
    public static Action<Vector3, float> OnBroadcastedInput;


    [SerializeField, Tooltip("The percentage of the virtual stick max magnitude.")]
    private float m_maxRadiusMagnitudeLimit = 0.3f;


    private Vector3 m_cursorStartPosition;
    private Vector3 m_cursorDirection;
    private float m_stickMagnitudeRatio;
    private bool m_isControlEnable;


    private void OnEnable()
    {
        Controller.OnTapBegin += GetCursorStartPosition;
        Controller.OnHold += ComputeStickData;
        Controller.OnRelease += ReleaseStick;

    }


    private void OnDisable()
    {
        Controller.OnTapBegin -= GetCursorStartPosition;
        Controller.OnHold -= ComputeStickData;
        Controller.OnRelease += ReleaseStick;

    }


    private void EnableControls()
    {
        m_isControlEnable = true;
    }


    private void DisableControls()
    {
        m_isControlEnable = false;
    }


    private void GetCursorStartPosition(Vector3 cursorPosition)
    {
        if (m_isControlEnable)
            m_cursorStartPosition = cursorPosition;
    }


    private void ComputeStickData(Vector3 currentCursorPosition)
    {
        if (m_isControlEnable)
        {
            m_stickMagnitudeRatio = Vector3.Distance(m_cursorStartPosition, currentCursorPosition) / (m_maxRadiusMagnitudeLimit * Screen.width);
            m_stickMagnitudeRatio = Mathf.Clamp(m_stickMagnitudeRatio, 0.0f, 1.0f);


            m_cursorDirection = currentCursorPosition - m_cursorStartPosition;
            m_cursorDirection.Normalize();

            BroadcastInput(m_cursorDirection, m_stickMagnitudeRatio);


            if (m_stickMagnitudeRatio >= 1.0f)
                UpdateCursorStartPosition(currentCursorPosition);
        }
    }


    private void UpdateCursorStartPosition(Vector3 cursorCurrentPosition)
    {
        Vector3 dir = (m_cursorStartPosition - cursorCurrentPosition).normalized;

        m_cursorStartPosition = cursorCurrentPosition + dir * m_maxRadiusMagnitudeLimit * Screen.width;
    }


    private void ReleaseStick(Vector3 cursorPosition)
    {
        if (m_isControlEnable)
        {
            m_cursorDirection = Vector3.zero;
            m_stickMagnitudeRatio = 0.0f;
            BroadcastInput(m_cursorDirection, m_stickMagnitudeRatio);
        }
    }


    private void BroadcastInput(Vector3 stickDirection, float magnitudePercentage)
    {
        if (OnBroadcastedInput != null)
            OnBroadcastedInput(stickDirection, magnitudePercentage);
    }
}
