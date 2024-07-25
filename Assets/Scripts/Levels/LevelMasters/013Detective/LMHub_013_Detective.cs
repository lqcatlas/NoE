using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LMHub_013_Detective : MonoBehaviour
{
    [Header("Tool Display")]
    public Sprite toolSprite;
    //public ToolStatusGroup toolStatusGroup;

    [Header("Drawings")]
    public Transform bgHolder;
    public GameObject ispTemplate;
    public List<GameObject> ispSigns;

    [Header("Audio")]
    public SFXClipGroup upgradeClips;
}
