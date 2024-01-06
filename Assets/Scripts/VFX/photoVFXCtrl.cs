using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class photoVFXCtrl : MonoBehaviour
{
    public SpriteRenderer sprtRender;
    [Header("VFX Param")]
    public bool SCALE_VFX_ENABLED = false;
    public float SCALE_BEFORE_ZOOM;
    public float SCALE_AFTER_ZOOM;

    public bool LOC_VFX_ENABLED = false;
    public Vector2 LOC_ORIGINAL;
    public Vector2 LOC_MOVING_RANGE;

    public bool REPLACE_VFX_ENABLED = false;
    public SpriteRenderer PHOTO_REPLACE;

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
        if (REPLACE_VFX_ENABLED && PHOTO_REPLACE != null)
        {
            PHOTO_REPLACE.color = new Color(1f, 1f, 1f, 0f);
        }
    }
    private void Update()
    {
        if (LOC_VFX_ENABLED)
        {
            Vector2 offset = CustomizedInputSystem.singleton.GetCursorOffsetPct();
            PositionOffset(offset.x, offset.y);
        }
    }
    void PositionOffset(float x_offset, float y_offset)
    {
        transform.localPosition = new Vector3(LOC_ORIGINAL.x - x_offset * LOC_MOVING_RANGE.x, LOC_ORIGINAL.y - y_offset * LOC_MOVING_RANGE.y, 0f);
    }
    public void PositionReset(float duration)
    {
        transform.DOLocalMove(LOC_ORIGINAL, duration);
    }
    public void ZoomIn(float duration)
    {
        //Debug.Log("photo zoom in vfx called");
        transform.DOScale(SCALE_AFTER_ZOOM, duration);
    }
    public void ZoomReset(float duration)
    {
        transform.DOScale(SCALE_BEFORE_ZOOM, duration);
    }
    public void Replace(float duration)
    {
        if (PHOTO_REPLACE != null)
        {
            PHOTO_REPLACE.DOFade(1f, duration);
        }
    }
    public void ColorReset(float duration)
    {
        if (PHOTO_REPLACE != null)
        {
            PHOTO_REPLACE.DOFade(0f, duration);
        }
    }
}
