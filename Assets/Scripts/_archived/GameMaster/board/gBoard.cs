using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class gBoard : MonoBehaviour
{
    [Header("Debug Mode")]
    [SerializeField] bool showPreview;

    [Header("Master")]
    public GameMaster GM;

    [Header("Boards")]
    public dBoard curBoard;
    public dBoard previewBoard;
    public dBoard initBoard;
    public bool failCondition;

    [Header("Tool")]
    public GameObject toolIcon1;
    public GameObject toolIcon2;
    public TextMeshProUGUI curToolCount;

    [Header("Cells")]
    
    public List<gCell> cells;

    // Start is called before the first frame update
    void Start()
    {
        //ResetCurBoard();
    }


    public void ResetCurBoard()
    {
        curBoard.SetBoard(initBoard);
        previewBoard.SetBoard(initBoard);
        failCondition = FailCheck();
        InitCells();
        UpdateCells();
    }

    //test logic suppose to be override by each own tool script
    public void PreviewPlay(int coord)
    {
        GM.curPuzzle.Play(coord, previewBoard);
        UpdateCells();
    }
    public void PreviewPlayEnd()
    {
        previewBoard.SetBoard(curBoard);
        UpdateCells();
    }
    public void ToolPlay(int coord)
    {
        if (!failCondition)
        {
            GM.curPuzzle.Play(coord, curBoard);
            previewBoard.SetBoard(curBoard);
            UpdateCells();
            ToolPlayFX();
            failCondition = FailCheck();
            if (WinCheck())
            {
                Debug.Log(string.Format("puzzle {0} is beaten! Congrats!", initBoard.boardName));
                GM.NextPuzzle();
            }
            else if (failCondition)
            {
                GM.CurrentPuzzleFailed();
                Debug.Log(string.Format("puzzle {0} is lost! Plz Retry!", initBoard.boardName));
            }
        }
        else
        {
            Debug.Log(string.Format("puzzle {0} is played after failure.", initBoard.boardName));
        }
    }
    bool FailCheck()
    {
        return GM.curPuzzle.FailCheck(curBoard);
    }
    bool WinCheck()
    {
        //Debug.Log(string.Format("end check returning {0}", GM.curPuzzle.GoalCheck(curBoard)));
        return GM.curPuzzle.GoalCheck(curBoard);
    }
    public void InitCells()
    {
        for (int i = 0; i < cells.Count; i++)
        {
            cells[i].CellInit();
        }
    }
    public void ToolPlayFX()
    {
        float duration = 0.5f;
        float strength = 180f;
        if (GM.puzzleTheme == dConstants.GameTheme.Coin && GM.puzzleIndex >= 2)
        {
            if (toolIcon1.activeSelf)
            {
                toolIcon1.GetComponent<RectTransform>().DOShakeRotation(duration, new Vector3(1f, 0.2f, 0) * strength);
                //Debug.Log("1 rotation shake triggered");
            }
            else
            {
                toolIcon2.GetComponent<RectTransform>().DOShakeRotation(duration, new Vector3(1f, 0.2f, 0) * strength);
                //Debug.Log("2 rotation shake triggered");
            }
        }   
    }
    public void UpdateCells()
    {
        if (showPreview)
        {
            for (int i = 0; i < cells.Count; i++)
            {
                cells[i].UpdateDisplay(previewBoard.GetCellByCoord(cells[i].coord), GM.puzzleTheme);
            }
            curToolCount.SetText(string.Format("x{0}", previewBoard.toolCount));
            if (GM.puzzleTheme == dConstants.GameTheme.Coin)
            {
                toolIcon1.SetActive(previewBoard.toolStatus == 0);
                toolIcon2.SetActive(previewBoard.toolStatus == 1);
            }
            else
            {
                toolIcon1.SetActive(true);
                toolIcon2.SetActive(false);
            }
        }
        else
        {
            for (int i = 0; i < cells.Count; i++)
            {
                cells[i].UpdateDisplay(curBoard.GetCellByCoord(cells[i].coord), GM.puzzleTheme);
            }
            curToolCount.SetText(string.Format("x{0}", curBoard.toolCount));
            if (GM.puzzleTheme == dConstants.GameTheme.Coin)
            {
                toolIcon1.SetActive(curBoard.toolStatus == 0);
                toolIcon2.SetActive(curBoard.toolStatus == 1);
                
            }
            else
            {
                toolIcon1.SetActive(true);
                toolIcon2.SetActive(false);
            }
        }

        
    }
    
}
