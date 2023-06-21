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
}
