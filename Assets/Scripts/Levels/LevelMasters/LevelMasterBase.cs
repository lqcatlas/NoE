using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//base script managing any level
//each theme should have its own theme master script derived from this base script
//eg. clock theme has a master script: LM_{ThemeIndexAS XXX}_{ThemeName}
//need to be registered in Resource/theme_prefabs/theme_lookup
public class LevelMasterBase : MonoBehaviour
{
    [Header("Debug Tools")]
    [SerializeField] bool InitTrigger;
    [SerializeField] bool PlayTrigger;
    [SerializeField] Vector2Int PlayCoord;
    [SerializeField] bool RewindTrigger;
    [SerializeField] bool SuccessTrigger;
    
    //Data setup by launcher before calling LevelInit()
    [Header("Setup Data")]
    public SheetItem_LevelSetup levelSetupData;
    public ThemeResourceLookup themeLookupTable;
    public float ThemeAnimationDelayAfterInit;
    public float ThemeAnimationDelayAfterPlay;

    [Header("In-Level Data")]
    public DataLevel levelData = new DataLevel();
    public enum LevelStatus { PLAYBLE, PROCESSING, END, };
    public enum NarrativeLineStatus { INQUEUE, PLAYED, NONE };
    public LevelStatus status = LevelStatus.PLAYBLE;

    public List<NarrativeLineStatus> lineStatus = new List<NarrativeLineStatus>();

    public float levelPlayTime;
    //Objects controlled by LevelMaster 
    [Header("Obj Hub")]
    public LevelObjHub hub;

