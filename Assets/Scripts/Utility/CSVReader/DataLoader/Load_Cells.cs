using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Load_Cells : MonoBehaviour
{
    public CSVSheetData dataSheet = new CSVSheetData();
    [SerializeField] string filename_cells;
    public List<SheetItem_LevelSetup> levels;
    [SerializeField] List<int> levelUIDIncluded;

#if UNITY_EDITOR
    private void Start()
    {
        //load level data
        dataSheet.sheet = CSVReader.Read("csv/" + filename_cells);
        dataSheet.sheetName = filename_cells;
        levels = new List<SheetItem_LevelSetup>();
        levels.Clear();
        levels.AddRange(Resources.LoadAll<SheetItem_LevelSetup>("dataFromCSV").ToList());
        levelUIDIncluded = GetAllUpdatedLevels(dataSheet);
        for(int i = 0; i < levels.Count; i++)
        {
            if (levelUIDIncluded.Contains(levels[i].levelUID))
            {
                UpdateCellDataByLevelFromCSV(levels[i], dataSheet);
            }
        }
        Debug.Log(string.Format("cells in level setup data updated. total of {0} level processed", levelUIDIncluded.Count));
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
    int GetUIDPerCell(int levelIndex, int cellIndex)
    {
        return levelIndex * 1000 + cellIndex;
    }
    List<int> GetAllUpdatedLevels(CSVSheetData csv)
    {
        List<int> levelUIDs = new();
        for(int i = 0; i < csv.sheet.Count; i++)
        {
            csv.GetLineByRow(i).TryGetValue("leveluid", out object result);
            int.TryParse((string)result, out int levelUID);
            if (!levelUIDs.Contains(levelUID))
            {
                levelUIDs.Add(levelUID);
            }
        }
        //Debug.Log(string.Format("{0} levels included in Cells csv", levelUIDIncluded.Count));
        return levelUIDs;
    }
    void UpdateCellDataByLevelFromCSV(SheetItem_LevelSetup level, CSVSheetData csv)
    {
        for(int i = 0; i < level.initBoard.cells.Count; i++)
        {
            Dictionary<string, object> cellDic = csv.GetLineByUID(GetUIDPerCell(level.levelUID, i));
            if (cellDic == null)
            {
                Debug.LogError(string.Format("unbale to load data from csv for uid({0})", GetUIDPerCell(level.levelUID, i)));
                return;
            }
            object result = null;
            int coord_x = -1;
            int coord_y = -1;
            if (cellDic.TryGetValue("x", out result))
            {
                //Debug.Log(result);
                int.TryParse((string)result, out coord_x);
            }
            result = null;
            if (cellDic.TryGetValue("y", out result))
            {
                //Debug.Log(result);
                int.TryParse((string)result, out coord_y);
            }
            level.initBoard.cells[i].coord = new Vector2Int(coord_x, coord_y);
            result = null;
            if (cellDic.TryGetValue("value", out result))
            {
                int.TryParse((string)result, out level.initBoard.cells[i].value);
            }
            result = null;
            if (cellDic.TryGetValue("status", out result))
            {
                int.TryParse((string)result, out level.initBoard.cells[i].status);
            }
        }
    } 
#endif
}
