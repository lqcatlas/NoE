using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjSwinging : MonoBehaviour
{
    [SerializeField] Vector2 SWINGING_RANGE;
    [SerializeField] Vector2 SWINGING_LOOP_TIME;

    private void OnEnable()
    {
        bool startReversed = Random.Range(0, 2) == 1;

        transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
        transform.DORotate(new Vector3(0f,0f,Random.Range(SWINGING_RANGE.x, SWINGING_RANGE.y) * (startReversed ? 1 : -1)), Random.Range(SWINGING_LOOP_TIME.x, SWINGING_LOOP_TIME.y))
            .SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine)
            .SetRelative(true);
    }
    private void OnDisable()
    {
        DOTween.Kill(transform);
        transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
        
    }
}
