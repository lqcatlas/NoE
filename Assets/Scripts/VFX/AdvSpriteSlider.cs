using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AdvSpriteSlider : MonoBehaviour
{
    public List<Sprite> availableSprites;
    public Sprite curSprite;
    public float triggerChance = 1f;
    public bool SwitchByOrder = false;
    private void Start()
    {
        if(availableSprites.Count == 0)
        {
            ResetBaseSprite();
        }
    }
    public void ResetBaseSprite()
    {
        availableSprites = new List<Sprite>();
        SpriteRenderer comp = null;
        if (TryGetComponent<SpriteRenderer>(out comp))
        {
            curSprite = comp.sprite;
        }
        else
        {
            Debug.LogError(string.Format("unable to get sprite renderer on game object ({0})", gameObject.name));
            return;
        }
        if (curSprite.name.Contains("@"))
        {
            int locator = curSprite.name.IndexOf("@");
            string spriteNameLocator = curSprite.name.Substring(0, locator);
            Debug.Log(string.Format("looking for sprite with name of {0}", spriteNameLocator, locator));

            availableSprites.Clear();
            //Debug.LogWarning("potential cpu consuming code LoadALL()");
            List<Sprite> allSprites = Resources.LoadAll<Sprite>("sprites").ToList();
            for (int i = 0; i < allSprites.Count; i++)
            {
                if (allSprites[i].name.Contains(spriteNameLocator+"@") && allSprites[i].name != curSprite.name)
                {
                    availableSprites.Add(allSprites[i]);
                }
            }
        }
        else
        {
            //Debug.LogWarning(string.Format("sprite renderer on game object ({0}) has no valid brothers sprites", gameObject.name));
            return;
        }
    }
    public void SpriteSwitch()
    {
        if(availableSprites.Count == 0)
        {
            return;
        }
        float roll = Random.Range(0f, 1f);
        if(roll <= triggerChance)
        {
            int index = 0;
            if (!SwitchByOrder)
            {
                index = Random.Range(0, availableSprites.Count);
            }
            availableSprites.Add(curSprite);
            curSprite = availableSprites[index];
            availableSprites.RemoveAt(index);
            GetComponent<SpriteRenderer>().sprite = curSprite;
        }
    }
}
