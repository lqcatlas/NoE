using DG.Tweening;
using System;
using TMPro;
using Unity.Mathematics;
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
    
    //Data setup by launcher before calling LevelInit()
    [Header("Setup Data")]
    public SheetItem_LevelSetup levelSetupData;
    public float ThemeAnimationDelayAfterInit;
    public float ThemeAnimationDelayAfterPlay;

    [Header("In-Level Data")]
    public DataLevel levelData = new DataLevel();
    public enum LevelStatus { PLAYBLE, PROCESSING, END, };
    public LevelStatus status = LevelStatus.PLAYBLE;

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
            Rewind();
        }
    }
    //Key Actions assembling from atomic functions
    public void ObjectInit(GameObject _themeHub = null)
    {
        GetObjectReferences(_themeHub);
        RegisterChildren();
        //GetDataReferences
        ThemeAnimationDelayAfterInit = dConstants.VFX.CallbackAnimationDelayAfterInit;
        ThemeAnimationDelayAfterPlay = dConstants.VFX.CallbackAnimationDelayAfterPlay;
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
        GenerateBoard();
        InitCells();
        InitNarrative();
        InitGoal();
        InitRuleset();
        InitTool();
        InitMiscs();
        AddtionalInit_Theme();
        UpdatePlayable();
        //Below should be called as calledback when all key FX is handled
        Sequence seq = DOTween.Sequence();
        seq.AppendInterval(ThemeAnimationDelayAfterInit);
        seq.AppendCallback(() => LevelInitCallback());
        gameObject.SetActive(true);
    }
    public void LevelInitCallback()
    {
        EnablePlayerInput();
        AnimateRuleset();
        DelayedInit_Theme();
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
        ToolConsume();
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
        EnablePlayerInput();
        DelayedPlay_Theme();
        if (CheckWinCondition())
        {
            Debug.Log("congrats! you win!");
            WinALevel();
        }
        else if (CheckLoseCondition())
        {
            Debug.Log("ooops! you lose!");
            LoseALevel();

        }
    }
    public void LevelRetry()
    {
        InitBoardData();
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
    public void Rewind()
    {
        //invalid check TO DO
        //reset one step
    }
    public void LevelExit()
    {
        gameObject.SetActive(false);
        LevelSelector.singleton.SelectorShow();
    }
    #region atomic methods
    //Atomic Funtions that can be overwritten by Theme-Specific LevlMaster Script
    public virtual void GetObjectReferences(GameObject _themeHub)
    {
        hub = GetComponent<LevelObjHub>();
    }
    public void RegisterChildren()
    {
        GetComponent<MiscMaster>().RegisterLevelMaster(this);
    }
    public virtual void DisablePlayerInput()
    {
        status = LevelStatus.PROCESSING;
    }
    public virtual void EnablePlayerInput()
    {
        status = LevelStatus.PLAYBLE;
    }
    public virtual void OnlyInFirstEntry()
    {
        hub.miscMaster.ScreenMaskInit();
        hub.miscMaster.ScreenMaskFadeOut(dConstants.VFX.CallbackAnimationDelayAfterInit);
        AudioDraft.singleton.PlayKeynote(levelData.themeIndex);
    }
    //init 2 board data(current, previous)
    public virtual void InitBoardData()
    {
        levelData.curBoard = new DataBoard(levelData.initBoard);
        levelData.previousBoard = null;
    }
    public virtual void GenerateBoard()
    //init the right number of cells and placed them in the right location
    {
        hub.boardMaster.levelMaster = this;
        hub.boardMaster.GenerateBoard_XbyY(levelData.initBoard.boardSize.x, levelData.initBoard.boardSize.y);
    }
    public virtual void InitCells()
    //setup all cells one by one
    {
        for(int i = 0; i < hub.boardMaster.cells.Count; i++)
        {
            DataCell temp_cellData = levelData.initBoard.GetCellDataByCoord(hub.boardMaster.cells[i].coord);
            if(temp_cellData != null)
            {
                hub.boardMaster.cells[i].numberTxt.SetText(temp_cellData.value.ToString());
            }
        }
    }
    public virtual void InitNarrative()
    {
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
        if(levelData.newInitNarrative.Length > 0)
        {
            hub.narrativeMaster.TypeALine(levelData.newInitNarrative);
        }
    }
    public virtual void InitGoal()
    {
        hub.goalMaster.title.SetText(LocalizedAssetLookup.singleton.Translate("@Loc=ui_goal_section_title@@"));
        hub.goalMaster.lines[0].SetText(LocalizedAssetLookup.singleton.Translate(levelData.goal));
        hub.goalMaster.lines[1].gameObject.SetActive(false);
        LayoutRebuilder.ForceRebuildLayoutImmediate(hub.goalMaster.goalLayout);

    }
    public virtual void InitRuleset()
    {
        /*string rulesetCombined = "";
        for(int i=0;i< levelData.ruleset.Count; i++)
        {
            rulesetCombined += levelData.ruleset[i].ruleDesc;
            rulesetCombined += "<br>";
        }
        hub.rulesetMaster.rulesetDesc.SetText(string.Format("<size=120%>{0}</size>£º<br><indent=5%>{1}", levelData.theme, rulesetCombined));
        */
        hub.rulesetMaster.ruleTitle.SetRuleLine(levelData.theme);
        for (int i = 0; i < hub.rulesetMaster.ruleDescs.Count; i++)
        {
            if(i < levelData.ruleset.Count)
            {
                hub.rulesetMaster.ruleDescs[i].SetRuleLine(levelData.ruleset[i]);
                hub.rulesetMaster.ruleDescs[i].gameObject.SetActive(true);
            }
            else
            {
                hub.rulesetMaster.ruleDescs[i].gameObject.SetActive(false);
            }
        }
    }
    public virtual void InitTool()
    {
        hub.toolMaster.toolDesc.SetText(string.Format("x{0}", levelData.initBoard.toolCount));
    }
    public virtual void InitMiscs()
    {
        hub.miscMaster.closeBtn.SetActive(true);
        hub.miscMaster.retryHint.gameObject.SetActive(false);
        hub.miscMaster.retryBtn.SetActive(true);
        hub.miscMaster.loseBanner.SetActive(false);
    }
    public virtual void AddtionalInit_Theme()
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
    public virtual void DelayedInit_Theme()
    {
        //this should always be theme-specific
    }
    public virtual void ToolConsume()
    {
        levelData.previousBoard = new DataBoard(levelData.curBoard);
        levelData.curBoard.toolCount -= 1;
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
                if(temp_cellData.value.ToString() != hub.boardMaster.cells[i].numberTxt.text)
                {
                    NumberShift(hub.boardMaster.cells[i].numberTxt, temp_cellData.value);
                }
                //hub.boardMaster.cells[i].numberTxt.SetText(temp_cellData.value.ToString());
            }
        }
        //TODO how to create anim fx for cell that number changes
    }
    public virtual void UpdateNarrative()
    {
        //not needed so far
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
        //not needed so far
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
        LevelSelector.singleton.FinishLevel(levelData.levelUID);

        hub.miscMaster.ScreenMaskFadeIn();
        bool result = false;
        Sequence seq = DOTween.Sequence();
        seq.AppendInterval(1.5f).AppendCallback(() => result = TryLoadNextLevel());

        
        /*if (!result)
        {
            Debug.Log(string.Format("Theme ends. Plz go back to theme selector"));
        }*/
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
        hub.miscMaster.retryHint.gameObject.SetActive(true);
        hub.miscMaster.loseBanner.SetActive(true);
        hub.miscMaster.loseBanner.GetComponent<SpriteRenderer>().DOFade(0f, 0.5f).From();
    }
    #endregion

    #region tool methods
    public void NumberShift(TextMeshPro tmp, int endValue)
    {
        Sequence seq = DOTween.Sequence();
        for(int i=0;i< dConstants.VFX.NumberShiftAnimCount; i++)
        {
            seq.AppendCallback(() => tmp.SetText(UnityEngine.Random.Range(Mathf.Min(1,Mathf.FloorToInt(endValue/10f))*10+1, endValue).ToString()));
            seq.AppendInterval(dConstants.VFX.NumberShiftAnimInterval);
        }
        seq.AppendInterval(UnityEngine.Random.Range(dConstants.VFX.NumberShiftAnimInterval*0.05f, dConstants.VFX.NumberShiftAnimInterval));
        seq.AppendCallback(() => tmp.SetText(endValue.ToString()));
    }
    #endregion
    bool TryLoadNextLevel()
    {
        //temp method, will be revamp with level launcher
        if(levelSetupData.nextLevelIndex > 0 && levelSetupData.nextLevel)
        {
            levelSetupData = levelSetupData.nextLevel;
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
        catch (Exception e)
        {
            Debug.LogError(string.Format("invalid play narrative caught on level ID ({0})", levelData.levelUID));
            return false;
        }
    }
    
}
