using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gFailBase : MonoBehaviour
{
    public int phase = 0;
    virtual public bool FailCheck(dBoard board)
    {
        Debug.Log("virtual FailCheck() from gFailBase");
        return false;
    }
}
