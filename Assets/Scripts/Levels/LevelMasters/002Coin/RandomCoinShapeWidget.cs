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
        float sizeMin = 0.6f;
        float sizeMax = 0.6f;
        float greyMin = 0.45f;
        float greyMax = 0.65f;

        int rngSpriteIndex = Random.Range(0, shapeOptions.Count);
        //coinShape.GetComponent<SpriteRenderer>().sprite = shapeOptions[rngSpriteIndex];
        coinShape.GetComponent<SpriteRenderer>().sprite = signOptions[sign];
        float grey = Random.Range(greyMin, greyMax);
        coinShape.GetComponent<SpriteRenderer>().color = new Color(grey, grey, grey);
        Transform trans = GetComponent<Transform>();
        trans.Rotate(new Vector3(0f, 0f, Random.Range(0f, 1f) * 360));
        float size = Random.Range(sizeMin, sizeMax);
        trans.localScale = new Vector3(size, size, 1f);

        //coinSign.sprite = signOptions[sign];
    }
}
