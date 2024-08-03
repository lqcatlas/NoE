using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI.Table;

public class LM_013_Detective : LevelMasterBase
{
    enum InspectionResult { none = 0, accurate = 1, suspect = 2, exclude = 3 };
    [Header("Theme Additions")]
    public LMHub_013_Detective themeHub;
    //anim param
    private float INSPECT_DURATRION = 2f;
    private float INSPECT_POP_SCALE = 1.5f;
    private float MURDERER_ROW_OFFSET = 0.3f;
    //inspect logic
    private List<InspectionResult> ispResult;
    private List<Vector2Int> appointedCoord;
    public List<Vector2Int> murdererCoord;
    private List<int> murdererValue;
    private List<int> suspectValue;
    //for multi cases
    private int consecutiveCaseSolved = 0;
    private bool needNewCase = false;
    private bool caseFailed = false;
    //level index
    private int RANDOMIZE_LVINDEX = 2;
    private int CONSECTIVE_CASE_2_LVINDEX = 10;
    private int CONSECTIVE_CASE_INF_LVINDEX = 13;
    private int RANDOM_MURDERER_LVINDEX = 13;

    public override void GetObjectReferences(GameObject _themeHub)
    {
        base.GetObjectReferences(null);
        themeHub = _themeHub.GetComponent<LMHub_013_Detective>();
        ThemeAnimationDelayAfterPlay = 0.8f;
    }
    public override void InitBoardData()
    {
        if (levelData.levelIndex >= RANDOMIZE_LVINDEX)
        {
            RandomizeDetectiveBoard(levelData.initBoard);
        }
        levelData.curBoard = new DataBoard(levelData.initBoard);
        if(levelData.levelIndex >= CONSECTIVE_CASE_INF_LVINDEX)
        {
            RandomMurderer(levelData.curBoard);
        }
        levelData.previousBoard = null;
        levelData.previousBoards = new List<DataBoard>();
    }
    public override void InitTool()
    {
        base.InitTool();
        hub.toolMaster.toolIcon.sprite = themeHub.toolSprite;
    }
    public override void InitCells()
    {
        for (int i = 0; i < hub.boardMaster.cells.Count; i++)
        {
            DataCell temp_cellData = levelData.curBoard.GetCellDataByCoord(hub.boardMaster.cells[i].coord);
            if (temp_cellData != null)
            {
                if(temp_cellData.status >= 0)
                {
                    hub.boardMaster.cells[i].DisplayNumber(temp_cellData.value);
                    hub.boardMaster.cells[i].SetColor(dConstants.UI.DefaultColor_1st);
                }
                else
                {
                    hub.boardMaster.cells[i].SetCellEmpty(true);
                }
            }
        }
    }
    public override void AddtionalInit_Theme(bool isRewind = false)
    {
        //reset play logs
        consecutiveCaseSolved = 0;
        caseFailed = false;
        needNewCase = false;
        //offset suspect row
        for (int i = 0; i < hub.boardMaster.cells.Count; i++)
        {
            if (hub.boardMaster.cells[i].coord.y == levelData.initBoard.boardSize.y - 1)
            {
                Vector3 curPos = hub.boardMaster.cells[i].transform.localPosition;
                hub.boardMaster.cells[i].transform.localPosition = curPos + new Vector3(0f, MURDERER_ROW_OFFSET, 0f);
            }
        }
        NewCaseDisplay();
    }
    public override void ToolConsume(Vector2Int coord)
    {
        //consume tool and save curboard to previous board/boards
        levelData.curBoard.curPlayCoord = coord;
        levelData.previousBoard = new DataBoard(levelData.curBoard);
        if (levelData.allowRewind)
        {
            levelData.previousBoards.Add(new DataBoard(levelData.curBoard));
        }
        if (coord.y != levelData.initBoard.boardSize.y - 1)
        {
            levelData.curBoard.toolCount -= 1;
        }
    }
    public override void HandlePlayerInput(Vector2Int coord)
    {
        if (coord.y == levelData.initBoard.boardSize.y - 1) //murder appointing 
        {
            if (!murdererCoord.Contains(coord))
            {
                caseFailed = true;
            }
            else
            {
                appointedCoord.Add(coord);
            }
        }
        else //row inspection
        {
            int inspectedRow = coord.y;
            InspectionResult result = InspectRow(inspectedRow);
            ispResult[inspectedRow] = result;
        }
    }
    
