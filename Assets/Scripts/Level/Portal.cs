using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    //Portal Statuses for next level (causes a different look of the portal)
    [SerializeField] private bool toNormalLevel = true;
    [SerializeField] private bool toBonusLevel = false;
    [SerializeField] private bool toEndingLevel = false;
    
    //Portal Sprites for different statuses. Contains:
    // [0] - Sprite for main part, [1] - Sprite for bottom part 
    [SerializeField] private Sprite[] pNormalSprites;
    [SerializeField] private Sprite[] pBonusSprites;
    [SerializeField] private Sprite[] pEndingSprites;
    
    //Variables to configure portal render settings;
    private GameObject portalMain;
    private GameObject portalBottom;

    // Start is called before the first frame update
    void Start()
    {
        portalMain = this.gameObject.transform.GetChild(0).gameObject;
        portalBottom = this.gameObject.transform.GetChild(1).gameObject;

        //For test
        setPortalStatus(true, false, false);
    }

    void OnTriggerEnter(Collider other){
        PlayerMovement movement = other.GetComponent<PlayerMovement>();

        if (movement != null){
            LevelsController.Instance.GoLevelUp();
            movement.transform.position = Vector3.zero;
        }
        
    }

    public bool isNormalLevNext() {
        return toNormalLevel;
    }
    
    public bool isBonusLevelNext() {
        return toBonusLevel;
    }

    public bool isEndingLevelNext() {
        return toEndingLevel;
    }

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
