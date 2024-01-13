using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StaticAssetLocalize : MonoBehaviour
{
    [SerializeField] TextMeshPro localizeTarget;
    [SerializeField] string LocalizedKey;

    private void Start()
    {
        if(localizeTarget != null)
        {
            localizeTarget.SetText(LocalizedAssetLookup.singleton.Translate(LocalizedKey != "" ? LocalizedKey : localizeTarget.text));
        }
    }
    private void OnEnable()
    {
        if (localizeTarget != null)
        {
            localizeTarget.SetText(LocalizedAssetLookup.singleton.Translate(LocalizedKey != "" ? LocalizedKey : localizeTarget.text));
        }
    }
}
