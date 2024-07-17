using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using static LM_008_Crown;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.UIElements;

public class LM_012_Skyscraper : LevelMasterBase
{

    [Header("Theme Additions")]
    public LMHub_012_Skyscraper themeHub;

    [Header("Play Log")]
    private List<SkyscraperLog> skyscraperLogs;
    private class SkyscraperLog
    {
        public bool popBoom;
        public List<Vector2Int> moveoutCoords;
        //public List<Vector2Int> moveinCoords;
    }

    private int RULE2_LVINDEX = 2;
    private int RULE3_LVINDEX = 6;

    private int BASIC_ADD = 2;
    private int MIGRATION_ADD = 3;
    private int BOOM_ADD = 5;

    private float ANIM_BOOM_SCALE_FACTOR = 1.5f;
    private float ANIM_BOOM_DURATION = .8f;
    private float ANIM_MIGRATION_DURATION = .8f;
    private float ANIM_FALL_DURATION = .5f;

    public override void GetObjectReferences(GameObject _themeHub)
    {
        base.GetObjectReferences(null);
        themeHub = _themeHub.GetComponent<LMHub_012_Skyscraper>();
    }
    public override void InitTool()
    {
        base.InitTool();
        hub.toolMaster.toolIcon.sprite = themeHub.toolSprite;
        UpdateToolStatusDisplay();
    }
    public override void AddtionalInit_Theme(bool isRewind = false)
    {
        themeHub.cellBgs = new List<KeyValuePair<CellMaster, GameObject>>();
        themeHub.bgHolder.transform.localScale = hub.boardMaster.cellHolder.localScale;
        //clear old bgs
        List<Transform> oldBgs = themeHub.bgHolder.GetComponentsInChildren<Transform>().ToList();
        oldBgs.Remove(themeHub.bgHolder.transform);
        for (int i = 0; i < oldBgs.Count; i++)
        {
            Destroy(oldBgs[i].gameObject);
        }
        //set new bgs
        for (int i = 0; i < hub.boardMaster.cells.Count; i++)
        {
            DataCell temp_cellData = levelData.curBoard.GetCellDataByCoord(hub.boardMaster.cells[i].coord);
            if (temp_cellData != null)
            {
                hub.boardMaster.cells[i].DisplayNumber(temp_cellData.value);
                //create new bg sprite
                GameObject obj = Instantiate(themeHub.bldgTemplate, themeHub.bgHolder);
                obj.transform.position = hub.boardMaster.cells[i].transform.position;
                obj.GetComponent<SkyscraperCellBg>().SetSpriteByPop(temp_cellData.value);
                themeHub.cellBgs.Add(new KeyValuePair<CellMaster, GameObject>(hub.boardMaster.cells[i], obj));
            }
        }
        //remove previous step log
        if (!isRewind)
        {
            skyscraperLogs = new List<SkyscraperLog>();
        }
        else
        {
            skyscraperLogs.RemoveAt(skyscraperLogs.Count - 1);
        }
    }
    public override void HandlePlayerInput(Vector2Int coord)
    {
        for (int i = 0; i < levelData.curBoard.cells.Count; i++)
        {
            if (levelData.curBoard.cells[i].coord == coord)
            {
                levelData.curBoard.cells[i].value += BASIC_ADD;
            }
        }
        SkyscraperLog currentLog = new SkyscraperLog();
        currentLog.popBoom = false;
        currentLog.moveoutCoords = new List<Vector2Int>();
        //currentLog.moveinCoords = new List<Vector2Int>();
        skyscraperLogs.Add(currentLog);
    }
    public override void HandleEnvironment(Vector2Int coord)
    {
        DataCell bldgCell = levelData.curBoard.GetCellDataByCoord(coord);
        //level 2+, check population move
        int moveinTotal = 0; // queue this to add to cur cell at the end
        int moveCount = 0;
        if (levelData.levelIndex >= RULE2_LVINDEX)
        {
            for (int i = 0; i < levelData.curBoard.cells.Count; i++)
            {
                if (BoardCalculation.Manhattan_Dist(coord, levelData.curBoard.cells[i].coord) == 1)
                {
                    if (bldgCell.value > levelData.curBoard.cells[i].value && levelData.curBoard.cells[i].value > 0)
                    {
                        int movingPop = Mathf.Min(levelData.curBoard.cells[i].value, MIGRATION_ADD);
                        levelData.curBoard.cells[i].value -= movingPop;
                        moveinTotal += movingPop;
                        moveCount += 1;
                        skyscraperLogs[skyscraperLogs.Count - 1].moveoutCoords.Add(levelData.curBoard.cells[i].coord);
                    }
                }
            }
            //add move in population
            bldgCell.value += moveinTotal;
        }
        //level 6+, if succeed, check population boom
        if (levelData.levelIndex >= RULE3_LVINDEX)
        {
            if (moveCount >= 2)
            {
                bldgCell.value += BOOM_ADD;
            }
        }
    }
    public override void UpdateCells(Vector2Int coord)
    {
        //update to only basic add value on the played cell
        for (int i = 0; i < hub.boardMaster.cells.Count; i++)
        {
            if (hub.boardMaster.cells[i].coord == coord)
            {
                DataCell temp_cellData = levelData.previousBoard.GetCellDataByCoord(hub.boardMaster.cells[i].coord);
                hub.boardMaster.cells[i].NumberShift(temp_cellData.value + BASIC_ADD);
            }
        }
    }
    public override void AddtionalUpdate_Theme(Vector2Int coord)
    {
        //play blgd fall
        BldgFall_Anim(coord);

        CellMaster playedCellMaster = null;
        for (int i = 0; i < themeHub.cellBgs.Count; i++)
        {
            if (themeHub.cellBgs[i].Key.coord == coord)
            {
                playedCellMaster = themeHub.cellBgs[i].Key;
            }
        }
        //play population move fx + numbershift
        if (skyscraperLogs[skyscraperLogs.Count - 1].moveoutCoords.Count > 0)
        {
            
            for (int i = 0; i < themeHub.cellBgs.Count; i++)
            {
                if (skyscraperLogs[skyscraperLogs.Count - 1].moveoutCoords.Contains(themeHub.cellBgs[i].Key.coord))
                {
                    PopMigration_Anim(themeHub.cellBgs[i].Key.coord, coord, ANIM_FALL_DURATION);
                    int tempValue = levelData.curBoard.GetCellDataByCoord(themeHub.cellBgs[i].Key.coord).value;
                    UpdateTargetCellValue(themeHub.cellBgs[i].Key, tempValue, ANIM_FALL_DURATION);
                }
                else if(themeHub.cellBgs[i].Key.coord == coord)
                {
                    int tempValue = levelData.curBoard.GetCellDataByCoord(themeHub.cellBgs[i].Key.coord).value;
                    if(skyscraperLogs[skyscraperLogs.Count - 1].moveoutCoords.Count >= 2 && levelData.levelIndex >= RULE3_LVINDEX)
                    {
                        tempValue -= BOOM_ADD;
                    }
                    UpdateTargetCellValue(themeHub.cellBgs[i].Key, tempValue, ANIM_FALL_DURATION + ANIM_MIGRATION_DURATION);
                }
            }
        }
        //then play popluation boom + numbershift
        if (levelData.levelIndex >= RULE3_LVINDEX)
        {
            if(skyscraperLogs[skyscraperLogs.Count - 1].moveoutCoords.Count >= 2)
            {
                PopBoom_Anim(coord, ANIM_FALL_DURATION + ANIM_MIGRATION_DURATION);
                int tempValue = levelData.curBoard.GetCellDataByCoord(coord).value;
                UpdateTargetCellValue(playedCellMaster, tempValue, ANIM_FALL_DURATION + ANIM_MIGRATION_DURATION + ANIM_BOOM_DURATION);
            }
        }
        //update bg bldg sprite
        Sequence seq = DOTween.Sequence();
        seq.AppendInterval(ANIM_FALL_DURATION + ANIM_MIGRATION_DURATION + ANIM_BOOM_DURATION);
        for (int i = 0; i < themeHub.cellBgs.Count; i++)
        {
            int prevPop = levelData.previousBoard.GetCellDataByCoord(themeHub.cellBgs[i].Key.coord).value;
            int curPop = levelData.curBoard.GetCellDataByCoord(themeHub.cellBgs[i].Key.coord).value;
            SkyscraperCellBg cellBg = themeHub.cellBgs[i].Value.GetComponent<SkyscraperCellBg>();
            seq.AppendCallback(() => cellBg.UpdateSpriteByPop(prevPop, curPop));
        }
    }

