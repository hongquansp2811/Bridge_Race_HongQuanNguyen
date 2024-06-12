using UnityEngine;
public class GameManager : Singleton<GameManager>
{
    public enum GameState
    {
        MainMenu,
        Playing,
        Paused,
        Finished,
        GameOver,
        GameSetting
    }

    public GameState gameState = GameState.MainMenu;

    public void ChangeState(GameState newState)
    {
        gameState = newState;

        switch (newState)
        {
            case GameState.MainMenu:
                UIManager.Instance.ShowMainMenu();
                break;
            case GameState.Playing:
                UIManager.Instance.HideAllMenus();
                Time.timeScale = 1f; // Chạy game khi chơi
                break;
            case GameState.Paused:
                UIManager.Instance.ShowPauseMenu();
                Time.timeScale = 0f; // Dừng game khi tạm dừng
                break;
            case GameState.Finished:
                UIManager.Instance.ShowFinnishMenu();
                Time.timeScale = 0f; // Dừng game khi hoàn thành
                break;
            case GameState.GameOver:
                UIManager.Instance.ShowGameOverMenu();
                Time.timeScale = 0f; // Dừng game khi game over
                break;
            case GameState.GameSetting:
                UIManager.Instance.ShowGameSettingMenu();
                Time.timeScale = 0f; // Dừng game khi game over
                break;
        }
    }

    public void MainMenu()
    {
        ChangeState(GameState.MainMenu);
    }

    public void StartGame()
    {
        ChangeState(GameState.Playing);
    }

    public void PauseGame()
    {
        ChangeState(GameState.Paused);
    }

    public void ResumeGame()
    {
        ChangeState(GameState.Playing);
    }

    public void SettingGame()
    {
        ChangeState(GameState.GameSetting);
    }

    public void FinishGame()
    {
        ChangeState(GameState.Finished);
    }

    public void GameOver()
    {
        ChangeState(GameState.GameOver);
    }

    public void Replay()
    {
        Time.timeScale = 1;
        UIManager.Instance.HideAllMenus();
        Lvmanager.Instance.ReplayCurrentLevel();
    }

    public void NextLevel()
    {
        Time.timeScale = 1;
        UIManager.Instance.HideAllMenus();
        Lvmanager.Instance.LoadNextLevel();
    }
}


