using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] bool isHorizontalMovement;
    [SerializeField] private float distance;
    [SerializeField] private float pauseTime;
    private bool isMovingToEnd = true;
    private const float MOVE_SPEED = 1f; 
    private float pauseCountdown = -1f;
    private Rigidbody2D body;
    private Vector3 startPos;
    private Vector3 endPos;
    private bool isActivated = false;
    private bool hasLoopMovement = false;
    private Vector2 velDir = Vector2.zero;

    private void Awake() {
        startPos = transform.position;
        body = GetComponent<Rigidbody2D>();
        CalculateVelocity();

        endPos = startPos + new Vector3(velDir.x * Mathf.Abs(distance), velDir.y * Mathf.Abs(distance), 0);
    }

    public void ActivatePlatform() {
        isActivated = true;
        isMovingToEnd = true;
        CalculateVelocity();
        body.velocity = MOVE_SPEED * velDir;
    }

    public void DeactivatePlatform() {
        isActivated = false;
        isMovingToEnd = false;
        CalculateVelocity();
        body.velocity = MOVE_SPEED * velDir;
    }

    public void TogglePlatformLoop() {
        hasLoopMovement = true;
        isActivated = !isActivated;

        if(!isActivated)
            body.velocity = Vector2.zero;
    }

    /** Resets position and variables, called when level is restarted **/
    public void ResetPlatform() {
        transform.position = startPos;
        isActivated = false;
        isMovingToEnd = true;
        hasLoopMovement = false;
        pauseCountdown = -1f;
        CalculateVelocity();
    }

    public float GetHorizontalVelocity() {
        return body.velocity.x;
    }

    private bool IsPastPosition(Vector3 destination) {

        if((destination - transform.position).sqrMagnitude < 0.001)
            return true;
        else
            return false;
    }

    private void CalculateVelocity() {

        // set values
        if(isHorizontalMovement)
            velDir = Vector2.right;
        else
            velDir = Vector2.up;

        // set sign
        velDir *= Mathf.Sign(distance);

        // flip direction if moving to start (for loop movement)
        if(!isMovingToEnd)
            velDir *= -1;
    }


    private void Update() {

        // for looping movement (switch activation)
        if(hasLoopMovement && isActivated) {
            
            // pause
            if(pauseCountdown > 0) {
                pauseCountdown -= Time.fixedDeltaTime;
                return;
            }

            // move in loop
            body.velocity = MOVE_SPEED * velDir;

            // change directions
            if(isMovingToEnd && IsPastPosition(endPos)) {

                isMovingToEnd = false;
                velDir *= -1;
                pauseCountdown = pauseTime;
                body.velocity = Vector2.zero;
            }
            else if(!isMovingToEnd && IsPastPosition(startPos)) {

                isMovingToEnd = true;
                velDir *= -1;
                pauseCountdown = pauseTime;
                body.velocity = Vector2.zero;
            }
        }

        // for non-looping movement (pressure plate activation)
        else if(!hasLoopMovement && body.velocity != Vector2.zero) {

            // reached end
            if(isActivated && IsPastPosition(endPos))
                body.velocity = Vector2.zero;
            // reached start
            else if(!isActivated && IsPastPosition(startPos))
                body.velocity = Vector2.zero;
        }
    }
}
