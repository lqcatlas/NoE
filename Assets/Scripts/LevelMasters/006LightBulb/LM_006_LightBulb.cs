using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LM_006_LightBulb : LevelMasterBase
{
    enum BulbStatus : int { on = 0, off = 1};
    [Header("Theme Additions")]
    public LMHub_006_LightBulb lightbulbHub;

    public override void GetObjectReferences(GameObject _themeHub)
    {
        base.GetObjectReferences(null);
        lightbulbHub = _themeHub.GetComponent<LMHub_006_LightBulb>();
        //init theme-specific params
    }
    public override void InitCells()
    {
        lightbulbHub.lightBulbs = new List<KeyValuePair<CellMaster, GameObject>>();
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
                hub.boardMaster.cells[i].numberTxt.SetText(temp_cellData.value.ToString());
                hub.boardMaster.cells[i].numberTxt.fontSize = 9;
                hub.boardMaster.cells[i].numberTxt.color = Color.white;
                //additional
                GameObject plateBg = Instantiate(lightbulbHub.bulbBgTemplate, lightbulbHub.cellBgHolder);
                plateBg.transform.position = hub.boardMaster.cells[i].transform.position;
                //clockBg.transform.localScale = hub.boardMaster.cells[i].transform.localScale;
                lightbulbHub.lightBulbs.Add(new KeyValuePair<CellMaster, GameObject>(hub.boardMaster.cells[i], plateBg));
                plateBg.gameObject.SetActive(true);
                plateBg.GetComponent<SpriteRenderer>().sprite = lightbulbHub.bulbSprites[temp_cellData.status];
            }
        }
    }
    public override void InitTool()
    {
        base.InitTool();
        hub.toolMaster.toolIcon.sprite = lightbulbHub.bulbToolSprite;
    }
    public override void HandlePlayerInput(Vector2Int coord)
    {
        Vector2Int numberRange = new Vector2Int(1, 5);
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
        }
    }
    public override void UpdateCells(Vector2Int coord)
    {
        base.UpdateCells(coord);
        //cell bg update addition
        for (int i = 0; i < lightbulbHub.lightBulbs.Count; i++)
        {
            DataCell prev_cellData = levelData.previousBoard.GetCellDataByCoord(lightbulbHub.lightBulbs[i].Key.coord);
            DataCell temp_cellData = levelData.curBoard.GetCellDataByCoord(lightbulbHub.lightBulbs[i].Key.coord);
            lightbulbHub.lightBulbs[i].Value.GetComponent<SpriteRenderer>().sprite = lightbulbHub.bulbSprites[temp_cellData.status];
            lightbulbHub.lightBulbs[i].Key.numberTxt.color = temp_cellData.status == 0 ? Color.white : dConstants.UI.BackgroundDefaultBalck;
            //VFX TBD
        }
    }
    public override bool CheckWinCondition()
    {
        /*
        点亮2盏灯泡
        点亮5盏灯泡
        点亮5盏灯泡
        点亮全部灯泡
        熄灭全部灯泡
        只点亮角落里的灯泡
        同一时间点亮5盏灯泡
        同时点亮全部灯泡
        */
        bool result = false;
        if (levelData.levelIndex == 1)
        {
            return BoardCalculation.CountStatusX_Ytimes(levelData.curBoard, 1, 2);
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
            return BoardCalculation.CountStatusX_All(levelData.curBoard, 1);
        }
        else if (levelData.levelIndex == 5)
        {
            return BoardCalculation.CountStatusX_All(levelData.curBoard, 0);
        }
        else if (levelData.levelIndex == 6)
        {
            return false;
        }
        else if (levelData.levelIndex == 7)
        {
            return false;
        }
        else if (levelData.levelIndex == 8)
        {
            return false;
        }
        else
        {
            Debug.LogError(string.Format("reach undefined level in CheckWinCondition of ({0})", levelData.theme));
        }
        return result;
    }
}
