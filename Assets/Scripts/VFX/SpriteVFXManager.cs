using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class TimedEvents
{
    public CustomizedEvent triggeringEvent;
    public float interval;
    public float timer;
}

public class SpriteVFXManager : MonoBehaviour
{
    //singleton setup
    static public SpriteVFXManager singleton;
    private void Awake()
    {
        if (singleton == null)
        {
            singleton = this;
        }
        else
        {
            Destroy(this);
        }
    }
    
    //[SerializeField] CustomizedEvent triggeringEvnet;
    //[SerializeField] float regularInterval = 1f;
    [Header("Settings")]
    [SerializeField] List<TimedEvents> events;
    

    [Header("Debug")]
    [SerializeField] bool testTrigger;

    private void Update()
    {
        for(int i=0;i< events.Count; i++)
        {
            if (testTrigger && events[i].triggeringEvent != null)
            {
                events[i].triggeringEvent.Raise();
            }
            events[i].timer += Time.deltaTime;
            if (events[i].timer >= events[i].interval && events[i].interval > 0f)
            {
                events[i].timer -= events[i].interval;
                events[i].triggeringEvent.Raise();
            }
        }
        testTrigger = false;
    }
}
