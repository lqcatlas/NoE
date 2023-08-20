using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjFloating : MonoBehaviour
{
    private void OnEnable()
    {
        bool startReversedX = Random.Range(0, 1) == 1;
        bool startReversedY = Random.Range(0, 1) == 1;
        
        transform.DOLocalMoveX(Random.Range(0.5f, 1f) * (startReversedX ? 1 : -1), Random.Range(6f, 9f)).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine).SetRelative(true);
        transform.DOLocalMoveY(Random.Range(1f, 1.5f) * (startReversedY ? 1 : -1), Random.Range(6f, 9f)).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine).SetRelative(true);
    }
}
