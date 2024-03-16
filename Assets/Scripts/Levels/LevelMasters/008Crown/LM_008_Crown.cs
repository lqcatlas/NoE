using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class LM_008_Crown : LevelMasterBase
{
    enum CrownStatus : int { off = 0, on = 1 };
    [Header("Theme Additions")]
    public LMHub_008_Crown crownHub;

    [Header("Crown Play Log")]
    public List<CrownLog> crownLogs;
    public class CrownLog
    {
        public bool success;
        public List<Vector2Int> losingCoords;
        public List<Vector2Int> winningCoords;
    }

    //private int Count_SwictchToOn;
    private float CROWN_SCALE_FACTOR = 1.05f;
    private float ANIM_WAR_DURATION = 1.2f;
    private float ANIM_FALL_DURATION = .5f;
    public override void GetObjectReferences(GameObject _themeHub)
    {
        base.GetObjectReferences(null);
        crownHub = _themeHub.GetComponent<LMHub_008_Crown>();
    }
    public override void InitCells()
    {
        //init bg for crowns
        crownHub.crownBgs = new List<KeyValuePair<CellMaster, GameObject>>();
        crownHub.cellBgHolder.transform.localScale = hub.boardMaster.cellHolder.localScale;
        List<Transform> oldCrowns = crownHub.cellBgHolder.GetComponentsInChildren<Transform>().ToList();
        oldCrowns.Remove(crownHub.cellBgHolder.transform);
        for (int i = 0; i < oldCrowns.Count; i++)
        {
            Destroy(oldCrowns[i].gameObject);
        }
        for (int i = 0; i < hub.boardMaster.cells.Count; i++)
        {
            DataCell temp_cellData = levelData.curBoard.GetCellDataByCoord(hub.boardMaster.cells[i].coord);
            if (temp_cellData != null)
            {
                hub.boardMaster.cells[i].DisplayNumber(temp_cellData.value);
                //create crown sprite
                GameObject obj = Instantiate(crownHub.crownTemplate, crownHub.cellBgHolder);
                obj.transform.position = hub.boardMaster.cells[i].transform.position;
                crownHub.crownBgs.Add(new KeyValuePair<CellMaster, GameObject>(hub.boardMaster.cells[i], obj));
                if (temp_cellData.status == (int)CrownStatus.on)
                {
                    obj.gameObject.SetActive(true);
                    float CROWN_SIZE = Mathf.Pow(CROWN_SCALE_FACTOR, levelData.curBoard.GetCellDataByCoord(crownHub.crownBgs[i].Key.coord).value);
                    obj.transform.localScale = Vector3.one * CROWN_SIZE;
                }
                else
                {
                    obj.gameObject.SetActive(false);
                    obj.transform.localScale = Vector3.one;
                }
            }
        }
    }
    public override void InitTool()
    {
        base.InitTool();
        hub.toolMaster.toolIcon.sprite = crownHub.toolSprite;
        UpdateToolStatusDisplay();
    }
    public override void AddtionalInit_Theme(bool isRewind = false)
    {
        if (!isRewind)
        {
            crownLogs = new List<CrownLog>();
        }
        else
        {
            crownLogs.RemoveAt(crownLogs.Count - 1);
        }
    }
    public override void HandlePlayerInput(Vector2Int coord)
    {
        Vector2Int numberCap = new Vector2Int(0, 9);
        //remove cell
        for (int i = 0; i < levelData.curBoard.cells.Count; i++)
        {
            if (levelData.curBoard.cells[i].coord == coord)
            {
                levelData.curBoard.cells[i].value += 3;
                levelData.curBoard.cells[i].value = Mathf.Min(levelData.curBoard.cells[i].value, numberCap.y);
                levelData.curBoard.cells[i].status = (int)CrownStatus.on;
            }
        }
        CrownLog currentLog = new CrownLog();
        currentLog.success = true;
        currentLog.losingCoords = new List<Vector2Int>();
        currentLog.winningCoords = new List<Vector2Int>();
        crownLogs.Add(currentLog);
    }

    public override void HandleEnvironment(Vector2Int coord)
    {
        //successThisRound = true;
        Vector2Int numberCap = new Vector2Int(0, 9);
        DataCell crownCell = levelData.curBoard.GetCellDataByCoord(coord);
        int RULE2_LVINDEX = 3;
        int RULE3_LVINDEX = 7;
        //level 3+, check if crowning is a success, level
        bool isSuccess = true;
        if (levelData.levelIndex >= RULE2_LVINDEX)
        {
            for (int i = 0; i < levelData.curBoard.cells.Count; i++)
            {
                if (BoardCalculation.Manhattan_Dist(coord, levelData.curBoard.cells[i].coord) == 1)
                {
                    if (crownCell.value < levelData.curBoard.cells[i].value)
                    {
                        isSuccess = false;
                        crownLogs[crownLogs.Count - 1].success = false;
                        crownLogs[crownLogs.Count - 1].losingCoords.Add(levelData.curBoard.cells[i].coord);
                    }
                }
            }
        }
        //queue the value reset at the end
        if (!isSuccess)
        {
            crownCell.value = 0;
            crownCell.status = (int)CrownStatus.off;
        }
        //level 6+, if succeed, taking crowns from neighbors
        if (levelData.levelIndex >= RULE3_LVINDEX)
        {
            if (GetCurrentCrownSuccess())
            {
                for (int i = 0; i < levelData.curBoard.cells.Count; i++)
                {
                    if (BoardCalculation.Manhattan_Dist(coord, levelData.curBoard.cells[i].coord) == 1)
                    {
                        if (crownCell.value > levelData.curBoard.cells[i].value && levelData.curBoard.cells[i].status == (int)CrownStatus.on)
                        {
                            levelData.curBoard.cells[i].value -= 2;
                            levelData.curBoard.cells[i].value = Mathf.Max(levelData.curBoard.cells[i].value, numberCap.x);
                            levelData.curBoard.cells[i].status = (int)CrownStatus.off;
                            crownLogs[crownLogs.Count - 1].winningCoords.Add(levelData.curBoard.cells[i].coord);
                        }
                    }
                }
            }
        }
    }  
    public override void UpdateCells(Vector2Int coord)
    {
        //update to a status that only +3 on the played cell
        for (int i = 0; i < hub.boardMaster.cells.Count; i++)
        {
            if (hub.boardMaster.cells[i].coord == coord)
            {
                DataCell temp_cellData = levelData.previousBoard.GetCellDataByCoord(hub.boardMaster.cells[i].coord);
                hub.boardMaster.cells[i].NumberShift(Mathf.Min(temp_cellData.value + 3, 9));
            }
        }
    }
    public override void AddtionalUpdate_Theme(Vector2Int coord)
    {
        Anim_CrownFall(coord);
        GameObject playedCrownCell = null;
        for (int i = 0; i < crownHub.crownBgs.Count; i++)
        {
            if (crownHub.crownBgs[i].Key.coord == coord)
            {
                playedCrownCell = crownHub.crownBgs[i].Value;
            }
        }
        if (GetCurrentCrownSuccess())
        {
            for (int i = 0; i < crownHub.crownBgs.Count; i++)
            {
                if (BoardCalculation.Manhattan_Dist(crownHub.crownBgs[i].Key.coord, coord) == 1 
                    && crownLogs[crownLogs.Count - 1].winningCoords.Contains(crownHub.crownBgs[i].Key.coord))
                {
                    GameObject obj = Instantiate(crownHub.warTemplate, crownHub.cellBgHolder);
                    obj.transform.position = (crownHub.crownBgs[i].Value.transform.position + playedCrownCell.transform.position) / 2f;
                    obj.SetActive(true);
                    Anim_CrownMove(crownHub.crownBgs[i].Key.coord, coord, ANIM_FALL_DURATION);
                    //Anim_CrownFail(crownHub.crownBgs[i].Key.coord, ANIM_WAR_DURATION);
                    UpdateTargetCellValue(crownHub.crownBgs[i].Key.coord, ANIM_FALL_DURATION);

                }
            }
            if (crownLogs[crownLogs.Count - 1].winningCoords.Count > 0)
            {
                Anim_CrownSuccess(coord, ANIM_WAR_DURATION * 0.8f);
                UpdateTargetCellValue(coord, ANIM_WAR_DURATION * 0.8f);
            }
            else
            {
                Anim_CrownSuccess(coord, ANIM_FALL_DURATION * 0.6f);
                UpdateTargetCellValue(coord, ANIM_FALL_DURATION * 0.6f);
            }
        }
        else
        {
            for (int i = 0; i < crownHub.crownBgs.Count; i++)
            {
                if (BoardCalculation.Manhattan_Dist(crownHub.crownBgs[i].Key.coord, coord) == 1
                    && crownLogs[crownLogs.Count - 1].losingCoords.Contains(crownHub.crownBgs[i].Key.coord))
                {
                    GameObject obj = Instantiate(crownHub.warTemplate, crownHub.cellBgHolder);
                    obj.transform.position = (crownHub.crownBgs[i].Value.transform.position + playedCrownCell.transform.position) / 2f;
                    obj.SetActive(true);
                    //Anim_CrownMove(coord, crownHub.crownBgs[i].Key.coord, ANIM_FALL_DURATION);
                    //Anim_CrownSuccess(crownHub.crownBgs[i].Key.coord, ANIM_WAR_DURATION);
                }
            }
            if(crownLogs[crownLogs.Count - 1].losingCoords.Count > 0)
            {
                Anim_CrownFail(coord, ANIM_WAR_DURATION * 0.8f);
                UpdateTargetCellValue(coord, ANIM_WAR_DURATION * 0.8f);
            }
            else
            {
                Anim_CrownFail(coord, ANIM_FALL_DURATION * 0.6f);
                UpdateTargetCellValue(coord, ANIM_FALL_DURATION * 0.6f);
            }
        }
        //add crown taken count
        if (levelData.levelIndex == 9)
        {
            hub.goalMaster.lines[1].SetText(string.Format("{0}{1}", LocalizedAssetLookup.singleton.Translate("@Loc=ui_goal_current_crown_taken@@"), GetTotalCrownTaken()));
            hub.goalMaster.lines[1].gameObject.SetActive(true);
            LayoutRebuilder.ForceRebuildLayoutImmediate(hub.goalMaster.goalLayout);
        }
    }
    public override bool CheckWinCondition()
    {
        bool result = false;
        if (levelData.levelIndex == 1)
        {
            return BoardCalculation.CountX_Ytimes(levelData.curBoard, 9, 2);
        }
        else if (levelData.levelIndex == 2)
        {
            return BoardCalculation.CountX_All(levelData.curBoard, 9);
        }
        else if (levelData.levelIndex == 3)
        {
            return BoardCalculation.CountX_All(levelData.curBoard, 9);
        }
        else if (levelData.levelIndex == 4)
        {
            return BoardCalculation.CountX_Ytimes(levelData.curBoard, 9, 4);
        }
        else if (levelData.levelIndex == 5)
        {
            return BoardCalculation.CountX_Ytimes(levelData.curBoard, 9, 6);
        }
        else if (levelData.levelIndex == 6)
        {
            return BoardCalculation.CountStatusX_Ytimes(levelData.curBoard, 1, 6);
        }
        else if (levelData.levelIndex == 7)
        {
            return BoardCalculation.CountStatusX_Ytimes(levelData.curBoard, 1, 4);
        }
        else if (levelData.levelIndex == 8)
        {
            return BoardCalculation.CountStatusX_Ytimes(levelData.curBoard, 1, 6);
        }
        else if (levelData.levelIndex == 9)
        {
            return GetTotalCrownTaken() >= 9;
        }
        else if (levelData.levelIndex == 10)
        {
            return BoardCalculation.CountXplus_Ytimes(levelData.curBoard, 6, 9);
        }
        else if (levelData.levelIndex == 11)
        {
            return BoardCalculation.CountStatusX_Ytimes(levelData.curBoard, 1, 9);
        }
        else
        {
            Debug.LogError(string.Format("reach undefined level in CheckWinCondition of ({0})", levelData.theme));
        }
        return result;
    }

    bool GetCurrentCrownSuccess()
    {
        return crownLogs[crownLogs.Count - 1].success;
    }
    int GetCurrentCrownTaken()
    {
        return crownLogs[crownLogs.Count - 1].winningCoords.Count;
    }
    int GetTotalSuccess()
    {
        int total = 0;
        for(int i = 0; i < crownLogs.Count; i++)
        {
            if (crownLogs[i].success)
            {
                total += 1;
            }
        }
        return total;
    }
    int GetTotalCrownTaken()
    {
        int total = 0;
        for (int i = 0; i < crownLogs.Count; i++)
        {
            total += crownLogs[i].winningCoords.Count;
        }
        return total;
    }
    void UpdateTargetCellValue(Vector2Int coord, float delay = 0f)
    {
        for (int i = 0; i < crownHub.crownBgs.Count; i++)
        {
            if (crownHub.crownBgs[i].Key.coord == coord)
            {
                DataCell temp_cellData = levelData.curBoard.GetCellDataByCoord(crownHub.crownBgs[i].Key.coord);
                Sequence seq = DOTween.Sequence();
                CellMaster targetCell = crownHub.crownBgs[i].Key;
                GameObject taregtObj = crownHub.crownBgs[i].Value;
                seq.AppendInterval(delay)
                    .AppendCallback(() => targetCell.NumberShift(temp_cellData.value))
                    .AppendCallback(() => taregtObj.SetActive(temp_cellData.status == (int)CrownStatus.on));
            }
        }
        
    }
    void Anim_CrownFall(Vector2Int coord)
    {
        float FALL_DISTANCE = 3f;
        for (int i = 0; i < crownHub.crownBgs.Count; i++)
        {
            if (crownHub.crownBgs[i].Key.coord == coord)
            {
                GameObject obj = Instantiate(crownHub.crownTemplate, crownHub.cellBgHolder);
                obj.transform.position = crownHub.crownBgs[i].Value.transform.position;
                obj.transform.GetChild(0).GetComponent<SpriteRenderer>().color = dConstants.UI.DefaultColor_2nd;
                obj.transform.GetChild(0).GetComponent<SpriteRenderer>().DOFade(0f, ANIM_FALL_DURATION);
                obj.transform.DOLocalMoveY(FALL_DISTANCE, ANIM_FALL_DURATION).From(true).OnComplete(()=>Destroy(obj));
            }
        }
    }
    void Anim_CrownSuccess(Vector2Int coord, float delay = 0f)
    {
        Debug.Log(string.Format("anim_crown_success with param ({0},{1})", coord.x, coord.y));
        float POPUP_DURATION = .8f;
        for (int i = 0; i < crownHub.crownBgs.Count; i++)
        {
            if (crownHub.crownBgs[i].Key.coord == coord)
            {
                GameObject originalBg = crownHub.crownBgs[i].Value;
                float POPUP_SIZE = Mathf.Pow(CROWN_SCALE_FACTOR, levelData.curBoard.GetCellDataByCoord(crownHub.crownBgs[i].Key.coord).value);
                originalBg.transform.DOScale(POPUP_SIZE, POPUP_DURATION).SetDelay(delay).SetEase(Ease.OutBounce).OnStart(() => originalBg.SetActive(true));
            }
        }
    }
    
    void Anim_CrownFail(Vector2Int coord, float delay = 0)
    {
        Debug.Log(string.Format("anim_crown_fail with param ({0},{1})", coord.x, coord.y));
        float failAnimTime = .3f;
        Vector2 fallingDist = new Vector2(2, -2);
        float tiltingDegree = -30f;
        for (int i = 0; i < crownHub.crownBgs.Count; i++)
        {
            if (crownHub.crownBgs[i].Key.coord == coord)
            {
                GameObject originalBg = crownHub.crownBgs[i].Value;
                GameObject obj = Instantiate(originalBg, crownHub.cellBgHolder);
                //obj.GetComponentInChildren<SpriteRenderer>().color = dConstants.UI.DefaultColor_2nd;
                obj.SetActive(false);
                obj.transform.DOMoveY(fallingDist.y, failAnimTime).SetDelay(delay).SetRelative(true).SetEase(Ease.OutQuad);
                obj.transform.DOMoveX(fallingDist.x, failAnimTime).SetDelay(delay).SetRelative(true).SetEase(Ease.Linear);
                obj.transform.DORotate(new Vector3(0f, 0f, tiltingDegree), failAnimTime).SetDelay(delay).SetRelative(true).SetEase(Ease.OutQuad)
                    .OnStart(() => obj.SetActive(true));
                obj.GetComponentInChildren<SpriteRenderer>().DOFade(0f, failAnimTime).SetEase(Ease.OutQuad).SetDelay(delay)
                    .OnStart(() => originalBg.SetActive(false))
                    .OnComplete(() => Destroy(obj));
            }
        }
    }
    void Anim_CrownMove(Vector2Int coordFrom, Vector2Int coordTo, float delay = 0f)
    {
        Debug.Log(string.Format("anim_crown_move with param ({0},{1}) >> ({2},{3})", coordFrom.x, coordFrom.y, coordTo.x, coordTo.y));
        float MOVE_DURATION = .5f;
        Transform fromTrans = null, toTrans = null;
        for (int i = 0; i < crownHub.crownBgs.Count; i++)
        {  
            if (crownHub.crownBgs[i].Key.coord == coordFrom)
            {
                fromTrans = crownHub.crownBgs[i].Value.transform;
            }
            else if(crownHub.crownBgs[i].Key.coord == coordTo)
            {
                toTrans = crownHub.crownBgs[i].Value.transform;
            }
        }
        GameObject obj = Instantiate(fromTrans.gameObject, crownHub.cellBgHolder);
        obj.gameObject.SetActive(false);
        obj.transform.DOMove(toTrans.position, MOVE_DURATION).SetDelay(delay).OnStart(()=> obj.gameObject.SetActive(true)).OnComplete(()=>Destroy(obj));
    }
    void UpdateToolStatusDisplay()
    {
        ToolStatusGroup targetDisplayTemplate = crownHub.toolStatusGroup;

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
