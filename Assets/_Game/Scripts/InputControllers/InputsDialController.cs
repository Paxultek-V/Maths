using System;
using UnityEngine;

public enum ControlMode
{
    Tap,
    TapSided,
    Hold
}

public class InputsDialController : MonoBehaviour
{
    public static Action<Collider, Vector3, ControlMode> OnSelectDial;
    public static Action<Collider, ControlMode> OnReleaseDial;
    public static Action<Vector3, float, ControlMode> OnHoldDial;
    public static Action OnNoDialSelected;
    public static Action<ControlMode> OnSendControlsMode;

    [SerializeField] private ControlMode m_controlMode;

    [SerializeField] private LayerMask m_effectiveLayer = 0;

    [SerializeField] private float m_pixelsToAngleDegreesFactor = 0.2f;

    [SerializeField] private float m_pixelCountThresholdToConsiderMovement = 20;

    private Vector3 m_cursorStartPosition;
    private Collider m_currentSelectedDial;
    private bool m_isControlEnabled;

    private void OnEnable()
    {
        Dials_Controller.OnDialsInitialized += EnableControls;
        Manager_GameState.OnSendCurrentGameState += OnSendCurrentGameState;
        LocksController.OnAllLocksDestroyed += DisableControls;
        
        Controller.OnTapBegin += OnTapBegin;
        Controller.OnHold += OnHold;
        Controller.OnRelease += OnRelease;
        UI_Button_SwitchControls.OnSwitchControlButtonPressed += OnSwitchControlButtonPressed;
    }

    private void OnDisable()
    {
        Dials_Controller.OnDialsInitialized -= EnableControls;
        Manager_GameState.OnSendCurrentGameState -= OnSendCurrentGameState;
        LocksController.OnAllLocksDestroyed -= DisableControls;
        
        Controller.OnTapBegin -= OnTapBegin;
        Controller.OnHold -= OnHold;
        Controller.OnRelease -= OnRelease;
        UI_Button_SwitchControls.OnSwitchControlButtonPressed -= OnSwitchControlButtonPressed;
    }

    private void Start()
    {
        OnSendControlsMode?.Invoke(m_controlMode);
    }

    private void OnTapBegin(Vector3 cursorPosition)
    {
        if(m_isControlEnabled == false)
            return;
        
        RaycastHit hit;

        m_cursorStartPosition = cursorPosition;

        Ray ray = Camera.main!.ScreenPointToRay(cursorPosition);

        if (Physics.Raycast(ray, out hit, 5000f, m_effectiveLayer))
        {
            m_currentSelectedDial = hit.collider;
            
            OnSelectDial?.Invoke(m_currentSelectedDial, m_cursorStartPosition, m_controlMode);
        }
        else
            OnNoDialSelected?.Invoke();
    }

    private void OnHold(Vector3 cursorPosition)
    {
        if(m_isControlEnabled == false)
            return;
        
        if (m_controlMode != ControlMode.Hold)
            return;

        float directionDistance = cursorPosition.x - m_cursorStartPosition.x;

        if (Mathf.Abs(directionDistance) < m_pixelCountThresholdToConsiderMovement)
            return;

        OnHoldDial?.Invoke(cursorPosition, directionDistance * m_pixelsToAngleDegreesFactor, m_controlMode);
    }

    private void OnRelease(Vector3 cursorPosition)
    {
        if(m_isControlEnabled == false)
            return;
        
        OnReleaseDial?.Invoke(m_currentSelectedDial, m_controlMode);
        OnNoDialSelected?.Invoke();
        m_currentSelectedDial = null;
    }

    private void OnSwitchControlButtonPressed()
    {
        if (m_controlMode == ControlMode.Hold)
        {
            m_controlMode = ControlMode.Tap;
        }
        else if (m_controlMode == ControlMode.Tap)
        {
            m_controlMode = ControlMode.TapSided;
        }
        else if (m_controlMode == ControlMode.TapSided)
        {
            m_controlMode = ControlMode.Hold;
        }
        
        OnSendControlsMode?.Invoke(m_controlMode);
    }

    
    
    private void OnSendCurrentGameState(GameState state)
    {
        if(state == GameState.Gameover)
            DisableControls();
    }
    
    private void EnableControls()
    {
        m_isControlEnabled = true;
    }

    private void DisableControls()
    {
        m_isControlEnabled = false;
    }
}