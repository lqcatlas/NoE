using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LM_011_Clover : LevelMasterBase
{
    enum CellStatus { regular = 0, special = 1, found = 2, wrong = 3 };

    [Header("Theme Additions")]
    public LMHub_011_Clover themeHub;

    private bool wrongSelection;

    public override void GetObjectReferences(GameObject _themeHub)
    {
        base.GetObjectReferences(null);
        themeHub = _themeHub.GetComponent<LMHub_011_Clover>();
    }
    public override void AdditionalGenerateBoard_Theme()
    {
        
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
            if (temp_cellData.status == (int)CellStatus.found)
            {
                GameObject obj = Instantiate(themeHub.drawingTemplate, themeHub.bgHolder);
                obj.GetComponent<CellChoice_Badge>().SetToCorrect(temp_cellData.status == (int)CellStatus.found);
                obj.transform.position = hub.boardMaster.cells[i].transform.position;
            }
        }
        //set each cell based on status
        for (int i = 0; i < hub.boardMaster.cells.Count; i++)
        {
            DataCell temp_cellData = levelData.curBoard.GetCellDataByCoord(hub.boardMaster.cells[i].coord);
            SetRegularCloverCell(hub.boardMaster.cells[i]);
            if (temp_cellData != null)
            {
                switch (temp_cellData.status)
                {
                    case (int)CellStatus.regular:
                        hub.boardMaster.cells[i].SetCellInteractable(true);
                        break;
                    case (int)CellStatus.special:
                        //set to special adv sprite slider
                        InsertSpecialCloverSprt(hub.boardMaster.cells[i]);
                        hub.boardMaster.cells[i].SetCellInteractable(true);
                        break;
                    case (int)CellStatus.found:
                        //set to special adv sprite slider
                        InsertSpecialCloverSprt(hub.boardMaster.cells[i]);
                        hub.boardMaster.cells[i].SetCellInteractable(false);
                        break;
                    case (int)CellStatus.wrong:
                        //set to regular adv sprite slider
                        InsertSpecialCloverSprt(hub.boardMaster.cells[i]);
                        hub.boardMaster.cells[i].SetCellInteractable(false);
                        break;
                    default:
                        //wrong status, hide
                        hub.boardMaster.cells[i].gameObject.SetActive(false);
                        break;
                }
            }
        }
    }
    public override void InitTool()
    {
        base.InitTool();
        hub.toolMaster.toolIcon.sprite = themeHub.toolSprite;
    }
    public override void HandlePlayerInput(Vector2Int coord)
    {
        wrongSelection = false;
        if(levelData.curBoard.GetCellDataByCoord(coord).status == (int)CellStatus.special)
        {
            levelData.curBoard.GetCellDataByCoord(coord).status = (int)CellStatus.found;
        }
        else
        {
            wrongSelection = true; 
        }
    }
    public override void UpdateCells(Vector2Int coord)
    {
        DataCell temp_cellData = levelData.curBoard.GetCellDataByCoord(coord);
        for (int i = 0; i < hub.boardMaster.cells.Count; i++)
        {
            if (hub.boardMaster.cells[i].coord == coord)
            {
                GameObject obj = Instantiate(themeHub.drawingTemplate, themeHub.bgHolder);
                obj.GetComponent<CellChoice_Badge>().SetToCorrect(temp_cellData.status == (int)CellStatus.found);
                obj.transform.position = hub.boardMaster.cells[i].transform.position;
                hub.boardMaster.cells[i].SetCellInteractable(false);
                AudioCentralCtrl.singleton.PlaySFX(themeHub.circleClips.GetClip());
            }
        }
    }
    public override bool CheckWinCondition()
    {
        if (wrongSelection)
        {
            return false;
        }
        else if (levelData.levelIndex >= 1)
        {
            return levelData.curBoard.toolCount == 0;
        }
        else
        {
            Debug.LogError(string.Format("reach undefined level in CheckWinCondition of ({0})", levelData.theme));
            return false;
        }
    }
    public override bool CheckLoseCondition()
    {
        if (wrongSelection)
        {
            //lose if any choice is wrong
            return true;
        }
        return false;
    }
    void SetRegularCloverCell(CellMaster cell)
    {
        /*int sprtCount = cell.numberInSprites.Count;
        for (int i = 0; i < sprtCount; i++) 
        {
            cell.numberInSprites[i].GetComponent<AdvSpriteSlider>().SwitchByOrder = true;
            cell.numberInSprites[i].GetComponent<AdvSpriteSlider>().triggerChance = 1f;
        }
        */
    }
    void InsertSpecialCloverSprt(CellMaster cell)
    {
        int sprtCount = cell.numberInSprites.Count;
        for (int i = 0; i < sprtCount; i++)
        {
            Sprite curSprite = cell.numberInSprites[i].sprite;
            int locator = curSprite.name.IndexOf("@");
            string cloverSpriteName = curSprite.name.Substring(0, locator) + "_clover_" + levelData.levelIndex;
            Debug.Log($"insert sprite name is {cloverSpriteName}");
            Sprite cloverSprite = Resources.LoadAll<Sprite>("sprites/spritesheet/spritesheet_level").FirstOrDefault(f => f.name == cloverSpriteName);
            if(cloverSprite != null)
            {
                cell.numberInSprites[i].GetComponent<AdvSpriteSlider>().availableSprites.Add(cloverSprite);
            }
            else
            {
                Debug.LogError($"unable to find sprite name ({cloverSpriteName})");
            }
        }
    }
}
