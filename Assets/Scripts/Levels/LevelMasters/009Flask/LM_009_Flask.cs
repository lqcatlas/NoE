using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static LM_008_Crown;

public class LM_009_Flask : LevelMasterBase
{
    [Header("Theme Additions")]
    public LMHub_009_Flask flaskHub;

    public int curAddRate = 1;

    public override void GetObjectReferences(GameObject _themeHub)
    {
        base.GetObjectReferences(null);
        flaskHub = _themeHub.GetComponent<LMHub_009_Flask>();
    }


    public override void HandlePlayerInput(Vector2Int coord)
    {
        Debug.Log(string.Format("Add with a current rate of {0}, final result as {1}", curAddRate, "TBD"));
    }
}
