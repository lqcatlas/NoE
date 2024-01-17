using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LM_002_Coin : LevelMasterBase
{
    [Header("Theme Additions")]
    public LMHub_002_Coin coinHub;

    public override void GetObjectReferences(GameObject _themeHub)
    {
        base.GetObjectReferences(null);
        coinHub = _themeHub.GetComponent<LMHub_002_Coin>();
    }
    public override void InitTool()
    {
        base.InitTool();
        hub.toolMaster.toolSubtitle.SetText(LocalizedAssetLookup.singleton.Translate(coinHub.toolDisplayName[levelData.curBoard.toolStatus]));
    }
    public override void AdditionalGenerateBoard_Theme()
    {
        //clear old coin bgs
        coinHub.coinBgHolder.transform.localScale = hub.boardMaster.cellHolder.localScale;
        List<Transform> oldBgs = coinHub.coinBgHolder.GetComponentsInChildren<Transform>().ToList();
        oldBgs.Remove(coinHub.coinBgHolder.transform);
        for (int i = 0; i < oldBgs.Count; i++)
        {
            Destroy(oldBgs[i].gameObject);
        }
    }
    public override void InitCells()
    {
        coinHub.coinTags = new List<KeyValuePair<CellMaster, GameObject>>();
        //clear old coin tags
        coinHub.cellTagHolder.transform.localScale = hub.boardMaster.cellHolder.localScale;
        List<Transform> oldTags = coinHub.cellTagHolder.GetComponentsInChildren<Transform>().ToList();
        oldTags.Remove(coinHub.cellTagHolder.transform);
        for (int i = 0; i < oldTags.Count; i++)
        {
            Destroy(oldTags[i].gameObject);
        }
        //generate new coin tag
        for (int i = 0; i < hub.boardMaster.cells.Count; i++)
        {
            DataCell temp_cellData = levelData.curBoard.GetCellDataByCoord(hub.boardMaster.cells[i].coord);
            if (temp_cellData != null)
            {
                hub.boardMaster.cells[i].DisplayNumber(temp_cellData.value);
                //additional
                GameObject coinTag = Instantiate(coinHub.cellTagTempalte, coinHub.cellTagHolder);
                coinTag.transform.position = hub.boardMaster.cells[i].transform.position;
                //clockBg.transform.localScale = hub.boardMaster.cells[i].transform.localScale;
                coinHub.coinTags.Add(new KeyValuePair<CellMaster, GameObject>(hub.boardMaster.cells[i], coinTag)); 
                if (temp_cellData.status >= 1)
                {
                    coinTag.GetComponent<CoinCellCountWidget>().stackNumber.SetText(temp_cellData.status.ToString());
                    coinTag.gameObject.SetActive(true);
                }
                else
                {
                    coinTag.gameObject.SetActive(false);
                }
            }
        }
    }
    public override void AddtionalInit_Theme()
    {
        hub.toolMaster.toolIcon.sprite = coinHub.coinToolSprites[levelData.curBoard.toolStatus];
    }
    public override void HandlePlayerInput(Vector2Int coord)
    {

        //cell number rule
        //head +1, tail -1
        if (levelData.levelIndex >= 1 && levelData.levelIndex <= 3)
        {
            for (int i = 0; i < levelData.curBoard.cells.Count; i++)
            {
                if (levelData.curBoard.cells[i].coord == coord)
                {
                    levelData.curBoard.cells[i].value += levelData.curBoard.toolStatus == 0 ? 1 : -1;
                    levelData.curBoard.cells[i].value = BoardCalculation.ModX_Range(levelData.curBoard.cells[i].value, new Vector2Int(1, 9));
                    levelData.curBoard.cells[i].status += 1;
                }
            }
        }
        //head +X, tail -1
        else if (levelData.levelIndex == 4)
        {
            for (int i = 0; i < levelData.curBoard.cells.Count; i++)
            {
                if (levelData.curBoard.cells[i].coord == coord)
                {
                    
                    levelData.curBoard.cells[i].value += levelData.curBoard.toolStatus == 0 ? levelData.curBoard.cells[i].status : -1;
                    levelData.curBoard.cells[i].value = BoardCalculation.ModX_Range(levelData.curBoard.cells[i].value, new Vector2Int(1, 9));
                    levelData.curBoard.cells[i].status += 1;
                }
            }
        }
        //head +X, tail -X
        else if (levelData.levelIndex >= 5 && levelData.levelIndex <= 8)
        {
            for (int i = 0; i < levelData.curBoard.cells.Count; i++)
            {
                if (levelData.curBoard.cells[i].coord == coord)
                {

                    levelData.curBoard.cells[i].value += levelData.curBoard.toolStatus == 0 ? levelData.curBoard.cells[i].status : -levelData.curBoard.cells[i].status;
                    levelData.curBoard.cells[i].value = BoardCalculation.ModX_Range(levelData.curBoard.cells[i].value, new Vector2Int(1, 9));
                    levelData.curBoard.cells[i].status += 1;
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
        //coin stay
        if (levelData.levelIndex >= 1 && levelData.levelIndex <= 2)
        {
            //do nothing
        }
        //coin flip
        else if (levelData.levelIndex >= 3 && levelData.levelIndex <= 6)
        {
            levelData.curBoard.toolStatus = (levelData.curBoard.toolStatus + 1) % 2;
        }
        //coin toss
        else if(levelData.levelIndex >= 7 && levelData.levelIndex <= 8)
        {
            int rng = Random.Range(0, 2);
            levelData.curBoard.toolStatus = rng;
        }
        else
        {
            Debug.LogError(string.Format("master script of {0} reaches undefined level", levelData.theme));
        }
    }
    public override void UpdateTool(Vector2Int coord)
    {
        base.UpdateTool(coord);
        if (levelData.levelIndex >= 7)
        {
            // strong toss
            float duration = 0.5f;
            float shake_strength = 240f;
            int shake_vibrato = 10;
            float shake_randomness = 20;
            float toss_scale = 8f;
            float toss_alpha = 0.4f;
            //Debug.Log("coin theme special tool update");
            Sequence seq = DOTween.Sequence();
            seq.AppendCallback(() => hub.toolMaster.toolIcon.GetComponent<Transform>().DOShakeRotation(duration, new Vector3(1f, 0.5f, 0) * shake_strength, shake_vibrato, shake_randomness));
            seq.AppendCallback(() => hub.toolMaster.toolIcon.GetComponent<Transform>().DOScale(new Vector3(toss_scale, toss_scale, 0), duration / 2f).SetLoops(2, LoopType.Yoyo).SetRelative(true));
            seq.AppendCallback(() => hub.toolMaster.toolIcon.GetComponent<SpriteRenderer>().DOFade(toss_alpha, duration / 2f).SetLoops(2, LoopType.Yoyo));
            seq.AppendInterval(duration+0.02f);
            seq.AppendCallback(() => hub.toolMaster.toolIcon.sprite = coinHub.coinToolSprites[levelData.curBoard.toolStatus]);
            seq.AppendCallback(() => hub.toolMaster.toolIcon.GetComponent<Transform>().rotation = Quaternion.Euler(0f, 0f, 0f));
            seq.AppendCallback(() => hub.toolMaster.toolIcon.GetComponent<Transform>().localScale = Vector3.one * 8f);
            seq.AppendCallback(() => hub.toolMaster.toolIcon.GetComponent<SpriteRenderer>().color = new Color(1f,1f,1f,1f));
        }
        else if (levelData.levelIndex >= 3)
        {
            //weak flip
            float duration = 0.3f;
            float shake_strength = 90f;
            int shake_vibrato = 10;
            float shake_randomness = 20;
            float toss_scale = 5f;
            float toss_alpha = 0.6f;
            //Debug.Log("coin theme special tool update");
            Sequence seq = DOTween.Sequence();
            seq.AppendCallback(() => hub.toolMaster.toolIcon.GetComponent<Transform>().DOShakeRotation(duration, new Vector3(1f, 0.5f, 0) * shake_strength, shake_vibrato, shake_randomness));
            seq.AppendCallback(() => hub.toolMaster.toolIcon.GetComponent<Transform>().DOScale(new Vector3(toss_scale, toss_scale, 0), duration / 2f).SetLoops(2, LoopType.Yoyo).SetRelative(true));
            seq.AppendCallback(() => hub.toolMaster.toolIcon.GetComponent<SpriteRenderer>().DOFade(toss_alpha, duration / 2f).SetLoops(2, LoopType.Yoyo));
            seq.AppendInterval(duration + 0.02f);
            seq.AppendCallback(() => hub.toolMaster.toolIcon.sprite = coinHub.coinToolSprites[levelData.curBoard.toolStatus]);
            seq.AppendCallback(() => hub.toolMaster.toolIcon.GetComponent<Transform>().rotation = Quaternion.Euler(0f, 0f, 0f));
            seq.AppendCallback(() => hub.toolMaster.toolIcon.GetComponent<Transform>().localScale = Vector3.one * 8f);
            seq.AppendCallback(() => hub.toolMaster.toolIcon.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f));
        }
        hub.toolMaster.toolSubtitle.SetText(LocalizedAssetLookup.singleton.Translate(coinHub.toolDisplayName[levelData.curBoard.toolStatus]));
    }
    public override void AddtionalUpdate_Theme(Vector2Int coord)
    {
        //update coin stack number
        DataCell temp_cellData = levelData.curBoard.GetCellDataByCoord(coord);
        Transform targetCellLocation = null;
        for (int i = 0; i < coinHub.coinTags.Count; i++)
        {
            if (coinHub.coinTags[i].Key.coord == coord)
            {
                coinHub.coinTags[i].Value.GetComponent<CoinCellCountWidget>().stackNumber.SetText(temp_cellData.status.ToString());
                coinHub.coinTags[i].Value.SetActive(true);
                targetCellLocation = coinHub.coinTags[i].Key.transform;
                break;
            }
        }
        //place random coin
        GameObject randomCoin = Instantiate(coinHub.RandomCoinTemplate, coinHub.coinBgHolder);
        randomCoin.GetComponent<RandomCoinShapeWidget>().GenerateARandomCoin(levelData.previousBoard.toolStatus);
        float XYRange = 2.5f;
        randomCoin.transform.position = targetCellLocation.position + new Vector3(Random.Range(-1f, 1f) * XYRange, Random.Range(-1f, 1f) * XYRange, 0f);
        //place animation
        float distance = 10f;
        float duration = 0.3f;
        float start_size = 10f;
        float shake_strength = 0.03f;
        float rotate_angle = Random.Range(0f, Mathf.PI * 2);
        Vector3 moveFromCoord = new Vector3(Mathf.Cos(rotate_angle) * distance, Mathf.Sin(rotate_angle) * distance, 0);
        randomCoin.transform.DOLocalMove(moveFromCoord, duration).From();
        randomCoin.transform.DOScale(Vector3.one * start_size, duration).From();
        randomCoin.GetComponent<RandomCoinShapeWidget>().coinShape.GetComponent<SpriteRenderer>().DOFade(0f, duration).From();
        randomCoin.transform.DOShakePosition(duration / 2, new Vector3(1f, 1f, 0f) * shake_strength, 50).SetDelay(duration * 1.2f).OnStart(()=> AudioDraft.singleton.PlaySFX(coinHub.GetNextPlayClip()));

    }
    public override bool CheckWinCondition()
    {
        bool result = false;
        if (levelData.levelIndex == 1)
        {
            return BoardCalculation.CountX_All(levelData.curBoard, 4);
        }
        else if (levelData.levelIndex == 2)
        {
            return BoardCalculation.CountX_All(levelData.curBoard, 4);
        }
        else if (levelData.levelIndex == 3)
        {
            return BoardCalculation.Same_All(levelData.curBoard);
        }
        else if (levelData.levelIndex == 4)
        {
            return BoardCalculation.CountX_Ytimes(levelData.curBoard, 9, 2);
        }
        else if (levelData.levelIndex == 5)
        {
            return BoardCalculation.CountX_Ytimes(levelData.curBoard, 1, 2);
        }
        else if (levelData.levelIndex == 6)
        {
            return BoardCalculation.Same_All(levelData.curBoard);
        }
        else if (levelData.levelIndex == 7)
        {
            return BoardCalculation.Unique_All(levelData.curBoard);
        }
        else if (levelData.levelIndex == 8)
        {
            return BoardCalculation.Same_All(levelData.curBoard);
        }
        else
        {
            Debug.LogError(string.Format("reach undefined level in CheckWinCondition of ({0})", levelData.theme));
        }
        return result;
    }
}
