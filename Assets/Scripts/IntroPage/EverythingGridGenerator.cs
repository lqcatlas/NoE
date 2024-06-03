using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EverythingGridGenerator : MonoBehaviour
{
    private int lengthPerStep = 10;
    [SerializeField] int curStep = 0;
    [SerializeField] GameObject cellTemplate;
    [SerializeField] List<GameObject> cells;

    private void Start()
    {
        curStep = 0;
    }
    public void ClearAllCells()
    {
        for (int i = 0; i < cells.Count; i++)
        {
            Destroy(cells[i]);
        }
        cells.Clear();
    }
    public void GenerateNextSetCells()
    {
        int targetStep = ++curStep;
        for(int i = -targetStep; i <= targetStep; i++)
        {
            int x = i;
            if(targetStep == Mathf.Abs(x))
            {
                int y0 = 0;
                //x, y0
                cells.Add(GenerateCellAtCoord(x, y0));
            }
            else
            {
                int y1 = targetStep - Mathf.Abs(x);
                int y2 = Mathf.Abs(x) - targetStep;
                //x, y1
                cells.Add(GenerateCellAtCoord(x, y1));
                //x, y2
                cells.Add(GenerateCellAtCoord(x, y2));
            }
        }
    }
    GameObject GenerateCellAtCoord(int x, int y)
    {
        GameObject obj = Instantiate(cellTemplate, transform);
        obj.transform.localPosition = new Vector3(x * lengthPerStep, y * lengthPerStep, 0);
        obj.GetComponent<EverythingCell>().UpdateSprite();
        obj.SetActive(true);
        return obj;
    }
}
