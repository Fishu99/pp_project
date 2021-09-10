using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Script controlling the main menu.
/// </summary>
public class MainMenu : MonoBehaviour{

    /// <summary>
    /// Enum for the types of submenu.
    /// </summary>
    public enum Submenu{
        Null = 0,
        Options,
        Authors
    }

    [SerializeField]
    Slider sliderMasterVolume;

    [SerializeField]
    Slider sliderMusicVolume;

    [SerializeField]
    Slider sliderSoundsVolume;

    [SerializeField]
    RectTransform submenuOptions;

    [SerializeField]
    RectTransform submenuAuthors;

    [SerializeField]
    AudioSource audioSource;

    /// <summary>
    /// The currently visible submenu.
    /// </summary>
    Submenu currentSubmenu;

    void Start(){
        currentSubmenu = Submenu.Null;
        audioSource.Stop();
        RefreshSubmenu();
        RevertOptions();
    }

    /// <summary>
    /// Loads a new scene.
    /// </summary>
    /// <param name="name">scene to load</param>
    public void ChangeScene(string name){
        SceneManager.LoadScene(name);
    }

    /// <summary>
    /// Exits the game.
    /// </summary>
    public void ExitApp(){
        Application.Quit();
    }

    /// <summary>
    /// Opens a submenu.
    /// </summary>
    /// <param name="newSubmenu">index of the submenu</param>
    public void OpenSubmenu(int newSubmenu){
        Submenu newSub = (Submenu)newSubmenu;
        if(newSub == currentSubmenu){
            currentSubmenu = Submenu.Null;
        }else{
            currentSubmenu = newSub;
        }
        
        RefreshSubmenu();
    }

    /// <summary>
    /// Reverts the option changes and resets the controls according to the playerPrefs.
    /// </summary>
    public void RevertOptions(){
        AudioManager.Instance.ResetValuesByPlayerPrefs();
        RefreshSliders();
    }

    /// <summary>
    /// Saves the option changes to PlayerPrefs.
    /// </summary>
    public void SaveOptions(){
        AudioManager.Instance.SavePlayerPrefs(GetVolumesBySlider());
    }

    /// <summary>
    /// Plays a sound using the audioSource.
    /// </summary>
    public void PlayRandomSound(){
        if(!audioSource.isPlaying){
            audioSource.Play();
        }
    }

    /// <summary>
    /// Sets the volumes in AudioManager according to the values set by sliders.
    /// </summary>
    public void RefreshVolumes(){
        AudioManager.Instance.SetVolumes(GetVolumesBySlider());
    }

    /// <summary>
    /// Shows the appropriate submenu depending on the value of currentSubmenu.
    /// </summary>
    void RefreshSubmenu(){

        switch(currentSubmenu){
            case Submenu.Options:
            submenuOptions.gameObject.SetActive(true);
            submenuAuthors.gameObject.SetActive(false);
            break;
            case Submenu.Authors:
            submenuOptions.gameObject.SetActive(false);
            submenuAuthors.gameObject.SetActive(true);
            break;
            default:
            submenuOptions.gameObject.SetActive(false);
            submenuAuthors.gameObject.SetActive(false);
            break;
        }

    }

    /// <summary>
    /// Sets thevolume sliders according to the playerPrefs.
    /// </summary>
    void RefreshSliders(){
        float [] values = AudioManager.Instance.GetValuesByPlayerPrefs();
        sliderMasterVolume.value = values[0];
        sliderMusicVolume.value = values[1];
        sliderSoundsVolume.value = values[2];
    }

    /// <summary>
    /// Gets an array of volumes from the three sliders.
    /// </summary>
    /// <returns>an array of volumes from the three sliders</returns>
    float[] GetVolumesBySlider(){
        return new float[3]{
            sliderMasterVolume.value,
            sliderMusicVolume.value,
            sliderSoundsVolume.value
        };
    }

}
