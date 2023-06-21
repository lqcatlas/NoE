using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteSlider : MonoBehaviour
{
    [Header("Refs")]
    private SpriteRenderer render;

    [Header("Assets")]
    [SerializeField] Sprite currentSprite;
    [SerializeField] List<Sprite> slides;

    [Header("Params")]
    public float fixedIntervalBySeconds;
    public float additionalIntervalRangeBySeconds;
    public bool SwitchByOrder;

    private Sequence seq;

    // Start is called before the first frame update
    void Start()
    {

    }
    void OnEnable()
    {
        if (InitShow())
        {
            StartShow();
        }
    }
    void OnDisable()
    {
        slides.Add(currentSprite);
        seq.Kill();
    }
    bool InitShow()
    {
        render = gameObject.GetComponent<SpriteRenderer>();
        if (render == null)
        {
            Debug.LogError(string.Format("no <Image> component on {0}", this.gameObject.name));
            return false;
        }
        if (slides.Count > 0)
        {
            SetCurrentSprite(slides[0]);
            slides.RemoveAt(0);
        }
        else
        {
            Debug.LogError(string.Format("not enough sprites on {0}", this.gameObject.name));
            return false;
        }
        return true;

    }
    public void StartShow()
    {
        if (gameObject.activeInHierarchy)
        {
            seq = DOTween.Sequence();
            seq.AppendInterval(fixedIntervalBySeconds + additionalIntervalRangeBySeconds * Random.Range(0, 1f));
            seq.AppendCallback(() => SwtichToNewSlide());
            seq.AppendCallback(() => StartShow());
            //Debug.Log("start show");
        }
    }
    void SwtichToNewSlide()
    {
        if (slides.Count == 0)
        {
            return;
        }
        int rng = SwitchByOrder ? 0 : Random.Range(0, slides.Count);
        slides.Add(currentSprite);
        SetCurrentSprite(slides[rng]);
        slides.RemoveAt(rng);

        //Debug.Log("swtich slide");
    }
    void SetCurrentSprite(Sprite sprt)
    {
        currentSprite = sprt;
        render.sprite = currentSprite;
    }
}
