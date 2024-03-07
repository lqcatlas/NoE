using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LMHub_007_Bonsai : MonoBehaviour
{
    [Header("Tool Display")]
    public Sprite toolSprite;
    public ToolStatusGroup toolStatusGroup;
    //public ToolStatusGroup toolStatusGroupV2;

    [Header("Tree Trunk")]
    public Transform trunkHolder;
    public SpriteRenderer trunk;
    public List<Sprite> trunkSprites;

    [Header("Cells")]
    public Transform cellBgHolder;
    public GameObject leafTemplate;
    public List<KeyValuePair<CellMaster, GameObject>> leaves;

}
