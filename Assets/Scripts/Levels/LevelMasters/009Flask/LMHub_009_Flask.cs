using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    public List<GameObject> rulesetInfographs;

    [Header("Panel")]
    public GameObject panelObj;
    public TextMeshPro stat1;
    public TextMeshPro stat2;

    [Header("Boom Anim")]
    public GameObject explodeAnim;
}
