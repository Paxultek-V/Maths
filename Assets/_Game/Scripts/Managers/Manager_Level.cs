using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager_Level : GameflowBehavior
{
    public static Action OnLevelInstantiated;
    public static Action<bool> OnLevelEnd; //bool : is win or lose
    public static Action<int> OnSendCurrentLevel;
    public static Action<int> OnSendTotalLevelCleared;

    
    [SerializeField] private string m_levelSaveKey = "Level";
    [SerializeField] private string m_totalLevelClearedSaveKey = "LevelCleared";

    [SerializeField] private List<LevelBase> m_levelSetList = null;

    [SerializeField] private Transform m_levelParent = null;

    private Coroutine m_instantiateLevelCoroutine;
    private LevelBase m_currentLevel;
    private int m_currentLevelIndex;
    private int m_totalLevelCleared;

    private void Awake()
    {
        LoadLevelIndex();
        LoadTotalLevelCleared();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        //UI_Button_NextLevel.OnNextLevelButtonPressed += OnNextLevelButtonPressed;
        UI_ButtonDebug_NextLevel.OnDebugNextLevelButtonPressed += OnDebugNextLevelButtonPressed;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        //UI_Button_NextLevel.OnNextLevelButtonPressed -= OnNextLevelButtonPressed;
        UI_ButtonDebug_NextLevel.OnDebugNextLevelButtonPressed -= OnDebugNextLevelButtonPressed;
    }

    protected override void OnMainMenu()
    {
        base.OnMainMenu();
        DestroyLevel();
    }

    protected override void OnGameplay()
    {
        base.OnGameplay();
        InstantiateLevel();
    }

    protected override void OnVictory()
    {
        base.OnVictory();
        m_currentLevelIndex = (m_currentLevelIndex+1) % m_levelSetList.Count;
        m_totalLevelCleared++;

        SaveLevel();
        SaveTotalLevelCleared();
        OnSendTotalLevelCleared?.Invoke(m_totalLevelCleared);
        OnLevelEnd?.Invoke(true);
    }

    protected override void OnGameover()
    {
        base.OnGameover();
        DestroyLevel();
        OnLevelEnd?.Invoke(false);
    }

    private void OnNextLevelButtonPressed()
    {
        InstantiateLevel();
    }

    private void OnDebugNextLevelButtonPressed()
    {
        m_currentLevelIndex = (m_currentLevelIndex+1) % m_levelSetList.Count;
        m_totalLevelCleared++;

        SaveLevel();
        SaveTotalLevelCleared();

        InstantiateLevel();
    }
    
    private void InstantiateLevel()
    {
        if(m_instantiateLevelCoroutine != null)
            StopCoroutine(m_instantiateLevelCoroutine);
        
        m_instantiateLevelCoroutine = StartCoroutine(InstantiateLevelCoroutine());
    }

    private IEnumerator InstantiateLevelCoroutine()
    {
        DestroyLevel();
        
        yield return new WaitForEndOfFrame();
        
        m_currentLevel = Instantiate(m_levelSetList[m_currentLevelIndex], m_levelParent);
        
        //Waiting a few frames to make sure all the level is instantiated
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        
        m_currentLevel.Initialize();

        yield return new WaitForEndOfFrame();
        
        OnSendCurrentLevel?.Invoke(m_currentLevelIndex);
        OnSendTotalLevelCleared?.Invoke(m_totalLevelCleared);
        OnLevelInstantiated?.Invoke();
    }

    private void DestroyLevel()
    {
        foreach (Transform child in m_levelParent)
        {
            Destroy(child.gameObject);
        }
    }

    private void LoadTotalLevelCleared()
    {
        if (PlayerPrefs.HasKey(m_totalLevelClearedSaveKey))
        {
            m_totalLevelCleared = PlayerPrefs.GetInt(m_totalLevelClearedSaveKey);
        }
        else
        {
            m_totalLevelCleared = 0;
            SaveTotalLevelCleared();
        }

        OnSendTotalLevelCleared?.Invoke(m_totalLevelCleared);
    }
    
    private void SaveTotalLevelCleared()
    {
        PlayerPrefs.SetInt(m_totalLevelClearedSaveKey, m_totalLevelCleared);
    }
    
    private void LoadLevelIndex()
    {
        if (PlayerPrefs.HasKey(m_levelSaveKey))
        {
            m_currentLevelIndex = PlayerPrefs.GetInt(m_levelSaveKey);
        }
        else
        {
            m_currentLevelIndex = 0;
            SaveLevel();
        }
        
        OnSendCurrentLevel?.Invoke(m_currentLevelIndex);
    }

    private void SaveLevel()
    {
        PlayerPrefs.SetInt(m_levelSaveKey, m_currentLevelIndex);
    }

    
}