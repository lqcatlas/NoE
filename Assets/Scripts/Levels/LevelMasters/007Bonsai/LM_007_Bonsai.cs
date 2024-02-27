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
        //resize tree trunk sprite based on board size
        //5 = 1.65f, 4 = 1.4f
        if(levelData.initBoard.boardSize == new Vector2(5, 5))
        {
            bonsaiHub.trunk.transform.localScale = Vector3.one * 1.65f;
        }
        else
        {
            bonsaiHub.trunk.transform.localScale = Vector3.one * 1.4f;
        }
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
                    bonsaiLeaf.transform.localScale = Vector3.one * 1.5f * Mathf.Pow(0.9f, 9-temp_cellData.value);
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
        for (int i = 0; i < bonsaiHub.leaves.Count; i++)
        {
            if(bonsaiHub.leaves[i].Key.coord == coord)
            {
                DataCell temp_cellData = levelData.curBoard.GetCellDataByCoord(bonsaiHub.leaves[i].Key.coord);
                if (temp_cellData.status == -1)
                {
                    bonsaiHub.leaves[i].Key.SetCellActive(false);
                    VFX_LeafFall(bonsaiHub.leaves[i].Value);
                }
            }
        }
    }
    void VFX_LeafFall(GameObject fallingLeaf)
    {
        float fallingAnimTime = 1f;
        Vector2 fallingDist = new Vector2(2, -8);
        float tiltingDegree = -30f;
        GameObject leaf = Instantiate(fallingLeaf, bonsaiHub.cellBgHolder);
        fallingLeaf.gameObject.SetActive(false);
        leaf.gameObject.SetActive(true);
        leaf.transform.DOMoveY(fallingDist.y, fallingAnimTime).SetRelative(true).SetEase(Ease.OutQuad);
        leaf.transform.DOMoveX(fallingDist.x, fallingAnimTime).SetRelative(true).SetEase(Ease.Linear);
        leaf.transform.DORotate(new Vector3(0f, 0f, tiltingDegree), fallingAnimTime).SetRelative(true).SetEase(Ease.OutQuad);
        //leaf.transform.DOScale(0.2f, fallingAnimTime).SetRelative(true).SetEase(Ease.OutQuad);
        leaf.GetComponentInChildren<SpriteRenderer>().DOFade(0f, fallingAnimTime).SetEase(Ease.OutQuad).OnComplete(()=>Destroy(leaf));

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
            DataCell temp_cellData = levelData.curBoard.GetCellDataByCoord(new Vector2Int(1, 2));
            return temp_cellData.status == -1;
        }
        else if (levelData.levelIndex == 2)
        {
            DataCell temp_cellData = levelData.curBoard.GetCellDataByCoord(new Vector2Int(2, 2));
            return temp_cellData.status == -1;
        }
        else if (levelData.levelIndex == 3)
        {
            DataCell temp_cellData = levelData.curBoard.GetCellDataByCoord(new Vector2Int(3, 3));
            return temp_cellData.status == -1;
        }
        else if (levelData.levelIndex == 4)
        {
            DataCell temp_cellData = levelData.curBoard.GetCellDataByCoord(new Vector2Int(1, 2));
            return temp_cellData.status == -1;
        }
        else if (levelData.levelIndex == 5)
        {
            DataCell temp_cellData = levelData.curBoard.GetCellDataByCoord(new Vector2Int(3, 2));
            return temp_cellData.status == -1;
        }
        else if (levelData.levelIndex == 6)
        {
            DataCell temp_cellData = levelData.curBoard.GetCellDataByCoord(new Vector2Int(2, 1));
            return temp_cellData.status == -1;
        }
        else if (levelData.levelIndex == 7)
        {
            DataCell temp_cellData = levelData.curBoard.GetCellDataByCoord(new Vector2Int(2, 2));
            return temp_cellData.status == -1;
        }
        else if (levelData.levelIndex == 8)
        {
            DataCell temp_cellData = levelData.curBoard.GetCellDataByCoord(new Vector2Int(0, 2));
            return temp_cellData.status == -1;
        }
        else if (levelData.levelIndex == 9)
        {
            DataCell temp_cellData = levelData.curBoard.GetCellDataByCoord(new Vector2Int(3, 3));
            return temp_cellData.status == -1;
        }
        else if (levelData.levelIndex == 10)
        {
            DataCell temp_cellData = levelData.curBoard.GetCellDataByCoord(new Vector2Int(4, 2));
            return temp_cellData.status == -1;
        }
        else if (levelData.levelIndex == 11)
        {
            DataCell temp_cellData = levelData.curBoard.GetCellDataByCoord(new Vector2Int(2, 1));
            return temp_cellData.status == -1;
        }
        else if (levelData.levelIndex == 12)
        {
            DataCell temp_cellData1 = levelData.curBoard.GetCellDataByCoord(new Vector2Int(3, 2));
            DataCell temp_cellData2 = levelData.curBoard.GetCellDataByCoord(new Vector2Int(1, 3));
            return temp_cellData1.status == -1 || temp_cellData2.status == -1;
        }
        else if (levelData.levelIndex == 13)
        {
            DataCell temp_cellData = levelData.curBoard.GetCellDataByCoord(new Vector2Int(4, 1));
            return temp_cellData.status == -1;
        }
        else if (levelData.levelIndex == 14)
        {
            DataCell temp_cellData = levelData.curBoard.GetCellDataByCoord(new Vector2Int(3, 2));
            return temp_cellData.status == -1;
        }
        else if (levelData.levelIndex == 15)
        {
            DataCell temp_cellData = levelData.curBoard.GetCellDataByCoord(new Vector2Int(1, 2));
            return temp_cellData.status == -1;
        }
        else if (levelData.levelIndex == 16)
        {
            DataCell temp_cellData = levelData.curBoard.GetCellDataByCoord(new Vector2Int(1, 2));
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
