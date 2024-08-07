using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "PlayingData/PlayerSettings")]
public class PlayerSettings : ScriptableObject
{
    public float audioVolume;
    public float musicVolume;
    public float soundVolume;
    public int introCount;
    public LanguageOption curLan;
}
