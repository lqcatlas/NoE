using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CustomizedInputSystem : MonoBehaviour
{
    static public CustomizedInputSystem singleton;
    private void Awake()
    {
        if(singleton == null)
        {
            singleton = this;
        }
        else
        {
            Destroy(this);
        }
    }
    static float MaxRaycastDistance = 500f;
    public GameObject CurrentPointerObject;
    //public List<GameObject> allHitObjs;
    public float CursorOffsetPct_X;
    public float CursorOffsetPct_Y;

    // Update is called once per frame
    void Update()
    {
        UpdateCursorPositionPct();
        //allHitObjs = RetrieveAllTriggers();
        GameObject obj = GetTopObject(RetrieveAllTriggers());
        if(obj != CurrentPointerObject)
        {
            MouseEnter(obj);
            MouseExit(CurrentPointerObject);
            CurrentPointerObject = obj;
        }
        if (Input.GetMouseButtonDown(0))
        {
            MousePrimaryBtnDown(CurrentPointerObject);
        }
        if (Input.GetMouseButtonUp(0))
        {
            MousePrimaryBtnUp(CurrentPointerObject);
        }
    }
    public RaycastHit2D[] RetrieveAllTriggers()
    {
        return Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, MaxRaycastDistance);
    }
    public Vector2 GetCursorOffsetPct()
    {
        return new Vector2(CursorOffsetPct_X, CursorOffsetPct_Y);
    }
    GameObject GetTopObject(RaycastHit2D[] allHits)
    {
        //get the object on top
        GameObject picked = null;
        int currentSpriteLayer = int.MinValue;
        int currentSpriteOrder = int.MinValue;

        //debug
        //allHitObjs.Clear();

        GameObject hitObj = null;
        if(allHits.Count() > 0)
        {
            for(int i=0;i< allHits.Count(); i++)
            {
                hitObj = allHits[i].transform.gameObject;
                //allHitObjs.Add(hitObj);
                SpriteRenderer render = hitObj.GetComponent<SpriteRenderer>();
                if (render)
                {
                    if(SortingLayer.GetLayerValueFromID(render.sortingLayerID) >= SortingLayer.GetLayerValueFromID(currentSpriteLayer) && render.sortingOrder > currentSpriteOrder)
                    {
                        //Debug.Log(string.Format("layer name: {0}, layer ID {1}", render.sortingLayerName, SortingLayer.GetLayerValueFromID(render.sortingLayerID)));
                        currentSpriteOrder = render.sortingOrder;
                        currentSpriteLayer = render.sortingLayerID;
                        picked = hitObj;
                    }
                    else if (picked == null)
                    {
                        currentSpriteOrder = render.sortingOrder;
                        currentSpriteLayer = render.sortingLayerID;
                        picked = hitObj;
                    }
                }
                else if(picked == null)
                {
                    picked = hitObj;
                }
            }
            return picked;
        }
        else
        {
            return null;
        }
    }
    void MousePrimaryBtnUp(GameObject _obj)
    {
        if (_obj)
        {
            CustomizedInputReceiver receiver = _obj.GetComponent<CustomizedInputReceiver>();
            if (receiver)
            {
                receiver.MouseUp();
            }
        }
    }
    void MousePrimaryBtnDown(GameObject _obj)
    {
        if (_obj)
        {
            CustomizedInputReceiver receiver = _obj.GetComponent<CustomizedInputReceiver>();
            if (receiver)
            {
                receiver.MouseDown();
            }
        }
    }
    void MouseEnter(GameObject _obj)
    {
        if (_obj)
        {
            CustomizedInputReceiver receiver = _obj.GetComponent<CustomizedInputReceiver>();
            if (receiver)
            {
                receiver.MouseEnter();
            }
        }
    }
    void MouseExit(GameObject _obj)
    {
        if (_obj)
        {
            CustomizedInputReceiver receiver = _obj.GetComponent<CustomizedInputReceiver>();
            if (receiver)
            {
                receiver.MouseExit();
            }
        }
    }

    void UpdateCursorPositionPct()
    {
        Vector3 CursorPos = Input.mousePosition;
        CursorOffsetPct_X = (CursorPos.x - Screen.width / 2f) / (Screen.width / 2f);
        CursorOffsetPct_Y = (CursorPos.y - Screen.height / 2f) / (Screen.height / 2f);
    }
}
