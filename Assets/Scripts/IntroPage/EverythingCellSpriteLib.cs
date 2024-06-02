using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Lookup/EverythingSpriteLib")]
public class EverythingCellSpriteLib : ScriptableObject
{
    public List<Sprite> spriteOptions;
    public Sprite GetRNGSprite()
    {
        if(spriteOptions.Count == 0)
        {
            return null;
        }
        int rng = Random.Range(0, spriteOptions.Count);
        return spriteOptions[rng];
    }
}
