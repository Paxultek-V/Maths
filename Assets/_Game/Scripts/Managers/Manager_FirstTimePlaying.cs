using UnityEngine;

public class Manager_FirstTimePlaying : MonoBehaviour
{
    [SerializeField] private string m_isFirstTimePlayingSaveKey = "FirstTimePlaying";

    public static Manager_FirstTimePlaying Instance;

    
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public bool IsFirstTimePlaying()
    {
        if (PlayerPrefs.HasKey(m_isFirstTimePlayingSaveKey) == false)
        {
            PlayerPrefs.SetInt(m_isFirstTimePlayingSaveKey, 1);
            return true;
        }

        return false;
    }
}