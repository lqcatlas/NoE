using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Runtime.CompilerServices;

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
        //level 3+, check if crowning is a success, level
        if (levelData.levelIndex >= 3)
        {
            for (int i = 0; i < levelData.curBoard.cells.Count; i++)
            {
                if (BoardCalculation.Manhattan_Dist(coord, levelData.curBoard.cells[i].coord) == 1)
                {
                    if (crownCell.value < levelData.curBoard.cells[i].value)
                    {
                        crownCell.value = 0;
                        crownCell.status = (int)CrownStatus.off;
                        crownLogs[crownLogs.Count - 1].success = false;
                        crownLogs[crownLogs.Count - 1].losingCoords.Add(levelData.curBoard.cells[i].coord);
                    }
                }
            }
        }
        //level 6+, if succeed, taking crowns from neighbors
        if (levelData.levelIndex >= 6)
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
        base.UpdateCells(coord);
        //play crown VFX based on success or not
        if (GetCurrentCrownSuccess())
        {

        }
        else
        {

        }
        for (int i = 0; i < crownHub.crownBgs.Count; i++)
        {
            DataCell temp_cellData = levelData.curBoard.GetCellDataByCoord(crownHub.crownBgs[i].Key.coord);
            if (temp_cellData.status == (int)CrownStatus.on)
            {
                crownHub.crownBgs[i].Value.SetActive(true);
            }
            else
            {
                crownHub.crownBgs[i].Value.SetActive(false);
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
                    Anim_CrownMove(crownHub.crownBgs[i].Key.coord, coord, ANIM_WAR_DURATION/2f);
                    Anim_CrownFail(crownHub.crownBgs[i].Key.coord, ANIM_WAR_DURATION);

                }
            }
            if (crownLogs[crownLogs.Count - 1].winningCoords.Count > 0)
            {
                Anim_CrownSuccess(coord, ANIM_WAR_DURATION * 0.8f);
            }
            else
            {
                Anim_CrownSuccess(coord, ANIM_FALL_DURATION * 0.6f);
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
                    Anim_CrownMove(coord, crownHub.crownBgs[i].Key.coord, ANIM_WAR_DURATION / 2f);
                    Anim_CrownSuccess(crownHub.crownBgs[i].Key.coord, ANIM_WAR_DURATION);
                }
            }
            if(crownLogs[crownLogs.Count - 1].losingCoords.Count > 0)
            {
                Anim_CrownFail(coord, ANIM_WAR_DURATION * 0.8f);
            }
            else
            {
                Anim_CrownFail(coord, ANIM_FALL_DURATION * 0.6f);
            }
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
            return BoardCalculation.CountX_Ytimes(levelData.curBoard, 0, 5);
        }
        else if (levelData.levelIndex == 4)
        {
            return BoardCalculation.CountX_Ytimes(levelData.curBoard, 9, 6);
        }
        else if (levelData.levelIndex == 5)
        {
            return BoardCalculation.CountX_Ytimes(levelData.curBoard, 9, 6);
        }
        else if (levelData.levelIndex == 6)
        {
            return BoardCalculation.CountStatusX_Ytimes(levelData.curBoard, 1, 5);
        }
        else if (levelData.levelIndex == 7)
        {
            return GetTotalCrownTaken() >= 8;
        }
        else if (levelData.levelIndex == 8)
        {
            return BoardCalculation.CountXplus_Ytimes(levelData.curBoard, 7, 9);
        }
        else if (levelData.levelIndex == 9)
        {

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
    void Anim_CrownFall(Vector2Int coord)
    {
        float FALL_DISTANCE = 2f;
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
        float POPUP_DURATION = .3f;
        for (int i = 0; i < crownHub.crownBgs.Count; i++)
        {
            if (crownHub.crownBgs[i].Key.coord == coord)
            {
                GameObject originalBg = crownHub.crownBgs[i].Value;
                float POPUP_SIZE = Mathf.Pow(CROWN_SCALE_FACTOR, levelData.curBoard.GetCellDataByCoord(crownHub.crownBgs[i].Key.coord).value);
                originalBg.transform.DOScale(POPUP_SIZE, POPUP_DURATION).SetDelay(delay).SetEase(Ease.OutBounce);
            }
        }
    }
    
    void Anim_CrownFail(Vector2Int coord, float delay = 0)
    {
        Debug.Log(string.Format("anim_crown_fail with param ({0},{1})", coord.x, coord.y));
        float EXPLODE_DURATION = .3f;
        float EXPLODE_SIZE = 3f;
        for (int i = 0; i < crownHub.crownBgs.Count; i++)
        {
            if (crownHub.crownBgs[i].Key.coord == coord)
            {
                GameObject originalBg = crownHub.crownBgs[i].Value;
                GameObject obj = Instantiate(originalBg, crownHub.cellBgHolder);
                obj.transform.GetChild(0).GetComponent<SpriteRenderer>().color = dConstants.UI.DefaultColor_2nd;
                obj.transform.GetChild(0).DOScale(EXPLODE_SIZE, EXPLODE_DURATION).SetDelay(delay).OnStart(() => originalBg.SetActive(false)).OnComplete(() => Destroy(obj));
            }
        }
    }
    void Anim_CrownMove(Vector2Int coordFrom, Vector2Int coordTo, float delay = 0f)
    {
        Debug.Log(string.Format("anim_crown_move with param ({0},{1}) >> ({2},{3})", coordFrom.x, coordFrom.y, coordTo.x, coordTo.y));
        float MOVE_DURATION = .3f;
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
        obj.transform.DOMove(toTrans.position, MOVE_DURATION).OnComplete(()=>Destroy(obj));
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
