using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsUIManager : MonoBehaviour
{
    public TMP_Dropdown levelDropdown;
    public Button applyButton;

    private void Start()
    {
        InitializeLevelDropdown();
        applyButton.onClick.AddListener(OnApplyButtonClicked);
    }

    private void InitializeLevelDropdown()
    {
        List<string> levelNames = new List<string>();
        for (int i = 0; i < Lvmanager.Instance.maps.Count; i++)
        {
            levelNames.Add("Level " + (i + 1));
        }

        levelDropdown.ClearOptions();
        levelDropdown.AddOptions(levelNames);
    }

    private void OnApplyButtonClicked()
    {
        Time.timeScale = 1.0f;
        int selectedLevel = levelDropdown.value;
        if (selectedLevel == Lvmanager.Instance.currentMapIndex)
        {
            Lvmanager.Instance.ReplayCurrentLevel();
        }
        else
        {
            Lvmanager.Instance.LoadMap(selectedLevel);
        }
        UIManager.Instance.HideAllMenus();
    }
}
