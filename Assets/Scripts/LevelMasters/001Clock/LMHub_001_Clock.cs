using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LMHub_001_Clock : MonoBehaviour
{
    [Header("Clock Sprite in Bg")]
    public SpriteRenderer runningClockBg;
    [Header("Clock Sprite in Cells")]
    public Transform cellBgHolder;
    public GameObject runningClockTemplate;
    public List<KeyValuePair<CellMaster, GameObject>> runningClocks;
    
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
