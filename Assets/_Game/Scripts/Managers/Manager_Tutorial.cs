using System;
using UnityEngine;

public class Manager_Tutorial : GameflowBehavior
{
    [SerializeField] private GameObject m_tutorialGameObject;

    private bool m_canDismissTutorial;

    protected override void OnEnable()
    {
        base.OnEnable();
        Controller.OnTapBegin += OnTapBegin;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        Controller.OnTapBegin -= OnTapBegin;
    }

    protected override void OnMainMenu()
    {
        base.OnMainMenu();

        if (Manager_FirstTimePlaying.Instance.IsFirstTimePlaying())
            m_tutorialGameObject.SetActive(true);
        else
            m_tutorialGameObject.SetActive(false);

        m_canDismissTutorial = false;
    }

    protected override void OnGameplay()
    {
        base.OnGameplay();
        m_canDismissTutorial = true;
    }

    private void OnTapBegin(Vector3 cursorPosition)
    {
        if (!m_canDismissTutorial)
            return;

        if (!m_tutorialGameObject.activeInHierarchy)
            return;

        m_tutorialGameObject.SetActive(false);
    }
}