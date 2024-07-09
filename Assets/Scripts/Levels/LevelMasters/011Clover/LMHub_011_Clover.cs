using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LMHub_011_Clover : MonoBehaviour
{
    [Header("Tool Display")]
    public Sprite toolSprite;
    public ToolStatusGroup toolStatusGroup;

    [Header("Drawings")]
    public Transform bgHolder;
    public GameObject drawingTemplate;

    [Header("Audio")]
    public SFXClipGroup circleClips;
}
