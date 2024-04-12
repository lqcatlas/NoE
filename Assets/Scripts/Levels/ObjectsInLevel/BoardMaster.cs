using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class BoardMaster : MonoBehaviour
{
    [Header("Static Params")]
    [SerializeField] float StandardizedXYSize = 3;
    [SerializeField] int MinXYSize = 2;
    //use buffer to make smaller board looker bigger but not as same big as the standardized board 
    [SerializeField] float ResizeBuffer = 0.4f;
    [SerializeField] float ResizeBuffer_Large = 0.55f;
    public GameObject cellTempalte;
    [Header("Master Objs")]
    public LevelMasterBase levelMaster;

    [Header("Children Objs")]
    public List<CellMaster> cells;
    public Transform cellHolder;
    public Transform boardHolder;

    //BoardMaster should handle tool and board data as well TODO

    /*private void Start()
    {
        //test
        //GenerateBoard_XbyY(3, 3);
    }*/

    public void GenerateBoard_XbyY(int x, int y)
    {
        //clear previous cells
        List<CellMaster> oldCells = cellHolder.GetComponentsInChildren<CellMaster>().ToList();
        for(int i=0;i< oldCells.Count; i++)
        {
            Destroy(oldCells[i].gameObject);
        }
        cells.Clear();
        //generate all cells from template
        for (int i=0; i < x; i++)
        {
            for(int j = 0; j < y; j++)
            {
                InstantiateCell_atXY(new Vector2Int(i, j), new Vector2Int(x, y));
            }
        }
        //resize holder
        if(Mathf.Max(MinXYSize, x, y) <= 5)
        {
            cellHolder.localScale = Vector3.one * Mathf.Pow(StandardizedXYSize / Mathf.Max(MinXYSize, x, y), ResizeBuffer);
        }
        else
        {
            cellHolder.localScale = Vector3.one * Mathf.Pow(StandardizedXYSize / Mathf.Max(MinXYSize, x, y), ResizeBuffer_Large);
        }
    }

    void InstantiateCell_atXY(Vector2Int cellCoord, Vector2Int boardSize)
    {
        CellMaster obj = Instantiate(cellTempalte, cellHolder).GetComponent<CellMaster>();
        obj.gameObject.name = string.Format("cell_gen_{0}_{1}", cellCoord.x, cellCoord.y);
        obj.InitCellPosition(cellCoord, boardSize);
        obj.RegisterLevelMaster(levelMaster);
        cells.Add(obj);
    }
}
