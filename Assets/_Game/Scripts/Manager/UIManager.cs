using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    public GameObject mainMenu;
    public GameObject pauseMenu;
    public GameObject finnishMenu;
    public GameObject gameOverMenu;
    public GameObject settingMenu;

    private void Start()
    {
        ShowMainMenu(); // Hiển thị MainMenu khi bắt đầu trò chơi
        Time.timeScale = 0f;
    }

    public void ShowMainMenu()
    {
        HideAllMenus();
        mainMenu.SetActive(true);
    }

    public void ShowPauseMenu()
    {
        HideAllMenus();
        pauseMenu.SetActive(true);
    }

    public void ShowFinnishMenu()
    {
        HideAllMenus();
        finnishMenu.SetActive(true);
    }

    public void ShowGameOverMenu()
    {
        HideAllMenus();
        gameOverMenu.SetActive(true);
    }
    
    public void ShowGameSettingMenu()
    {
        HideAllMenus();
        settingMenu.SetActive(true);
    }

    public void HideAllMenus()
    {
        mainMenu.SetActive(false);
        pauseMenu.SetActive(false);
        finnishMenu.SetActive(false);
        gameOverMenu.SetActive(false);
        settingMenu.SetActive(false);
    }
}
