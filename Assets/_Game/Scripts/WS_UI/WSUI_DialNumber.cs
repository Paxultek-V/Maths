using TMPro;
using UnityEngine;

public class WSUI_DialNumber : MonoBehaviour
{
    [SerializeField] private TMP_Text m_numberText = null;

    private Transform m_anchorTransformToFollow;

    public void Initialize(int number, Transform anchor)
    {
        m_numberText.text = number.ToString();
        m_anchorTransformToFollow = anchor;
    }


    private void Update()
    {
        if(m_anchorTransformToFollow == null)
            return;

        transform.position = m_anchorTransformToFollow.position;
    }
}
