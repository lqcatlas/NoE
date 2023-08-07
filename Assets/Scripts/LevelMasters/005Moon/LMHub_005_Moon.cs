using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LMHub_005_Moon : MonoBehaviour
{
    [Header("Tool")]
    public List<Sprite> statusSprites;
    [Header("Phase Plate")]
    public GameObject phaseMask;
    public GameObject phaseRotate;
    public GameObject phasePlate;
    public GameObject phaseStar;
    public List<float> phaseDegrees;
    public float PLATE_ROTATION_DURATION_PLAY = 0.5f;

    public void SetPlateWidget(bool enabled)
    {
        phaseMask.SetActive(enabled);
        phaseRotate.SetActive(enabled);
    }
    public void SetTabletToDegree(int phaseIndex)
    {
        phasePlate.transform.localRotation = Quaternion.Euler(0f, 0f, phaseDegrees[phaseIndex]);
        phasePlate.GetComponent<SpriteRenderer>().DOFade(0f, PLATE_ROTATION_DURATION_PLAY * 2).From();
        phaseStar.GetComponent<SpriteRenderer>().DOFade(0f, PLATE_ROTATION_DURATION_PLAY * 5).From();
    }
    public void AnimateTabletToDegree(int phaseIndex)
    {
        phasePlate.transform.DORotate(new Vector3(0f, 0f, phaseDegrees[phaseIndex]), PLATE_ROTATION_DURATION_PLAY);
    }
}
