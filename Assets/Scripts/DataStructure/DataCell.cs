using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;


[System.Serializable]
public class DataCell
{
    //Covers all data needed a cell. 
    //TBD: Whether this should be a mono / scriptable / etc 

    //XY coordinate of the cell in a board (bottom left is 1,1, bottom right is X,1)
    public Vector2Int coord;
    //cell value
    public int value;
    //status and buff list are two things for the same purpose. To make it easier:
    //status can be passed from initial board data but buff can not.
    //cell status: status is the secondary value of a cell to support gameplay. eg. in clock 1 represent a clock was played in this cell. 0 represents none.
    public int status;
    //cell buff: buff list is a more flexible structure in case more complicated gameplay logic is needed.  
    public List<KeyValuePair<string, int>> buffList;
    
    public DataCell()
    {
        coord = new Vector2Int(0, 0);
        value = 0;
        status = 0;
        buffList = new List<KeyValuePair<string, int>>();
    }
    public DataCell(DataCell copyCell)
    {
        coord = copyCell.coord;
        value = copyCell.value;
        status = copyCell.status;
        //this should make the list is cloned not referenced. I GUESS?
        buffList = new List<KeyValuePair<string, int>>(); 
        for(int i=0;i< copyCell.buffList.Count; i++)
        {
            buffList.Add(new KeyValuePair<string, int>(copyCell.buffList[i].Key, copyCell.buffList[i].Value));
        }
    }
}
