using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LMHub_012_Skyscraper : MonoBehaviour
{
    [Header("Tool Display")]
    public Sprite toolSprite;
    public ToolStatusGroup toolStatusGroup;

    [Header("Drawings")]
    public Transform bgHolder;
    public GameObject bldgTemplate;

    [Header("Audio")]
    public SFXClipGroup upgradeClips;
}
