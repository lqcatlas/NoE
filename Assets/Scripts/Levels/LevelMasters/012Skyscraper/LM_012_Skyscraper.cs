using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using static LM_008_Crown;
using UnityEngine.UI;

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
        //clear old bgs
        List<Transform> oldBgs = themeHub.bgHolder.GetComponentsInChildren<Transform>().ToList();
        oldBgs.Remove(themeHub.bgHolder.transform);
        for (int i = 0; i < oldBgs.Count; i++)
        {
            Destroy(oldBgs[i].gameObject);
        }
        themeHub.bgHolder.transform.localScale = hub.boardMaster.cellHolder.localScale;
        //set existing circles
        for (int i = 0; i < hub.boardMaster.cells.Count; i++)
        {
            DataCell temp_cellData = levelData.curBoard.GetCellDataByCoord(hub.boardMaster.cells[i].coord);
            GameObject obj = Instantiate(themeHub.bldgTemplate, themeHub.bgHolder);
            //obj.GetComponent<CellChoice_Badge>().SetToCorrect(temp_cellData.status == (int)CellStatus.found);
            obj.transform.position = hub.boardMaster.cells[i].transform.position;
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
    public override void AddtionalUpdate_Theme(Vector2Int coord)
    {
        //play population move fx + numbershift

        //then play popluation boom + numbershift

        //update bg bldg sprite
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
            return BoardCalculation.CountXplus_Ytimes(levelData.curBoard, 30, 3);
        }
        else if (levelData.levelIndex == 8)
        {
            return BoardCalculation.CountXplus_Ytimes(levelData.curBoard, 30, 5);
        }
        else
        {
            Debug.LogError(string.Format("reach undefined level in CheckWinCondition of ({0})", levelData.theme));
        }
        return result;
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
