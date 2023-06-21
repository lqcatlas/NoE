using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[System.Serializable]
public class CameraMovementParams
{
    public string stepID;
    public Vector2 coordinates;
    public float size;
    public float delay;
    public float duration;
    public bool isInsert;
}
public class CameraMove : MonoBehaviour
{
    public List<CameraMovementParams> CameraMoveSteps;
    //[SerializeField] float fullCameraSize = 540;
    [SerializeField] Camera cam;
    private int currentStep;
    // Start is called before the first frame update
    void Start()
    {
        if(cam == null)
        {
            Debug.LogError("no camera is given on CameraMove");
            return;
        }
        
        if(CameraMoveSteps.Count == 0)
        {
            Debug.LogError("no initial setup for CameraMove on Movie Camera");
            return;
        }
        currentStep = 0;
        ExecuteStep(CameraMoveSteps[0]);
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.N))
        {
            CameraMoveNext();
        }
        */
    }
    void CameraMoveNext()
    {
        while(currentStep < CameraMoveSteps.Count - 1)
        {
            ExecuteStep(CameraMoveSteps[++currentStep]);
            if (currentStep < CameraMoveSteps.Count - 1)
            {
                if (CameraMoveSteps[currentStep + 1].isInsert)
                {
                    continue;
                }
            }
            break;  
        }
    }
    void ExecuteStep(CameraMovementParams stepParams)
    {
        Debug.Log(string.Format("CameraMove step ID {0}", stepParams.stepID));
        cam.transform.DOMove(stepParams.coordinates, stepParams.duration).SetDelay(stepParams.delay).SetEase(Ease.InOutSine);
        cam.DOOrthoSize(stepParams.size, stepParams.duration).SetDelay(stepParams.delay).SetEase(Ease.InOutSine);
        //to do: add callback based on stepID
    }
    public void ExecuteStepByID(string targetID)
    {
        bool IDfound = false;
        for (int i = 0; i < CameraMoveSteps.Count; i++)
        {
            if (CameraMoveSteps[i].stepID == targetID)
            {
                currentStep = Mathf.Max(0, i - 1);
                IDfound = true;
                break;
            }
        }
        if (IDfound)
        {
            CameraMoveNext();
        }
        else
        {
            Debug.Log(string.Format("Unable find ID {0} on SpriteEnbale to execute, skip.", targetID));
        }

    }
}
