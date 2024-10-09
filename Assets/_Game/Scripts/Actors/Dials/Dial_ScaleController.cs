using UnityEngine;

public class Dial_ScaleController : MonoBehaviour
{
    
    private const float DIAL_SCALE_OFFSET = 1f;
    
    
    [Header("Scale Params")] [SerializeField]
    private Transform m_scaleController = null;
    
    public void Initialize(int dialOrderNumber)
    {
        UpdateDialScale(dialOrderNumber);
    }
    
    
    private void UpdateDialScale(int dialOrderNumber)
    {
        m_scaleController.transform.localScale =
            new Vector3((dialOrderNumber + DIAL_SCALE_OFFSET), (dialOrderNumber + DIAL_SCALE_OFFSET), 1f);
    }
}
