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
    [Header("Coin in Tool")]
    public List<Sprite> coinToolSprites;
    
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