    private void Update()
    {
        //test only
        if (InitTrigger)
        {
            InitTrigger = false;
            LevelInit();
        }
        if (PlayTrigger)
        {
            PlayTrigger = false;
            Play(PlayCoord);
            PlayCoord = Vector2Int.zero;
        }
        if (RewindTrigger)
        {
            RewindTrigger = false;
            LevelRewind();
        }
        if (Input.GetKeyUp(KeyCode.P))
        {
            SuccessTrigger = true;
        }
        if (SuccessTrigger)
        {
            SuccessTrigger = false;
            WinALevel();
        }
        levelPlayTime += Time.deltaTime;
    }
    //Key Actions assembling from atomic functions
    public void ObjectInit(GameObject _themeHub = null)
    {
        ThemeAnimationDelayAfterInit = dConstants.VFX.CallbackAnimationDelayAfterInit;
        ThemeAnimationDelayAfterPlay = dConstants.VFX.CallbackAnimationDelayAfterPlay;

        GetObjectReferences(_themeHub);
        themeLookupTable = Resources.Load<ThemeResourceLookup>("theme_prefabs/ThemeLookup");
        RegisterChildren();
        //GetDataReferences
        hub.miscMaster.InitThemeBackground();
    }
    public void LevelInit()
    {
        
        if (!levelData.LoadLevelFromSheetItem(levelSetupData))
        {
            return;
        }
        //setup board for a level
        DisablePlayerInput();
        OnlyInFirstEntry();
        InitBoardData();
        AdditionalGenerateBoard_Theme();
        GenerateBoard();
        InitCells();
        bool hasInitNarrative = InitNarrative();
        InitGoal();
        InitRuleset();
        InitTool();
        InitMiscs();
        AddtionalInit_Theme();
        UpdatePlayable();
        //Below should be called as calledback when all key FX is handled
        Sequence seq = DOTween.Sequence();
        seq.AppendInterval(hasInitNarrative ? ThemeAnimationDelayAfterInit + dConstants.VFX.AdditionalDelayWithNarrative : ThemeAnimationDelayAfterInit);
        seq.AppendCallback(() => LevelInitCallback());
        gameObject.SetActive(true);
    }
    public void LevelInitCallback()
    {
        EnablePlayerInput();
        ShowGoalSection();
        AnimateRuleset();
        DelayedInit_Theme();
        levelPlayTime = 0;
    }
    public void Play(Vector2Int coord)
    {
        //invalid check TO DO
        //play a tool on a given XY cell
        if(status != LevelStatus.PLAYBLE)
        {
            return;
        }
        DisablePlayerInput();
        ToolConsume(coord);
        HandlePlayerInput(coord);
        HandleEnvironment(coord);
        UpdateCells(coord);
        UpdateNarrative();
        UpdateGoal();
        UpdateRuleset();
        UpdateTool(coord);
        UpdateMiscs();
        AddtionalUpdate_Theme(coord);
        UpdatePlayable();
        //Below should be called as calledback when all key FX is handled
        Sequence seq = DOTween.Sequence();
        seq.AppendInterval(ThemeAnimationDelayAfterPlay);
        seq.AppendCallback(() => PlayCallback());
    }
    public void PlayCallback()
    {
        DelayedPlay_Theme();
        if (CheckWinCondition())
        {
            //Debug.Log("congrats! you win!");
            WinALevel();
        }
        else if (CheckLoseCondition())
        {
            //Debug.Log("ooops! you lose!");
            LoseALevel();
        }
        EnablePlayerInput();
    }
    public void LevelRetry()
    {
        DisablePlayerInput();
        InitBoardData();
        AdditionalGenerateBoard_Theme();
        GenerateBoard();
        InitCells();
        //InitNarrative();
        InitGoal();
        InitRuleset();
        InitTool();
        InitMiscs();
        AddtionalInit_Theme();
        UpdatePlayable();
        //This should be called as calledback when all key FX is handled
        Sequence seq = DOTween.Sequence();
        seq.AppendInterval(dConstants.VFX.CallbackAnimationDelayAfterPlay);
        seq.AppendCallback(() => LevelInitCallback());
    }
    public void LevelRewind()
    {
        DisablePlayerInput();
        Vector2Int lastPlayCoord = BoardRestore();
        InitCells();
        //InitNarrative();
        UpdateGoal();
        UpdateRuleset();
        InitTool();
        InitMiscs();
        AddtionalInit_Theme(true);
        UpdatePlayable();
        //Below should be called as calledback when all key FX is handled
        Sequence seq = DOTween.Sequence();
        seq.AppendInterval(dConstants.VFX.CallbackAnimationDelayAfterPlay);
        seq.AppendCallback(() => RewindCallback());
    }
    public void RewindCallback()
    {
        EnablePlayerInput();
    }
    public void LevelExit()
    {
        gameObject.SetActive(false);
        HiddenObjectLauncher.singleton.ClearExistingPages();
        LevelSelector.singleton.GoToSelector();
    }
    public void StartNextLevel()
    {
        //LevelSelector.singleton.FinishLevel(levelData.levelUID, levelData.isHard);
        hub.miscMaster.ScreenMaskFadeIn();
        bool result = false;
        Sequence seq = DOTween.Sequence();
        seq.AppendInterval(1.5f).AppendCallback(() => result = TryLoadNextLevel(false));
    }
    #region atomic methods
    //Atomic Funtions that can be overwritten by Theme-Specific LevlMaster Script
    public virtual void AlternativeMouseDown_Theme(Vector2Int coord)
    {

    }
    public virtual void AlternativeMouseHold_Theme(Vector2Int coord)
    {

    }
    public virtual void AlternativeMouseUp_Theme(Vector2Int coord)
    {

    }
    public virtual void GetObjectReferences(GameObject _themeHub)
    {
        hub = GetComponent<LevelObjHub>();
    }
    public void RegisterChildren()
    {
        GetComponent<MiscMaster>().RegisterLevelMaster(this);
        GetComponent<GoalMaster>().RegisterLevelMaster(this);
        GetComponent<PopupMaster>().RegisterLevelMaster(this);
    }
    public virtual void DisablePlayerInput()
    {
        status = LevelStatus.PROCESSING;
    }
    public virtual void EnablePlayerInput()
    {
        if(status != LevelStatus.END)
        {
            status = LevelStatus.PLAYBLE;
        }
    }
    public virtual void OnlyInFirstEntry()
    {
        hub.miscMaster.ScreenMaskInit();
        hub.miscMaster.ScreenMaskFadeOut(dConstants.VFX.CallbackAnimationDelayAfterInit);
        AudioCentralCtrl.singleton.PlayKeynote(levelData.themeIndex);
    }
    //init 2 board data(current, previous)
    public virtual void InitBoardData()
    {
        levelData.curBoard = new DataBoard(levelData.initBoard);
        levelData.previousBoard = null;
        levelData.previousBoards = new List<DataBoard>();
    }
    public virtual void GenerateBoard()
    //init the right number of cells and placed them in the right location
    {
        hub.boardMaster.levelMaster = this;
        hub.boardMaster.GenerateBoard_XbyY(levelData.curBoard.boardSize.x, levelData.curBoard.boardSize.y);
    }
    public virtual void AdditionalGenerateBoard_Theme()
    {
        //this should always be theme-specific
    }
    public virtual void InitCells()
    //setup all cells one by one
    {
        for(int i = 0; i < hub.boardMaster.cells.Count; i++)
        {
            DataCell temp_cellData = levelData.curBoard.GetCellDataByCoord(hub.boardMaster.cells[i].coord);
            if(temp_cellData != null)
            {
                hub.boardMaster.cells[i].DisplayNumber(temp_cellData.value);
            }
        }
    }
    public virtual bool InitNarrative()
    {
        bool hasInitNarrative = false;
        hub.narrativeMaster.title.SetText(LocalizedAssetLookup.singleton.Translate(levelData.title));
        //narrative lines
        hub.narrativeMaster.ClearAllLines();
        int indexOffset = levelData.oldNarratives.Count;
        for (int i = 0; i < hub.narrativeMaster.lines.Count; i++)
        {
            
            if(i < levelData.oldNarratives.Count)
            {
                hub.narrativeMaster.lines[i].SetText(LocalizedAssetLookup.singleton.Translate(levelData.oldNarratives[i]));
            }
            else
            {
                hub.narrativeMaster.lines[i].SetText("");
            }
        }
        if (levelData.newInitNarrative != null)
        {
            if (levelData.newInitNarrative.Length > 0)
            {
                hasInitNarrative = true;
                hub.narrativeMaster.TypeALine(levelData.newInitNarrative);
            }
        }
        lineStatus.Clear();
        if (levelData.newPlayNarratives.Count > 0)
        {
            for(int i=0;i< levelData.newPlayNarratives.Count; i++)
            {
                lineStatus.Add(NarrativeLineStatus.INQUEUE);
            }
        }
        LayoutRebuilder.ForceRebuildLayoutImmediate(hub.narrativeMaster.group);
        return hasInitNarrative;
    }
    public virtual void InitGoal()
    {
        hub.goalMaster.title.SetText(LocalizedAssetLookup.singleton.Translate("@Loc=ui_goal_section_title@@"));
        hub.goalMaster.lines[0].SetText(LocalizedAssetLookup.singleton.Translate(levelData.goal));
        hub.goalMaster.lines[1].SetText("");

        hub.goalMaster.goalLayout.gameObject.SetActive(false);
    }
    public virtual void InitRuleset()
    {
        //hub.rulesetMaster.ruleTitle.SetRuleLine("@Loc=ui_ruleset_title@@");
        for (int i = 0; i < hub.rulesetMaster.ruleDescs.Count; i++)
        {
            if(i < levelData.ruleset.Count)
            {
                hub.rulesetMaster.ruleDescs[i].SetRuleLine(levelData.ruleset[i]);
                hub.rulesetMaster.ruleDescs[i].gameObject.SetActive(true);
                if (levelData.ruleset[i].tag != RuleItem.RuleItemTag.none)
                {
                    hub.rulesetMaster.ruleDescs[i].InitLineAnimation();
                }
            }
            else
            {
                hub.rulesetMaster.ruleDescs[i].gameObject.SetActive(false);
            }
        }
    }
    public virtual void InitTool()
    {
        hub.toolMaster.toolIcon.sprite = themeLookupTable.GetThemeIcon(levelData.themeIndex);
        hub.toolMaster.toolTitle.SetText(LocalizedAssetLookup.singleton.Translate(levelData.theme));
        hub.toolMaster.toolDesc.SetText(string.Format("x{0}", levelData.curBoard.toolCount));

        hub.toolMaster.toolSubtitle.SetText("");
        hub.toolMaster.infographGroup.SetActive(false);
    }
    public virtual void InitMiscs()
    {
        hub.popupMaster.ResetAllPopups();
        hub.miscMaster.ResetMiscs();
        //show rewind btn only if it can be rewinded
        hub.miscMaster.rewindBtn.SetActive(levelData.canRewind());
        InitLevelNavigation();
        //set new theme hint to be true if there are at least 1 unlockable themes;
        hub.miscMaster.newthemeHint.gameObject.SetActive(LevelSelector.singleton.LocateFirstUnlockableTheme() >= 0);
    }
    public virtual void AddtionalInit_Theme(bool isRewind = false)
    {
        //this should always be theme-specific
    }
    