    public override void HandleEnvironment(Vector2Int coord)
    {
        if (appointedCoord.Count == murdererCoord.Count) // made enough appointment so start checking
        {
            bool result = CheckCurCase();
            if(result)
            {
                needNewCase = true;
                consecutiveCaseSolved += 1;
            }
            else
            {
                caseFailed = true;
            }
        }
    }
    public override void UpdateTool(Vector2Int coord)
    {
        base.UpdateTool(coord);
        UpdateToolStatusDisplay();
    }
    public override void UpdateCells(Vector2Int coord)
    {
        //value do not update in Detective
    }
    public override void AddtionalUpdate_Theme(Vector2Int coord)
    {
        if (coord.y == levelData.initBoard.boardSize.y - 1) //murder appointing 
        {
            Lockdown_Anim(coord.x);
        }
        else //row inspection
        {
            InspectRow_Anim(coord.y);
        }
        UpdateCellInteractable();

    }
    public override bool CheckLoseCondition()
    {
        if(caseFailed) // made enough appointment so start checking
        {
            caseFailed = false;
            return true;
        }
        return false;
    }
    public override bool CheckWinCondition()
    {
        if (levelData.levelIndex >= 1 && levelData.levelIndex < CONSECTIVE_CASE_2_LVINDEX)
        {
            return consecutiveCaseSolved >= 1;
        }
        else if (levelData.levelIndex >= CONSECTIVE_CASE_2_LVINDEX && levelData.levelIndex < CONSECTIVE_CASE_INF_LVINDEX)
        {
            if (consecutiveCaseSolved >= 2)
            {
                return true;
            }
            else if(needNewCase)
            {
                StartNewCaseInLevel();
            }
        }
        else if (levelData.levelIndex >= CONSECTIVE_CASE_INF_LVINDEX)
        {
            if (consecutiveCaseSolved >= 50)
            {
                return true;
            }
            else if (needNewCase)
            {
                StartNewCaseInLevel();
            }
        }
        else
        {
            Debug.LogError(string.Format("reach undefined level in CheckWinCondition of ({0})", levelData.theme));
            return false;
        }
        return false;
    }
    void StartNewCaseInLevel()
    {
        //start a new case
        int curToolLeft = levelData.curBoard.toolCount;
        needNewCase = false;
        RandomizeDetectiveBoard(levelData.initBoard);
        levelData.curBoard = new DataBoard(levelData.initBoard);
        if (levelData.levelIndex >= RANDOM_MURDERER_LVINDEX)
        {
            RandomMurderer(levelData.curBoard);

        }
        levelData.previousBoard = null;
        levelData.previousBoards = new List<DataBoard>();
        levelData.curBoard.toolCount = curToolLeft;
        if (levelData.levelIndex >= RANDOM_MURDERER_LVINDEX)
        {
            levelData.curBoard.toolCount += 3;
        }
        InitCells();
        NewCaseDisplay();
        CaseSolved_Anim();
        InitTool();
        UpdateToolStatusDisplay();
    }
    void NewCaseDisplay()
    {
        //clear appointed
        appointedCoord = new List<Vector2Int>();
        //locate the murderers
        murdererCoord = new List<Vector2Int>();
        murdererValue = new List<int>();
        suspectValue = new List<int>();
        for (int i = 0; i < levelData.curBoard.cells.Count; i++)
        {
            if (levelData.curBoard.cells[i].coord.y == levelData.curBoard.boardSize.y - 1)
            {
                if (levelData.curBoard.cells[i].status > 0)
                {
                    murdererCoord.Add(levelData.curBoard.cells[i].coord);
                    murdererValue.Add(levelData.curBoard.cells[i].value);
                }    
                suspectValue.Add(levelData.curBoard.cells[i].value);
            }
        }
        //clear old bgs
        List<Transform> oldBgs = themeHub.bgHolder.GetComponentsInChildren<Transform>().ToList();
        oldBgs.Remove(themeHub.bgHolder.transform);
        for (int i = 0; i < oldBgs.Count; i++)
        {
            Destroy(oldBgs[i].gameObject);
        }
        //init isp signs and murderer bg
        themeHub.suspectBgs = new List<KeyValuePair<CellMaster, GameObject>>();
        themeHub.witnessBgs = new List<KeyValuePair<CellMaster, GameObject>>();
        themeHub.bgHolder.transform.localScale = hub.boardMaster.cellHolder.localScale;
        themeHub.ispSigns = new List<GameObject>();
        ispResult = new List<InspectionResult>();
        float signXOffset = 0;
        List<float> signYOffsetPerY = new List<float>();
        for (int i = 0; i < levelData.initBoard.boardSize.y; i++)
        {
            signYOffsetPerY.Add(0);
        }
        for (int i = 0; i < hub.boardMaster.cells.Count; i++)
        {
            if (hub.boardMaster.cells[i].coord.x == levelData.initBoard.boardSize.x - 1)
            {
                signXOffset = hub.boardMaster.cells[i].transform.localPosition.x;
                signYOffsetPerY[hub.boardMaster.cells[i].coord.y] = hub.boardMaster.cells[i].transform.localPosition.y;
            }
            if (hub.boardMaster.cells[i].coord.y == levelData.initBoard.boardSize.y - 1)
            {
                //create new murder bg sprite
                GameObject obj = Instantiate(themeHub.suspectBgTemplate, themeHub.bgHolder);
                obj.transform.position = hub.boardMaster.cells[i].transform.position;
                themeHub.suspectBgs.Add(new KeyValuePair<CellMaster, GameObject>(hub.boardMaster.cells[i], obj));
            }
            else
            {
                //create new witness bg sprite
                GameObject obj = Instantiate(themeHub.witnessBgTemplate, themeHub.bgHolder);
                obj.transform.position = hub.boardMaster.cells[i].transform.position;
                themeHub.witnessBgs.Add(new KeyValuePair<CellMaster, GameObject>(hub.boardMaster.cells[i], obj));
                DataCell temp_cellData = levelData.curBoard.GetCellDataByCoord(hub.boardMaster.cells[i].coord);
                if (temp_cellData.status < 0)
                {
                    obj.transform.Find("unknown_sign").GetComponent<SpriteRenderer>().color = dConstants.UI.DefaultColor_1st;
                }
            }
        }
        float additionalOffset = 2f;
        for (int i = 0; i < levelData.initBoard.boardSize.y - 1; i++)
        {
            ispResult.Add(InspectionResult.none);
            GameObject obj = Instantiate(themeHub.ispSignTemplate, themeHub.bgHolder);
            obj.transform.localPosition = new Vector3(signXOffset + additionalOffset, signYOffsetPerY[i], 0);
            obj.GetComponent<inspection_sign>().SetSign((int)ispResult[i]);
            themeHub.ispSigns.Add(obj);
        }
        //pose the split line
        float splitLine_Y = (signYOffsetPerY[levelData.initBoard.boardSize.y - 1] + signYOffsetPerY[levelData.initBoard.boardSize.y - 2] - MURDERER_ROW_OFFSET) / 2f;
        themeHub.splitLine.localPosition = new Vector3(0f, (splitLine_Y + MURDERER_ROW_OFFSET) * hub.boardMaster.cellHolder.localScale.y, 0f);

        //update cell interactable
        UpdateCellInteractable();

        //update tool status
        UpdateToolStatusDisplay();

        //update consective case desc
        if (levelData.levelIndex >= CONSECTIVE_CASE_2_LVINDEX && levelData.levelIndex < CONSECTIVE_CASE_INF_LVINDEX)
        {
            hub.goalMaster.lines[1].SetText($"{LocalizedAssetLookup.singleton.Translate("@Loc=tm13_goal_case_solved@@")}{consecutiveCaseSolved}/2");
            hub.goalMaster.lines[1].gameObject.SetActive(true);
            LayoutRebuilder.ForceRebuildLayoutImmediate(hub.goalMaster.goalLayout);
        }
        else if(levelData.levelIndex >= CONSECTIVE_CASE_INF_LVINDEX)
        {
            hub.goalMaster.lines[1].SetText($"{LocalizedAssetLookup.singleton.Translate("@Loc=tm13_goal_case_solved@@")}{consecutiveCaseSolved}");
            hub.goalMaster.lines[1].gameObject.SetActive(true);
            LayoutRebuilder.ForceRebuildLayoutImmediate(hub.goalMaster.goalLayout);
        }
    }
    bool CheckCurCase()
    {
        bool success = true;
        for (int i = 0; i < murdererCoord.Count; i++)
        {
            bool found = false;
            for (int j = 0; j < appointedCoord.Count; j++)
            {
                if (murdererCoord[i] == appointedCoord[j])
                {
                    found = true;
                    break;
                }
            }
            if (!found)
            {
                success = false;
                break;
            }
        }
        return success;
    }
    
