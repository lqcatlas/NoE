using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpriteAsNumber
{
    public int targetNumber;
    public Sprite matchingSprite;
}
[CreateAssetMenu(menuName = "Lookup/NumberSprite")]
public class NumberSpriteAssets : ScriptableObject
{
    public List<SpriteAsNumber> availableNumbers;
    public Sprite GetSprite(int number)
    {
        for(int i=0;i< availableNumbers.Count; i++)
        {
            if (availableNumbers[i].targetNumber == number)
            {
                return availableNumbers[i].matchingSprite;
            }
        }
        return null;
    }
}
