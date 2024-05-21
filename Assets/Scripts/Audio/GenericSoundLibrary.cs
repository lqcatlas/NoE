using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Lookup/GenericSFXLib")]
public class GenericSoundLibrary : ScriptableObject
{
    public List<AudioClip> play;
    public List<AudioClip> levelWin;
    public List<AudioClip> levelFail;

    public AudioClip GetPlaySFX()
    {
        return play[Random.Range(0, play.Count)];
    }
    public AudioClip GetLevelWinSFX()
    {
        return levelWin[Random.Range(0, levelWin.Count)];
    }
    public AudioClip GetLevelFailSFX()
    {
        return levelFail[Random.Range(0, levelFail.Count)];
    }
}
