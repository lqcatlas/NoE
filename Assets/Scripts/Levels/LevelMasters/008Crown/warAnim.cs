using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class warAnim : MonoBehaviour
{
    [SerializeField] GameObject sword1;
    [SerializeField] GameObject sword2;
    [Header("Anim Params")]
    [SerializeField] float MOVEMENT_X = 0.5f;
    [SerializeField] float ROTATE_Z = -60f;
    [SerializeField] float ONE_HIT_DURATION = 0.2f;
    private void OnEnable()
    {
        //Debug.Log("play War animation TODO");
        sword1.transform.DOLocalMoveX(MOVEMENT_X, ONE_HIT_DURATION).SetRelative(true).SetLoops(2, LoopType.Restart);
        sword2.transform.DOLocalMoveX(-MOVEMENT_X, ONE_HIT_DURATION).SetRelative(true).SetLoops(2, LoopType.Restart);
        sword1.transform.DORotate(Vector3.forward * ROTATE_Z, ONE_HIT_DURATION).SetRelative(true).SetLoops(2, LoopType.Restart);
        sword2.transform.DORotate(Vector3.forward * -ROTATE_Z, ONE_HIT_DURATION).SetRelative(true).SetLoops(2, LoopType.Restart);
        sword1.GetComponent<SpriteRenderer>().DOFade(0f, ONE_HIT_DURATION).SetEase(Ease.OutCubic).SetLoops(2, LoopType.Restart);
        sword2.GetComponent<SpriteRenderer>().DOFade(0f, ONE_HIT_DURATION).SetEase(Ease.OutCubic).SetLoops(2, LoopType.Restart).OnComplete(() => Destroy(gameObject));

    }
}
