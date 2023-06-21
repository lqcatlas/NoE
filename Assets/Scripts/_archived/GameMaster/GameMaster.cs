using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

[System.Serializable]
public class BoardOption
{
    public int boardID;
    public gBoard boardObj;
}

[System.Serializable]
public class PuzzleItem
{
    public string puzzleID;
    public gPuzzle puzzleObj;
    //public string nextPuzzleID;
}
public class GameMaster : MonoBehaviour
{
    // in charge of:
    // set up a given puzzle to the board
    // handle player input inside a puzzle
    // check puzzle end
    [Header("Debug")]
    public bool skipPuzzle;

    [Header("Data")]

    public bool isActivePuzzle;
    public dConstants.GameTheme puzzleTheme;
    public int puzzleIndex;
    public gPuzzle curPuzzle;
    public int curBoardID;
    public gBoard curBoardRef;
    public GameObject keynote;
    //public bool puzzleFail;

    public List<PuzzleItem> puzzles;

    [Header("Children Objs")]
    public GameObject GameBoardUISet;
    public List<BoardOption> boardOpts;
    public TextMeshProUGUI title;
    public TextMeshProUGUI goalDesc;
    public TextMeshProUGUI toolDesc;
    public TextMeshProUGUI narrative;
    public TextMeshProUGUI failHint;
    public Curtain curtain;
    public GameObject failBlock;
    public GameObject winBanner;
    //public TextMeshProUGUI toolCount;
    //tool icon to do

    // Start is called before the first frame update
    void Start()
    {
        GameBoardUISet.SetActive(false);
        curtain.CurtainOn();
        gameObject.SetActive(false);
        isActivePuzzle = false;
    }
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.P))
        {
            skipPuzzle = true;
        }
        if (skipPuzzle && isActivePuzzle)
        {
            NextPuzzle();
            skipPuzzle = false;
        }
    }
    public void InitFirstPuzzle()
    {
        narrative.SetText("");
        puzzleIndex = 0;
        InitPuzzleByID("start_puzzle");
        GameBoardUISet.SetActive(true);
        //play keynote
        keynote.SetActive(false);
        keynote.SetActive(true);
        //
        isActivePuzzle = true;
        gameObject.SetActive(true);
    }
    public void PuzzleThemeEnd()
    {
        isActivePuzzle = false;
    }
    public void NextPuzzle()
    {
        curtain.CurtainOn();
        if(puzzles.Count == puzzleIndex + 1)
        {
            //no more puzzle
            Sequence seq = DOTween.Sequence();
            seq.AppendInterval(1f).AppendCallback(() => winBanner.SetActive(true));
        }
        else
        {
            Sequence seq = DOTween.Sequence();
            seq.AppendInterval(0.5f).AppendCallback(() => InitPuzzle(puzzles[++puzzleIndex].puzzleObj));
        }
        //play keynote
        keynote.SetActive(false);
        keynote.SetActive(true);
        //

    }
    public void InitPuzzleByID(string targetID)
    {
        for(int i = 0; i < puzzles.Count; i++)
        {
            if (puzzles[i].puzzleID == targetID)
            {
                InitPuzzle(puzzles[i].puzzleObj);
                return;
            }
        }
        Debug.LogError(string.Format("no puzzleID {0} is found", targetID));
    }
    public void ResetCurPuzzle()
    {
        curBoardRef.ResetCurBoard();
        failHint.gameObject.SetActive(false);
        failBlock.SetActive(false);
    }
    void InitPuzzle(gPuzzle puzzle)
    {
        curPuzzle = puzzle;
        curBoardID = DetermineBoardSize(puzzle);
        curBoardRef = null;

        //active ans setup correct board
        for (int i=0;i< boardOpts.Count; i++)
        {
            boardOpts[i].boardObj.gameObject.SetActive(false);
            if (boardOpts[i].boardID == curBoardID)
            {
                curBoardRef = boardOpts[i].boardObj;
            }
        }
        if(curBoardRef == null)
        {
            Debug.LogError(string.Format("GameMaster: no valid board options is found"));
            return;
        }
        curBoardRef.initBoard = curPuzzle.puzzleBoard;
        curBoardRef.ResetCurBoard();
        curBoardRef.gameObject.SetActive(true);

        //update puzzle text info
        title.SetText(LocalizedAssetLookup.singleton.Translate(curPuzzle.title));
        goalDesc.SetText(LocalizedAssetLookup.singleton.Translate(curPuzzle.goalDesc));
        toolDesc.SetText(LocalizedAssetLookup.singleton.Translate(curPuzzle.toolDesc));
        toolDesc.ForceMeshUpdate();
        failHint.gameObject.SetActive(false);
        failBlock.SetActive(false);

        for (int i=0;i< curPuzzle.narratives.Count; i++)
        {
            narrative.text += curPuzzle.narratives[i];
            narrative.text += "<br><br>";
        }
        curtain.CurtainOff();
    }
    public void CurrentPuzzleFailed()
    {
        failHint.gameObject.SetActive(true);
        failBlock.SetActive(true);
    }
    int DetermineBoardSize(gPuzzle puzzle)
    {
        int boardID = 0;
        if(puzzle.puzzleBoard.boardSizeX != puzzle.puzzleBoard.boardSizeY)
        {
            Debug.LogError(string.Format("GameMaster: no sqaure board is not supported"));
            return 0;
        }
        else
        {
            boardID = curPuzzle.puzzleBoard.boardSizeX;
        }
        return boardID;
    }
    
}
