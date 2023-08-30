using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LMHub_006_LightBulb : MonoBehaviour
{
    [Header("Assets")]
    //public List<Sprite> bulbSprites;
    public Sprite bulbToolSprite;
    public GameObject electricity;
    [Header("Cells")]
    public Transform cellBgHolder;
    public GameObject bulbBgTemplate;
    public List<KeyValuePair<CellMaster, LightbulbCellBg>> lightBulbs;
}
