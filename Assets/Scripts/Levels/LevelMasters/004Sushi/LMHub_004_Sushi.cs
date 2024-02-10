using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LMHub_004_Sushi : MonoBehaviour
{
    [Header("Assets")]
    public List<Sprite> statusSprites;
    //public List<string> toolDisplayName = new List<string>();
    [Header("Background")]
    public SpriteRenderer chopsticks;

    [Header("Cells")]
    public Transform cellBgHolder;
    public GameObject sushiPlateTemplate;
    public List<KeyValuePair<CellMaster, GameObject>> sushiPlates;

    [Header("Tool Display")]
    public ToolStatusGroup toolStatusGroup;

    [Header("Play Audio")]
    public List<AudioClip> playClips;
    //private int clipIndex = 0;
    public List<AudioClip> endingClips;
    public AudioClip GetPlayClip(int index)
    {
        if (index > playClips.Count)
        {
            return null;
        }
        else
        {
            return playClips[index];
        }
    }
    public AudioClip GetEndingClip(int level)
    {
        int rng = Random.Range(Mathf.Min(endingClips.Count, level), 1);
        if (rng >= endingClips.Count)
        {
            Debug.LogError("invalid sushi ending VO");
            return null;
        }
        else
        {
            return endingClips[rng];
        }
    }
}
