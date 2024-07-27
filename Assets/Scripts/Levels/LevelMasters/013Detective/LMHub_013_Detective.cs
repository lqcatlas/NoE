using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LMHub_013_Detective : MonoBehaviour
{
    [Header("Tool Display")]
    public Sprite toolSprite;
    public ToolStatusGroup toolStatusGroup;

    [Header("Drawings")]
    public Transform bgHolder;
    public List<KeyValuePair<CellMaster, GameObject>> cellBgs;
    public List<GameObject> ispSigns;
    public Transform splitLine;
    [Header("Template")]
    public GameObject bgTemplate;
    public GameObject ispSignTemplate;
    public GameObject ispAnimTemplate;

    [Header("Audio")]
    public SFXClipGroup upgradeClips;
}
