using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dial_SeparatorsController : MonoBehaviour
{
    private const float DIAL_SCALE_OFFSET = 1f;

    [Header("Separator Params")] [SerializeField]
    private GameObject m_separatorPrefab = null;

    [SerializeField] private Transform m_separatorsParent = null;


    public void Initialize(int dialOrderNumber, int numbersCountOnDial, float angleStep)
    {
        GenerateSeparators(dialOrderNumber, numbersCountOnDial, angleStep);
    }

    private void GenerateSeparators(int dialOrderNumber, int numbersCountOnDial, float angleStep)
    {
        GameObject separatorBuffer;
        for (int i = 0; i < numbersCountOnDial; i++)
        {
            separatorBuffer = Instantiate(m_separatorPrefab, transform.position - Vector3.forward * 0.1f, Quaternion.identity,
                m_separatorsParent);
            Vector3 direction = CalculateSeparatorUpDirection(angleStep, i, dialOrderNumber);
            direction.z = 0f;
            separatorBuffer.transform.up = Quaternion.Euler(0, 0, angleStep / 2f) * direction;
            separatorBuffer.transform.localScale = new Vector3(1f, (dialOrderNumber + DIAL_SCALE_OFFSET), 1f);
        }
    }


    private Vector3 CalculateSeparatorUpDirection(float angleStep, int index, int dialOrderNumber)
    {
        Vector3 position = new Vector3(
            Mathf.Cos(Mathf.Deg2Rad * (angleStep * index)) * (dialOrderNumber) +
            transform.position.x,
            Mathf.Sin(Mathf.Deg2Rad * (angleStep * index)) * (dialOrderNumber) +
            transform.position.y,
            transform.position.z - 0.3f
        );

        return position - transform.position;
    }
}