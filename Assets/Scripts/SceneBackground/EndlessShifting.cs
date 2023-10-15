using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessShifting : MonoBehaviour
{
    [Header("Anim Params")]
    [SerializeField] float MOVEMENT_SPEED;
    [SerializeField] float MAX_X;
    [SerializeField] float resetPosition;
    [SerializeField] float initialPositionA;
    [SerializeField] float initialPositionB;


    [Header("Children Objs")]
    [SerializeField] Transform spriteA;
    [SerializeField] Transform spriteB;

    private void OnEnable()
    {
        spriteA.localPosition = new Vector3(initialPositionA, 0f, 0f);
        spriteB.localPosition = new Vector3(initialPositionB, 0f, 0f);
    }
    private void Update()
    {
        spriteA.localPosition += new Vector3(MOVEMENT_SPEED, 0f, 0f) * Time.deltaTime;
        spriteB.localPosition += new Vector3(MOVEMENT_SPEED, 0f, 0f) * Time.deltaTime;
        if(spriteA.localPosition.x >= MAX_X)
        {
            ReturnToStartPosition(spriteA);
        }
        if (spriteB.localPosition.x >= MAX_X)
        {
            ReturnToStartPosition(spriteB);
        }
    }
    void ReturnToStartPosition(Transform spriteTrans)
    {
        spriteTrans.localPosition = new Vector3(resetPosition, 0f, 0f);
    }
    private void OnDisable()
    {
        DOTween.Kill(spriteA);
        DOTween.Kill(spriteB);
        //transform.localPosition = Vector3.zero;
    }
}
