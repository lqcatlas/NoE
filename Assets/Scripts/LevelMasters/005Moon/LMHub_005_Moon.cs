using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LMHub_005_Moon : MonoBehaviour
{
    [Header("Tool")]
    public List<Sprite> statusSprites;
    [Header("Phase Plate")]
    public GameObject phaseMask;
    public GameObject phasePlate;
    public List<float> phaseDegrees;
    public float PLATE_ROTATION_DURATION_PLAY = 0.5f;

    public void SetPlateWidget(bool enabled)
    {
        phaseMask.SetActive(enabled);
        phasePlate.SetActive(enabled);
    }
}
