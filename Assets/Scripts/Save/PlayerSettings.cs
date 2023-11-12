using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "PlayingData/PlayerSettings")]
public class PlayerSettings : ScriptableObject
{
    public float audioVolume = 0.5f;
    public int introCount;

}
