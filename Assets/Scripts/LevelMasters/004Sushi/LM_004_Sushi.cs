using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class LM_004_Sushi : LevelMasterBase
{
    enum SushiStatus:int { none = 0, fish = 1, rice = 2, sushi = 3, onigiri = 4};

    [Header("Theme Additions")]
    public LMHub_004_Sushi sushiHub;

    [Header("Theme Animation Params")]
    float CellFoodTrasitionDistance = 1f;
    float CellFoodTrasitionDuration = 1f;
    float originalFoodAlpha = 0.4f;

    public override void GetObjectReferences(GameObject _themeHub)
    {
        base.GetObjectReferences(null);
        sushiHub = _themeHub.GetComponent<LMHub_004_Sushi>();
    }
    public override void InitCells()
    {
        sushiHub.sushiPlates = new List<KeyValuePair<CellMaster, GameObject>>();
        sushiHub.cellBgHolder.transform.localScale = hub.boardMaster.cellHolder.localScale;
        //clear old running clocks
        List<Transform> oldBgs = sushiHub.cellBgHolder.GetComponentsInChildren<Transform>().ToList();
        oldBgs.Remove(sushiHub.cellBgHolder.transform);
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
                GameObject plateBg = Instantiate(sushiHub.sushiPlateTemplate, sushiHub.cellBgHolder);
                plateBg.transform.position = hub.boardMaster.cells[i].transform.position;
                //clockBg.transform.localScale = hub.boardMaster.cells[i].transform.localScale;
                sushiHub.sushiPlates.Add(new KeyValuePair<CellMaster, GameObject>(hub.boardMaster.cells[i], plateBg));
                plateBg.gameObject.SetActive(temp_cellData.status != 0);
                plateBg.GetComponent<SpriteRenderer>().sprite = sushiHub.statusSprites[temp_cellData.status];
            }
        }
    }
    public override void InitTool()
    {
        base.InitTool();
        hub.toolMaster.toolIcon.sprite = sushiHub.statusSprites[levelData.curBoard.toolStatus];
    }
    public override void AddtionalInit_Theme()
    {
        if(levelData.levelIndex == 7 || levelData.levelIndex == 8)
        {
            hub.goalMaster.lines[1].SetText(string.Format("{0}{1}", LocalizedAssetLookup.singleton.Translate("@Loc=ui_goal_current_sum@@"), levelData.curBoard.CurrentSum()));
            hub.goalMaster.lines[1].gameObject.SetActive(true);
            LayoutRebuilder.ForceRebuildLayoutImmediate(hub.goalMaster.goalLayout);
        }
    }
    public override void HandlePlayerInput(Vector2Int coord)
    {
        Vector2Int numberRange = new Vector2Int(0, 9);
        //cell number rule
        //lv 1-3
        //fish +3, rice -3
        if (levelData.levelIndex >= 1 && levelData.levelIndex <= 3)
        {
            for (int i = 0; i < levelData.curBoard.cells.Count; i++)
            {
                if (levelData.curBoard.cells[i].coord == coord)
                {
                    levelData.curBoard.cells[i].value += levelData.curBoard.toolStatus == (int)SushiStatus.fish ? 3 : -3;
                    levelData.curBoard.cells[i].value = BoardCalculation.ModX_Range(levelData.curBoard.cells[i].value, numberRange);
                    levelData.curBoard.cells[i].status = levelData.curBoard.toolStatus;
                    //Play sfx
                    AudioDraft.singleton.PlaySFX(sushiHub.GetPlayClip(levelData.curBoard.cells[i].status - 1));
                }
                
            }
        }
        //lv 4
        //fish +3, rice -3, sushi =7
        else if (levelData.levelIndex == 4)
        {
            for (int i = 0; i < levelData.curBoard.cells.Count; i++)
            {
                if (levelData.curBoard.cells[i].coord == coord)
                {
                    //check sushi
                    if(levelData.curBoard.cells[i].status == (int)SushiStatus.rice && levelData.curBoard.toolStatus == (int)SushiStatus.fish)
                    {
                        levelData.curBoard.cells[i].value = 7;
                        levelData.curBoard.cells[i].status = (int)SushiStatus.sushi;

                    }
                    else
                    {
                        levelData.curBoard.cells[i].value += levelData.curBoard.toolStatus == (int)SushiStatus.fish ? 3 : -3;
                        levelData.curBoard.cells[i].value = BoardCalculation.ModX_Range(levelData.curBoard.cells[i].value, numberRange);
                        levelData.curBoard.cells[i].status = levelData.curBoard.toolStatus;
                    }
                    //Play sfx
                    AudioDraft.singleton.PlaySFX(sushiHub.GetPlayClip(levelData.curBoard.cells[i].status - 1));
                }
            }
        }
        //lv 5 - 8
        //fish +3, rice -3, sushi =7, maki =2
        else if (levelData.levelIndex >= 5 && levelData.levelIndex <= 8)
        {
            for (int i = 0; i < levelData.curBoard.cells.Count; i++)
            {
                if (levelData.curBoard.cells[i].coord == coord)
                {
                    //check sushi
                    if (levelData.curBoard.cells[i].status == (int)SushiStatus.rice && levelData.curBoard.toolStatus == (int)SushiStatus.fish)
                    {
                        levelData.curBoard.cells[i].value = 7;
                        levelData.curBoard.cells[i].status = (int)SushiStatus.sushi;
                    }
                    else if (levelData.curBoard.cells[i].status == (int)SushiStatus.fish && levelData.curBoard.toolStatus == (int)SushiStatus.rice)
                    {
                        levelData.curBoard.cells[i].value = 2;
                        levelData.curBoard.cells[i].status = (int)SushiStatus.onigiri;
                    }
                    else
                    {
                        levelData.curBoard.cells[i].value += levelData.curBoard.toolStatus == (int)SushiStatus.fish ? 3 : -3;
                        levelData.curBoard.cells[i].value = BoardCalculation.ModX_Range(levelData.curBoard.cells[i].value, numberRange);
                        levelData.curBoard.cells[i].status = levelData.curBoard.toolStatus;
                    }
                    //Play sfx
                    AudioDraft.singleton.PlaySFX(sushiHub.GetPlayClip(levelData.curBoard.cells[i].status - 1));
                }
            }
        }
        else
        {
            Debug.LogError(string.Format("master script of {0} reaches undefined level", levelData.theme));
        }
        //cell bg update addition
        for (int i = 0; i < sushiHub.sushiPlates.Count; i++)
        {
            if (sushiHub.sushiPlates[i].Key.coord == coord)
            {
                sushiHub.sushiPlates[i].Value.SetActive(true);
                DataCell prev_cellData = levelData.previousBoard.GetCellDataByCoord(sushiHub.sushiPlates[i].Key.coord);
                DataCell temp_cellData = levelData.curBoard.GetCellDataByCoord(sushiHub.sushiPlates[i].Key.coord);
                FoodPlaceAnimation(sushiHub.sushiPlates[i].Value, prev_cellData.status, levelData.curBoard.toolStatus, temp_cellData.status);
                //Debug.Log(string.Format("food animation input params {0},{1},{2}", prev_cellData.status, levelData.curBoard.toolStatus, temp_cellData.status));
                break;
            }
        }
    }
    public override void HandleEnvironment(Vector2Int coord)
    {
        //lv 1-2
        //only fish
        if (levelData.levelIndex >= 1 && levelData.levelIndex <= 2)
        {
            levelData.curBoard.toolStatus = (int)SushiStatus.fish;
        }
        //lv 3-8
        //fish, rice alternate
        else if (levelData.levelIndex >= 3 && levelData.levelIndex <= 8)
        {
            levelData.curBoard.toolStatus = levelData.curBoard.toolStatus == (int)SushiStatus.fish ? (int)SushiStatus.rice : (int)SushiStatus.fish;
        }
        else
        {
            Debug.LogError(string.Format("master script of {0} reaches undefined level", levelData.theme));
        }
    }
    public override void UpdateTool(Vector2Int coord)
    {
        base.UpdateTool(coord);
        hub.toolMaster.toolIcon.sprite = sushiHub.statusSprites[levelData.curBoard.toolStatus];
    }
    private bool narrative_lv2_1 = false;
    private bool narrative_lv3_1 = false;
    private bool narrative_lv4_1 = false;
    private bool narrative_lv5_1 = false;
    private bool narrative_lv8_1 = false;
    public override void AddtionalUpdate_Theme(Vector2Int coord)
    {
        //update current goal status
        if (levelData.levelIndex == 7 || levelData.levelIndex == 8)
        {
            hub.goalMaster.lines[1].SetText(string.Format("{0}{1}", LocalizedAssetLookup.singleton.Translate("@Loc=ui_goal_current_sum@@"), levelData.curBoard.CurrentSum()));
            hub.goalMaster.lines[1].gameObject.SetActive(true);
            LayoutRebuilder.ForceRebuildLayoutImmediate(hub.goalMaster.goalLayout);
        }

        
        //in-play narrative
        if (!narrative_lv2_1 && levelData.levelIndex == 2 && BoardCalculation.CountX_Ytimes(levelData.curBoard, 5, 4))
        {
            narrative_lv2_1 = true;
            TryTypeNextPlayLine(0);
        }
        if (!narrative_lv3_1 && levelData.levelIndex == 3 && BoardCalculation.CountX_Ytimes(levelData.curBoard, 4, 7))
        {
            narrative_lv3_1 = true;
            TryTypeNextPlayLine(0);
        }
        if (!narrative_lv4_1 && levelData.levelIndex == 4 && BoardCalculation.CountStatusX_Ytimes(levelData.curBoard, (int)SushiStatus.sushi, 1))
        {
            narrative_lv4_1 = true;
            TryTypeNextPlayLine(0);
        }
        if (!narrative_lv5_1 && levelData.levelIndex == 5 && BoardCalculation.CountStatusX_Ytimes(levelData.curBoard, (int)SushiStatus.onigiri, 1))
        {
            narrative_lv5_1 = true;
            TryTypeNextPlayLine(0);
        }
        if (!narrative_lv8_1 && levelData.levelIndex == 8 && levelData.curBoard.toolCount == 10)
        {
            narrative_lv8_1 = true;
            TryTypeNextPlayLine(0);
        }
        
    }
    public override bool CheckWinCondition()
    {
        /*
        将至少3个格子变为3
        将至少5个格子变为5
        将所有格子变为4
        将所有格子变为7
        将所有格子为2或7
        将数字总和变为20（初始40）
        将所有格子变为不同值
        在每个格子都是寿司或饭卷的情况下，将数字总和变为90（初始75）
         */
        bool result = false;
        if (levelData.levelIndex == 1)
        {
            return BoardCalculation.CountX_Ytimes(levelData.curBoard, 3, 3);
        }
        else if (levelData.levelIndex == 2)
        {
            return BoardCalculation.CountX_Ytimes(levelData.curBoard, 5, 5);
        }
        else if (levelData.levelIndex == 3)
        {
            return BoardCalculation.CountX_All(levelData.curBoard, 4);
        }
        else if (levelData.levelIndex == 4)
        {
            return BoardCalculation.CountX_All(levelData.curBoard, 7);
        }
        else if (levelData.levelIndex == 5)
        {
            return BoardCalculation.CountXorY_All(levelData.curBoard, 2, 7);
        }
        else if (levelData.levelIndex == 6)
        {
            return BoardCalculation.Unique_All(levelData.curBoard);
        }
        else if (levelData.levelIndex == 7)
        {
            return BoardCalculation.Sum_As_X(levelData.curBoard, 20);
        }
        else if (levelData.levelIndex == 8)
        {
            return (BoardCalculation.Sum_As_X(levelData.curBoard, 82) && BoardCalculation.CountStatusXorY_All(levelData.curBoard, 3, 4));
        }
        else
        {
            Debug.LogError(string.Format("reach undefined level in CheckWinCondition of ({0})", levelData.theme));
        }
        return result;
    }
    public override void WinALevel()
    {
        AudioDraft.singleton.PlaySFX(sushiHub.GetEndingClip(levelData.levelIndex));
        base.WinALevel();
    }

    void FoodPlaceAnimation(GameObject sushiPlate, int curStatus, int toolStatus, int finalStatus)
    {
        //float originalAlpha = sushiPlate.GetComponent<SpriteRenderer>().color.a;
        //Debug.Log("originalAlpha is " + originalAlpha);
        sushiPlate.GetComponent<SpriteRenderer>().sprite = sushiHub.statusSprites[curStatus];
        Sprite finalSprite = sushiHub.statusSprites[finalStatus];
        //creation
        GameObject placed_sprite = Instantiate(sushiPlate, sushiPlate.transform.parent);
        GameObject new_sprite = Instantiate(sushiPlate, sushiPlate.transform.parent);
        //placement
        placed_sprite.transform.parent = sushiPlate.transform;
        new_sprite.transform.parent = sushiPlate.transform;
        placed_sprite.transform.localPosition = Vector3.zero;
        placed_sprite.transform.localScale = Vector3.one;
        new_sprite.transform.localPosition = Vector3.zero;
        new_sprite.transform.localScale = Vector3.one;
        //animation
        placed_sprite.GetComponent<SpriteRenderer>().sprite = sushiHub.statusSprites[toolStatus];
        placed_sprite.GetComponent<SpriteRenderer>().DOFade(0f, CellFoodTrasitionDuration * 0.6f);
        placed_sprite.transform.DOLocalMoveY(CellFoodTrasitionDistance, CellFoodTrasitionDuration * 0.6f).From();

        new_sprite.GetComponent<SpriteRenderer>().sprite = finalSprite;
        new_sprite.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);
        new_sprite.GetComponent<SpriteRenderer>().DOFade(originalFoodAlpha, CellFoodTrasitionDuration * 0.5f).SetDelay(CellFoodTrasitionDuration * 0.5f);

        sushiPlate.GetComponent<SpriteRenderer>().DOFade(0f, CellFoodTrasitionDuration * 0.6f);
        
        Sequence seq = DOTween.Sequence();
        seq.AppendInterval(CellFoodTrasitionDuration)
            .AppendCallback(() => sushiPlate.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, originalFoodAlpha))
            .AppendCallback(() => sushiPlate.GetComponent<SpriteRenderer>().sprite = finalSprite)
            .AppendCallback(() => Destroy(new_sprite))
            .AppendCallback(() => Destroy(placed_sprite));
        //.OnComplete(() => Destroy(temp_sprite))
        //sushiHub.sushiPlates[i].Value.SetActive(temp_cellData.status != 0);
        //sushiHub.sushiPlates[i].Value.GetComponent<SpriteRenderer>().sprite = sushiHub.statusSprites[temp_cellData.status];
    }
}
