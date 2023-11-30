using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GoalMaster : MonoBehaviour
{
    [Header("Children Objs")]
    //public TextMeshPro goalDesc;
    public RectTransform goalLayout;
    public TextMeshPro title;
    public List<TextMeshPro> lines;
    public GameObject nextBtn;

    [Header("Data")]
    public LevelMasterBase levelMaster;

    public void RegisterLevelMaster(LevelMasterBase _master)
    {
        levelMaster = _master;
    }
    public void NextLevel()
    {
        nextBtn.gameObject.SetActive(false);
        levelMaster.StartNextLevel();
    }
}
