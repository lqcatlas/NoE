using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class photoVFXCtrl : MonoBehaviour
{
    public SpriteRenderer sprtRender;
    [Header("VFX Param")]
    public bool SCALE_VFX_ENABLED;
    public float SCALE_BEFORE_ZOOM;
    public float SCALE_AFTER_ZOOM;

    public bool LOC_VFX_ENABLED;
    public Vector3 LOC_ORIGINAL;
    public Vector3 LOC_MOVING_RANGE;

    public bool CLR_VFX_ENABLED;
    public Color CLR_ORIGINAL;
    public Color CLR_FADED;

    // Start is called before the first frame update
    void Start()
    {
        sprtRender = this.GetComponent<SpriteRenderer>();

        if (SCALE_VFX_ENABLED)
        {
            transform.localScale = Vector3.one * SCALE_BEFORE_ZOOM;
        }
        if (LOC_VFX_ENABLED)
        {
            transform.localPosition = LOC_ORIGINAL;
        }
        if (SCALE_VFX_ENABLED)
        {
            sprtRender.color = CLR_ORIGINAL;
        }
    }

    public void ZoomIn(float duration)
    {
        Debug.Log("photo zoom in vfx called");
        transform.DOScale(SCALE_AFTER_ZOOM, duration);
    }
    public void ZoomReset(float duration)
    {
        transform.DOScale(SCALE_BEFORE_ZOOM, duration);
    }
    public void ColorFade(float duration)
    {

    }
    public void ColorReset(float duration)
    {

    }
}
