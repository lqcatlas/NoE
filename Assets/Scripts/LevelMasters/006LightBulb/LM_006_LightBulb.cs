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
        //TBD
        // cell +1 etc.
    }
    public override void HandleEnvironment(Vector2Int coord)
    {
        //TBD
        //calculate cell status(light up or not)
    }
    public override void UpdateCells(Vector2Int coord)
    {
        base.UpdateCells(coord);
        //cell bg update addition
        for (int i = 0; i < lightbulbHub.lightBulbs.Count; i++)
        {
            if (lightbulbHub.lightBulbs[i].Key.coord == coord)
            {
                //lightbulbHub.lightBulbs[i].Value.SetActive(true);
                DataCell prev_cellData = levelData.previousBoard.GetCellDataByCoord(lightbulbHub.lightBulbs[i].Key.coord);
                DataCell temp_cellData = levelData.curBoard.GetCellDataByCoord(lightbulbHub.lightBulbs[i].Key.coord);
                lightbulbHub.lightBulbs[i].Value.GetComponent<SpriteRenderer>().sprite = lightbulbHub.bulbSprites[temp_cellData.status];

                //TBD light on/off VFX
                //FoodPlaceAnimation(sushiHub.sushiPlates[i].Value, prev_cellData.status, levelData.curBoard.toolStatus, temp_cellData.status);
                //Debug.Log(string.Format("food animation input params {0},{1},{2}", prev_cellData.status, levelData.curBoard.toolStatus, temp_cellData.status));
                break;
            }
        }
    }
    public override bool CheckWinCondition()
    {
        /*
        将日历设为8月10日
        让某一行成为8月13日
        让某一行和某一列成为8月15日
        让所有格子成为相同数字
        让所有格子成为不同数字
        经历3次完整月相
        让所有格子为6
        经历两次月食后，让所有格子回到最初值
         */
        bool result = false;
        if (levelData.levelIndex == 1)
        {
            List<int> targetLineNumber = new List<int>() { 0, 8, 1, 0 };
            return BoardCalculation.ExactLineMatchX(levelData.curBoard, targetLineNumber);
        }
        else if (levelData.levelIndex == 2)
        {
            List<int> targetLineNumber = new List<int>() { 0, 8, 1, 3 };
            return BoardCalculation.ExactLineMatchX(levelData.curBoard, targetLineNumber);
        }
        else if (levelData.levelIndex == 3)
        {
            List<int> targetNumber = new List<int>() { 0, 8, 1, 5 };
            return BoardCalculation.ExactLineMatchX(levelData.curBoard, targetNumber) || BoardCalculation.ExactRowMatchX(levelData.curBoard, targetNumber);
        }
        else if (levelData.levelIndex == 4)
        {
            return BoardCalculation.Same_All(levelData.curBoard);
        }
        else if (levelData.levelIndex == 5)
        {
            return BoardCalculation.Unique_All(levelData.curBoard);
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
