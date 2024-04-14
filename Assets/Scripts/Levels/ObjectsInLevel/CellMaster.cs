using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class CellMaster : MonoBehaviour
{
    [Header("Static Params")]
    static float MaskFadeTime = 0.3f;
    static float MaskFadeAlpha = 0.5f;
    static float StandardizedCellSize = 2f;
    //number sprite params
    static List<float> scalarByDigits = new List<float> { 1f, 1f, 0.9f, 0.7f, 0.4f };
    static int maxValueSupported = 10000;
    static int maxDigitsSupported = 4;
    [Header("Data")]
    public Vector2Int coord;
    public LevelMasterBase levelMaster;
    public NumberSpriteAssets NumberSpriteLookup;
    public int curNumber;
    bool usingSprite;
    [Header("Children Objs")]
    public Transform numberGroup;
    [SerializeField] TextMeshPro numberTxt;
    [SerializeField] Transform numberInSpriteGroup;
    [SerializeField] List<SpriteRenderer> numberInSprites;
    [SerializeField] BoxCollider2D cellCollider;
    public SpriteRenderer frameSprt;
    public SpriteRenderer maskSprt;

    void Awake()
    {
        SwitchDisplayMode(false);
    }
    public void SwitchDisplayMode(bool spriteMode)
    {
        if (spriteMode)
        {
            usingSprite = true;
            numberTxt.gameObject.SetActive(false);
            //ResetToEmpty();
        }
        else
        {
            usingSprite = false;
            numberTxt.gameObject.SetActive(true);
        }
        numberTxt.SetText("");
        for (int i = 0; i < numberInSprites.Count; i++)
        {
            numberInSprites[i].gameObject.SetActive(false);
        }
    }
    public void DisplayNumber(int _number)
    {
        curNumber = _number;
        if (_number >= 0 && _number < maxValueSupported)
        {
            SwitchDisplayMode(true);
            SetNumberSprite(_number);
        }
        else
        {
            SwitchDisplayMode(false);
            SetNumberTxt(_number);
        }
        //SetCellActive(true);
    }
    public void SetColor(Color _clr, float duration = 0f)
    {
        if (duration == 0f)
        {
            numberTxt.color = _clr;
            for (int i = 0; i < numberInSprites.Count; i++)
            {
                numberInSprites[i].color = _clr;
            }
        }
        else 
        {
            numberTxt.DOColor(_clr, duration);
            for (int i = 0; i < numberInSprites.Count; i++)
            {
                numberInSprites[i].DOColor(_clr, duration);
            }
        }
    }
    public void SetFrameColor(Color _clr, float duration = 0f)
    {
        if (duration == 0f)
        {
            frameSprt.color = _clr;
        }
        else
        {
            frameSprt.DOColor(_clr, duration);
        }
    }
    void SetNumberTxt(int _number)
    {
        numberTxt.SetText(_number.ToString());
    }
    void SetNumberSprite(int _number)
    {
        int amountLeft = _number;
        int activatedCount = 0;
        bool started = false;
        for(int i=0;i< maxDigitsSupported; i++)
        {
            int digitIndex = (int)Mathf.Pow(10, maxDigitsSupported - i - 1);
            int displayNumber = Mathf.FloorToInt(amountLeft / digitIndex);
            amountLeft = amountLeft - digitIndex * displayNumber;
            //Debug.Log("digitUnit =" + digitUnit + ", calculated digit =" + targetDigit + ", number left =" + numberLeft);
            if(displayNumber != 0 || started || digitIndex == 1)
            {
                started = true;
                Sprite sprt = NumberSpriteLookup.GetSprite(displayNumber);
                if (sprt != null)
                {
                    numberInSprites[i].sprite = sprt;
                    numberInSprites[i].GetComponent<AdvSpriteSlider>().ResetBaseSprite();
                    numberInSprites[i].gameObject.SetActive(true);
                    activatedCount += 1;
                }
                else
                {
                    numberInSprites[i].gameObject.SetActive(false);
                }
            }
            else
            {
                numberInSprites[i].gameObject.SetActive(false);
            }
        }
        numberInSpriteGroup.localScale = Vector3.one * scalarByDigits[Mathf.Min(activatedCount, scalarByDigits.Count-1)];
    }
    public void SetCellEmpty(bool isActive)
    {
        cellCollider.enabled = isActive;
        if (!isActive)
        {
            numberTxt.SetText("");
            for (int i = 0; i < numberInSprites.Count; i++)
            {
                numberInSprites[i].gameObject.SetActive(false);
            }
        }
    }
    public void SetCellInteractable(bool isInteractable)
    {
        cellCollider.enabled = isInteractable;
    }
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
    public void MouseDown()
    {
        //Debug.Log(string.Format("cell coord {0},{1} clicked.", coord.x, coord.y));
        if (levelMaster != null)
        {
            if (levelMaster.levelData.themeIndex == 9)
            {
                levelMaster.AlternativeMouseDown_Theme(coord);
            }
            else
            {
                //do nothing for now
            }
        }
        else
        {
            Debug.LogError(string.Format("cell({0}) mouse down with no level master owner", gameObject.name));
        }
    }
    public void MouseUp()
    {
        //Debug.Log(string.Format("cell coord {0},{1} clicked.", coord.x, coord.y));
        if(levelMaster != null)
        {
            if (levelMaster.levelData.themeIndex == 9)
            {
                levelMaster.AlternativeMouseUp_Theme(coord);
            }
            else
            {
                levelMaster.Play(coord);
            }
        }
        else
        {
            Debug.LogError(string.Format("cell({0}) mouse up with no level master owner", gameObject.name));
        }
    }
    public void MouseExit()
    {
        //Debug.Log(string.Format("cell coord {0},{1} clicked.", coord.x, coord.y));
        if (levelMaster != null)
        {
            if (levelMaster.levelData.themeIndex == 9)
            {
                levelMaster.AlternativeMouseUp_Theme(coord);
            }
        }
        else
        {
            Debug.LogError(string.Format("cell({0}) mouse up with no level master owner", gameObject.name));
        }
    }
    public void MouseHold()
    {
        if (levelMaster != null)
        {
            if (levelMaster.levelData.themeIndex == 9)
            {
                levelMaster.AlternativeMouseHold_Theme(coord);
            }
        }
        else
        {
            Debug.LogError(string.Format("cell({0}) mouse up with no level master owner", gameObject.name));
        }
    }
    public void NumberShift(int endValue, float shuffleDuration = 0)
    {
        /*Sequence seq = DOTween.Sequence();
        int ShiftTimes = Mathf.FloorToInt(shuffleDuration / dConstants.VFX.NumberShiftAnimInterval);
        for (int i = 0; i < ShiftTimes; i++)
        {
            int digits = Mathf.FloorToInt(endValue / 10f);
            seq.AppendCallback(() => DisplayNumber(Random.Range(digits * 10, (digits + 1) * 10 - 1)));
            seq.AppendInterval(dConstants.VFX.NumberShiftAnimInterval);
        }
        seq.AppendInterval(dConstants.VFX.NumberShiftAnimInterval);
        seq.AppendCallback(() => DisplayNumber(endValue));
        */
        DisplayNumber(endValue);
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
