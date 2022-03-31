using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;
//created with tutorial https://www.youtube.com/watch?v=YOaYQrN1oYQ

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private AudioMixer mainMixer;

    Resolution[] screenResolutions;

    [SerializeField]
    TMP_Dropdown resolutionsDropdown;

    private void Start()
    {
        screenResolutions = Screen.resolutions;

        resolutionsDropdown.ClearOptions();

        List<string> resolutionOptions = new List<string>();
        int currentResolutionIndex = 0;
        for (int i = 0; i < screenResolutions.Length; i++)
        {
            resolutionOptions.Add($"{screenResolutions[i].width} x {screenResolutions[i].height}");
            if(screenResolutions[i].width == Screen.currentResolution.width && screenResolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionsDropdown.AddOptions(resolutionOptions);
        resolutionsDropdown.value = currentResolutionIndex;
        resolutionsDropdown.RefreshShownValue();
    }


    public void StartButtonPressed()
    {
        SceneManager.LoadScene(1);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void SetResolution(int resolutionIndex)
    {
        Resolution current = screenResolutions[resolutionIndex];
        Screen.SetResolution(current.width, current.height, Screen.fullScreen);
    }
    public void  SetMasterVolume(float volume)
    {
        mainMixer.SetFloat("masterVolume", volume);
    }    
    public void  SetSFXVolume(float volume)
    {
        mainMixer.SetFloat("sfxVolume", volume);
    }    
    public void  SetMusicVolume(float volume)
    {
        mainMixer.SetFloat("musicVolume", volume);
    }
    public void SetQuality(int detailLevel)
    {
        QualitySettings.SetQualityLevel(detailLevel);
    }
    public void SetFullscreen (bool fullscreen)
    {
        Screen.fullScreen = fullscreen;
    }
    public void ToggleActive(GameObject toggleGameObject)
    {
        if (toggleGameObject.activeInHierarchy)
        {
            toggleGameObject.SetActive(false);
        }
        else
        {
            toggleGameObject.SetActive(true);
        }
    }
    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void OpenAndi()
    {
        Application.OpenURL("https://ampersound.bandcamp.com/");
    }    
    public void OpenSuhpi()
    {
        Application.OpenURL("https://ultimacolor.com/");
    }    
    public void OpenTimbre()
    {
        Application.OpenURL("https://freesound.org/people/Timbre/");
    }
}
