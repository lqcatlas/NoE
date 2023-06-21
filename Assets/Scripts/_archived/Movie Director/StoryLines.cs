using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class LineParams
{
    public string stepID;
    public List<string> lineTexts;
    public float delay;
    public float duration;
    public bool isSequence;
}
public class StoryLines : MonoBehaviour
{
    public List<LineParams> StoryLinesSteps;

    [Header("[Children Objs]")]
    [SerializeField] List<TextMeshProUGUI> lines;
    [SerializeField] List<Image> lineMasks;
    [SerializeField] float delayPerCharacter = 0.15f;
    private int currentStep;

    // Start is called before the first frame update
    void Start()
    {
        if (StoryLinesSteps.Count == 0)
        {
            Debug.LogError("no initial setup for StoryLines");
            return;
        }
        currentStep = 0;
        ExecuteStep(StoryLinesSteps[0]);
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.N))
        {
            StoryLinesNext();
        }
        */
    }
    void StoryLinesNext()
    {
        float preload_delay = 0;
        while (currentStep < StoryLinesSteps.Count - 1)
        {
            ExecuteStep(StoryLinesSteps[++currentStep], preload_delay);
            if (currentStep < StoryLinesSteps.Count - 1)
            {
                if (StoryLinesSteps[currentStep + 1].isSequence)
                {
                    preload_delay += StoryLinesSteps[currentStep].duration + StoryLinesSteps[currentStep].delay;
                    continue;
                }
            }
            break;
        }
    }
    void ExecuteStep(LineParams stepParams, float preloadDelay = 0)
    {
        Debug.Log(string.Format("StoryLine step on ID {0}", stepParams.stepID));
        //sequence for fadein lines
        Sequence seq = DOTween.Sequence().Pause();
        seq.AppendInterval(preloadDelay + stepParams.delay);
        seq.AppendCallback(() => ClearTexts());
        for (int i = 0; i < lines.Count; i++)
        {
            if (i < stepParams.lineTexts.Count)
            {
                //Debug.Log(string.Format("StoryLine step on ID {0} on index {1}", stepParams.stepID, i));
                string lineTxt = stepParams.lineTexts[i];
                int curIndex = i;
                seq.AppendCallback(() => lines[curIndex].SetText(LocalizedAssetLookup.singleton.Translate(lineTxt)));
                //seq.AppendCallback(() => Debug.Log(string.Format("execute callback on index {0}", curIndex)));
                seq.AppendInterval(((float)lineTxt.Length + 1) * delayPerCharacter);
                //lines[i].SetText(lineTxt);
            }
        }
        seq.Play();
        //sequence for fadeout lines
        //Sequence end_seq = DOTween.Sequence();
        //end_seq.AppendInterval(preloadDelay + stepParams.delay + stepParams.duration).AppendCallback(() => Debug.Log(string.Format("execute line step fadeout on ID {0}", stepParams.stepID)));
        //to do: line masks
        //to do: add callback based on stepID
    }
    public void ExecuteStepByID(string targetID)
    {
        bool IDfound = false;
        for (int i = 0; i < StoryLinesSteps.Count; i++)
        {
            if (StoryLinesSteps[i].stepID == targetID)
            {
                currentStep = Mathf.Max(0, i - 1);
                IDfound = true;
                break;
            }
        }
        if (IDfound)
        {
            StoryLinesNext();
        }
        else
        {
            Debug.Log(string.Format("Unable find ID {0} on StoryLines to execute, skip.", targetID));
        }
    }
    void ClearTexts()
    {
        //clear text
        for (int i = 0; i < lines.Count; i++)
        {
            lines[i].SetText("");
        }
    }
}
