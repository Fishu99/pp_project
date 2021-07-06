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
    Slider sliderSensitiveMouse;

    [SerializeField]
    Slider sliderSoundsVolume;

    [SerializeField]
    RectTransform submenuOptions;

    [SerializeField]
    RectTransform submenuAuthors;

    Submenu currentSubmenu;

    void Start(){
        currentSubmenu = Submenu.Null;
        RefreshSubmenu();
        GetValuesFromPlayerPrefs();
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

    public void SavePlayerPrefs(){

        PlayerPrefs.SetFloat("mouseSensitive", sliderSensitiveMouse.value);
        PlayerPrefs.SetFloat("soundsVolume", sliderSoundsVolume.value);

        PlayerPrefs.Save();
    }

    public void RevertOptions(){
        GetValuesFromPlayerPrefs();
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

    void GetValuesFromPlayerPrefs(){
        if(PlayerPrefs.HasKey("mouseSensitive")){
            sliderSensitiveMouse.value = PlayerPrefs.GetFloat("mouseSensitive");
        }
        if(PlayerPrefs.HasKey("soundsVolume")){
            sliderSoundsVolume.value = PlayerPrefs.GetFloat("soundsVolume");
        }
    }
}
