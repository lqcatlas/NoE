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

    // Update is called once per frame
    void Update()
    {
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
    GameObject GetTopObject(RaycastHit2D[] allHits)
    {
        //get the object on top
        GameObject picked = null;
        int currentSpriteOrder = int.MinValue;

        GameObject hitObj = null;
        if(allHits.Count() > 0)
        {
            for(int i=0;i< allHits.Count(); i++)
            {
                hitObj = allHits[i].transform.gameObject;
                SpriteRenderer render = hitObj.GetComponent<SpriteRenderer>();
                if (render)
                {
                    if(render.sortingOrder > currentSpriteOrder)
                    {
                        currentSpriteOrder = render.sortingOrder;
                        picked = hitObj;
                    }
                }
                else if(currentSpriteOrder == int.MinValue)
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
}
