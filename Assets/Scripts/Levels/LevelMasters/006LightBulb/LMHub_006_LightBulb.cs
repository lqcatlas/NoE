using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LMHub_006_LightBulb : MonoBehaviour
{
    [Header("Assets")]
    //public List<Sprite> bulbSprites;
    public Sprite bulbToolSprite;
    public GameObject electricity;
    [Header("Tool Display")]
    public ToolStatusGroup toolStatusGroupV1;
    public ToolStatusGroup toolStatusGroupV2;
    [Header("Cells")]
    public Transform cellBgHolder;
    public GameObject bulbBgTemplate;
    public List<KeyValuePair<CellMaster, LightbulbCellBg>> lightBulbs;
    [Header("Play Audio")]
    public List<AudioClip> switchClips;
    public AudioClip GetSwitchClip(int index)
    {
        if (index > switchClips.Count)
        {
            return null;
        }
        else
        {
            return switchClips[index];
        }
    }
}
