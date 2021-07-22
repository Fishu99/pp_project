using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour{

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

    Submenu currentSubmenu;

    void Start(){
        currentSubmenu = Submenu.Null;
        audioSource.Stop();
        RefreshSubmenu();
        RevertOptions();
    }

    public void ChangeScene(int index){
        SceneManager.LoadScene(index);
    }

    public void ExitApp(){
        Application.Quit();
    }

    public void OpenSubmenu(int newSubmenu){
        Submenu newSub = (Submenu)newSubmenu;
        if(newSub == currentSubmenu){
            currentSubmenu = Submenu.Null;
        }else{
            currentSubmenu = newSub;
        }
        
        RefreshSubmenu();
    }

    public void RevertOptions(){
        AudioManager.Instance.ResetValuesByPlayerPrefs();
        RefreshSliders();
    }

    public void SaveOptions(){
        AudioManager.Instance.SavePlayerPrefs(GetVolumesBySlider());
    }

    public void PlayRandomSound(){
        if(!audioSource.isPlaying){
            audioSource.Play();
        }
    }

    public void RefreshVolumes(){
        AudioManager.Instance.SetVolumes(GetVolumesBySlider());
    }

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

    void RefreshSliders(){
        float [] values = AudioManager.Instance.GetValuesByPlayerPrefs();
        sliderMasterVolume.value = values[0];
        sliderMusicVolume.value = values[1];
        sliderSoundsVolume.value = values[2];
    }

    float [] GetVolumesBySlider(){
        return new float[3]{
            sliderMasterVolume.value,
            sliderMusicVolume.value,
            sliderSoundsVolume.value
        };
    }

}
