using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gRuleBase : MonoBehaviour
{
    public int phase = 1; 

    virtual public void Play(int coord, dBoard board)
    {
        Debug.Log("virtual Play(x,y) from gRuleBase");
    }
}
