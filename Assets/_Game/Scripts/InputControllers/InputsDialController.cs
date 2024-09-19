using System;
using UnityEngine;

public class InputsDialController : MonoBehaviour
{
    public static Action<Collider, Vector3> OnSelectDial;
    public static Action<Collider> OnReleaseDial;
    public static Action<Vector3, float> OnSendRotationInfos;
    public static Action OnNoDialSelected;

    [SerializeField] private LayerMask m_effectiveLayer = 0;

    [SerializeField] private float m_pixelsToAngleDegreesFactor = 0.2f;

    [SerializeField] private float m_pixelCountThresholdToConsiderMovement = 20;
    
    private Vector3 m_cursorStartPosition;
    private Collider m_currentSelectedDial;

    private void OnEnable()
    {
        Controller.OnTapBegin += OnTapBegin;
        Controller.OnHold += OnHold;
        Controller.OnRelease += OnRelease;
    }

    private void OnDisable()
    {
        Controller.OnTapBegin -= OnTapBegin;
        Controller.OnHold -= OnHold;
        Controller.OnRelease -= OnRelease;
    }

    private void OnTapBegin(Vector3 cursorPosition)
    {
        RaycastHit hit;

        m_cursorStartPosition = cursorPosition;
        
        Ray ray = Camera.main!.ScreenPointToRay(cursorPosition);

        if (Physics.Raycast(ray, out hit, 5000f, m_effectiveLayer))
        {
            m_currentSelectedDial = hit.collider;
            //Debug.Log("hit", hit.collider.gameObject);
            OnSelectDial?.Invoke(m_currentSelectedDial, m_cursorStartPosition);
        }
        else
            OnNoDialSelected?.Invoke();
    }

    private void OnHold(Vector3 cursorPosition)
    {
        float directionDistance = cursorPosition.x - m_cursorStartPosition.x;

        if(Mathf.Abs(directionDistance) < m_pixelCountThresholdToConsiderMovement)
            return;
        
        OnSendRotationInfos?.Invoke(cursorPosition, directionDistance * m_pixelsToAngleDegreesFactor);
    }

    private void OnRelease(Vector3 cursorPosition)
    {
        OnReleaseDial?.Invoke(m_currentSelectedDial);
        OnNoDialSelected?.Invoke();
        m_currentSelectedDial = null;
    }
}