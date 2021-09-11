using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Functionality of the portal is contained in this class.
/// </summary>
public class Portal : MonoBehaviour
{
    //Portal Statuses for next level (causes a different look of the portal)
    /// <summary>
    /// Is next level a normal level.
    /// </summary>
    private bool toNormalLevel = false;
    /// <summary>
    /// Is next level a bonus level.
    /// </summary>
    private bool toBonusLevel = false;
    /// <summary>
    /// Is next level a end of the game.
    /// </summary>
    private bool toEndingLevel = false;
    
    //Portal Sprites for different statuses. Contains:
    // [0] - Sprite for main part, [1] - Sprite for bottom part 
    /// <summary>
    /// Sprites for the portal in normal state - when next level is normal level.
    /// </summary>
    [SerializeField] private Sprite[] pNormalSprites;
    /// <summary>
    /// Sprites for the portal in bonus state - when next level is bonus level.
    /// </summary>
    [SerializeField] private Sprite[] pBonusSprites;
    /// <summary>
    /// Sprites for the portal in ending state - when next level is ending level.
    /// </summary>
    [SerializeField] private Sprite[] pEndingSprites;

    //Variables on which next level depends
    /// <summary>
    /// Variable specifying the time the player reaches the last portal 
    /// so that it can be a portal to the bonus level.
    /// </summary>
    private float timeForBonusLevel = 15.0f; // 15 minutes for player to enter bonus room;
    /// <summary>
    /// Reference to GameTimer, which stores information about current time of the player game.
    /// </summary>
    private GameTimer timer;
    
    //Variables to configure portal render settings;
    /// <summary>
    /// Sprite for main part of the portal.
    /// </summary>
    private GameObject portalMain;
    /// <summary>
    /// Sprite for bottom part of the portal.
    /// </summary>
    private GameObject portalBottom;


    /// <summary>
    /// Function that initializes all parameters of the portal.
    /// </summary>
    void Start()
    {
        portalMain = this.gameObject.transform.GetChild(0).gameObject;
        portalBottom = this.gameObject.transform.GetChild(1).gameObject;

        //For test
        int levelCount = LevelsController.Instance.GetLevelCount();
        int levelCurrent = LevelsController.Instance.GetCurrentLevelID() + 1;

        timer = (GameObject.FindGameObjectWithTag("GameTimer").GetComponent<GameTimer>());
        float currentTime = timer.GetRAWgameTime() / 60.0f;
        Debug.Log(currentTime);

        if (levelCurrent == (levelCount - 1)) {
            if (currentTime <= timeForBonusLevel)
                setPortalStatus(false, true, false);    // normal, bonus, ending
            else {
                setPortalStatus(false, false, true);
                LevelsController.Instance.BonusLevelFailure();
            }
        }
        else {
            if (levelCurrent == levelCount)
                setPortalStatus(false, false, true);
            else
                setPortalStatus(true, false, false);
        }
    }

    /// <summary>
    /// Change of the level when player gets to the portal.
    /// </summary>
    /// <param name="other">Player collider</param>
    void OnTriggerEnter(Collider other){
        PlayerMovement movement = other.GetComponent<PlayerMovement>();

        if (movement != null){
            LevelsController.Instance.GoLevelUp();
            movement.transform.position = Vector3.zero;
        }
        
    }

    /// <summary>
    /// Access to toNormalLevel variable.
    /// </summary>
    /// <returns>toNormalLevel variable</returns>
    public bool isNormalLevNext() {
        return toNormalLevel;
    }
    
    /// <summary>
    /// Access to toBonusLevel variable.
    /// </summary>
    /// <returns>toBonusLevel variable</returns>
    public bool isBonusLevelNext() {
        return toBonusLevel;
    }

    /// <summary>
    /// Access to toEndingLevel variable.
    /// </summary>
    /// <returns>toEndingLevel variable</returns>
    public bool isEndingLevelNext() {
        return toEndingLevel;
    }

    /// <summary>
    /// Sets portal status with given parameters.
    /// </summary>
    /// <param name="normal">toNormalLevel variable</param>
    /// <param name="bonus">toBonusLevel variable</param>
    /// <param name="ending">toEndingLevel variable</param>
    private void setPortalStatus(bool normal, bool bonus, bool ending) {
        int checkValidity = (int)(normal ? 1:0) + (int)(bonus ? 1:0) + (int)(ending ? 1:0);
        if(checkValidity != 1) {
            Debug.LogError("NotValid set of portal status arguments!");
        }

        toNormalLevel = normal;
        toBonusLevel = bonus;
        toEndingLevel = ending;

        setValidPortalSprite();  
    }

    /// <summary>
    /// Sets portal sprite appropriate to the portal status.
    /// </summary>
    private void setValidPortalSprite() {
        SpriteRenderer pMainSprite = portalMain.GetComponent<SpriteRenderer>();
        SpriteRenderer pBottomSprite = portalBottom.GetComponent<SpriteRenderer>();

        if(toNormalLevel) {
            pMainSprite.sprite = pNormalSprites[0];
            pBottomSprite.sprite = pNormalSprites[1];
        }
        else if (toBonusLevel) {
            pMainSprite.sprite = pBonusSprites[0];
            pBottomSprite.sprite = pBonusSprites[1];
        }
        else if (toEndingLevel) {
            pMainSprite.sprite = pEndingSprites[0];
            pBottomSprite.sprite = pEndingSprites[1];
        }
        else {
            Debug.LogError("Portal Status is not in a valid range! (toNormal/toBonus/toEnding)");
        }
    }

}
