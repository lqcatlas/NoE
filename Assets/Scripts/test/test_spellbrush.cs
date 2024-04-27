using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class test_spellbrush : MonoBehaviour
{
    private void Start()
    {
        /*List<int> testList = new();
        testList.Add(1);
        testList.Add(2);
        testList.Add(3);
        int runtime = 10000;
        List<int> countPostions = new();
        countPostions.Add(0);
        countPostions.Add(0);
        countPostions.Add(0);
        for (int i=0;i< runtime; i++)
        {
            List<int> resultList = shuffuleAList(testList);
            for(int j=0;j< resultList.Count; j++)
            {
                countPostions[resultList[j] - 1] += 1;
            }
        }
        PrintAList(countPostions);
        */

    }
    List<int> shuffuleAList(List<int> oldList)
    {
        //randomly shuffle it
        List<int> newList = new();
        int originalListLength = oldList.Count;
        for (int i=0; i< originalListLength; i++)
        {
            int ranIndex = Random.Range(0, oldList.Count);
            newList.Add(oldList[ranIndex]);
            oldList.RemoveAt(ranIndex);
        }
        //string printOutList = "";
        
        return newList;
        
    }
    void PrintAList(List<int> printList)
    {
        Debug.Log(string.Format("new list has {0} elements", printList.Count));
        for (int i = 0; i < printList.Count; i++)
        {
            Debug.Log(string.Format("new List element {0} is {1}", i, printList[i]));
        }
    }
    struct CoordAndCost
    {
        public Vector2Int coord;
        public int cost;
    }
    List<CoordAndCost> pathFindingResultBeforeGetMin;
    List<List<int>> movingCostGrid;
    List<List<int>> visitedGrid;
    void InitAGrid(List<List<int>> grid, Vector2Int size, int initValue)
    {
        grid = new();
        for (int i = 0; i < size.y; i++)
        {
            grid.Add(new List<int>());
            for (int j = 0; j < size.x; j++)
            {
                grid[i].Add(initValue);
            }
        }
    }
    void PrintAGrid(List<List<int>> grid)
    {
        //to do 
    }
    //get all adjacent cells besides the one have been marked and its cost
    List<CoordAndCost> Navigate(List<List<int>> visitedGrid, List<List<int>> movingCostGrid, CoordAndCost prevResult)
    {
        List<CoordAndCost> costToCoord = new();
        for (int y = 0; y < movingCostGrid.Count; y++)
        {
            for (int x = 0; x < movingCostGrid[y].Count; x++)
            {
                if(CalculDistance(prevResult.coord, new Vector2Int(x, y)) == 1 && visitedGrid[y][x] != 1)
                {
                    CoordAndCost result = new();
                    result.coord = new Vector2Int(x, y);
                    result.cost = prevResult.cost + movingCostGrid[y][x];
                    costToCoord.Add(result);
                    //Navigate();
                }
            }
        }
        return costToCoord;
    }
    int CalculDistance(Vector2Int a, Vector2Int b)
    {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
    }    
}
