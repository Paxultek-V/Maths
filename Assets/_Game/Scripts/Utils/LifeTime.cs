using UnityEngine;

public class LifeTime : MonoBehaviour
{
    [SerializeField] private float m_lifeTime = 5f;

    private float m_timer = 0;

    private void Update()
    {
        m_timer += Time.deltaTime;
        
        if(m_timer > m_lifeTime)
            Destroy(gameObject);
    }
}
