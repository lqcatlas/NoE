using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CustomizedInputReceiver : MonoBehaviour
{
    public UnityEvent mouseUpFuncs;
    public UnityEvent mouseDownFuncs;
    public UnityEvent mouseEnterFuncs;
    public UnityEvent mouseExitFuncs;
    

    public void MouseUp()
    {
        mouseUpFuncs.Invoke();
    }
    public void MouseDown()
    {
        mouseDownFuncs.Invoke();
    }
    public void MouseEnter()
    {
        mouseEnterFuncs.Invoke();
    }
    public void MouseExit()
    {
        mouseExitFuncs.Invoke();
    }
}
