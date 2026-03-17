using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] Button continueButton;
    [SerializeField] Button exitButton;
    [SerializeField] GameObject menu;

    bool isShown = false;

    private void Awake()
    {
        Hide();
        continueButton.onClick.AddListener(Hide);
        exitButton.onClick.AddListener(GoToMenu);
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void Show()
    {
        Time.timeScale = 0f;
        menu.SetActive(true);
        isShown = true;
    }

    public void Hide()
    {
        Time.timeScale = 1f;
        menu.SetActive(false);
        isShown = false;

    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isShown)
                Show();
            else
                Hide();
        }
    }
}
