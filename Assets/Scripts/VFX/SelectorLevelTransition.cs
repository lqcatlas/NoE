using DG.Tweening;

using UnityEngine;

public class SelectorLevelTransition : MonoBehaviour
{
    [SerializeField] float TransitionTimeCircle1;
    [SerializeField] float TransitionTimeCircle2;
    [SerializeField] SpriteRenderer VFX_Sprite;
    [SerializeField] Transform mask_1st;
    [SerializeField] Transform mask_2nd;

    private void OnEnable()
    {
        StartTransition();
    }
    public void StartTransition()
    {
        mask_1st.localScale = Vector3.zero;
        mask_2nd.localScale = Vector3.zero;
        VFX_Sprite.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
        Sequence seq = DOTween.Sequence();
        seq.Append(mask_1st.DOScale(4f, dConstants.VFX.SelectorToLevelAnimTransitionPhase1).SetEase(Ease.InSine));
        seq.AppendInterval(dConstants.VFX.SelectorToLevelAnimTransitionPhase1 + dConstants.VFX.SelectorToLevelAnimTransitionOnHold);
        seq.AppendCallback(() => VFX_Sprite.maskInteraction = SpriteMaskInteraction.VisibleOutsideMask);
        seq.AppendCallback(() => mask_1st.gameObject.SetActive(false));
        seq.Append(mask_2nd.DOScale(4f, dConstants.VFX.SelectorToLevelAnimTransitionPhase2).SetEase(Ease.InSine));
        seq.AppendInterval(dConstants.VFX.SelectorToLevelAnimTransitionPhase2);
        seq.AppendCallback(() => Destroy(gameObject)); 
    }
}
