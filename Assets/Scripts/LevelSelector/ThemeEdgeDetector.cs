using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThemeEdgeDetector : MonoBehaviour
{
    [SerializeField] SelectorMove scrollBtn;
    //[SerializeField] string edge_tag;
    private void OnTriggerEnter2D(Collider2D col)
    {
        //Debug.Log("collision enter");
        if (col.gameObject.GetComponent<ThemeEndingCollider>() != null)
        {
            //Debug.Log("collision enter with obj has EndingCollider");
            scrollBtn.reachEnding = true;
        }
    }
    private void OnTriggerExit2D(Collider2D col)
    {
        //Debug.Log("collision exit");
        if (col.gameObject.GetComponent<ThemeEndingCollider>() != null)
        {
            //Debug.Log("collision exit with obj has EndingCollider");
            scrollBtn.reachEnding = false;
        }
    }
}
