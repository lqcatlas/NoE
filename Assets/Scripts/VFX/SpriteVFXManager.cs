using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    [SerializeField] CustomizedEvent triggeringEvnet;
    [Header("Settings")]
    [SerializeField] float regularInterval = 1f;

    [Header("Debug")]
    [SerializeField] bool testTrigger;

    private float timer;
    private void Update()
    {
        if (testTrigger && triggeringEvnet != null)
        {
            testTrigger = false;
            triggeringEvnet.Raise();
        }
        timer += Time.deltaTime;
        if(timer >= regularInterval && regularInterval > 0)
        {
            timer = 0;
            triggeringEvnet.Raise();
        }
    }
}
