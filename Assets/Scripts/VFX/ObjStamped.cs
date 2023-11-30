using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjStamped : MonoBehaviour
{
    [SerializeField] List<SpriteRenderer> stampedList;
    [SerializeField] List<GameObject> appearAfterStampedList;
    static float stampDuration = 0.3f;
    private void OnEnable()
    {
        
        for(int i = 0; i < stampedList.Count; i++)
        {
            Vector3 finalScale = stampedList[i].transform.localScale;
            stampedList[i].transform.DOScale(5f, stampDuration).From().SetRelative(true).SetEase(Ease.InSine);
            //stampedList[i].transform.DOShakeScale(stampDuration * 0.1f, 1, 1).SetDelay(stampDuration);
            stampedList[i].DOFade(0f, stampDuration).From();
        }
        Sequence seq = DOTween.Sequence();
        seq.AppendInterval(stampDuration);
        seq.AppendCallback(() => EnableAfterStamped());
    }
    void EnableAfterStamped()
    {
        for (int i = 0; i < appearAfterStampedList.Count; i++)
        {
            appearAfterStampedList[i].gameObject.SetActive(true);
        }
    }
    private void OnDisable()
    {
        //Destroy(gameObject);
    }
}
