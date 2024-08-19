using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class TeaserPhotoGroup : MonoBehaviour
{
    public RectTransform photoGroup;
    public Transform infoGroup;

    //public GameObject photoBg;
    //public GameObject photoCover;
    //public TextMeshPro unlockReq;
    //public GameObject photo;
    //public GameObject photo_black;
    //public GameObject themeIcon;
    //public GameObject completeSign;
    //public TextMeshPro photoLine;
    public GameObject connectingString;

    private Sequence seq;
    private Vector3 photoGroupOriginalRotate;
    public void InitTeaserPhoto()
    {
        photoGroupOriginalRotate = photoGroup.rotation.eulerAngles;
        seq = DOTween.Sequence();
    }
    public void EnterPageAnimation()
    {
        float rng_delay = Random.Range(0f, 0.2f);
        gameObject.SetActive(false);
        connectingString.SetActive(false);
        photoGroup.DOScale(1.2f, dConstants.UI.StandardizedBtnAnimDuration).From().SetDelay(rng_delay).SetRelative(true)
            .OnStart(() => gameObject.SetActive(true))
            .OnComplete(() => SwingByForce(2f));
        infoGroup.DOScaleY(0f, dConstants.UI.StandardizedBtnAnimDuration).From().SetDelay(dConstants.UI.StandardizedBtnAnimDuration / 2f + rng_delay);
        connectingString.transform.DOScaleX(0f, dConstants.UI.StandardizedBtnAnimDuration).From().SetDelay(dConstants.UI.StandardizedBtnAnimDuration / 2f + rng_delay)
            .OnStart(() => connectingString.SetActive(true));
    }

    public void SwingByForce(float swingDegree)
    {
        float rng_timerange = Random.Range(0.8f, 1.2f);
        seq.Kill();
        seq = DOTween.Sequence();
        seq.Append(photoGroup.DORotate(new Vector3(0f, 0f, swingDegree), 4f * rng_timerange).SetRelative(true).SetEase(Ease.InOutFlash, 6, 1));
        seq.Append(photoGroup.DORotate(photoGroupOriginalRotate, rng_timerange).SetEase(Ease.InSine));
    }
}
