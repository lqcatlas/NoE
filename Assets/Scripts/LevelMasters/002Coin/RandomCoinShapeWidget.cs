using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomCoinShapeWidget : MonoBehaviour
{
    public SpriteRenderer coinShape;
    public SpriteRenderer coinSign;

    public List<Sprite> shapeOptions;
    public List<Sprite> signOptions;

    public void GenerateARandomCoin(int sign)
    {
        float sizeMin = 0.5f;
        float sizeMax = 0.7f;
        float greyMin = 0.55f;
        float greyMax = 0.8f;

        int rngSpriteIndex = Random.Range(0, shapeOptions.Count);
        coinShape.GetComponent<SpriteRenderer>().sprite = shapeOptions[rngSpriteIndex];
        float grey = Random.Range(greyMin, greyMax);
        coinShape.GetComponent<SpriteRenderer>().color = new Color(grey, grey, grey);
        Transform trans = GetComponent<Transform>();
        trans.Rotate(new Vector3(0f, 0f, Random.Range(0f, 1f) * 360));
        float size = Random.Range(sizeMin, sizeMax);
        trans.localScale = new Vector3(size, size, 1f);

        coinSign.sprite = signOptions[sign];
    }
}
