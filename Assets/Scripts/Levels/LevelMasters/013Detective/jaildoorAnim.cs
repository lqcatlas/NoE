using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jaildoorAnim : MonoBehaviour
{
    [SerializeField] Transform door;
    [SerializeField] Transform glass;

    private float GLASS_MOVE_DURATION = .3f;
    private float DOOR_DROP_DURATION = .5f;

    private Sequence seq;
    private void OnEnable()
    {
        door.localPosition = new Vector3(0f, 2f, 0f);

        seq.Kill();
        seq = DOTween.Sequence();
        seq.Append(glass.DOLocalMove(Vector3.left * 0.5f, GLASS_MOVE_DURATION).From().SetEase(Ease.OutCubic));
        seq.Append(glass.DOScale(3f, DOOR_DROP_DURATION));
        seq.Insert(GLASS_MOVE_DURATION, glass.GetComponent<SpriteRenderer>().DOFade(0f, DOOR_DROP_DURATION));
        seq.Insert(GLASS_MOVE_DURATION, door.DOLocalMoveY(0f, DOOR_DROP_DURATION).SetEase(Ease.InQuart));
        
    }

}