    public virtual void AnimateRuleset()
    {
        float delayPerLine = 0.3f;
        float curDelay = 0f;
        for (int i = 0; i < hub.rulesetMaster.ruleDescs.Count; i++)
        {
            if (i < levelData.ruleset.Count)
            {
                if(levelData.ruleset[i].tag != RuleItem.RuleItemTag.none)
                {
                    hub.rulesetMaster.ruleDescs[i].AnimateLine(curDelay);
                    curDelay += delayPerLine;
                }
            }
        }
    }
    public virtual void ShowGoalSection()
    {
        hub.goalMaster.goalLayout.gameObject.SetActive(true);
        LayoutRebuilder.ForceRebuildLayoutImmediate(hub.goalMaster.goalLayout);
    }
    public virtual void DelayedInit_Theme()
    {
        //this should always be theme-specific
    }
    public virtual void ToolConsume(Vector2Int coord)
    {
        //consume tool and save curboard to previous board/boards
        levelData.curBoard.curPlayCoord = coord;
        levelData.previousBoard = new DataBoard(levelData.curBoard);
        if (levelData.allowRewind)
        {
            levelData.previousBoards.Add(new DataBoard(levelData.curBoard));
        }
        levelData.curBoard.toolCount -= 1;
    }
    public virtual Vector2Int BoardRestore()
    {
        
        levelData.curBoard = levelData.previousBoards[levelData.previousBoards.Count - 1];
        levelData.previousBoards.RemoveAt(levelData.previousBoards.Count - 1);
        if(levelData.previousBoards.Count > 0)
        {
            levelData.previousBoard = levelData.previousBoards[levelData.previousBoards.Count - 1];
        }
        else
        {
            levelData.previousBoard = new DataBoard();
        }

        return levelData.curBoard.curPlayCoord;
    }
    public virtual void HandlePlayerInput(Vector2Int coord)
    {
        levelData.curBoard.GetCellDataByCoord(coord).value += 1;
        
        //this should always be theme-specific
    }
    public virtual void HandleEnvironment(Vector2Int coord)
    {
        //this should always be theme-specific
    }
    
