using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerComputer : MonoBehaviour
{
    public static Action<DirectionDeterminer.Direction> OnSwipe;
    public static Action<Vector3> OnSendControllerData;

    private Vector3 m_tapStartPosition;
    private Vector3 m_currentCursorPosition;
    private Vector3 m_cursorEndPosition;

    public Vector3 TapStartPosition { get => m_tapStartPosition; }
    public Vector3 CurrentCursorPosition { get => m_currentCursorPosition; }
    public Vector3 CursorEndPosition { get => m_cursorEndPosition; }

    public Vector3 SwipeDirection { get => m_currentCursorPosition - m_tapStartPosition; }
    public float SwipeMagnitude { get => (m_currentCursorPosition - m_tapStartPosition).magnitude; }

    public float SwipeXMagnitude { get => SwipeDirection.x; }
    public float SwipeYMagnitude { get => SwipeDirection.y; }
    public float SwipeZMagnitude { get => SwipeDirection.z; }

    private DirectionDeterminer.Direction m_swipeDirection;

    private void OnEnable()
    {
        Controller.OnTapBegin += OnTapBegin;
        Controller.OnHold += OnHold;
        Controller.OnRelease+= OnRelease;
    }

    private void OnDisable()
    {
        Controller.OnTapBegin -= OnTapBegin;
        Controller.OnHold -= OnHold;
        Controller.OnRelease -= OnRelease;
    }



    private void OnTapBegin(Vector3 cursorPosition)
    {
        m_tapStartPosition = cursorPosition;
    }


    private void OnHold(Vector3 cursorPosition)
    {
        m_currentCursorPosition = cursorPosition;
        
        OnSendControllerData?.Invoke(SwipeDirection);
    }

    private void OnRelease(Vector3 cursorPosition)
    {
        m_cursorEndPosition = cursorPosition;

        m_swipeDirection = DirectionDeterminer.DetermineDirection(SwipeDirection.x, SwipeDirection.y, 1f);
        
        OnSwipe?.Invoke(m_swipeDirection);

        m_currentCursorPosition = cursorPosition;
        m_tapStartPosition = cursorPosition;
        
        OnSendControllerData?.Invoke(Vector3.zero);
    }

}
