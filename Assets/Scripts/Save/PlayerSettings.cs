using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "PlayingData/PlayerSettings")]
public class PlayerSettings : ScriptableObject
{
    public float audioVolume = 1f;
    public float musicVolume = 0.7f;
    public float soundVolume = 0.7f;
    public int introCount;

}