    public virtual void UpdateCells(Vector2Int coord)
    {
        for (int i = 0; i < hub.boardMaster.cells.Count; i++)
        {
            DataCell temp_cellData = levelData.curBoard.GetCellDataByCoord(hub.boardMaster.cells[i].coord);
            if (temp_cellData != null)
            {
                if(temp_cellData.value != hub.boardMaster.cells[i].curNumber)
                {
                    hub.boardMaster.cells[i].NumberShift(temp_cellData.value);
                }
                //hub.boardMaster.cells[i].numberTxt.SetText(temp_cellData.value.ToString());
            }
        }
    }
    public virtual void UpdateNarrative()
    {
        //trigger 1st play
        if(levelData.curBoard.toolCount <= levelData.initBoard.toolCount / 2f && lineStatus.Count >= 1)
        {
            if (lineStatus[0] == NarrativeLineStatus.INQUEUE)
            {
                TryTypeNextPlayLine(0);
                lineStatus[0] = NarrativeLineStatus.PLAYED;
            }
        }
        //trigger 2nd play
        if (levelData.curBoard.toolCount <= levelData.initBoard.toolCount / 4f && lineStatus.Count >= 2)
        {
            if (lineStatus[1] == NarrativeLineStatus.INQUEUE)
            {
                TryTypeNextPlayLine(1);
                lineStatus[1] = NarrativeLineStatus.PLAYED;
            }
        }
        //trigger 3rd play
        if (levelData.curBoard.toolCount <= levelData.initBoard.toolCount / 8f && lineStatus.Count >= 3)
        {
            if (lineStatus[2] == NarrativeLineStatus.INQUEUE)
            {
                TryTypeNextPlayLine(2);
                lineStatus[2] = NarrativeLineStatus.PLAYED;
            }
        }
    }
    public virtual void UpdateGoal()
    {
        //not needed so far
    }
    public virtual void UpdateRuleset()
    {
        //not needed so far
    }
    public virtual void UpdateTool(Vector2Int coord)
    {
        if(levelData.curBoard.toolCount != levelData.previousBoard.toolCount)
        {
            hub.toolMaster.toolDesc.SetText(string.Format("x{0}", levelData.curBoard.toolCount));
        }
    }
    public virtual void UpdateMiscs()
    {
        hub.miscMaster.rewindBtn.SetActive(levelData.canRewind());
    }
    public virtual void AddtionalUpdate_Theme(Vector2Int coord)
    {
        //this should always be theme-specific
    }
    public virtual void UpdatePlayable()
    {
        
    }
    public virtual void DelayedPlay_Theme()
    {
        //this should always be theme-specific
    }
    public virtual bool CheckWinCondition()
    {
        return false;
    }
    public virtual void WinALevel()
    {
        status = LevelStatus.END;
        //hub.goalMaster.nextBtn.gameObject.SetActive(true);
        bool isNew = LevelSelector.singleton.FinishLevel(levelData.levelUID);
        string finalPlayTime = string.Format("{0}:{1}:{2}"
            , Mathf.FloorToInt(levelPlayTime / 60).ToString("00")
            , (Mathf.FloorToInt(levelPlayTime) % 60).ToString("00")
            , Mathf.RoundToInt((levelPlayTime - Mathf.FloorToInt(levelPlayTime)) * 60f).ToString("00"));
        hub.popupMaster.ShowVictoryPopup(isNew, levelData.isHard, finalPlayTime, (levelData.initBoard.toolCount - levelData.curBoard.toolCount).ToString());
        /*if (!result)
        {
            Debug.Log(string.Format("Theme ends. Plz go back to theme selector"));
        }*/
        AudioCentralCtrl.singleton.PlayGenericLevelWinSFX();
    }
    public virtual bool CheckLoseCondition()
    {
        if(levelData.curBoard.toolCount == 0)
        {
            return true;
        }
        return false;
    }
    public virtual void LoseALevel()
    {
        status = LevelStatus.END;
        hub.popupMaster.ShowFailurePopup(levelData.canRewind());
        //hub.miscMaster.retryHint.gameObject.SetActive(true);
        //hub.miscMaster.loseBanner.SetActive(true);
        //hub.miscMaster.loseBanner.GetComponent<SpriteRenderer>().DOFade(0f, 0.5f).From();
        AudioCentralCtrl.singleton.PlayGenericLevelFailSFX();
    }
    #endregion

