using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using System;

public class CellMaster : MonoBehaviour
{
    [Header("Static Params")]
    static float MaskFadeTime = 0.3f;
    static float MaskFadeAlpha = 0.5f;
    static float StandardizedCellSize = 2f;
    [Header("Data")]
    public Vector2Int coord;
    public LevelMasterBase levelMaster;
    [Header("Children Objs")]
    public TextMeshPro numberTxt;
    public SpriteRenderer numberSprt;
    public SpriteRenderer frameSprt;
    public SpriteRenderer maskSprt;

    
    public void InitCellPosition(Vector2Int _coord, Vector2Int _size)
    {
        //reposition cell based on the board size and its coord
        coord = _coord;
        GetComponent<Transform>().localPosition = 
            new Vector3(_coord.x - (_size.x -1)/2f, _coord.y - (_size.y - 1) / 2f, 0) * StandardizedCellSize;
    }
    public void RegisterLevelMaster(LevelMasterBase _master)
    {
        levelMaster = _master;
    }
    public void MouseUp()
    {
        //Debug.Log(string.Format("cell coord {0},{1} clicked.", coord.x, coord.y));
        if(levelMaster != null)
        {
            levelMaster.Play(coord);
        }
        else
        {
            Debug.LogError(string.Format("cell({0}) clicked with no level master owner", gameObject.name));
        }
    }
    public void ResetCellHover()
    {
        maskSprt.DOFade(0f, 0.001f);
    }
    public void MaskFadeIn()
    {
        maskSprt.DOFade(MaskFadeAlpha, MaskFadeTime);
    }
    public void MaskFadeOut()
    {
        maskSprt.DOFade(0f, MaskFadeTime);
    }
}
