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
    public List<AudioClip> playClips;
    private int clipIndex = 0;
    public AudioClip GetNextPlayClip()
    {
        if (playClips.Count > 0)
        {
            clipIndex = (clipIndex + 1) % playClips.Count;
            return playClips[clipIndex];
        }
        else
        {
            return null;
        }
    }
}
