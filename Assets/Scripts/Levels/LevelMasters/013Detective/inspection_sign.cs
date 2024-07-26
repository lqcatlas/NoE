using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class inspection_sign : MonoBehaviour
{
    public int ispStatus;
    public SpriteRenderer sr;
    public List<Sprite> ispSprites;

    public void SetSign(int status)
    {
        ispStatus = status;
        sr.sprite = ispSprites[status];
    }
    public void UpdateSign(int status)
    {
        ispStatus = status;
        sr.sprite = ispSprites[status];
        //TODO: animation needed
    }
}
