using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LMHub_001_Clock : MonoBehaviour
{
    [Header("Clock Sprite in Bg")]
    public SpriteRenderer runningClockBg;
    [Header("Clock Sprite in Cells")]
    public Transform cellBgHolder;
    public GameObject runningClockTemplate;
    public List<KeyValuePair<CellMaster, GameObject>> runningClocks;
    [Header("Clock Sprite in Tool")]
    public Transform clockTool;
    [Header("Tool Display")]
    public ToolStatusGroup toolStatusGroup;

    [Header("Play Audio")]
    public SFXClipGroup tickClips;
}
