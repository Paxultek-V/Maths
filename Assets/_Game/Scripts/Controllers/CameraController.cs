using Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera m_vCam = null;

    [SerializeField] private int m_baseSize = 5;

    private void OnEnable()
    {
        Dials_Controller.OnSendTotalDialCount += UpdateOrthoSize;
    }

    private void OnDisable()
    {
        Dials_Controller.OnSendTotalDialCount -= UpdateOrthoSize;
    }


    private void UpdateOrthoSize(int dialCount)
    {
        m_vCam.m_Lens.OrthographicSize = m_baseSize + dialCount * 2;
    }
}