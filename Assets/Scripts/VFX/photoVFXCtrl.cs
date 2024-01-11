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
    public float LOC_MOVEMENT_SCALE = 1f;

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
            //LOC_MOVEMENT_SCALE = 1f;
            transform.localPosition = LOC_ORIGINAL;
        }
        if (REPLACE_VFX_ENABLED && PHOTO_REPLACE != null)
        {
            Color clr = new Color(PHOTO_REPLACE.color.r, PHOTO_REPLACE.color.g, PHOTO_REPLACE.color.b, 0f);
            PHOTO_REPLACE.color = clr;
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
        transform.localPosition = new Vector3(LOC_ORIGINAL.x - x_offset * LOC_MOVING_RANGE.x * LOC_MOVEMENT_SCALE, LOC_ORIGINAL.y - y_offset * LOC_MOVING_RANGE.y * LOC_MOVEMENT_SCALE, 0f);
    }
    public void PositionReset(float duration)
    {
        transform.DOLocalMove(LOC_ORIGINAL, duration);
    }
    public void ReduceOffsetMovement()
    {
        LOC_MOVEMENT_SCALE = 0.3f;
    }
    public void ZoomIn(float duration)
    {
        if (duration == 0)
        {
            transform.localScale = SCALE_AFTER_ZOOM * Vector3.one;
        }
        else
        {
            //Debug.Log("photo zoom in vfx called");
            transform.DOScale(SCALE_AFTER_ZOOM, duration);
        }
        
    }
    public void ZoomReset(float duration)
    {
        transform.DOScale(SCALE_BEFORE_ZOOM, duration);
    }
    public void Replace(float duration)
    {
        if (PHOTO_REPLACE != null)
        {
            if(duration == 0)
            {
                GetComponent<SpriteRenderer>().sprite = PHOTO_REPLACE.sprite;
                GetComponent<SpriteRenderer>().color = new Color(PHOTO_REPLACE.color.r, PHOTO_REPLACE.color.g, PHOTO_REPLACE.color.b, 1f);
            }
            else
            {
                PHOTO_REPLACE.DOFade(1f, duration).OnComplete(() => GetComponent<SpriteRenderer>().sprite = PHOTO_REPLACE.sprite);
            }
        }
    }
    /*public void ColorReset(float duration)
    {
        if (PHOTO_REPLACE != null)
        {
            PHOTO_REPLACE.DOFade(0f, duration);
        }
    }*/
}
