using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LM_009_Flask : LevelMasterBase
{
    [Header("Theme Additions")]
    public LMHub_009_Flask flaskHub;

    public enum BoomReason { EXCEED_MAX = 0, EXCEED_DIGITS = 1, EQUAL_DIGIT = 2, PRIME_NUM = 3 };
    public BoomReason boomReason;
    public bool isBoom;

    //public float cubicFactor = 1.5f;
    int MAX_CELL_VALUE = 1000;
    float curAddRate = 100f;
    float pourAmountPerTick = 100f;
    float accumulatedAmount = 0f;
    float maxPourRatePerSecond = 30000f;
    float minPourRatePerSecond = 100f;
    float startTimestamp;
    float lastTimestamp;
    bool pouring;

    //level step ups
    int POURING_LVINDEX = 2;
    int CHECK1_LVINDEX = 3;
    int CHECK2_LVINDEX = 4;
    int CHECK3_LVINDEX = 5;
    int CHECK4_LVINDEX = 10;

    public override void GetObjectReferences(GameObject _themeHub)
    {
        base.GetObjectReferences(null);
        flaskHub = _themeHub.GetComponent<LMHub_009_Flask>();
        ThemeAnimationDelayAfterPlay = 0.01f;
    }
    public override void AlternativeMouseDown_Theme(Vector2Int coord)
    {
        pouring = true;
        curAddRate = minPourRatePerSecond;
        lastTimestamp = Time.time;
        startTimestamp = Time.time;
        accumulatedAmount = 0f;
        IEnumerator coroutine = PouringSim(coord, 0.001f);
        StartCoroutine(coroutine);
    }
    public override void AlternativeMouseUp_Theme(Vector2Int coord)
    {
        pouring = false;
        //curAddRate = 0;
    }
    public override void InitCells()
    {
        //init bg for flask
        flaskHub.cellBgs = new List<KeyValuePair<CellMaster, GameObject>>();
        flaskHub.cellBgHolder.transform.localScale = hub.boardMaster.cellHolder.localScale;
        List<Transform> oldBgs = flaskHub.cellBgHolder.GetComponentsInChildren<Transform>().ToList();
        oldBgs.Remove(flaskHub.cellBgHolder.transform);
        for (int i = 0; i < oldBgs.Count; i++)
        {
            Destroy(oldBgs[i].gameObject);
        }
        for (int i = 0; i < hub.boardMaster.cells.Count; i++)
        {
            DataCell temp_cellData = levelData.curBoard.GetCellDataByCoord(hub.boardMaster.cells[i].coord);
            if (temp_cellData != null)
            {
                hub.boardMaster.cells[i].DisplayNumber(temp_cellData.value);
                //create crown sprite
                GameObject obj = Instantiate(flaskHub.bgTemplate, flaskHub.cellBgHolder);
                obj.transform.position = hub.boardMaster.cells[i].transform.position;
                flaskHub.cellBgs.Add(new KeyValuePair<CellMaster, GameObject>(hub.boardMaster.cells[i], obj));
                obj.GetComponent<flaskBg>().liquidPct.localScale = new Vector3(1f, (float)temp_cellData.value / MAX_CELL_VALUE, 1f);
            }
        }
    }
    public override void ToolConsume(Vector2Int coord)
    {
        //consume tool and save curboard to previous board/boards
        levelData.curBoard.curPlayCoord = coord;
        levelData.previousBoard = new DataBoard(levelData.curBoard);
        levelData.previousBoards.Add(new DataBoard(levelData.curBoard));
        //move tool deduction after player input handling
    }
    public override void HandlePlayerInput(Vector2Int coord)
    {
        //increase add rate accumulatively
        float curTimestamp = Time.time;
        float timeIntervalSinceLastPouring = curTimestamp - lastTimestamp;
        float timeIntervalSincePouringStart = curTimestamp - startTimestamp;
        //curAddRate = PourRateIncreaseLinear(timeIntervalSinceLastPouring);
        curAddRate = PourRateIncreaseCubic(timeIntervalSincePouringStart);
        accumulatedAmount += curAddRate * timeIntervalSinceLastPouring;
        int numberAdd = Mathf.FloorToInt(accumulatedAmount / pourAmountPerTick);
        accumulatedAmount -= numberAdd * pourAmountPerTick;
        for (int i = 0; i < levelData.curBoard.cells.Count; i++)
        {
            if (levelData.curBoard.cells[i].coord == coord)
            {
                levelData.curBoard.cells[i].value += numberAdd;
            }
        }
        lastTimestamp = curTimestamp;
        //move tool deduction after player input handling
        levelData.curBoard.toolCount -= numberAdd;
    }
    public override void HandleEnvironment(Vector2Int coord)
    {
        //check explosion conditions and make marks
        isBoom = false;
        if (levelData.levelIndex >= CHECK1_LVINDEX && BoardCalculation.CountXplus_Ytimes(levelData.curBoard, 1000, 1))
        {
            boomReason = BoomReason.EXCEED_MAX;
            isBoom = true;
        }
        if (levelData.levelIndex >= CHECK2_LVINDEX && BoardCalculation.FlaskSpecialBoomCheck1(levelData.curBoard, 18))
        {
            boomReason = BoomReason.EXCEED_DIGITS;
            isBoom = true;
        }
        if (levelData.levelIndex >= CHECK3_LVINDEX && BoardCalculation.FlaskSpecialBoomCheck2(levelData.curBoard))
        {
            boomReason = BoomReason.EQUAL_DIGIT;
            isBoom = true;
        }
        if (levelData.levelIndex >= CHECK4_LVINDEX && BoardCalculation.FlaskSpecialBoomCheck3(levelData.curBoard))
        {
            boomReason = BoomReason.PRIME_NUM;
            isBoom = true;
        }
        //use status 1 to represent explostion start pos
        if (isBoom)
        {
            for (int i = 0; i < levelData.curBoard.cells.Count; i++)
            {
                if (levelData.curBoard.cells[i].coord == coord)
                {
                    levelData.curBoard.cells[i].status = 1;
                }
            }
        }
    }
    public override void UpdateCells(Vector2Int coord)
    {
        for (int i = 0; i < flaskHub.cellBgs.Count; i++)
        {
            if(flaskHub.cellBgs[i].Key.coord == coord)
            {
                DataCell temp_cellData = levelData.curBoard.GetCellDataByCoord(coord);
                if (temp_cellData != null)
                {
                    flaskHub.cellBgs[i].Key.DisplayNumber(temp_cellData.value);
                    flaskHub.cellBgs[i].Value.GetComponent<flaskBg>().liquidPct.localScale = new Vector3(1f, (float)temp_cellData.value / MAX_CELL_VALUE, 1f);
                }
            }  
        }
    }
    public override bool CheckLoseCondition()
    {
        if (levelData.curBoard.toolCount == 0 || isBoom)
        {
            return true;
        }
        return false;
    }
    IEnumerator PouringSim(Vector2Int coord, float interval)
    {
        while (pouring)
        {
            yield return new WaitForFixedUpdate();
            Play(coord);
        }
    }
    float PourRateIncreaseLinear(float timeIntervalSinceLastPouring)
    {
        float speedUpFactor = .5f;
        float PourRate = Mathf.Min(maxPourRatePerSecond, curAddRate + speedUpFactor * timeIntervalSinceLastPouring);
        Debug.Log(string.Format("set current rate as {0}, interval time since last pouring is {1}", curAddRate, timeIntervalSinceLastPouring));
        return PourRate;
    }
    float PourRateIncreaseCubic(float timeIntervalSincePouringStart)
    {
        float speedUpFactor = 3f;
        float PourRate = Mathf.Min(maxPourRatePerSecond, minPourRatePerSecond * Mathf.Pow(speedUpFactor, timeIntervalSincePouringStart));
        Debug.Log(string.Format("set current rate as {0}, interval time since pouring start is {1}", curAddRate, timeIntervalSincePouringStart));
        return PourRate;
    }
}
