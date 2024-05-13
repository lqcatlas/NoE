using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EndlessNoise : MonoBehaviour
{
    [Header("Anim Params")]
    [SerializeField] ThemeResourceLookup resourceHub;
    [SerializeField] LevelRecords levelRecords;
    [SerializeField] Vector2 screenRangeX;
    [SerializeField] Vector2 screenRangeY;
    [SerializeField] Vector2 sizeRange;
    [SerializeField] Vector2 durationRange;
    [SerializeField] Color taregtClr;

    [SerializeField] float frequencyPerSec;
    [SerializeField] int flakesPerSplash = 1;

    [Header("Children Objs")]
    [SerializeField] GameObject flakeTemplate;

    float timer;
    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer >= 1 / frequencyPerSec)
        {
            for (int i = 0; i < flakesPerSplash; i++)
            {
                GenerateOneFlake();
            }
            timer = 0;
        }
    }
    void GenerateOneFlake()
    {
        List<int> unlockedThemes = levelRecords.unlockedThemes;
        Sprite targetSprite = null;
        if (unlockedThemes.Count > 0)
        {
            int rng = Random.Range(0, unlockedThemes.Count);
            targetSprite = resourceHub.GetThemeIcon(unlockedThemes[rng]);
        }
        else
        {
            targetSprite = resourceHub.GetThemeIcon(1);
        }
        GameObject obj = Instantiate(flakeTemplate, transform);
        obj.GetComponent<SpriteRenderer>().sprite = targetSprite;
        obj.transform.localScale = Vector3.one * Random.Range(sizeRange.x, sizeRange.y);
        obj.transform.localPosition = new Vector3(Random.Range(screenRangeX.x, screenRangeX.y), Random.Range(screenRangeY.x, screenRangeY.y), 0);
        float duration  = Random.Range(durationRange.x, durationRange.y);

        obj.GetComponent<SpriteRenderer>().color = new Color(taregtClr.r, taregtClr.g, taregtClr.b, 0f);
        obj.GetComponent<SpriteRenderer>().DOFade(taregtClr.a, duration / 2f).OnStart(() => obj.SetActive(true)).OnComplete(() => Destroy(obj)).SetLoops(2, LoopType.Yoyo);
    }
}
