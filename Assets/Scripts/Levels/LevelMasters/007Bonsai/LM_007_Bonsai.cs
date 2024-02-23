using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;

public class LM_007_Bonsai : LevelMasterBase
{
    //enum BulbStatus : int { on = 0, off = 1};
    [Header("Theme Additions")]
    public LMHub_007_Bonsai bonsaiHub;

    //private int Count_SwictchToOn;

    public override void GetObjectReferences(GameObject _themeHub)
    {
        base.GetObjectReferences(null);
        bonsaiHub = _themeHub.GetComponent<LMHub_007_Bonsai>();
        //init theme-specific params
        ThemeAnimationDelayAfterInit = 2f;
        ThemeAnimationDelayAfterPlay = 0.5f;
    }
    public override void GenerateBoard()
    //init the right number of cells and placed them in the right location
    {
        base.GenerateBoard();
        hub.boardMaster.boardHolder.localPosition = new Vector3(2f, 6f, 0f);
    }
    public override void InitCells()
    {
        //display bonsai trunk sprite
        bonsaiHub.trunkHolder.transform.localScale = hub.boardMaster.cellHolder.localScale;
        bonsaiHub.trunk.sprite = bonsaiHub.trunkSprites[levelData.levelIndex - 1];
        //display bonsai leaves
        bonsaiHub.leaves = new List<KeyValuePair<CellMaster, GameObject>>();
        bonsaiHub.cellBgHolder.transform.localScale = hub.boardMaster.cellHolder.localScale;
        List<Transform> oldLeaves = bonsaiHub.cellBgHolder.GetComponentsInChildren<Transform>().ToList();
        oldLeaves.Remove(bonsaiHub.cellBgHolder.transform);
        for (int i = 0; i < oldLeaves.Count; i++)
        {
            Destroy(oldLeaves[i].gameObject);
        }
        for (int i = 0; i < hub.boardMaster.cells.Count; i++)
        {
            DataCell temp_cellData = levelData.curBoard.GetCellDataByCoord(hub.boardMaster.cells[i].coord);
            if (temp_cellData != null)
            {
                if (temp_cellData.status != -1)
                {
                    hub.boardMaster.cells[i].DisplayNumber(temp_cellData.value);
                }
                else
                {
                    hub.boardMaster.cells[i].SetCellActive(false);
                }
                //display bonsai leaves sprite
                GameObject bonsaiLeaf = Instantiate(bonsaiHub.leafTemplate, bonsaiHub.cellBgHolder);
                bonsaiLeaf.transform.position = hub.boardMaster.cells[i].transform.position;
                //clockBg.transform.localScale = hub.boardMaster.cells[i].transform.localScale;
                bonsaiHub.leaves.Add(new KeyValuePair<CellMaster, GameObject>(hub.boardMaster.cells[i], bonsaiLeaf));
                if (temp_cellData.status != -1)
                {
                    bonsaiLeaf.gameObject.SetActive(true);
                    bonsaiLeaf.transform.localScale = Vector3.one * Mathf.Pow(0.9f, 9-temp_cellData.value);
                }
                else
                {
                    bonsaiLeaf.gameObject.SetActive(false);
                }
            }
        }
    }

    public override void InitTool()
    {
        base.InitTool();
        hub.toolMaster.toolIcon.sprite = bonsaiHub.toolSprite;
        UpdateToolStatusDisplay();
    }
    public override void HandlePlayerInput(Vector2Int coord)
    {
        //Vector2Int numberRange = new Vector2Int(1, 4);
        //remove cell
        for (int i = 0; i < levelData.curBoard.cells.Count; i++)
        {
            if (levelData.curBoard.cells[i].coord == coord)
            {
                levelData.curBoard.cells[i].status = -1;
            }
        }
    }
    public override void UpdateCells(Vector2Int coord)
    {
        for (int i = 0; i < hub.boardMaster.cells.Count; i++)
        {
            DataCell temp_cellData = levelData.curBoard.GetCellDataByCoord(hub.boardMaster.cells[i].coord);
            if (temp_cellData != null)
            {
                if (temp_cellData.status == -1)
                {
                    hub.boardMaster.cells[i].SetCellActive(false);
                    //TO DO: cut anim
                }
            }
        }
    }
    public override void AddtionalUpdate_Theme(Vector2Int coord)
    {
        
    }
    public override bool CheckWinCondition()
    {
        /*
        2,3
        2,3
        */
        bool result = false;
        if (levelData.levelIndex == 1)
        {
            DataCell temp_cellData = levelData.curBoard.GetCellDataByCoord(new Vector2Int(1,2));
            return temp_cellData.status == -1;
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
        return false;
    }
    void UpdateToolStatusDisplay()
    {
        ToolStatusGroup targetDisplayTemplate = bonsaiHub.toolStatusGroup;

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
