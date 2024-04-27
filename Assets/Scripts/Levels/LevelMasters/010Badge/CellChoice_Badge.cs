using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CellChoice_Badge : MonoBehaviour
{
    [SerializeField] Image img;
    [SerializeField] List<Sprite> correctChoices;
    [SerializeField] List<Sprite> wrongChoices;

    public void SetToCorrect(bool isCorrect)
    {
        if (isCorrect)
        {
            int rng = Random.Range(0, correctChoices.Count);
            img.sprite = correctChoices[rng];
        }
        else
        {
            int rng = Random.Range(0, wrongChoices.Count);
            img.sprite = wrongChoices[rng];
        }
    }
    private void OnEnable()
    {
        img.fillAmount = 0;
        img.DOFillAmount(1f, dConstants.UI.StandardizedBtnAnimDuration);
    }
}
