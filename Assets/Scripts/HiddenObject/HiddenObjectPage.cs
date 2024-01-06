using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using TreeEditor;
using UnityEngine;

public class HiddenObjectPage : MonoBehaviour
{
    [Header("Gameplay Data")]
    //public LevelSelector selector;
    [SerializeField] SheetItem_ThemeSetup themeData;

    [Header("Animation Params")]
    public float ANIM_DURATION;
    public float HINT_DELAY;
    [Header("Children Objs")]
    public GameObject objectGroup;
    public GameObject riddleGroup;
    public GameObject photo;

    public TextMeshPro riddleTitle;
    public TextMeshPro riddleDesc;
    public GameObject themeIcon;
    public GameObject hintRing;

    [SerializeField] bool displayOn;
    [SerializeField] float hintTimer;
    public void Awake()
    {
        displayOn = false;
    }
    public void StartHiddenObject(SheetItem_ThemeSetup source = null)
    {
        displayOn = true;
        if(source != null)
        {
            themeData = source;
        }
        riddleTitle.SetText(LocalizedAssetLookup.singleton.Translate("@Loc=ui_hiddenobject_riddletitle@@"));
        riddleDesc.SetText(LocalizedAssetLookup.singleton.Translate(themeData.hint));
        hintTimer = 0;
        themeIcon.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);
        hintRing.SetActive(false);
    }
    private void FixedUpdate()
    {
        if (displayOn)
        {
            hintTimer += Time.fixedDeltaTime;
        }
        if(hintTimer >= HINT_DELAY)
        {
            hintRing.SetActive(true);
            hintTimer -= HINT_DELAY;
        }
    }
    #region BtnFunc
    public void OnIconClick()
    {
        //play found animation
        themeIcon.GetComponent<SpriteRenderer>().DOFade(1f, ANIM_DURATION);
        photo.GetComponent<photoVFXCtrl>().ZoomIn(ANIM_DURATION);
        photo.GetComponent<photoVFXCtrl>().Replace(ANIM_DURATION);
    }
    public void OnCloseClick()
    {
        Destroy(gameObject);
    }
    #endregion
}
