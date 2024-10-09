using System;
using UnityEngine;

public class Dial_InputsReactor : MonoBehaviour
{
    public Action<ControlMode, Vector3> OnInteractionStart;
    public Action<ControlMode, Vector3, Vector3> OnInteractionContinued;
    public Action<ControlMode> OnInteractionEnd;

    [SerializeField] private bool m_isInteractable = true;

    [SerializeField] private Collider m_collider;

    private Vector3 m_dialCenterScreenPosition => Camera.main.WorldToScreenPoint(transform.position);
    private Vector3 m_cursorStartPositionRelativeToDial;
    private Vector3 m_cursorCurrentPositionRelativeToDial;
    private bool m_isSelected;


    private void OnEnable()
    {
        if (m_isInteractable == false)
            return;

        InputsDialController.OnSelectDial += OnSelectDial;
        InputsDialController.OnHoldDial += OnHoldDial;
        InputsDialController.OnNoDialSelected += OnNoDialSelected;
        InputsDialController.OnReleaseDial += OnReleaseDial;
    }

    private void OnDisable()
    {
        if (m_isInteractable == false)
            return;

        InputsDialController.OnSelectDial -= OnSelectDial;
        InputsDialController.OnHoldDial -= OnHoldDial;
        InputsDialController.OnNoDialSelected -= OnNoDialSelected;
        InputsDialController.OnReleaseDial -= OnReleaseDial;
    }


    #region INPUTS

    private void OnSelectDial(Collider selectedDialCollider, Vector3 cursorPosition, ControlMode controlMode)
    {
        m_isSelected = m_collider == selectedDialCollider;
        
        if (!m_isSelected)
            return;
        
        m_cursorStartPositionRelativeToDial = cursorPosition - m_dialCenterScreenPosition;
        m_cursorStartPositionRelativeToDial.z = 0;
        m_cursorStartPositionRelativeToDial.Normalize();

        OnInteractionStart?.Invoke(controlMode, m_cursorStartPositionRelativeToDial);
    }

    private void OnHoldDial(Vector3 cursorPosition, float rotationAngle, ControlMode controlMode)
    {
        if (!m_isSelected)
            return;

        m_cursorCurrentPositionRelativeToDial = cursorPosition - m_dialCenterScreenPosition;
        m_cursorCurrentPositionRelativeToDial.z = 0;
        m_cursorCurrentPositionRelativeToDial.Normalize();

        OnInteractionContinued?.Invoke(controlMode, m_cursorStartPositionRelativeToDial,
            m_cursorCurrentPositionRelativeToDial);
    }

    private void OnNoDialSelected()
    {
        m_isSelected = false;
    }

    private void OnReleaseDial(Collider dialCollider, ControlMode controlMode)
    {
        if (dialCollider != m_collider)
            return;

        OnInteractionEnd?.Invoke(controlMode);
    }

    #endregion
}