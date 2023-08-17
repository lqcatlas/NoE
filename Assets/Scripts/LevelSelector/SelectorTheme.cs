using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Timeline;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class SelectorTheme : MonoBehaviour
{
    enum ThemeStatus { locked = 1, unlocked = 2, finished = 3 };

    [Header("Theme Status")]
    [SerializeField] ThemeStatus status = ThemeStatus.locked;

    [Header("Children Objs")]
    [SerializeField] SpriteRenderer frame;
    [SerializeField] TextMeshPro title;
    [SerializeField] SpriteRenderer tokenIcon;
    [SerializeField] TextMeshPro tokenNeed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void HoverOn()
    {
        if (status == ThemeStatus.locked)
        {
            
        }
    }
    public void HoverOff()
    {
        if (status == ThemeStatus.locked)
        {
            
        }
        else if (status == ThemeStatus.unlocked)
        {

        }
        else if (status == ThemeStatus.finished)
        {
            
        }
    }
    public void MouseUp()
    {
        if (status == ThemeStatus.locked)
        {
            
        }
    }
    public int InitStatus()
    {
        //levelName.SetText(LocalizedAssetLookup.singleton.Translate(setupData.title));
        /*if (master.playerLevelRecords.isLevelFinished(setupData.levelUID))
        {
            SetToFinished();
            return 3;
        }
        else if (setupData.previousLevel == null)
        {
            SetToUnlocked();
            return 2;
        }

        else if (master.playerLevelRecords.isLevelFinished(setupData.previousLevel.levelUID))
        {
            SetToUnlocked();
            return 2;
        }
        else
        {
            SetToLocked();
            return 1;
        }*/
        return 0;
    }
    public void SetLocked()
    {
        status = ThemeStatus.locked;

    }
    public void SetToUnlocked()
    {

    }
    public void SetToFinished()
    {

    }
}
