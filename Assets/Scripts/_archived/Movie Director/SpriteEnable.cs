using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class EnableSetParams
{
    public string stepID;
    public List<SpriteHandler_SH> img_SH;
    //public float endAlpha;
    public float delay;
    //public float duration;
    public bool isInsert;
}
public class SpriteEnable : MonoBehaviour
{
    public List<EnableSetParams> SpriteEnableSteps;
    public int currentStep;
    // Start is called before the first frame update
    void Start()
    {
        if (SpriteEnableSteps.Count == 0)
        {
            Debug.LogError("no initial setup for SpriteEnable on SceneLayer");
            return;
        }
        currentStep = 0;
        ExecuteStep(SpriteEnableSteps[0]);
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.N))
        {
            SpriteEnableNext();
        }
        */
    }
    void SpriteEnableNext()
    {
        while (currentStep < SpriteEnableSteps.Count - 1)
        {
            ExecuteStep(SpriteEnableSteps[++currentStep]);
            if (currentStep < SpriteEnableSteps.Count - 1)
            {
                if (SpriteEnableSteps[currentStep + 1].isInsert)
                {
                    continue;
                }
            }
            break;
        }
    }
    void ExecuteStep(EnableSetParams stepParams)
    {
        Debug.Log(string.Format("SpriteEnable step ID {0}", stepParams.stepID));
        Sequence seq = DOTween.Sequence().Pause();
        seq.AppendInterval(stepParams.delay);
        for (int i=0;i<stepParams.img_SH.Count; i++)
        {
            //stepParams.imgs[i].DOFade(stepParams.endAlpha, stepParams.duration).SetDelay(stepParams.delay);
            SpriteHandler_SH sh = stepParams.img_SH[i];
            seq.AppendCallback(()=> sh.SpriteSet());
        }
        seq.Play();
        //to do: add callback based on stepID
    }
    public void ExecuteStepByID(string targetID)
    {
        bool IDfound = false;
        for(int i = 0; i < SpriteEnableSteps.Count; i++)
        {
            if (SpriteEnableSteps[i].stepID == targetID)
            {
                currentStep = Mathf.Max(0, i - 1);
                IDfound = true;
                break;
            }
        }
        if (IDfound)
        {
            SpriteEnableNext();
        }
        else
        {
            Debug.Log(string.Format("Unable find ID {0} on SpriteEnbale to execute, skip.", targetID));
        }
        
    }
}
