using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SFXClipGroup
{
    public List<AudioClip> clips;
    private int lastPlayedIndex;
    public AudioClip GetClip(int index = -1)
    {
        if (index == -1)
        {
            int rng = Random.Range(0, clips.Count);
            if (rng == lastPlayedIndex)
            {
                rng = (lastPlayedIndex + 1) % clips.Count;
            }
            return clips[rng];
        }
        else if(index >= clips.Count || index < 0)
        {
            Debug.LogError(string.Format("invalid audio clip index {0}", index));
            return null;
        }
        else
        {
            return clips[index];
        }
    }
}
