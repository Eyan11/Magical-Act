using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private Transform endPoint;
    [SerializeField] private float moveSpeed;
    private Vector3 startPos;
    private Vector3 endPos;
    private float lerpPercent;
    private bool isActivated = false;
    private bool hasLoopMovement = false;
    private int direction = 1;

    private void Awake() {
        startPos = transform.position;
        endPos = endPoint.position;
    }

    public void ActivatePlatform() {
        isActivated = true;
    }

    public void DeactivatePlatform() {
        isActivated = false;
    }

    public void TogglePlatformLoop() {
        hasLoopMovement = true;
        isActivated = !isActivated;
    }

    private void Update() {

        // for looping movement (switch activation)
        if(hasLoopMovement && isActivated) {
            
            // move in loop
            lerpPercent += moveSpeed * direction * Time.deltaTime;
            transform.position = Vector3.Lerp(startPos, endPos, lerpPercent);

            // change directions when start or end is reached
            if(lerpPercent >= 1)
                direction = -1;
            else if(lerpPercent <= 0)
                direction = 1;
        }

        // for non-looping movement (pressure plate activation)
        else if(!hasLoopMovement) {

            // move to end
            if(isActivated && lerpPercent <= 1) {
                lerpPercent += moveSpeed * Time.deltaTime;
                transform.position = Vector3.Lerp(startPos, endPos, lerpPercent);
            }
            // move to start
            else if(!isActivated && lerpPercent >= 0) {
                lerpPercent -= moveSpeed * Time.deltaTime;
                transform.position = Vector3.Lerp(startPos, endPos, lerpPercent);
            }
        }
    }
}
