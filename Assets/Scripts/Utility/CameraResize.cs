using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class CameraResize : MonoBehaviour
{

    public float TargetSceneWidth = 19.2f;
    public float TargetSceneHeight = 10.8f;
    public float sizeFactor = 1000f;

    float preferredSizebyWidth;
    float preferredSizebyHeight;


    Camera _camera;
    void Start()
    {
        _camera = GetComponent<Camera>();
    }

    // Adjust the camera's height so the desired scene width fits in view
    // even if the screen/window size changes dynamically.
    void Update()
    {
        float currentRatio = (float)Screen.width / Screen.height;
        preferredSizebyWidth = TargetSceneWidth / currentRatio * 0.5f;
        preferredSizebyHeight = TargetSceneHeight * 0.5f;

        _camera.orthographicSize = Mathf.Max(preferredSizebyWidth, preferredSizebyHeight) / sizeFactor;
    }
}
