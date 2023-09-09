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
        transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
    }
    private void OnDisable()
    {
        DOTween.Kill(transform);
        transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
        
    }
}
