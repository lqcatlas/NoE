using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LMHub_008_Crown : MonoBehaviour
{
    [Header("Tool Display")]
    public Sprite toolSprite;
    public ToolStatusGroup toolStatusGroup;

    [Header("Cells")]
    public Transform cellBgHolder;
    public GameObject crownTemplate;
    public List<KeyValuePair<CellMaster, GameObject>> crownBgs;

    [Header("War")]
    public GameObject warTemplate;
}
