using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//[ExecuteInEditMode]
public class SecondaryCameraSetting : MonoBehaviour
{
    public RectTransform wholeScreen;
    public RectTransform targetScreen;

    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<Camera>().aspect = 16f / 9f;
    }

    // Update is called once per frame
    void Update()
    {
        float x = wholeScreen.sizeDelta.x;
        float y = wholeScreen.sizeDelta.y;
        float a = targetScreen.sizeDelta.x;
        float b = targetScreen.sizeDelta.y;
        Rect calculated_rect = new Rect((x - a) / 2 / x, (y - b) / 2 / y, a / x, b / y);
        this.GetComponent<Camera>().rect = calculated_rect;
        /*if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log(calculated_rect);

        }*/
        

    }
}
