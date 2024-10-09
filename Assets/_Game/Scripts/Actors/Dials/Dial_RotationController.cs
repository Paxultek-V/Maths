using System;
using System.Collections;
using UnityEngine;

public class Dial_RotationController : MonoBehaviour
{
    public Action<int, bool> OnApplyRotation;
    public Action OnRotationComplete;

    [Header("Rotation Params")] [SerializeField]
    private Transform m_rotationController = null;

    private Dial_InputsReactor m_dialInputsReactor;
    private Quaternion m_startRotationQuaternion;
    private Coroutine m_snapRotationCoroutine;
    private Coroutine m_moveByIncrementCoroutine;
    private float m_rotationAngle;
    private float m_angleRotationDiff;
    private float m_angleStep;
    private bool m_canRotate = true;

    private void Awake()
    {
        m_dialInputsReactor = GetComponent<Dial_InputsReactor>();
    }

    private void OnEnable()
    {
        m_dialInputsReactor.OnInteractionStart += OnInteractionStart;
        m_dialInputsReactor.OnInteractionContinued += OnInteractionContinued;
        m_dialInputsReactor.OnInteractionEnd += OnInteractionEnd;
    }

    private void OnDisable()
    {
        m_dialInputsReactor.OnInteractionStart -= OnInteractionStart;
        m_dialInputsReactor.OnInteractionContinued -= OnInteractionContinued;
        m_dialInputsReactor.OnInteractionEnd -= OnInteractionEnd;
    }

    public void Initialize(float newAngleStep)
    {
        m_angleStep = newAngleStep;
    }

    private void OnInteractionStart(ControlMode controlMode, Vector3 cursorStartPositionRelativeToDial)
    {
        if(!m_canRotate)
            return;
        
        switch (controlMode)
        {
            case ControlMode.Tap:
                if (m_moveByIncrementCoroutine != null)
                    StopCoroutine(m_moveByIncrementCoroutine);

                m_moveByIncrementCoroutine = StartCoroutine(MoveByIncrement(1));
                break;
            case ControlMode.TapSided:
                if (m_moveByIncrementCoroutine != null)
                    StopCoroutine(m_moveByIncrementCoroutine);

                m_moveByIncrementCoroutine = StartCoroutine(MoveByIncrement(cursorStartPositionRelativeToDial.x > 0 ? -1 : 1));
                break;
            case ControlMode.Hold:
                m_startRotationQuaternion = m_rotationController.rotation;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(controlMode), controlMode, null);
        }
    }

    private void OnInteractionContinued(ControlMode controlMode, Vector3 cursorStartPositionRelativeToDial, Vector3 cursorCurrentPositionRelativeToDial)
    {
        if(controlMode != ControlMode.Hold)
            return;
        
        if(!m_canRotate)
            return;
        
        m_rotationAngle = Vector3.SignedAngle(cursorStartPositionRelativeToDial, cursorCurrentPositionRelativeToDial, transform.forward);

        Quaternion targetRotation =
            m_startRotationQuaternion * Quaternion.AngleAxis(m_rotationAngle, transform.forward);

        m_rotationController.rotation = targetRotation;
    }

    private void OnInteractionEnd(ControlMode controlMode)
    {
        if(!m_canRotate)
            return;
        
        if (controlMode == ControlMode.Hold)
        {
            if (m_snapRotationCoroutine != null)
                StopCoroutine(m_snapRotationCoroutine);

            m_snapRotationCoroutine = StartCoroutine(SnapDialRotation());
        }
    }

    #region SNAPING AND NUMBERS MANAGEMENTS

    private IEnumerator MoveByIncrement(int stepCount)
    {
        m_canRotate = false;

        OnApplyRotation?.Invoke(stepCount, false);

        float targetAngle = m_rotationController.rotation.eulerAngles.z + m_angleStep * stepCount;

        Vector3 targetRotation = m_rotationController.rotation.eulerAngles;
        targetRotation.z = targetAngle;

        Quaternion targetRotationQuaternion = Quaternion.Euler(targetRotation);

        while (Quaternion.Angle(m_rotationController.rotation, targetRotationQuaternion) > 1)
        {
            m_rotationController.rotation =
                Quaternion.RotateTowards(m_rotationController.rotation, targetRotationQuaternion,
                    Time.deltaTime * 360f);

            yield return new WaitForEndOfFrame();
        }

        m_rotationController.rotation = targetRotationQuaternion;

        OnRotationComplete?.Invoke();
        
        m_canRotate = true;
    }

    private IEnumerator SnapDialRotation()
    {
        float delta = m_rotationController.rotation.eulerAngles.z % m_angleStep;
        int stepCount = (int)m_rotationController.rotation.eulerAngles.z / (int)m_angleStep;

        if (Mathf.Abs(delta) >= m_angleStep / 2f)
            stepCount += (int)Mathf.Sign(delta);

        OnApplyRotation?.Invoke(stepCount, false);

        float targetAngle = m_angleStep * stepCount;

        Vector3 targetRotation = m_rotationController.rotation.eulerAngles;
        targetRotation.z = targetAngle;

        Quaternion targetRotationQuaternion = Quaternion.Euler(targetRotation);

        while (Quaternion.Angle(m_rotationController.rotation, targetRotationQuaternion) > 1)
        {
            m_rotationController.rotation =
                Quaternion.RotateTowards(m_rotationController.rotation, targetRotationQuaternion,
                    Time.deltaTime * 360f);

            yield return new WaitForEndOfFrame();
        }

        m_rotationController.rotation = targetRotationQuaternion;

        OnRotationComplete?.Invoke();
    }

    #endregion
}