    bool TryLoadNextLevel(bool isNavigation = false)
    {
        if(levelSetupData.nextLevelIndex > 0)
        {
            if (levelSetupData.nextLevel.isHard && !isNavigation && !levelSetupData.isHard)
            {
                //return to main if next level is hard level
                LevelExit();
                return false;
            }
            else if (levelSetupData.nextLevelIndex > 0 && levelSetupData.nextLevel)
            {
                levelSetupData = levelSetupData.nextLevel;
                LevelInit();
                return true;
            }
        }
        else
        {
            LevelExit();
            return false;
        }
        return false;

    }
    bool TryLoadPreviousLevel()
    {
        //temp method, will be revamp with level launcher
        if (levelSetupData.previousLevel)
        {
            levelSetupData = levelSetupData.previousLevel;
            LevelInit();
            return true;
        }
        else
        {
            LevelExit();
        }
        return false;
    }

    public bool TryTypeNextPlayLine(int index)
    {
        try
        {
            hub.narrativeMaster.TypeALine(levelData.newPlayNarratives[index]);
            return true;
        }
        catch (System.Exception e)
        {
            Debug.LogError(string.Format("invalid play narrative caught on level ID ({0})", levelData.levelUID));
            return false;
        }
    }
    public virtual void ShowCellSumAtGoal()
    {
        hub.goalMaster.lines[1].SetText(string.Format("{0}{1}", LocalizedAssetLookup.singleton.Translate("@Loc=ui_goal_current_sum@@"), levelData.curBoard.CurrentSum()));
        hub.goalMaster.lines[1].gameObject.SetActive(true);
        LayoutRebuilder.ForceRebuildLayoutImmediate(hub.goalMaster.goalLayout);
    }
    void InitLevelNavigation()
    {
        hub.miscMaster.prevBtn.SetActive(false);
        if (levelSetupData.previousLevel != null)
        {
            //Debug.Log(string.Format("previous level ID is {0}", levelSetupData.previousLevel.levelUID));
            if (LevelLauncher.singleton.playerRecords.isLevelPlayable(levelSetupData.previousLevel.levelUID))
            {
                hub.miscMaster.prevBtn.SetActive(true);
            }
        }
        hub.miscMaster.nextBtn.SetActive(false);
        if (levelSetupData.nextLevel != null)
        {
            //Debug.Log(string.Format("next level ID is {0}", levelSetupData.nextLevel.levelUID));
            if (LevelLauncher.singleton.playerRecords.isLevelPlayable(levelSetupData.nextLevel.levelUID))
            {
                hub.miscMaster.nextBtn.SetActive(true);
            }
        }
        hub.miscMaster.levelName.SetText(string.Format("{0} <size=60%>{1}</size>{2}", 
            LocalizedAssetLookup.singleton.Translate(levelData.theme),
            LocalizedAssetLookup.singleton.Translate(levelData.title), 
            levelData.isHard? " <sprite name=currency_gem>":""));
    }
    public void GoToPreviousLevel()
    {
        TryLoadPreviousLevel();
    }
    public void GoToNextLevel()
    {
        TryLoadNextLevel(true);
    }
}
