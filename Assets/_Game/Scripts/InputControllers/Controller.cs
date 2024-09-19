using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class Controller : MonoBehaviour, IController
{
    #region EVENTS
    public delegate void ControllerEvent(Vector3 cursorPosition);
    public delegate void ControllerEventSwipe();

    public static ControllerEvent OnTap;
    public static ControllerEvent OnTapBegin;
    public static ControllerEvent OnSwipe;
    public static ControllerEvent OnHold;
    public static ControllerEvent OnRelease;
    public static ControllerEventSwipe OnSwipeUp;
    public static ControllerEventSwipe OnSwipeDown;
    public static ControllerEventSwipe OnSwipeRight;
    public static ControllerEventSwipe OnSwipeLeft;
    #endregion


    abstract protected void UpdateInputs();


    void Update()
    {
        UpdateInputs();
    }


    protected bool IsInputOverUI(int cursorID)
    {
        if (EventSystem.current != null)
        {
            if (EventSystem.current.IsPointerOverGameObject(cursorID))
                return true;
        }

        return false;
    }


    virtual public void Tap(Vector3 cursorPosition)
    {
        if (OnTap != null)
            OnTap(cursorPosition);
    }

    virtual public void TapBegin(Vector3 cursorPosition)
    {
        if (OnTapBegin != null)
            OnTapBegin(cursorPosition);
    }

    virtual public void Swipe(Vector3 cursorPosition)
    {
        if (OnSwipe != null)
            OnSwipe(cursorPosition);
    }

    virtual public void Hold(Vector3 cursorPosition)
    {
        if (OnHold != null)
            OnHold(cursorPosition);
    }

    virtual public void Release(Vector3 cursorPosition)
    {
        if (OnRelease != null)
            OnRelease(cursorPosition);
    }


    virtual public void DetermineSwipeDirection(Vector3 direction)
    {
        float positiveX = Mathf.Abs(direction.x);
        float positiveY = Mathf.Abs(direction.y);

        if (positiveX > positiveY)
        {
            if (direction.x > 0)
                SwipeRight();
            else
                SwipeLeft();
        }
        else
        {
            if (direction.y > 0)
                SwipeUp();
            else
                SwipeDown();
        }
    }


    virtual public void SwipeUp()
    {
        if (OnSwipeUp != null)
            OnSwipeUp();
    }

    virtual public void SwipeDown()
    {
        if (OnSwipeDown != null)
            OnSwipeDown();
    }

    virtual public void SwipeRight()
    {
        if (OnSwipeRight != null)
            OnSwipeRight();
    }

    virtual public void SwipeLeft()
    {
        if (OnSwipeLeft != null)
            OnSwipeLeft();
    }
}
