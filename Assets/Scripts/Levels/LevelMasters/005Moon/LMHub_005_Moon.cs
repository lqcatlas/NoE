using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;

public class LMHub_005_Moon : MonoBehaviour
{
    [Header("Tool")]
    public List<Sprite> statusSprites;
    public List<string> toolDisplayName = new List<string>();
    public List<GameObject> toolCoords;
    public List<SpriteRenderer> tools = new List<SpriteRenderer>();
    public Transform toolGroup;
    public GameObject toolTemplate;

    public int MAX_TOOL_COUNT_IN_CYCLE = 7;

    [Header("Eclipse")]
    public GameObject eclipseVFX;
    [Header("Phase Plate")]
    //public GameObject phaseMask;
    public GameObject phaseRotate;
    //public GameObject phasePlate;
    public GameObject phaseStar;
    public List<float> phaseDegrees;
    public float PLATE_ROTATION_DURATION_PLAY = 0.5f;

    public void SetPlateWidget(bool enabled)
    {
        //phaseMask.SetActive(enabled);
        phaseRotate.SetActive(enabled);
    }
    public void SetTabletToDegree(int phaseIndex)
    {
        phaseRotate.transform.localRotation = Quaternion.Euler(0f, 0f, phaseDegrees[phaseIndex]);
        //phasePlate.GetComponent<SpriteRenderer>().DOFade(0f, PLATE_ROTATION_DURATION_PLAY * 2).From();
        phaseStar.GetComponent<SpriteRenderer>().DOFade(0f, PLATE_ROTATION_DURATION_PLAY * 5).From();
    }
    public void AnimateTabletToDegree(int phaseIndex)
    {
        phaseRotate.transform.DORotate(new Vector3(0f, 0f, phaseDegrees[phaseIndex]), PLATE_ROTATION_DURATION_PLAY);
    }
    public void ResetToolCycle()
    {
        tools = toolGroup.GetComponentsInChildren<SpriteRenderer>().ToList();
        for (int i = 0; i < tools.Count; i++)
        {
            Destroy(tools[i].gameObject);
        }
        tools.Clear();
    }
    public void AddPredictedToolToCycle(int toolStatus, int level)
    {
        if (level == 1)
        {
            AddToolToCycle((toolStatus + 2) % 2 + 1);
        }
        else
        {
            AddToolToCycle((toolStatus + 2) % 5 + 1);
        }
    }
    public void AddToolToCycle(int toolStatus)
    {
        //Debug.Log("AddToolToCycle: tool" + toolStatus);
        SpriteRenderer newTool = Instantiate(toolTemplate, toolGroup).GetComponent<SpriteRenderer>();
        newTool.GetComponent<SpriteRenderer>().sprite = statusSprites[toolStatus];
        newTool.transform.position = toolCoords[6].transform.position;
        newTool.gameObject.SetActive(true);
        tools.Add(newTool);
        if(tools.Count > MAX_TOOL_COUNT_IN_CYCLE)
        {
            GameObject oldestTool = tools[0].gameObject;
            tools.RemoveAt(0);
            Destroy(oldestTool);
        }
    }
    public void AnimateCycle()
    {
        for (int i = 0; i < tools.Count; i++)
        {
            tools[i].transform.DOMove(toolCoords[i].transform.position, dConstants.UI.StandardizedBtnAnimDuration);
            tools[i].transform.DOScale(toolCoords[i].transform.localScale, dConstants.UI.StandardizedBtnAnimDuration);
            tools[i].DOColor(toolCoords[i].GetComponent<SpriteRenderer>().color, dConstants.UI.StandardizedBtnAnimDuration);
        }
    }
    public void InitCycle()
    {
        for (int i = 0; i < tools.Count; i++)
        {
            tools[i].transform.position = toolCoords[i].transform.position;
            tools[i].transform.localScale = toolCoords[i].transform.localScale;
            tools[i].color = toolCoords[i].GetComponent<SpriteRenderer>().color;
        }
    }
    public void InitToolToCycle(int startToolStatus, int level)
    {
        ResetToolCycle();
        if (level == 1)
        {
            AddToolToCycle(0);
            AddToolToCycle(0);
            AddToolToCycle(0);
            AddToolToCycle((startToolStatus - 1) % 2 + 1);
            AddToolToCycle((startToolStatus) % 2 + 1);
            AddToolToCycle((startToolStatus + 1) % 2 + 1);
            AddToolToCycle((startToolStatus + 2) % 2 + 1);
        }
        else
        {
            AddToolToCycle(0);
            AddToolToCycle(0);
            AddToolToCycle(0);
            AddToolToCycle((startToolStatus - 1) % 5 + 1);
            AddToolToCycle((startToolStatus) % 5 + 1);
            AddToolToCycle((startToolStatus + 1) % 5 + 1);
            AddToolToCycle((startToolStatus + 2) % 5 + 1);
        }
        InitCycle();
    }
}
