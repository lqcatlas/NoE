using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LMHub_004_Sushi : MonoBehaviour
{
    [Header("Tool")]
    public List<Sprite> statusSprites;
    [Header("Cells")]
    public Transform cellBgHolder;
    public GameObject sushiPlateTemplate;
    public List<KeyValuePair<CellMaster, GameObject>> sushiPlates;

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
