using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gGoalBase : MonoBehaviour
{
    public int level = 0;
    virtual public bool GoalCheck(dBoard board)
    {
        Debug.Log("virtual GoalCheck() from gGoalBase");
        return false;
    }
}
