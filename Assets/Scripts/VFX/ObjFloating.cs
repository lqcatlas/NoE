using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjFloating : MonoBehaviour
{
    [SerializeField] Vector2 FLOATING_RANGE_X;
    [SerializeField] Vector2 FLOATING_RANGE_Y;
    [SerializeField] Vector2 FLOATING_LOOP_TIME;
    private void OnEnable()
    {
        bool startReversedX = Random.Range(0, 2) == 1;
        bool startReversedY = Random.Range(0, 2) == 1;
        
        transform.DOLocalMoveX(Random.Range(FLOATING_RANGE_X.x, FLOATING_RANGE_X.y) * (startReversedX ? 1 : -1), Random.Range(FLOATING_LOOP_TIME.x, FLOATING_LOOP_TIME.y)).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine).SetRelative(true);
        transform.DOLocalMoveY(Random.Range(FLOATING_RANGE_Y.x, FLOATING_RANGE_Y.y) * (startReversedY ? 1 : -1), Random.Range(FLOATING_LOOP_TIME.x, FLOATING_LOOP_TIME.y)).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine).SetRelative(true);
    }
}
