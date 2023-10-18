using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SelectorNode;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class NoteLauncher : MonoBehaviour
{
    [Header("Children Objs")]
    [SerializeField] SpriteRenderer icon;

    public void HoverOn()
    {
        icon.DOColor(dConstants.UI.DefaultColor_2nd, dConstants.UI.StandardizedBtnAnimDuration);
    }
    public void HoverOff()
    {
        icon.DOColor(dConstants.UI.DefaultColor_1st, dConstants.UI.StandardizedBtnAnimDuration);
    }
    public void MouseUp()
    {
        //launcher msg box
        //update text
        //written in theme
    }
}