    InspectionResult InspectRow(int row)
    {
        InspectionResult result = InspectionResult.exclude;
        for (int i = 0;i < levelData.curBoard.cells.Count;i++)
        {
            if (levelData.curBoard.cells[i].coord.y == row)
            {
                if (murdererValue.Contains(levelData.curBoard.cells[i].value)) //ambiguous match
                {
                    if (result == InspectionResult.exclude) //upgrade status only if havee not been mathced
                    {
                        result = InspectionResult.suspect; 
                    }
                    for(int j = 0; j < murdererCoord.Count; j++)
                    {
                        if (murdererCoord[j].x == levelData.curBoard.cells[i].coord.x 
                            && murdererValue[j] == levelData.curBoard.cells[i].value)  //exact match
                        {
                            result = InspectionResult.accurate;
                        }
                    }
                }
            }
        }
        return result;
    }
    void Lockdown_Anim(int col)
    {
        for(int i = 0;i<themeHub.suspectBgs.Count;i++)
        {
            if (themeHub.suspectBgs[i].Key.coord.x == col)
            {
                themeHub.suspectBgs[i].Value.transform.Find("jail_door_anim").gameObject.SetActive(true);
            }
        }
    }
    void InspectRow_Anim(int row)
    {
        //collect all cells of the row in order
        List<GameObject> cellsInRow = new List<GameObject>();
        List<GameObject> bgsInRow = new List<GameObject>();
        for (int i=0;i<levelData.curBoard.boardSize.x;i++)
        {
            cellsInRow.Add(null); //add placeholders as the same amount as board's X size
            bgsInRow.Add(null);
        }
        for(int i = 0; i < themeHub.witnessBgs.Count; i++)
        {
            if(themeHub.witnessBgs[i].Key.coord.y == row)
            {
                cellsInRow[themeHub.witnessBgs[i].Key.coord.x] = themeHub.witnessBgs[i].Key.gameObject;
                bgsInRow[themeHub.witnessBgs[i].Key.coord.x] = themeHub.witnessBgs[i].Value;
            }
        }
        //create inspect obj, move it
        float cellSize = 2f;
        GameObject animObj = Instantiate(themeHub.ispAnimTemplate, themeHub.bgHolder);
        Vector3 startPos = cellsInRow[0].transform.localPosition - Vector3.right * cellSize/2f;
        Vector3 endPos = cellsInRow[cellsInRow.Count - 1].transform.localPosition + Vector3.right * cellSize / 2f;
        animObj.transform.localPosition = startPos;
        animObj.SetActive(true);
        animObj.transform.DOLocalMove(endPos, INSPECT_DURATRION).SetEase(Ease.Linear).OnComplete(()=>Destroy(animObj));
        //create number popup sequence
        Sequence seq = DOTween.Sequence();
        float inspectCellInterval = INSPECT_DURATRION / cellsInRow.Count;
        for (int i = 0; i < cellsInRow.Count; i++)
        {
            Transform trans = cellsInRow[i].GetComponent<CellMaster>().numberGroup;
            float delay = inspectCellInterval * i;
            trans.localScale = Vector3.one;
            seq.Insert(delay, trans.DOScale(INSPECT_POP_SCALE, dConstants.UI.StandardizedBtnAnimDuration / 2f).SetLoops(2, LoopType.Yoyo));
            
            DataCell temp_cellData = levelData.curBoard.GetCellDataByCoord(cellsInRow[i].GetComponent<CellMaster>().coord);
            CellMaster cm = cellsInRow[i].GetComponent<CellMaster>();
            //check and reveal the unknown
            if (temp_cellData.status == -1)
            {
                
                SpriteRenderer unknownSign = bgsInRow[i].transform.Find("unknown_sign").GetComponent<SpriteRenderer>();
                seq.Insert(delay, unknownSign.DOFade(0f, dConstants.UI.StandardizedVFXAnimDuration));
                seq.InsertCallback(delay, () => cm.DisplayNumber(temp_cellData.value));
            }
            //check and update irrelevant numbers
            float totalFinishedTime = INSPECT_DURATRION + dConstants.UI.StandardizedBtnAnimDuration;
            if (!suspectValue.Contains(temp_cellData.value)) //irrelevant numbers
            {
                seq.InsertCallback(delay, () => cm.SetColor(dConstants.UI.DefaultColor_3rd));
            }
        }
        //update sign
        seq.InsertCallback(INSPECT_DURATRION, ()=>themeHub.ispSigns[row].GetComponent<inspection_sign>().UpdateSign((int)ispResult[row]));
    }
    void CaseSolved_Anim()
    {
        Sequence seq = DOTween.Sequence();
        hub.goalMaster.lines[1].transform.DOScale(.2f, dConstants.UI.StandardizedBtnAnimDuration).SetLoops(2, LoopType.Yoyo).SetRelative(true);
    }
    void RandomizeDetectiveBoard(DataBoard board)
    {
        //shuffle on numbers
        List<int> numberShift = Enumerable.Range(1, 9).ToList();
        numberShift = numberShift.OrderBy(x => Guid.NewGuid()).ToList();
        numberShift.Insert(0, 0);
        //Debug.LogWarning("numbershift list: " + string.Join(", ", numberShift));

        int[,] valueInArray = new int[board.boardSize.x, board.boardSize.y];
        int[,] statusInArray = new int[board.boardSize.x, board.boardSize.y];
        for (int i = 0; i < board.cells.Count; i++)
        {
            if(board.cells[i].value >= 0 && board.cells[i].value <= 9)
            {
                board.cells[i].value = numberShift[board.cells[i].value]; //shift numbers
            }
            valueInArray[board.cells[i].coord.x, board.cells[i].coord.y] = board.cells[i].value; //create a 2D array at the same time
            statusInArray[board.cells[i].coord.x, board.cells[i].coord.y] = board.cells[i].status;
        }
        //shuffle on col
        for (int i = board.boardSize.x - 1; i > 0; i--)
        {
            int j = UnityEngine.Random.Range(0, i + 1);
            if (i != j)
            {
                for (int k = 0; k < board.boardSize.y; k++)
                {
                    (valueInArray[i, k], valueInArray[j, k]) = (valueInArray[j, k], valueInArray[i, k]);
                    (statusInArray[i, k], statusInArray[j, k]) = (statusInArray[j, k], statusInArray[i, k]);
                }
            }
        }
        //shuffle on row but keep the last row stay untouched
        for (int i = board.boardSize.y - 2; i > 0; i--)
        {
            int j = UnityEngine.Random.Range(1, i + 1);
            if (i != j)
            {
                for (int k = 0; k < board.boardSize.x; k++)
                {
                    (valueInArray[k, i], valueInArray[k, j]) = (valueInArray[k, j], valueInArray[k, i]);
                    (statusInArray[k, i], statusInArray[k, j]) = (statusInArray[k, j], statusInArray[k, i]);
                }
            }
        }
        //copy array back to data cells
        for (int i = 0; i < board.cells.Count; i++)
        {
            board.cells[i].value = valueInArray[board.cells[i].coord.x, board.cells[i].coord.y];
            board.cells[i].status = statusInArray[board.cells[i].coord.x, board.cells[i].coord.y];
        }
        
    }
    void RandomMurderer(DataBoard board)
    {
        //shuffule the murderer row
        List<int> newMurdererStatus = new List<int>();
        float TwoMurdererChance = .7f;
        float rng = UnityEngine.Random.Range(0, 1f);
        for (int i = 0; i < board.boardSize.x; i++)
        {
            newMurdererStatus.Add(0);
        }
        if(board.boardSize.x <= 1)
        {
            Debug.LogError("baord size X is less than required murderer count:2");
            return;
        }
        int rngIndex = UnityEngine.Random.Range(0, board.boardSize.x);
        if (newMurdererStatus[rngIndex] == 0)
        {
            newMurdererStatus[rngIndex] = 1; //place murderer 1
        }
        if(rng < TwoMurdererChance)
        {
            while (true)
            {
                rngIndex = UnityEngine.Random.Range(0, board.boardSize.x);
                if (newMurdererStatus[rngIndex] == 0)
                {
                    newMurdererStatus[rngIndex] = 2;  //place murderer 2
                    break;
                }
            }
        }
        for (int i = 0; i < board.cells.Count; i++)
        {
            if(board.cells[i].coord.y == board.boardSize.y - 1) //copy to the murderer row
            {
                board.cells[i].status = newMurdererStatus[board.cells[i].coord.x];
            }
        }
    }
    void UpdateCellInteractable()
    {
        bool hasEnoughTools = levelData.curBoard.toolCount > 0;
        for (int i = 0; i < hub.boardMaster.cells.Count; i++)
        {
            int row = hub.boardMaster.cells[i].coord.y;
            if (row == levelData.initBoard.boardSize.y - 1)
            {
                if (!appointedCoord.Contains(hub.boardMaster.cells[i].coord))
                {
                    hub.boardMaster.cells[i].SetCellInteractable(true);
                }
                else
                {
                    hub.boardMaster.cells[i].SetCellInteractable(false);
                }
            }
            else if (ispResult[row] == InspectionResult.none && hasEnoughTools)
            {
                hub.boardMaster.cells[i].SetCellInteractable(true);
            }
            else
            {
                hub.boardMaster.cells[i].SetCellInteractable(false);
            }
        }
    }
    void UpdateToolStatusDisplay()
    {
        levelData.curBoard.toolStatus = levelData.curBoard.toolCount > 0 ? 0 : 1;
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
