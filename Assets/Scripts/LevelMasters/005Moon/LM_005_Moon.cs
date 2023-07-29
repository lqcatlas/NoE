using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LM_005_Moon : LevelMasterBase
{
    enum MoonPhase : int { none = 0, crescent_1 = 1, quarter_1 = 2, full = 3, quarter_2 = 4, crescent_2 = 5 };

    [Header("Theme Additions")]
    public LMHub_005_Moon moonHub;
    public int fullPhaseCount = 0;;

    [Header("Theme Animation Params")]
    float CellFoodTrasitionDistance = 1f;
    float CellFoodTrasitionDuration = 1f;
    float originalFoodAlpha = 0.4f;

    public override void GetObjectReferences(GameObject _themeHub)
    {
        base.GetObjectReferences(null);
        moonHub = _themeHub.GetComponent<LMHub_005_Moon>();
    }

    public override void InitTool()
    {
        base.InitTool();
        hub.toolMaster.toolIcon.sprite = moonHub.statusSprites[levelData.curBoard.toolStatus];
        fullPhaseCount = 0;
        //moon phase tablet
    }
    public override void HandlePlayerInput(Vector2Int coord)
    {
        Vector2Int numberRange_lv1 = new Vector2Int(0, 9);
        Vector2Int numberRange = new Vector2Int(0, 10000);
        //cell number rule
        //lv 1
        //crescent +1, quarter +3
        if (levelData.levelIndex == 1)
        {
            for (int i = 0; i < levelData.curBoard.cells.Count; i++)
            {
                if (levelData.curBoard.cells[i].coord == coord)
                {
                    if(levelData.curBoard.toolStatus == (int)MoonPhase.crescent_1 || levelData.curBoard.toolStatus == (int)MoonPhase.crescent_2)
                    {
                        levelData.curBoard.cells[i].value += 1;
                        levelData.curBoard.cells[i].value = BoardCalculation.ModX_Range(levelData.curBoard.cells[i].value, numberRange_lv1);
                        //Play sfx ?
                    }
                    else if (levelData.curBoard.toolStatus == (int)MoonPhase.quarter_1 || levelData.curBoard.toolStatus == (int)MoonPhase.quarter_2)
                    {
                        levelData.curBoard.cells[i].value += 3;
                        levelData.curBoard.cells[i].value = BoardCalculation.ModX_Range(levelData.curBoard.cells[i].value, numberRange_lv1);
                    }
                }

            }
        }     
        //lv 2+
        //crescent +1, quarter +3, full +5
        else if (levelData.levelIndex >= 2)
        {
            for (int i = 0; i < levelData.curBoard.cells.Count; i++)
            {
                if (levelData.curBoard.cells[i].coord == coord)
                {
                    if (levelData.curBoard.toolStatus == (int)MoonPhase.crescent_1 || levelData.curBoard.toolStatus == (int)MoonPhase.crescent_2)
                    {
                        levelData.curBoard.cells[i].value += 1;
                        levelData.curBoard.cells[i].value = BoardCalculation.ModX_Range(levelData.curBoard.cells[i].value, numberRange);
                        //Play sfx ?
                    }
                    else if (levelData.curBoard.toolStatus == (int)MoonPhase.quarter_1 || levelData.curBoard.toolStatus == (int)MoonPhase.quarter_2)
                    {
                        levelData.curBoard.cells[i].value += 3;
                        levelData.curBoard.cells[i].value = BoardCalculation.ModX_Range(levelData.curBoard.cells[i].value, numberRange);
                    }
                    else if (levelData.curBoard.toolStatus == (int)MoonPhase.full)
                    {
                        levelData.curBoard.cells[i].value += 5;
                        levelData.curBoard.cells[i].value = BoardCalculation.ModX_Range(levelData.curBoard.cells[i].value, numberRange);
                    }
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
        //lv 1
        //crescent/quarter alternate
        if (levelData.levelIndex == 1)
        {
            if(levelData.curBoard.toolStatus == (int)MoonPhase.crescent_1)
            {
                levelData.curBoard.toolStatus = (int)MoonPhase.quarter_1;
            }
            else if(levelData.curBoard.toolStatus == (int)MoonPhase.quarter_1)
            {
                levelData.curBoard.toolStatus = (int)MoonPhase.crescent_1;
                fullPhaseCount += 1;
            }
        }
        //lv 2-8
        //phase rotate
        else if (levelData.levelIndex >= 2)
        {
            levelData.curBoard.toolStatus = levelData.curBoard.toolStatus + 1;
            if(levelData.curBoard.toolStatus == 6)
            {
                levelData.curBoard.toolStatus = (int)MoonPhase.crescent_1;
                fullPhaseCount += 1;
            }
        }
        else
        {
            Debug.LogError(string.Format("master script of {0} reaches undefined level", levelData.theme));
        }
        
    }
    public override void UpdateTool(Vector2Int coord)
    {
        base.UpdateTool(coord);
        //moon phase tablet rotate
        hub.toolMaster.toolIcon.sprite = moonHub.statusSprites[levelData.curBoard.toolStatus];
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
            return fullPhaseCount >= 3;
        }
        else if (levelData.levelIndex == 7)
        {
            return BoardCalculation.CountX_All(levelData.curBoard, 6);
        }
        else if (levelData.levelIndex == 8)
        {
            return (fullPhaseCount >= 2 && BoardCalculation.CountX_All(levelData.curBoard, 2));
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
        else if(BoardCalculation.CountXplus_Ytimes(levelData.curBoard, 9, 1))
        {
            return true;
        }
        return false;
    }
    public override void LoseALevel()
    {
        
    }
}
