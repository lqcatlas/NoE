using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ThemeSelection : MonoBehaviour
{
    [SerializeField] GameObject page;
    [SerializeField] GameObject clockOption;
    [SerializeField] GameObject coinOption;

    [SerializeField] GameMaster clockGM;
    [SerializeField] GameMaster coinGM;


    [Header("Children Objs")]

    public Curtain curtain;

    void Start()
    {
        curtain.CurtainOn();
        page.SetActive(false);
    }
    public void ShowPage()
    {
        clockOption.SetActive(true);
        coinOption.SetActive(true);
        curtain.CurtainOff();
        page.SetActive(true);
    }
    public void ShowPage_ClockOnly()
    {
        clockOption.SetActive(true);
        curtain.CurtainOff();
        page.SetActive(true);
    }
    public void ShowPage_CoinOnly()
    {
        coinOption.SetActive(true);
        curtain.CurtainOff();
        page.SetActive(true);
    }
    public void ChooseClock()
    {
        curtain.CurtainOn();
        Sequence seq = DOTween.Sequence();
        seq.AppendInterval(0.5f).AppendCallback(() => clockGM.InitFirstPuzzle());
        
    }
    public void ChooseCoin()
    {
        curtain.CurtainOn();
        Sequence seq = DOTween.Sequence();
        seq.AppendInterval(0.5f).AppendCallback(() => coinGM.InitFirstPuzzle());
    }
}