    public override bool CheckWinCondition()
    {
        bool result = false;
        if (levelData.levelIndex == 1)
        {
            return BoardCalculation.CountXplus_Ytimes(levelData.curBoard, 10, 2);
        }
        else if (levelData.levelIndex == 2)
        {
            return BoardCalculation.CountXplus_Ytimes(levelData.curBoard, 15, 1);
        }
        else if (levelData.levelIndex == 3)
        {
            return BoardCalculation.CountXplus_Ytimes(levelData.curBoard, 15, 2);
        }
        else if (levelData.levelIndex == 4)
        {
            return BoardCalculation.CountXplus_Ytimes(levelData.curBoard, 15, 3);
        }
        else if (levelData.levelIndex == 5)
        {
            return BoardCalculation.CountXplus_Ytimes(levelData.curBoard, 15, 4);
        }
        else if (levelData.levelIndex == 6)
        {
            return BoardCalculation.CountXplus_Ytimes(levelData.curBoard, 30, 1);
        }
        else if (levelData.levelIndex == 7)
        {
            return BoardCalculation.CountXplus_Ytimes(levelData.curBoard, 40, 1);
        }
        else if (levelData.levelIndex == 8)
        {
            return BoardCalculation.CountXplus_Ytimes(levelData.curBoard, 30, 2);
        }
        else if (levelData.levelIndex == 9)
        {
            return BoardCalculation.CountXplus_Ytimes(levelData.curBoard, 30, 4);
        }
        else if (levelData.levelIndex == 10)
        {
            return BoardCalculation.Sum_Larger_X(levelData.curBoard, 100);
        }
        else
        {
            Debug.LogError(string.Format("reach undefined level in CheckWinCondition of ({0})", levelData.theme));
        }
        return result;
    }
    void BldgFall_Anim(Vector2Int coord)
    {
        //drop a bldg block
        float FALL_DISTANCE = 3f;
        for (int i = 0; i < themeHub.cellBgs.Count; i++)
        {
            if (themeHub.cellBgs[i].Key.coord == coord)
            {
                GameObject obj = Instantiate(themeHub.bldgTemplate, themeHub.bgHolder);
                obj.transform.position = themeHub.cellBgs[i].Value.transform.position;
                obj.transform.GetChild(0).GetComponent<SpriteRenderer>().color = dConstants.UI.DefaultColor_2nd;
                obj.transform.GetChild(0).GetComponent<SpriteRenderer>().DOFade(0f, ANIM_FALL_DURATION);
                obj.transform.DOLocalMoveY(FALL_DISTANCE, ANIM_FALL_DURATION).From(true).OnComplete(() => Destroy(obj));
            }
        }
    }
    
