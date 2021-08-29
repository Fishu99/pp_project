using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour{

    public static string [] NamesVolume{
        get{
            return new string [3]{"masterVolume", "musicVolume", "soundsVolume"};
        }
    }

    public static AudioManager Instance { private set; get; }

    [SerializeField]
    AudioMixer audioMixer;

    AudioSource audioSource;

    void Awake(){
        if(Instance == null){
            Instance = this;
        }else if(Instance != this){
            Destroy(gameObject);
        }
       // DontDestroyOnLoad(gameObject);

        audioSource = GetComponent<AudioSource>();
        audioSource.Play();
    }

    void Start(){
        ResetValuesByPlayerPrefs();
    }

    public void SetVolumes(float [] values){
        string [] names = NamesVolume;

        for(int i = 0; i < values.Length && i < names.Length; i++){
            audioMixer.SetFloat(names[i], values[i]);
        } 
    }
    
    public void SavePlayerPrefs(float [] values){

        string [] names = NamesVolume;

        for(int i = 0; i < values.Length && i < names.Length; i++){
            PlayerPrefs.SetFloat(names[i], values[i]);
        } 

        PlayerPrefs.Save();
    }

    public float [] GetValuesByPlayerPrefs(){
        string [] names = NamesVolume;
        float [] values = new float[names.Length];
        for(int i = 0; i < values.Length && i < names.Length; i++){
            values[i] = PlayerPrefs.GetFloat(names[i]);
        }
        return values;
    }

    public void ResetValuesByPlayerPrefs(){
        LoadVolumesFromPalyerPrefs();
    }

    void LoadVolumesFromPalyerPrefs(){

        string [] names = NamesVolume;

        foreach(string name in NamesVolume){
            if(PlayerPrefs.HasKey(name)){
                audioMixer.SetFloat(name, PlayerPrefs.GetFloat(name));
            }
        } 

    }
}
