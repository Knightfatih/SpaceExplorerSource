using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject settingsMenu;
    public static bool gameIsPaused;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        settingsMenu.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void Pause()
    {
        pauseMenu.SetActive(true);
        settingsMenu.SetActive(false);
        Time.timeScale = 0f;
        gameIsPaused = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void SettingMenu()
    {
        pauseMenu.SetActive(false);
        settingsMenu.SetActive(true);
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        gameIsPaused = false;
        SceneManager.LoadScene("MainMenu");
    }

    public void Exit()
    {
        Debug.Log("Exit");
        Application.Quit();
    }
}
