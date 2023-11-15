using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;
using UnityEngine.Rendering.Universal;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.UI.Image;

public class LM_006_LightBulb : LevelMasterBase
{
    enum BulbStatus : int { on = 0, off = 1};
    [Header("Theme Additions")]
    public LMHub_006_LightBulb lightbulbHub;

    private int Count_SwictchToOn;

    public override void GetObjectReferences(GameObject _themeHub)
    {
        base.GetObjectReferences(null);
        lightbulbHub = _themeHub.GetComponent<LMHub_006_LightBulb>();
        //init theme-specific params
        ThemeAnimationDelayAfterInit = 2f;
        ThemeAnimationDelayAfterPlay = 0.5f;
    }
    public override void InitCells()
    {
        lightbulbHub.lightBulbs = new List<KeyValuePair<CellMaster, LightbulbCellBg>>();
        lightbulbHub.cellBgHolder.transform.localScale = hub.boardMaster.cellHolder.localScale;
        //clear old bg sprites
        List<Transform> oldBgs = lightbulbHub.cellBgHolder.GetComponentsInChildren<Transform>().ToList();
        oldBgs.Remove(lightbulbHub.cellBgHolder.transform);
        for (int i = 0; i < oldBgs.Count; i++)
        {
            Destroy(oldBgs[i].gameObject);
        }
        //generate new bgs
        for (int i = 0; i < hub.boardMaster.cells.Count; i++)
        {
            DataCell temp_cellData = levelData.initBoard.GetCellDataByCoord(hub.boardMaster.cells[i].coord);
            if (temp_cellData != null)
            {
                hub.boardMaster.cells[i].DisplayNumber(temp_cellData.value);
                hub.boardMaster.cells[i].numberGroup.localScale = Vector3.one * 0.65f;
                hub.boardMaster.cells[i].SetColor(Color.white, 0f);
                //additional
                GameObject cellBg = Instantiate(lightbulbHub.bulbBgTemplate, lightbulbHub.cellBgHolder);
                cellBg.transform.position = hub.boardMaster.cells[i].transform.position;
                //clockBg.transform.localScale = hub.boardMaster.cells[i].transform.localScale;
                lightbulbHub.lightBulbs.Add(new KeyValuePair<CellMaster, LightbulbCellBg>(hub.boardMaster.cells[i], cellBg.GetComponent<LightbulbCellBg>()));
                cellBg.gameObject.SetActive(true);
                cellBg.GetComponent<LightbulbCellBg>().fill.transform.localScale = temp_cellData.status == 1 ? Vector3.one : Vector3.zero;   
            }
        }
        InitCells_AllCellsStatusUpdate();
    }
    public override void InitTool()
    {
        base.InitTool();
        hub.toolMaster.toolIcon.sprite = lightbulbHub.bulbToolSprite;
    }
    public override void HandlePlayerInput(Vector2Int coord)
    {
        Vector2Int numberRange = new Vector2Int(1, 4);
        //cell number rule
        //lv 1
        //cell +1
        if (levelData.levelIndex >= 1 && levelData.levelIndex <= 2)
        {
            for (int i = 0; i < levelData.curBoard.cells.Count; i++)
            {
                if (levelData.curBoard.cells[i].coord == coord)
                {
                    levelData.curBoard.cells[i].value += 1;
                    levelData.curBoard.cells[i].value = BoardCalculation.ModX_Range(levelData.curBoard.cells[i].value, numberRange);
                }

            }
        }
        //lv 3+
        //cross cells +1
        else if (levelData.levelIndex >= 3 && levelData.levelIndex <= 8)
        {
            for (int i = 0; i < levelData.curBoard.cells.Count; i++)
            {
                if (BoardCalculation.Manhattan_Dist(levelData.curBoard.cells[i].coord, coord) <= 1)
                {
                    levelData.curBoard.cells[i].value += 1;
                    levelData.curBoard.cells[i].value = BoardCalculation.ModX_Range(levelData.curBoard.cells[i].value, numberRange);
                }
            }
        }
        else
        {
            Debug.LogError(string.Format("master script of {0} reaches undefined level", levelData.theme));
        }
        
    }
    public override void HandleEnvironment(Vector2Int coord)
    {
        Count_SwictchToOn = 0;
        int LightOnReq = 2;
        //double pass on cells to update light on/off
        for (int i = 0; i < levelData.curBoard.cells.Count; i++)
        {
            int SameCount = 0;
            for (int j = 0; j < levelData.curBoard.cells.Count; j++)
            {
                if (BoardCalculation.Manhattan_Dist(levelData.curBoard.cells[i].coord, levelData.curBoard.cells[j].coord) == 1)
                {
                    if (levelData.curBoard.cells[i].value == levelData.curBoard.cells[j].value)
                    {
                        SameCount += 1;
                    }
                }
            }
            levelData.curBoard.cells[i].status = SameCount >= LightOnReq ? 1 : 0;
            if(levelData.previousBoard.cells[i].status == 0 && levelData.curBoard.cells[i].status == 1)
            {
                Count_SwictchToOn += 1;
            }
        }
    }
    float POWER_SPREAD_DURATION = .3f;
    float VFX_INTERVAL_DURATION = .0f;
    float LIGHT_SWITCH_DURATION = .2f;
    public override void UpdateCells(Vector2Int coord)
    {
        Sequence seq = DOTween.Sequence();
        seq.AppendCallback(() => UpdateCells_PlayCellValueUpdate(coord));
        seq.AppendInterval(POWER_SPREAD_DURATION);
        if(levelData.levelIndex >= 3 && levelData.levelIndex <= 8)
        {
            seq.AppendCallback(() => UpdateCells_AdjacentCellsValueUpdate(coord));
            seq.AppendInterval(VFX_INTERVAL_DURATION);
        } 
        seq.AppendCallback(() => UpdateCells_AllCellsStatusUpdate());
    }
    void UpdateCells_PlayCellValueUpdate(Vector2Int coord)
    {
        for (int i = 0; i < hub.boardMaster.cells.Count; i++)
        {
            if (hub.boardMaster.cells[i].coord == coord)
            {
                DataCell temp_cellData = levelData.curBoard.GetCellDataByCoord(hub.boardMaster.cells[i].coord);
                if (temp_cellData.value != hub.boardMaster.cells[i].curNumber)
                {
                    hub.boardMaster.cells[i].NumberShift(temp_cellData.value);
                }

            }
            else if (BoardCalculation.Manhattan_Dist(hub.boardMaster.cells[i].coord, coord) == 1 && levelData.levelIndex >= 3 && levelData.levelIndex <= 8)
            {
                VFX_GetElectricity(coord, hub.boardMaster.cells[i].coord, hub.boardMaster.cells[i].transform);
            }
        }
    }
    void UpdateCells_AdjacentCellsValueUpdate(Vector2Int coord)
    {
        for (int i = 0; i < hub.boardMaster.cells.Count; i++)
        {
            if (BoardCalculation.Manhattan_Dist(hub.boardMaster.cells[i].coord, coord) == 1)
            {
                DataCell temp_cellData = levelData.curBoard.GetCellDataByCoord(hub.boardMaster.cells[i].coord);
                if (temp_cellData.value != hub.boardMaster.cells[i].curNumber)
                {
                    hub.boardMaster.cells[i].NumberShift(temp_cellData.value);
                }
            }
        }
    }
    void VFX_GetElectricity(Vector2Int origin, Vector2Int target, Transform localTrans)
    {
        float MOVE_DISTANCE = 1f;
        //Debug.Log(string.Format("VFX_GetElectricity() called on ({0}) to ({1})", origin, target));
        GameObject elec = Instantiate(lightbulbHub.electricity, localTrans);
        
        Vector3 eulerAngle = Vector3.zero;
        if(origin.x - target.x > 0)
        {
            eulerAngle = new Vector3(0f, 0f, 90f);
        }
        else if(origin.x - target.x < 0)
        {
            eulerAngle = new Vector3(0f, 0f, -90f);
        }
        else if(origin.y - target.y < 0)
        {
            eulerAngle = new Vector3(0f, 0f, 180f);
        }
        else
        {
            eulerAngle = new Vector3(0f, 0f, 0f);
        }
        elec.transform.localEulerAngles = eulerAngle;
        elec.GetComponent<SpriteRenderer>().DOFade(0f, POWER_SPREAD_DURATION).From();
        elec.transform.DOLocalMove(MOVE_DISTANCE * new Vector3((origin.x - target.x), (origin.y - target.y), 0f), POWER_SPREAD_DURATION).From().OnComplete(()=>Destroy(elec));
    }
    void UpdateCells_AllCellsStatusUpdate()
    {
        bool hasSwitch = false;
        for (int i = 0; i < lightbulbHub.lightBulbs.Count; i++)
        {
            DataCell prev_cellData = levelData.previousBoard.GetCellDataByCoord(lightbulbHub.lightBulbs[i].Key.coord);
            DataCell temp_cellData = levelData.curBoard.GetCellDataByCoord(lightbulbHub.lightBulbs[i].Key.coord);
            if(prev_cellData.status != temp_cellData.status)
            {
                VFX_LightSwitch(lightbulbHub.lightBulbs[i], temp_cellData.status == 1);
                hasSwitch = true;
            }
        }
        if (hasSwitch)
        {
            AudioDraft.singleton.PlaySFX(lightbulbHub.GetSwitchClip(Random.Range(0, 2)));
        }
    }
    void InitCells_AllCellsStatusUpdate()
    {
        //bool hasSwitch = false;
        for (int i = 0; i < lightbulbHub.lightBulbs.Count; i++)
        {
            
            DataCell temp_cellData = levelData.curBoard.GetCellDataByCoord(lightbulbHub.lightBulbs[i].Key.coord);
            VFX_LightSwitch(lightbulbHub.lightBulbs[i], temp_cellData.status == 1);
        }
    }
    void VFX_LightSwitch(KeyValuePair<CellMaster, LightbulbCellBg> taregtCell, bool isOn)
    {
        //Debug.Log(string.Format("VFX_LightSwitch() called on cell ({0}) set to ({1})", taregtCell.Key.coord, isOn ? "On" : "Off"));
        if (isOn)
        {
            taregtCell.Key.SetColor(dConstants.UI.DefaultColor_Black, LIGHT_SWITCH_DURATION);
            taregtCell.Value.fill.DOFade(1f, LIGHT_SWITCH_DURATION);
            taregtCell.Value.fill.transform.localScale = Vector3.one;
            taregtCell.Value.fill.transform.DOScale(0f, LIGHT_SWITCH_DURATION).From();
        }
        else
        {
            taregtCell.Key.SetColor(Color.white, LIGHT_SWITCH_DURATION);
            taregtCell.Value.fill.DOFade(0f, LIGHT_SWITCH_DURATION);
            taregtCell.Value.fill.transform.localScale = Vector3.zero;
            taregtCell.Value.fill.transform.DOScale(1f, LIGHT_SWITCH_DURATION).From();
        }
    }
    private bool narrative_lv4_1 = false;
    private bool narrative_lv7_1 = false;
    private bool narrative_lv8_1 = false;
    public override void AddtionalUpdate_Theme(Vector2Int coord)
    {
        //in-play narrative
        if (!narrative_lv4_1 && levelData.levelIndex == 4 && BoardCalculation.CountStatusX(levelData.curBoard, 1) >= 2)
        {
            narrative_lv4_1 = true;
            TryTypeNextPlayLine(0);
        }
        if (!narrative_lv7_1 && levelData.levelIndex == 7 && levelData.curBoard.toolCount <= 5)
        {
            narrative_lv7_1 = true;
            TryTypeNextPlayLine(0);
        }
        if (!narrative_lv8_1 && levelData.levelIndex == 8 && BoardCalculation.CountStatusX(levelData.curBoard, 1) >= 12)
        {
            narrative_lv8_1 = true;
            TryTypeNextPlayLine(0);
        }
    }
    public override bool CheckWinCondition()
    {
        /*
        点亮1盏灯泡
        点亮5盏灯泡
        点亮4盏灯泡
        点亮6盏灯泡
        一次性点亮4盏灯泡
        一次性点亮8盏灯泡
        用完所有灯泡，<br>保持点亮的灯泡<br>在4-6盏之间
        点亮全部灯泡
        */
        bool result = false;
        if (levelData.levelIndex == 1)
        {
            return BoardCalculation.CountStatusX_Ytimes(levelData.curBoard, 1, 1);
        }
        else if (levelData.levelIndex == 2)
        {
            return BoardCalculation.CountStatusX_Ytimes(levelData.curBoard, 1, 5);
        }
        else if (levelData.levelIndex == 3)
        {
            return BoardCalculation.CountStatusX_Ytimes(levelData.curBoard, 1, 4);
        }
        else if (levelData.levelIndex == 4)
        {
            return BoardCalculation.CountStatusX_Ytimes(levelData.curBoard, 1, 6);
        }
        else if (levelData.levelIndex == 5)
        {
            return Count_SwictchToOn >= 4;
        }
        else if (levelData.levelIndex == 6)
        {
            return Count_SwictchToOn >= 8;
        }
        else if (levelData.levelIndex == 7)
        {
            return levelData.curBoard.toolCount == 0;
        }
        else if (levelData.levelIndex == 8)
        {
            return BoardCalculation.CountStatusX_All(levelData.curBoard, 1);
        }
        else
        {
            Debug.LogError(string.Format("reach undefined level in CheckWinCondition of ({0})", levelData.theme));
        }
        return result;
    }
    public override bool CheckLoseCondition()
    {
        if (levelData.curBoard.toolCount == 0)
        {
            return true;
        }
        int lightOnCount = BoardCalculation.CountStatusX(levelData.curBoard, 1);
        if (levelData.levelIndex == 7)
        {
            if (lightOnCount >= 4 && lightOnCount <= 6)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        return false;
    }
}
