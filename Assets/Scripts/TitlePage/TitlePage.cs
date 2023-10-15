using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitlePage : MonoBehaviour
{
    static public TitlePage singleton;
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
    [SerializeField] SpriteRenderer titleSprite;
    [SerializeField] GameObject confirmBtn;
    public void GoToTitlePage()
    {
        gameObject.SetActive(true);
    }

    public void OpenSelector()
    {
        LevelSelector.singleton.GoToSelector();
        gameObject.SetActive(false);
    }
}
