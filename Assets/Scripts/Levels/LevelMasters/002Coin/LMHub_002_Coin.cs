using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LMHub_002_Coin : MonoBehaviour
{
    [Header("Coin Sprite in Bg")]
    public Transform coinBgHolder;
    public GameObject RandomCoinTemplate;
    [Header("Coin Count in Cells")]
    public Transform cellTagHolder;
    public GameObject cellTagTempalte;
    public List<KeyValuePair<CellMaster, GameObject>> coinTags;
    [Header("Tool Display")]
    public List<Sprite> coinToolSprites;
    //public List<string> toolDisplayName = new List<string>();

    public ToolStatusGroup toolStatusGroupV1;
    public ToolStatusGroup toolStatusGroupV2;
    public ToolStatusGroup toolStatusGroupV3;

    [Header("Play Audio")]
    public SFXClipGroup coinLandingClips;
}
