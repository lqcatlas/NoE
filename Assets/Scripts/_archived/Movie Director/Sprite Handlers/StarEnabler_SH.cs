using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class StarEnabler_SH : SpriteHandler_SH
{
    [Header("[Children Objs]")]
    [SerializeField] Image img;

    [Header("[Params]")]
    [SerializeField] float fadeTime;
    //[SerializeField] float alphaTarget;
    [SerializeField] Color colorTarget;
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
        if (img != null)
        {
            img.DOColor(colorTarget, fadeTime);
            //img.DOFade(alphaTarget, fadeTime);
        }
    }
}
