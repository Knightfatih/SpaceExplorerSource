﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Audio;

namespace MainMenu
{
    public class MainMenuUI : MonoBehaviour
    {
        //Menu Game Objects
        public GameObject mainMenu;
        public GameObject playMenu;
        public GameObject settingsMenu;
        public GameObject creditsMenu;
        public GameObject controlsMenu;
        public GameObject videoMenu;
        public GameObject audioMenu;

        //UI Components
        public Button playButton;
        public Slider[] volumeSliders;
        public Toggle[] resolutionToggles;
        public Toggle fullscreenToogle;
        public int[] screenWidths;
        int activeScreenResolutionIndex;

        //Reference
        private MusicManager musicManager;

        // Start is called before the first frame update
        public void Start()
        {
            musicManager = FindObjectOfType<MusicManager>();

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
            switch (val)
            {
                case 0:
                    playButton.onClick.AddListener(() => Play("Classic"));
                    break;
                case 1:
                    playButton.onClick.AddListener(() => Play("ECSConversion"));
                    break;
                case 2:
                    playButton.onClick.AddListener(() => Play("ECSPure"));
                    break;
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

        public void ShowMenu(GameObject menuToShow)
        {
            mainMenu.SetActive(false);
            playMenu.SetActive(false);
            settingsMenu.SetActive(false);
            creditsMenu.SetActive(false);
            controlsMenu.SetActive(false);
            videoMenu.SetActive(false);
            audioMenu.SetActive(false);

            menuToShow.SetActive(true);
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
            PlayerPrefs.SetInt("Fullscreen", isFullscreen ? 1 : 0);
            PlayerPrefs.Save();
            Debug.Log(isFullscreen);
        }

        public void SetMasterVolume(float value)
        {
            foreach (Slider slider in volumeSliders)
            {
                if (slider.name == "MasterVolume")
                {
                    musicManager.SetVolume(slider.value);
                    PlayerPrefs.SetFloat("MasterVolume", slider.value);
                    PlayerPrefs.Save();
                }
            }
        }
    }

}
