using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static System.TimeZoneInfo;

[System.Serializable]
public class SceneParams
{
    public string sceneID;
    public float duration;
}

public class Director : MonoBehaviour
{
    [Header("Debug")]
    [SerializeField] AudioCentralCtrl audioCtrl;

    [Header("Crew")]
    [SerializeField] CameraMove cameraCrew;
    [SerializeField] StoryLines storyCrew;
    [SerializeField] SpriteEnable spriteCrew;

    //[SerializeField] GameMaster GM;
    [SerializeField] ThemeSelection ThemeSelector;
    [SerializeField] Image FinalBg1;
    [SerializeField] Image FinalBg2;

    public List<SceneParams> scenes;
    public bool autoPlay = false;
    public bool skipPlay = false;
    public bool atStart = false;
    public int currentID = 0;
    // Start is called before the first frame update
    void Start()
    {
        currentID = 0;
        atStart = true;
    }

    // Update is called once per frame
    void Update()
    {
        /*if (Input.GetKeyUp(KeyCode.N) && !skipPlay)
        {
            ContinuePlay();
            atStart = false;
        }
        if (Input.GetKeyUp(KeyCode.S) && atStart)
        {
            skipPlay = true;
        }
        if (skipPlay)
        {
            SkipPlay();
        }*/
    }
    public void ContinuePlay()
    {
        Debug.Log(string.Format("ContinuePlay()on ID {0}", currentID));
        //audioCtrl.IntroContinue();

        if (currentID+1 < scenes.Count)
        {
            string nextID = scenes[++currentID].sceneID;
            float duration = scenes[currentID].duration;
            cameraCrew.ExecuteStepByID(nextID);
            storyCrew.ExecuteStepByID(nextID);
            spriteCrew.ExecuteStepByID(nextID);
            if (autoPlay)
            {
                Sequence seq = DOTween.Sequence();
                seq.AppendInterval(duration).AppendCallback(()=> ContinuePlay());
            }
        }
        else
        {
            Debug.Log("no more steps in Director's mind");
            StartGameplay();
        }
    }
    public void SkipPlay()
    {
        Debug.Log(string.Format("Skip Intro Play", currentID));
        currentID = Mathf.Max(0,scenes.Count - 1);
        string nextID = scenes[currentID].sceneID;
        float duration = scenes[currentID].duration;
        cameraCrew.ExecuteStepByID(nextID);
        storyCrew.ExecuteStepByID(nextID);
        spriteCrew.ExecuteStepByID(nextID);
        skipPlay = false;
        StartGameplay();
    }
    void StartGameplay()
    {
        //GM.InitFirstPuzzle();
        ThemeSelector.ShowPage();
        audioCtrl.BgMusicStart();
        DOTween.ToAlpha(() => FinalBg1.color, x => FinalBg1.color = x, 1f, 0.5f);
        DOTween.ToAlpha(() => FinalBg2.color, x => FinalBg2.color = x, 1f, 0.5f);
    }
}
