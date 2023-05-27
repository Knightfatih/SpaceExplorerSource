using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject playMenu;
    public GameObject settingsMenu;
    public GameObject creditsMenu;
    public GameObject controlsMenu;
    public GameObject videoMenu;
    public GameObject audioMenu;

    public Button playButton;

    public Slider[] volumeSliders;
    public Toggle[] resolutionToggles;
    public Toggle fullscreenToogle;
    public int[] screenWidths;
    int activeScreenResolutionIndex;

    // Start is called before the first frame update
    public void Start()
    {
        activeScreenResolutionIndex = PlayerPrefs.GetInt("Screen Resolution Index");
        bool isFullscreen = (PlayerPrefs.GetInt("Fullscreen") == 1) ? true : false;

        for (int i = 0; i < resolutionToggles.Length; i++)
        {
            resolutionToggles[i].isOn = i == activeScreenResolutionIndex;
        }

        fullscreenToogle.isOn = isFullscreen;
    }

    public void HandleInputData(int val)
    {
        if (val == 0)
        {
            playButton.onClick.AddListener(() => Play("Classic"));
        }
        if (val == 1)
        {
            playButton.onClick.AddListener(() => Play("ECSConversion"));
        }
        if (val == 2)
        {
            playButton.onClick.AddListener(() => Play("ECSPure"));
        }

    }

    public void Play(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        Debug.Log(sceneName);
    }

    public void Demo()
    {
        SceneManager.LoadScene("Hipparcos");
    }

    public void Exit()
    {
        Debug.Log("Exit");
        Application.Quit();
    }

    public void MainMenu()
    {
        mainMenu.SetActive(true);
        settingsMenu.SetActive(false);
        creditsMenu.SetActive(false);
        playMenu.SetActive(false);
    }

    public void PlayMenu()
    {
        mainMenu.SetActive(false);
        playMenu.SetActive(true);
    }

    public void SettingsMenu()
    {
        mainMenu.SetActive(false);
        settingsMenu.SetActive(true);
        creditsMenu.SetActive(false);
        controlsMenu.SetActive(false);
        videoMenu.SetActive(false);
        audioMenu.SetActive(false);
    }

    public void CreditsMenu()
    {
        mainMenu.SetActive(false);
        settingsMenu.SetActive(false);
        creditsMenu.SetActive(true);
    }

    public void ControlsMenu()
    {
        controlsMenu.SetActive(true);
        settingsMenu.SetActive(false);
    }

    public void VideoMenu()
    {
        videoMenu.SetActive(true);
        settingsMenu.SetActive(false);
    }

    public void AudioMenu()
    {
        audioMenu.SetActive(true);
        settingsMenu.SetActive(false);
    }
    public void SetScreenResolution(int i)
    {
        if (resolutionToggles[i].isOn)
        {
            activeScreenResolutionIndex = i;
            float aspectRatio = 16 / 9f;
            Screen.SetResolution(screenWidths[i], (int)(screenWidths[i] / aspectRatio), false);
            PlayerPrefs.SetInt("Screen Resolution Index", activeScreenResolutionIndex);
            PlayerPrefs.Save();
        }
    }

    public void SetFullscreen(bool isFullscreen)
    {
        for (int i = 0; i < resolutionToggles.Length; i++)
        {
            resolutionToggles[i].interactable = !isFullscreen;
        }
        if (isFullscreen)
        {
            Resolution[] allResolutions = Screen.resolutions;
            Resolution maxResolution = allResolutions[allResolutions.Length - 1];
            Screen.SetResolution(maxResolution.width, maxResolution.height, true);
        }
        else
        {
            SetScreenResolution(activeScreenResolutionIndex);
        }
        PlayerPrefs.SetInt("Fullscreen", ((isFullscreen) ? 1 : 0));
        PlayerPrefs.Save();
    }

    public void SetMasterVolume(float value)
    {
        foreach (Slider slider in volumeSliders)
        {
            if (slider.name == "MasterVolume")
            {
                MusicManager musicManager = FindObjectOfType<MusicManager>();
                musicManager.SetVolume(slider.value);
                PlayerPrefs.SetFloat("MasterVolume", slider.value);
                PlayerPrefs.Save();
            }
        }
    }
}
