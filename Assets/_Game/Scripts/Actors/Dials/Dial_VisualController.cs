using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Dial_VisualController : MonoBehaviour
{
    private static int m_indexOffset;
    
    [SerializeField] private Dial_VisualData_SO m_dialVisualData;

    [SerializeField] private MeshRenderer m_dialColorMeshRenderer = null;
    [SerializeField] private MeshRenderer m_dialOutlineMeshRenderer = null;

    [SerializeField] private Transform m_separatorParent;

    public static void SetRandomIndexOffset()
    {
        m_indexOffset = Random.Range(0, 7);
    }
    
    public void Initialize(int dialOrderNumber)
    {
        int visualIndex = (dialOrderNumber + m_indexOffset) % m_dialVisualData.VisualDataList.Count;
        Tuple<Material, Material> colorDuo = m_dialVisualData.VisualDataList[visualIndex];

        m_dialColorMeshRenderer.material = colorDuo.Item1;
        m_dialOutlineMeshRenderer.material = m_dialVisualData.OutlineMaterial;

        MeshRenderer[] separatorMeshRendererArray = m_separatorParent.GetComponentsInChildren<MeshRenderer>();

        for (int i = 0; i < separatorMeshRendererArray.Length; i++)
        {
            separatorMeshRendererArray[i].material = colorDuo.Item2;
        }
    }
}
