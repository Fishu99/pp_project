using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

/// <summary>
/// Class for managing audio settings.
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour{

    /// <summary>
    /// Types of volume which can be controlled by the player.
    /// </summary>
    public static string [] NamesVolume{
        get{
            return new string [3]{"masterVolume", "musicVolume", "soundsVolume"};
        }
    }

    /// <summary>
    /// The singleton instance of the class.
    /// </summary>
    public static AudioManager Instance { private set; get; }

    /// <summary>
    /// The mixer used for mixing audio sources.
    /// </summary>
    [SerializeField]
    AudioMixer audioMixer;

    /// <summary>
    /// The Audio source used for playing the music.
    /// </summary>
    AudioSource audioSource;

    /// <summary>
    /// Creates the singleton instance of the manager and plays menu music.
    /// </summary>
    void Awake(){
        if(Instance == null){
            Instance = this;
        }else if(Instance != this){
            Destroy(gameObject);
        }

        audioSource = GetComponent<AudioSource>();
        audioSource.Play();
    }

    /// <summary>
    /// Reads saved values from PlayerPrefs.
    /// </summary>
    void Start(){
        ResetValuesByPlayerPrefs();
    }

    /// <summary>
    /// Sets the volumes.
    /// </summary>
    /// <param name="values">An array of volumes corresponding to the volume types from NamesVolume (masterVolume, musicVolume, soundsVolume).</param>
    public void SetVolumes(float [] values){
        string [] names = NamesVolume;

        for(int i = 0; i < values.Length && i < names.Length; i++){
            audioMixer.SetFloat(names[i], values[i]);
        } 
    }

    /// <summary>
    /// Saves the volumes to the PlayerPrefs.
    /// </summary>
    /// <param name="values">An array of volumes corresponding to the volume types from NamesVolume (masterVolume, musicVolume, soundsVolume).</param>
    public void SavePlayerPrefs(float [] values){

        string [] names = NamesVolume;

        for(int i = 0; i < values.Length && i < names.Length; i++){
            PlayerPrefs.SetFloat(names[i], values[i]);
        } 

        PlayerPrefs.Save();
    }

    /// <summary>
    /// Reads the volume values from PlayerPrefs.
    /// </summary>
    /// <returns>An array of volumes corresponding to the volume types from NamesVolume (masterVolume, musicVolume, soundsVolume).</returns>
    public float [] GetValuesByPlayerPrefs(){
        string [] names = NamesVolume;
        float [] values = new float[names.Length];
        for(int i = 0; i < values.Length && i < names.Length; i++){
            values[i] = PlayerPrefs.GetFloat(names[i]);
        }
        return values;
    }

    /// <summary>
    /// Sets the volume values according to the values from PlayerPrefs.
    /// </summary>
    public void ResetValuesByPlayerPrefs(){
        LoadVolumesFromPalyerPrefs();
    }

    /// <summary>
    /// Sets the volume values according to the values from PlayerPrefs.
    /// </summary>
    void LoadVolumesFromPalyerPrefs(){

        string [] names = NamesVolume;

        foreach(string name in NamesVolume){
            if(PlayerPrefs.HasKey(name)){
                audioMixer.SetFloat(name, PlayerPrefs.GetFloat(name));
            }
        } 

    }
}
