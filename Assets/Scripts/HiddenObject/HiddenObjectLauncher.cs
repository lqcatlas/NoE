using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HiddenObjectLauncher : MonoBehaviour
{
    static public HiddenObjectLauncher singleton;
    private void Awake()
    {
        if (singleton == null)
        {
            singleton = this;
        }
        else
        {
            Destroy(this);
        }
    }
    public ThemeResourceLookup themeResourceLookup;
    public Transform pageHolder;

    public void LauncherHiddenObjectPage(SheetItem_ThemeSetup themeData)
    {
        ClearExistingPages();
        GameObject templatePage = themeResourceLookup.GetThemeHiddenObjectPage(themeData.themeUID);
        if(templatePage == null)
        {
            return;
        }
        GameObject newPage = Instantiate(templatePage, pageHolder);
        newPage.GetComponent<HiddenObjectPage>().StartHiddenObject(themeData);
    }
    void ClearExistingPages()
    {
        List<HiddenObjectPage> currentPages = pageHolder.GetComponentsInChildren<HiddenObjectPage>().ToList();
        for(int i=0;i< currentPages.Count; i++)
        {
            Destroy(currentPages[i].gameObject);
        }
    }
}
