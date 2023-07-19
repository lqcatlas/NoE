using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXHolder : MonoBehaviour
{
    static public VFXHolder singleton;
    private void Awake()
    {
        if (singleton == null)
        {
            singleton = this;
        }
        else
        {
            Destroy(this);
        }
    }
}
