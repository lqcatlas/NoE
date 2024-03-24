using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LMHub_009_Flask : MonoBehaviour
{
    [Header("Tool Display")]
    public Sprite toolSprite;
    public ToolStatusGroup toolStatusGroup;

    [Header("Background")]
    public GameObject bgTemplate;
    public Transform cellBgHolder;
    public List<KeyValuePair<CellMaster, GameObject>> cellBgs;

    [Header("Boom Anim")]
    public GameObject explodeAnim;
}