    void PopMigration_Anim(Vector2Int fromCoord, Vector2Int toCoord, float delay)
    {
        Debug.LogWarning(string.Format("anim pop migartion with param ({0},{1}) >> ({2},{3})", fromCoord.x, fromCoord.y, toCoord.x, toCoord.y));
        Transform fromTrans = null, toTrans = null;
        for (int i = 0; i < themeHub.cellBgs.Count; i++)
        {
            if (themeHub.cellBgs[i].Key.coord == fromCoord)
            {
                fromTrans = themeHub.cellBgs[i].Value.transform;
            }
            else if (themeHub.cellBgs[i].Key.coord == toCoord)
            {
                toTrans = themeHub.cellBgs[i].Value.transform;
            }
        }
        GameObject obj = Instantiate(themeHub.populationTemplate, themeHub.bgHolder);
        obj.transform.position = fromTrans.position;
        obj.gameObject.SetActive(false);
        obj.transform.GetChild(0).GetComponent<SpriteRenderer>().DOFade(1f, ANIM_MIGRATION_DURATION/2f).SetLoops(2, LoopType.Yoyo).SetDelay(delay);
        obj.transform.DOMove(toTrans.position, ANIM_MIGRATION_DURATION).SetDelay(delay).OnStart(() => obj.gameObject.SetActive(true)).OnComplete(() => Destroy(obj));
    }
    void PopBoom_Anim(Vector2Int coord, float delay)
    {
        Debug.LogWarning($"play pop boom vfx TBD with a delay of {delay} sec");
        for (int i = 0; i < themeHub.cellBgs.Count; i++)
        {
            if (themeHub.cellBgs[i].Key.coord == coord)
            {
                GameObject obj = Instantiate(themeHub.populationTemplate, themeHub.bgHolder);
                obj.transform.position = themeHub.cellBgs[i].Value.transform.position;
                obj.SetActive(false);
                obj.transform.GetChild(0).GetComponent<SpriteRenderer>().DOFade(0f, ANIM_BOOM_DURATION).SetDelay(delay);
                obj.transform.DOScale(ANIM_BOOM_SCALE_FACTOR, ANIM_BOOM_DURATION).SetDelay(delay).OnStart(() => obj.SetActive(true)).OnComplete(() => Destroy(obj));
            }
        }
    }
    void UpdateTargetCellValue(CellMaster targetCell, int targetValue, float delay = 0f)
    {
        Sequence seq = DOTween.Sequence();
        seq.AppendInterval(delay)
            .AppendCallback(() => targetCell.NumberShift(targetValue));
    }
    void UpdateToolStatusDisplay()
    {
        ToolStatusGroup targetDisplayTemplate = themeHub.toolStatusGroup;

        string toolName = targetDisplayTemplate.GetStatusName(levelData.curBoard.toolStatus);
        if (toolName != null)
        {
            hub.toolMaster.toolSubtitle.SetText(LocalizedAssetLookup.singleton.Translate(toolName));
        }
        Sprite toolInfograph = targetDisplayTemplate.GetStatusInfograph(levelData.curBoard.toolStatus);
        if (toolInfograph != null)
        {
            hub.toolMaster.infographGroup.SetActive(true);
            hub.toolMaster.infograph.sprite = toolInfograph;
        }
        else
        {
            hub.toolMaster.infographGroup.SetActive(false);
        }
    }
}
