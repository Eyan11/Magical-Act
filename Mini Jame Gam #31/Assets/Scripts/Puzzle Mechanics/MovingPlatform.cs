using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private Transform endPoint;
    [SerializeField] private float pauseTime;
    private const float MOVE_SPEED = 0.3f;
    private const float PLAYER_PLATFORM_VEL = 0.7f;
    private const float HAT_PLATFORM_VEL = 1f;
    private int returnedDir;
    private bool isHorizontalMovement = false;
    private bool isDirectionReversed = false; 
    private float pauseCountdown = -1f;
    private Rigidbody2D body;
    private Vector3 startPos;
    private Vector3 endPos;
    private float lerpPercent = 0.001f;
    private bool isActivated = false;
    private bool hasLoopMovement = false;
    private int direction = 1;

    private void Awake() {
        startPos = transform.position;
        endPos = endPoint.position;
        body = GetComponent<Rigidbody2D>();

        // determine if horizontal or vertical movement
        if((Mathf.Abs(startPos.x) - Mathf.Abs(endPos.x)) > 0.5)
            isHorizontalMovement = true;

        // calculate right direction
        if(isHorizontalMovement && (endPos.x < startPos.x))
            isDirectionReversed = true;
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

    public float GetVelocity(char movementType) {

        // platform is moving
        if(isHorizontalMovement && lerpPercent > 0 && lerpPercent < 1) {

            // account for right direction
            if(isDirectionReversed)
                returnedDir = direction * -1;
            else
                returnedDir = direction;
        }
        else
            returnedDir = 0;

        switch(movementType) {
            case 'P':
                return returnedDir * PLAYER_PLATFORM_VEL;
            case 'H':
                return returnedDir * HAT_PLATFORM_VEL;
            default:
                Debug.LogError("Invalid argument in MovingPlatform > GetVelocity()");
                return 0;
        }
    }

    private void FixedUpdate() {

        // for looping movement (switch activation)
        if(hasLoopMovement && isActivated) {
            
            // pause
            if(pauseCountdown > 0) {
                pauseCountdown -= Time.fixedDeltaTime;
                return;
            }

            // move in loop
            lerpPercent += MOVE_SPEED * direction * Time.fixedDeltaTime;
            body.MovePosition(Vector3.Lerp(startPos, endPos, lerpPercent));

            // change directions when start or end is reached
            if(lerpPercent >= 1) {
                direction = -1;
                pauseCountdown = pauseTime;
            }
            else if(lerpPercent <= 0) {
                direction = 1;
                pauseCountdown = pauseTime;
            }
            
        }

        // for non-looping movement (pressure plate activation)
        else if(!hasLoopMovement) {

            // move to end
            if(isActivated && lerpPercent <= 1) {
                direction = 1;
                lerpPercent += MOVE_SPEED * Time.fixedDeltaTime;
                body.MovePosition(Vector3.Lerp(startPos, endPos, lerpPercent));
            }
            // move to start
            else if(!isActivated && lerpPercent >= 0) {
                direction = -1;
                lerpPercent -= MOVE_SPEED * Time.fixedDeltaTime;
                body.MovePosition(Vector3.Lerp(startPos, endPos, lerpPercent));
            }
        }
    }
}
