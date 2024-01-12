using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupMaster : MonoBehaviour
{
    [Header("Children Objs")]
    public GameObject victoryPopupGroup;
    public GameObject failureyPopupGroup;

    [Header("Parent")]
    public LevelMasterBase levelMaster;
    public void RegisterLevelMaster(LevelMasterBase _master)
    {
        levelMaster = _master;
    }
    
}
