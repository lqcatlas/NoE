using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class LM_009_Flask : LevelMasterBase
{
    [Header("Theme Additions")]
    public LMHub_009_Flask flaskHub;

    public enum BoomReason { EXCEED_MAX = 0, FORBIDDEN_HUNDRED = 1, EXCEED_DIGITS = 2, EXCEED_REPEATS = 3, PRIME_NUM = 4 };
    public BoomReason boomReason;
    public bool isBoom;

    //public float cubicFactor = 1.5f;
    int MAX_CELL_VALUE = 1000;
    float curAddRate = 200f;
    float pourAmountPerTick = 100f;
    float accumulatedAmount = 0f;
    float maxPourRatePerSecond = 70000f;
    float minPourRatePerSecond = 200f;
    float startTimestamp;
    float lastTimestamp;
    bool pouring;
    bool simEnd;

    //level step ups
    int POURING_LVINDEX = 2;
    int CHECK1_LVINDEX = 5;
    int CHECK2_LVINDEX = 4;
    int CHECK3_LVINDEX = 6;
    //int CHECK4_LVINDEX = 10;

    string missingNumbers;
    public override void GetObjectReferences(GameObject _themeHub)
    {
        base.GetObjectReferences(null);
        flaskHub = _themeHub.GetComponent<LMHub_009_Flask>();
        ThemeAnimationDelayAfterPlay = 1f;
    }
    public override void GenerateBoard()
    {
        base.GenerateBoard();
        hub.boardMaster.boardHolder.localPosition = new Vector3(2f, 10f, 0f);
    }
    public override void AddtionalInit_Theme(bool isRewind = false)
    {
        pouring = false;
        simEnd = false;
        missingNumbers = "";
        //panel init
        flaskHub.panelObj.SetActive(levelData.levelIndex > CHECK2_LVINDEX);
        PanelInfoUpdate();
        //rule sketches init
        RuleSketchInit();
    }
    public override void DelayedInit_Theme()
    {
        //panel appear
        if (levelData.levelIndex == CHECK2_LVINDEX)
        {
            flaskHub.panelObj.SetActive(true);
            flaskHub.panelObj.transform.DOMoveY(3f, dConstants.UI.StandardizedVFXAnimDuration).From(true);
        }
        //rule sketches animation appear
        RuleSketchAnimate();
    }
    public override void AlternativeMouseDown_Theme(Vector2Int coord)
    {
        if (status != LevelStatus.PLAYBLE)
        {
            return;
        }
        DisablePlayerInput();
        //puring special below
        pouring = true;
        curAddRate = minPourRatePerSecond;
        lastTimestamp = Time.time;
        startTimestamp = Time.time;
        accumulatedAmount = pourAmountPerTick;
        IEnumerator coroutine = PouringSim(coord, 0.05f);
        StartCoroutine(coroutine);
    }
    IEnumerator PouringSim(Vector2Int coord, float interval)
    {
        simEnd = false;
        if (levelData.levelIndex >= POURING_LVINDEX)
        {
            while (pouring)
            {
                yield return new WaitForFixedUpdate();
                //yield return new WaitForSeconds(interval);
                //Play(coord);
                //puring special below
                ToolConsume(coord);
                HandlePlayerInput(coord);
                HandleEnvironment(coord);
                UpdateCells(coord);
                UpdateNarrative();
                UpdateGoal();
                UpdateRuleset();
                UpdateTool(coord);
                UpdatePlayable();
            }
        }
        else
        {
            //level 1 special
            ToolConsume(coord);
            levelData.curBoard.GetCellDataByCoord(coord).value += 1;
            levelData.curBoard.toolCount -= 1;
            HandleEnvironment(coord);
            UpdateCells(coord);
            UpdateNarrative();
            UpdateGoal();
            UpdateRuleset();
            UpdateTool(coord);
            UpdatePlayable();
        }
        simEnd = true;
    }
    public override void AlternativeMouseHold_Theme(Vector2Int coord)
    {
        //Debug.Log(string.Format("hold mouse on {0},{1}", coord.x, coord.y));
    }
    public override void AlternativeMouseUp_Theme(Vector2Int coord)
    {
        if (pouring)
        {
            //puring special below
            pouring = false;
            /*AddtionalUpdate_Theme(coord);
            Sequence seq = DOTween.Sequence();
            seq.AppendInterval(isBoom ? ThemeAnimationDelayAfterPlay : .2f);
            seq.AppendCallback(() => PlayCallback());
            */
            IEnumerator coroutine = PouringEnd(coord);
            StartCoroutine(coroutine);

        }
    }
    IEnumerator PouringEnd(Vector2Int coord)
    {
        while (!simEnd)
        {
            yield return new WaitForFixedUpdate();
        }
        AddtionalUpdate_Theme(coord);
        Sequence seq = DOTween.Sequence();
        seq.AppendInterval(isBoom ? ThemeAnimationDelayAfterPlay : .1f);
        seq.AppendCallback(() => PlayCallback());
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
                int maxBoardSize = Mathf.Max(levelData.initBoard.boardSize.x, levelData.initBoard.boardSize.y);
                if (maxBoardSize <= 2)
                {
                    obj.transform.localScale = new Vector3(1.25f, 1.1f, 1.25f);
                }
                else
                {
                    obj.transform.localScale = Vector3.one * 1.25f;
                }

                flaskHub.cellBgs.Add(new KeyValuePair<CellMaster, GameObject>(hub.boardMaster.cells[i], obj));
                int curMaxCellValue = levelData.levelIndex == 1 ? 20 : MAX_CELL_VALUE;
                obj.GetComponent<flaskBg>().liquidPct.localScale = new Vector3(1f, (float)temp_cellData.value / curMaxCellValue, 1f);
            }
        }
    }
    public override void InitTool()
    {
        base.InitTool();
        hub.toolMaster.toolIcon.sprite = flaskHub.toolSprite;
        //UpdateToolStatusDisplay();
    }
    public override bool InitNarrative()
    {
        bool hasInitNarrative = false;
        hub.narrativeMaster.title.SetText(LocalizedAssetLookup.singleton.Translate(levelData.title));
        //narrative lines
        hub.narrativeMaster.ClearAllLines();
        int indexOffset = levelData.oldNarratives.Count;
        for (int i = 0; i < hub.narrativeMaster.lines.Count; i++)
        {

            if (i < levelData.oldNarratives.Count)
            {
                hub.narrativeMaster.lines[i].SetText(LocalizedAssetLookup.singleton.Translate(levelData.oldNarratives[i]));
            }
            else
            {
                hub.narrativeMaster.lines[i].SetText("");
            }
        }
        if (levelData.newInitNarrative != null)
        {
            if (levelData.newInitNarrative.Length > 0)
            {
                hasInitNarrative = true;
                hub.narrativeMaster.TypeALine(levelData.newInitNarrative);
            }
        }
        lineStatus.Clear();
        if (levelData.newPlayNarratives.Count > 0)
        {
            for (int i = 0; i < levelData.newPlayNarratives.Count; i++)
            {
                lineStatus.Add(NarrativeLineStatus.INQUEUE);
            }
        }
        //play all lines on level 3
        if(levelData.levelIndex == 3)
        {
            if (lineStatus[0] == NarrativeLineStatus.INQUEUE)
            {
                TryTypeNextPlayLine(0);
                lineStatus[0] = NarrativeLineStatus.PLAYED;
            }
            if (lineStatus[1] == NarrativeLineStatus.INQUEUE)
            {
                TryTypeNextPlayLine(1);
                lineStatus[1] = NarrativeLineStatus.PLAYED;
            }
        }
        return hasInitNarrative;
    }
    public override void ToolConsume(Vector2Int coord)
    {
        //consume tool and save curboard to previous board/boards
        levelData.curBoard.curPlayCoord = coord;
        levelData.previousBoard = new DataBoard(levelData.curBoard);
        //levelData.previousBoards.Add(new DataBoard(levelData.curBoard));
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
        if (numberAdd > levelData.curBoard.toolCount)
        {
            numberAdd = levelData.curBoard.toolCount;
            AlternativeMouseUp_Theme(coord);
        }
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
        bool inDanger = false;
        //check explosion on exceed max
        if (levelData.levelIndex >= POURING_LVINDEX && BoardCalculation.CountXplus_Ytimes(levelData.curBoard, 1000, 1))
        {
            //force end if exceed
            AlternativeMouseUp_Theme(coord);
        }
        else if (levelData.levelIndex >= CHECK1_LVINDEX && BoardCalculation.FlaskSpecialBoomCheck1(levelData.curBoard, 7))
        {
            inDanger = true;
        }
        else if (levelData.levelIndex >= CHECK2_LVINDEX && BoardCalculation.FlaskSpecialBoomCheck2(levelData.curBoard, 7))
        {
            inDanger = true;
        }
        else if (levelData.levelIndex >= CHECK3_LVINDEX && BoardCalculation.FlaskSpecialBoomCheck3(levelData.curBoard))
        {
            inDanger = true;
        }
        /*else if (levelData.levelIndex >= CHECK4_LVINDEX && BoardCalculation.FlaskSpecialBoomCheck4(levelData.curBoard))
        {
            inDanger = true;
        }*/
        if (inDanger)
        {
            //set cur cell to status 1
            levelData.curBoard.GetCellDataByCoord(coord).status = 1;
        }
        else
        {
            levelData.curBoard.GetCellDataByCoord(coord).status = 0;
        }
    }

    public override void UpdateCells(Vector2Int coord)
    {
        for (int i = 0; i < flaskHub.cellBgs.Count; i++)
        {
            if (flaskHub.cellBgs[i].Key.coord == coord)
            {
                DataCell temp_cellData = levelData.curBoard.GetCellDataByCoord(coord);
                if (temp_cellData != null)
                {
                    flaskHub.cellBgs[i].Key.DisplayNumber(temp_cellData.value);
                    int curMaxCellValue = levelData.levelIndex == 1 ? 20 : MAX_CELL_VALUE;
                    flaskHub.cellBgs[i].Value.GetComponent<flaskBg>().liquidPct.localScale = new Vector3(1f, (float)temp_cellData.value / curMaxCellValue, 1f);
                    flaskHub.cellBgs[i].Value.GetComponent<flaskBg>().dangerHint.SetActive(temp_cellData.status == 1);
                }
            }
        }
    }
    public override void UpdateGoal()
    {
        if (levelData.levelIndex == 9)
        {
            List<int> NumbersCount = BoardCalculation.CountByDigits(levelData.curBoard);
            missingNumbers = "";
            for (int i = 0; i < NumbersCount.Count; i++)
            {
                if (NumbersCount[i] == 0)
                {
                    missingNumbers += string.Format(" {0}", i);
                }
            }
            if (missingNumbers.Length > 0)
            {
                hub.goalMaster.lines[1].SetText(string.Format("{0}{1}", LocalizedAssetLookup.singleton.Translate("@Loc=ui_goal_missing_digits@@"), missingNumbers));
                hub.goalMaster.lines[1].gameObject.SetActive(true);
                LayoutRebuilder.ForceRebuildLayoutImmediate(hub.goalMaster.goalLayout);
            }
        }                                          
    }
    public override void UpdateNarrative()
    {
        if (levelData.levelIndex == 2)
        {
            if (levelData.curBoard.toolCount <= levelData.initBoard.toolCount / 4f * 3f && lineStatus.Count >= 1)
            {
                if (lineStatus[0] == NarrativeLineStatus.INQUEUE)
                {
                    TryTypeNextPlayLine(0);
                    lineStatus[0] = NarrativeLineStatus.PLAYED;
                }
            }
            //trigger 2nd play
            if (levelData.curBoard.toolCount <= levelData.initBoard.toolCount / 4f && lineStatus.Count >= 2)
            {
                if (lineStatus[1] == NarrativeLineStatus.INQUEUE)
                {
                    TryTypeNextPlayLine(1);
                    lineStatus[1] = NarrativeLineStatus.PLAYED;
                }
            }
            //trigger 3rd play
            if (levelData.curBoard.toolCount <= levelData.initBoard.toolCount / 20f && lineStatus.Count >= 3)
            {
                if (lineStatus[2] == NarrativeLineStatus.INQUEUE)
                {
                    TryTypeNextPlayLine(2);
                    lineStatus[2] = NarrativeLineStatus.PLAYED;
                }
            }
        }
        else
        {
            base.UpdateNarrative();
        }

    }
    public override void AddtionalUpdate_Theme(Vector2Int coord)
    {
        isBoom = false;
        string reasonTxt = "";
        if (levelData.levelIndex >= POURING_LVINDEX && BoardCalculation.CountXplus_Ytimes(levelData.curBoard, 1000, 1))
        {
            boomReason = BoomReason.EXCEED_MAX;
            if (levelData.levelIndex == 2)
            {
                reasonTxt = "@Loc=rule2_lv903@@";
            }
            else
            {
                reasonTxt = levelData.ruleset[1].ruleDesc;
            }
            isBoom = true;
        }
        else if (levelData.levelIndex >= CHECK1_LVINDEX && BoardCalculation.FlaskSpecialBoomCheck1(levelData.curBoard, 7))
        {
            boomReason = BoomReason.FORBIDDEN_HUNDRED;
            reasonTxt = levelData.ruleset[3].ruleDesc;
            isBoom = true;
        }
        else if (levelData.levelIndex >= CHECK2_LVINDEX && BoardCalculation.FlaskSpecialBoomCheck2(levelData.curBoard, 7))
        {
            boomReason = BoomReason.EXCEED_DIGITS;
            reasonTxt = levelData.ruleset[2].ruleDesc;
            isBoom = true;
        }
        else if (levelData.levelIndex >= CHECK3_LVINDEX && BoardCalculation.FlaskSpecialBoomCheck3(levelData.curBoard))
        {
            boomReason = BoomReason.EXCEED_REPEATS;
            reasonTxt = levelData.ruleset[4].ruleDesc;
            isBoom = true;
        }
        /*else if (levelData.levelIndex >= CHECK4_LVINDEX && BoardCalculation.FlaskSpecialBoomCheck4(levelData.curBoard))
        {
            boomReason = BoomReason.PRIME_NUM;
            isBoom = true;
        }*/
        for (int i = 0; i < levelData.curBoard.cells.Count; i++)
        {
            if (levelData.curBoard.cells[i].coord == coord && isBoom)
            {
                //levelData.curBoard.cells[i].status = 1;
                Explode_Anim(coord, reasonTxt);
            }
            else
            {
                //levelData.curBoard.cells[i].status = 0;
            }
        }
        //update panel info
        PanelInfoUpdate();
    }
    public override bool CheckWinCondition()
    {
        bool result = false;
        if (isBoom && levelData.levelIndex != 2)
        {
            return false;
        }
        if (levelData.levelIndex == 1)
        {
            return BoardCalculation.CountXplus_Ytimes(levelData.curBoard, 10, 2);
        }
        else if (levelData.levelIndex == 2)
        {
            return BoardCalculation.CountXplus_Ytimes(levelData.curBoard, 1000, 1);
        }
        else if (levelData.levelIndex == 3)
        {
            return BoardCalculation.CountXplus_Ytimes(levelData.curBoard, 900, 2);
        }
        else if (levelData.levelIndex == 4)
        {
            return BoardCalculation.CountXplus_Ytimes(levelData.curBoard, 910, 2);
        }
        else if (levelData.levelIndex == 5)
        {
            return BoardCalculation.CountXplus_Ytimes(levelData.curBoard, 920, 2);
        }
        else if (levelData.levelIndex == 6)
        {
            return BoardCalculation.CountXplus_Ytimes(levelData.curBoard, 930, 3);
        }
        else if (levelData.levelIndex == 7)
        {
            return BoardCalculation.CountXplus_Ytimes(levelData.curBoard, 940, 3);
        }
        else if (levelData.levelIndex == 8)
        {
            List<int> NumbersCount = BoardCalculation.CountByDigits(levelData.curBoard);
            bool hasDup = false;
            for (int i = 0; i < NumbersCount.Count; i += 2)
            {
                if (NumbersCount[i] > 1)
                {
                    hasDup = true;
                }
            }
            return !hasDup;
        }
        else if (levelData.levelIndex == 9)
        {
            return missingNumbers.Length == 0;
        }
        else if (levelData.levelIndex == 10)
        {
            return levelData.curBoard.toolCount == 0;
        }
        else if (levelData.levelIndex == 11)
        {
            List<int> NumbersCount = BoardCalculation.CountByDigits(levelData.curBoard);
            bool hasEven = false;
            for (int i = 0; i < NumbersCount.Count; i += 2)
            {
                if (NumbersCount[i] > 0)
                {
                    hasEven = true;
                }
            }
            return !hasEven;
        }
        else if (levelData.levelIndex == 12)
        {
            return levelData.curBoard.toolCount == 0;
        }
        else
        {
            Debug.LogError(string.Format("reach undefined level in CheckWinCondition of ({0})", levelData.theme));
        }
        return result;
    }
    public override bool CheckLoseCondition()
    {
        if (levelData.curBoard.toolCount == 0 || isBoom)
        {
            return true;
        }
        return false;
    }
    void Explode_Anim(Vector2Int coord, string reason)
    {
        for (int i = 0; i < flaskHub.cellBgs.Count; i++)
        {
            if (flaskHub.cellBgs[i].Key.coord == coord)
            {
                GameObject obj = Instantiate(flaskHub.explodeAnim, flaskHub.transform);
                obj.transform.position = flaskHub.cellBgs[i].Value.transform.position;
                obj.GetComponent<ExplodeAnim_Flask>().SetReason(reason);
                obj.SetActive(true);
            }
        }
    }
    float PourRateIncreaseLinear(float timeIntervalSinceLastPouring)
    {
        float speedUpFactor = .5f;
        float PourRate = Mathf.Min(maxPourRatePerSecond, curAddRate + speedUpFactor * timeIntervalSinceLastPouring);
        //Debug.Log(string.Format("set current rate as {0}, interval time since last pouring is {1}", curAddRate, timeIntervalSinceLastPouring));
        return PourRate;
    }
    float PourRateIncreaseCubic(float timeIntervalSincePouringStart)
    {
        float speedUpFactor = 3f;
        float PourRate = Mathf.Min(maxPourRatePerSecond, minPourRatePerSecond * Mathf.Pow(speedUpFactor, timeIntervalSincePouringStart));
        //Debug.Log(string.Format("set current rate as {0}, interval time since pouring start is {1}", curAddRate, timeIntervalSincePouringStart));
        return PourRate;
    }
    void PanelInfoUpdate()
    {
        string statTxt = "";
        if (levelData.levelIndex >= CHECK2_LVINDEX)
        {
            statTxt += string.Format("@Loc=ui_goal_current_avg@@{0}", BoardCalculation.AverageByDigits(levelData.curBoard).ToString("0.00"));
        }
        if (levelData.levelIndex >= CHECK3_LVINDEX)
        {
            string stat2txt = string.Format("<br>@Loc=ui_goal_current_repeat3@@");
            List<int> digitCount = BoardCalculation.CountByDigits(levelData.curBoard);
            bool hasRepeat3 = false;
            for (int i = 0; i < digitCount.Count; i++)
            {
                if (digitCount[i] >= 3)
                {
                    stat2txt += string.Format(" {0}", i);
                    hasRepeat3 = true;
                }
            }
            if (hasRepeat3)
            {
                statTxt += LocalizedAssetLookup.singleton.Translate(stat2txt);
            }
        }
        flaskHub.stat1.SetText(LocalizedAssetLookup.singleton.Translate(statTxt));
        flaskHub.stat1.gameObject.SetActive(true);
        flaskHub.stat2.gameObject.SetActive(false);
        
    }
    void RuleSketchInit()
    {
        flaskHub.rulesetInfographs[0].SetActive(levelData.levelIndex > POURING_LVINDEX + 1);
        flaskHub.rulesetInfographs[1].SetActive(levelData.levelIndex > CHECK1_LVINDEX);
        flaskHub.rulesetInfographs[2].SetActive(levelData.levelIndex > CHECK2_LVINDEX);
        flaskHub.rulesetInfographs[3].SetActive(levelData.levelIndex > CHECK3_LVINDEX);

    }
    void RuleSketchAnimate()
    {
        if (levelData.levelIndex == POURING_LVINDEX + 1)
        {
            flaskHub.rulesetInfographs[0].SetActive(true);
            flaskHub.rulesetInfographs[0].GetComponent<SpriteRenderer>().DOFade(0f, dConstants.UI.StandardizedVFXAnimDuration).From();
        }
        if (levelData.levelIndex == CHECK1_LVINDEX)
        {
            flaskHub.rulesetInfographs[1].SetActive(true);
            flaskHub.rulesetInfographs[1].GetComponent<SpriteRenderer>().DOFade(0f, dConstants.UI.StandardizedVFXAnimDuration).From();
        }
        if (levelData.levelIndex == CHECK2_LVINDEX)
        {
            flaskHub.rulesetInfographs[2].SetActive(true);
            flaskHub.rulesetInfographs[2].GetComponent<SpriteRenderer>().DOFade(0f, dConstants.UI.StandardizedVFXAnimDuration).From();
        }
        if (levelData.levelIndex == CHECK3_LVINDEX)
        {
            flaskHub.rulesetInfographs[3].SetActive(true);
            flaskHub.rulesetInfographs[3].GetComponent<SpriteRenderer>().DOFade(0f, dConstants.UI.StandardizedVFXAnimDuration).From();
        }
    }
}
