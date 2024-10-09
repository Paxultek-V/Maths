using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dial_WSUI : MonoBehaviour
{
    private const float WSUI_POSITION_RADIUS_OFFSET = 0.4f;
    
    [Header("UI Numbers Params")] [SerializeField]
    private WSUI_DialNumber m_wsuiDialNumberPrefab = null;

    [SerializeField] private GameObject m_anchorPrefab = null;

    [SerializeField] private Transform m_anchorParent = null;

    [SerializeField] private Transform m_wsuiParent = null;
    
    
    private List<Transform> m_wsuiAnchorsList;
    private WSUI_DialNumber m_wsuiNumberBuffer;
    
    public void Initialize(List<int> dialNumberList, int dialOrderNumber, int numbersCountOnDial, float angleStep)
    {
        GenerateWSUIAnchors(dialOrderNumber, numbersCountOnDial, angleStep);

        GenerateWSUI(dialNumberList, numbersCountOnDial);
    }
    
    private void GenerateWSUIAnchors(int dialOrderNumber, int numbersCountOnDial, float angleStep)
    {
        m_wsuiAnchorsList = new List<Transform>();

        for (int i = 0; i < numbersCountOnDial; i++)
        {
            Transform anchor = Instantiate(m_anchorPrefab, m_anchorParent).transform;

            anchor.position = new Vector3(
                Mathf.Cos(Mathf.Deg2Rad * (angleStep * i)) * (dialOrderNumber + WSUI_POSITION_RADIUS_OFFSET) +
                transform.position.x,
                Mathf.Sin(Mathf.Deg2Rad * (angleStep * i)) * (dialOrderNumber + WSUI_POSITION_RADIUS_OFFSET) +
                transform.position.y,
                transform.position.z - 0.3f
            );

            m_wsuiAnchorsList.Add(anchor);
        }
    }

    private void GenerateWSUI(List<int> dialNumberList, int numbersCountOnDial)
    {
        for (int i = 0; i < numbersCountOnDial; i++)
        {
            m_wsuiNumberBuffer = Instantiate(m_wsuiDialNumberPrefab, m_wsuiParent);
            m_wsuiNumberBuffer.Initialize(dialNumberList[i], m_wsuiAnchorsList[i]);
        }
    }
}
