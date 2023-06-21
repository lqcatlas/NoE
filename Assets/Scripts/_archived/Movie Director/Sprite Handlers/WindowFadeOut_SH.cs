using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Security.Cryptography;

public class WindowFadeOut_SH : SpriteHandler_SH
{
    [Header("[Children Objs]")]
    [SerializeField] Image targetFrame;
    [SerializeField] Image targetFill;

    [Header("[Params]")]
    [SerializeField] float fadeTime;
    [SerializeField] Color frameColor;
    //[SerializeField] Color fillColor;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public override void SpriteSet()
    {
        //Debug.Log("WindowFadeOut SpriteSet()");
        float rng = Random.Range(0.9f, 1.1f);
        if (targetFrame != null)
        {
            targetFrame.DOColor(frameColor, fadeTime * rng);
        }
        if (targetFill != null)
        {
            targetFill.DOFade(0, fadeTime * rng);
        }
    }
}
