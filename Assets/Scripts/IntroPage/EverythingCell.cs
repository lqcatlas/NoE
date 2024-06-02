using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class EverythingCell : MonoBehaviour
{
    static float switchChance = .4f;
    [SerializeField] EverythingCellSpriteLib lib;
    [SerializeField] SpriteRenderer sprt;
    public void UpdateSprite()
    {
        float rng = Random.Range(0f, 1f);
        if (rng > switchChance)
        {
            sprt.sprite = lib.GetRNGSprite();
        }
    }
}
