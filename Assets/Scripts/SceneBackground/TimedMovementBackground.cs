using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TimedMovementBackground : MonoBehaviour
{
    [SerializeField] Transform taregtTrans;

    [SerializeField] int MaxLoopTimes = 1000;
    [SerializeField] Vector2 initCoord;
    [SerializeField] Vector2 maxCoord;
    [SerializeField] Vector2 minCoord;
    [SerializeField] float distancePerMovement;
    [SerializeField] float degreePerMovement;

    [SerializeField] float moveDiretion;
    [SerializeField] Vector2 targetCoord;
    private void Start()
    {
        taregtTrans.localPosition = new Vector3(targetCoord.x, targetCoord.y, 0);
        moveDiretion = Random.Range(0, 360);
    }
    public void MoveAFrame()
    {
        //gen a diration
        int count = 0;
        float rng = 0;
        float newDirection = 0;
        Vector2 potentialCoord = Vector2.zero;
        while (count++ <= MaxLoopTimes)
        {
            if(count < MaxLoopTimes / 10)
            {
                rng = Random.Range(-degreePerMovement, degreePerMovement);
            }
            else
            {
                rng = Random.Range(0, 360);
            }
            newDirection = (moveDiretion + rng + 360) % 360;
            potentialCoord = targetCoord + distancePerMovement * new Vector2(Mathf.Cos(newDirection), Mathf.Sin(newDirection));
            if (potentialCoord.x > minCoord.x && potentialCoord.x < maxCoord.x && potentialCoord.y > minCoord.y && potentialCoord.y < maxCoord.y)
            {
                targetCoord = potentialCoord;
                moveDiretion = newDirection;
                break;
            }
            if(count == MaxLoopTimes)
            {
                targetCoord = initCoord;
            }
        }
        taregtTrans.localPosition = new Vector3(targetCoord.x, targetCoord.y, 0);
    }
}
