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
    public SFXClipGroup toolClips;
    public SFXClipGroup endingVOClips;
}
