using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using System.Linq;
using DG.Tweening;

public class gCell : MonoBehaviour
{
    [Header("parent")]
    public int coord;
    [SerializeField] gBoard parentBoard;

    [Header("children")]
    [SerializeField] GameObject bg_group;
    [SerializeField] GameObject bg_template;
    [SerializeField] TextMeshProUGUI number;
    [SerializeField] Image tagIcon;
    [SerializeField] TextMeshProUGUI tagNumber;

    [Header("data")]
    [SerializeField] Cell curCell;
    [SerializeField] List<Sprite> flavor_sprites = new List<Sprite>();

    // Start is called before the first frame update
    void Start()
    {
        CellInit();
    }
    public void CellInit()
    {
        if (parentBoard == null)
        {
            Debug.LogError(string.Format("{0}: NO gBoard attached. Abort self.", gameObject.name));
            gameObject.SetActive(false);
            return;
        }
        if(coord == 0)
        {
            Debug.LogError(string.Format("{0}: 0 on coord. Abort self.", gameObject.name));
            gameObject.SetActive(false);
            return;
        }
        //clear bg_group children
        List<Image> deprecatedObjs = bg_group.transform.GetComponentsInChildren<Image>().ToList();
        if(deprecatedObjs.Count > 0)
        {
            for (int i = 0; i < deprecatedObjs.Count; i++)
            {
                Destroy(deprecatedObjs[i].gameObject);
            }
            bg_group.transform.DetachChildren();
        }
    }
    public void CellPreview()
    {
        parentBoard.PreviewPlay(coord);
    }
    public void CellPreviewRevert()
    {
        parentBoard.PreviewPlayEnd();
    }
    public void CellPlay()
    {
        parentBoard.ToolPlay(coord);
        if (parentBoard.GM.puzzleTheme == dConstants.GameTheme.Coin)
        {
            CoinPlacedFX();
        }
    }
    //update cell display based on cll data and puzzle theme
    public void UpdateDisplay(Cell targetCell, dConstants.GameTheme theme)
    {
        curCell = new Cell(targetCell);
        if(theme == dConstants.GameTheme.Clock)
        {
            number.text = curCell.value == 0 ? "" : curCell.value.ToString();
            tagIcon.gameObject.SetActive(curCell.status == 1);
        }
        else if(theme == dConstants.GameTheme.Coin)
        {
            number.text = curCell.value.ToString();
            tagNumber.text = curCell.status.ToString();
            tagIcon.gameObject.SetActive(curCell.status >= 1);
            tagNumber.gameObject.SetActive(curCell.status >= 1);
        }
        gameObject.SetActive(true);
    }
    public void CoinPlacedFX()
    {
        int XYRange = 80;
        float sizeMin = 80;
        float sizeMax = 120;
        float greyMin = 0.2f;
        float greyMax = 0.5f;
        if (flavor_sprites.Count == 0)
        {
            return;
        }
        else
        {
            int rngSpriteIndex = Random.Range(0, flavor_sprites.Count);
            GameObject sprt = Instantiate(bg_template, bg_group.transform);
            sprt.GetComponent<Image>().sprite = flavor_sprites[rngSpriteIndex];
            float grey = Random.Range(greyMin, greyMax);
            sprt.GetComponent<Image>().color = new Color(grey, grey, grey);
            RectTransform rect = sprt.GetComponent<RectTransform>();
            rect.anchoredPosition = new Vector2(Random.Range(-1f, 1f) * XYRange, Random.Range(-1f, 1f) * XYRange);
            rect.Rotate(new Vector3(0f, 0f, Random.Range(0f, 1f) * 360));
            float size = Random.Range(sizeMin, sizeMax);
            rect.sizeDelta = new Vector2(size, size);

            //place animation
            float distance = 2000f;
            float duration = 0.3f;
            float start_size = 10f;
            float shake_strength = 5f;
            float rotate_angle = Random.Range(0f, Mathf.PI * 2);
            Vector3 moveFromCoord = new Vector3(Mathf.Cos(rotate_angle) * distance, Mathf.Sin(rotate_angle) * distance, 0);
            rect.DOLocalMove(moveFromCoord, duration).From();
            rect.DOScale(Vector3.one * start_size, duration).From();
            rect.GetComponent<Image>().DOFade(0f, duration).From();
            rect.DOShakePosition(duration/2, Vector3.one * shake_strength, 50).SetDelay(duration);
        }
    }
}
