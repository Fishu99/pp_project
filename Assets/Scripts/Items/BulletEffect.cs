using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A class for playing bullet sound effects.
/// </summary>
public class BulletEffect : MonoBehaviour
{

    [SerializeField]
    AudioClip [] hittedClips;

    [SerializeField]
    AudioClip [] missedClips;

    [SerializeField]
    float timeToDie = 5f;

    [SerializeField]
    AudioSource audioSource;
    // Start is called before the first frame update
    void Awake(){
        
    }

    /// <summary>
    /// Starts the effect.
    /// </summary>
    /// <param name="isHitted">true if the object was hit</param>
    public void Init(bool isHitted){
        if(isHitted && hittedClips.Length > 0){
            audioSource.PlayOneShot(hittedClips[Random.Range(0,hittedClips.Length)]);
        }else if(!isHitted && missedClips.Length > 0){
            audioSource.PlayOneShot(missedClips[Random.Range(0,missedClips.Length)]);
        }
        Destroy(gameObject, timeToDie);
    }


}
