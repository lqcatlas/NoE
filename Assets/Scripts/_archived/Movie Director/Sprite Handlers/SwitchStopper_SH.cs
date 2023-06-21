using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchStopper_SH : SpriteHandler_SH
{
    [Header("[Children Objs]")]
    [SerializeField] SpriteSwitch switch_script;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public override void SpriteSet()
    {
        
        if(switch_script != null)
        {
            //switch_script.StopShow();
        }
        
    }
}
