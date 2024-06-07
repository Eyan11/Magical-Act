using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header ("References")]
    [SerializeField] private LayerMask floorLayer;
    private Rigidbody2D body;
    private BoxCollider2D coll;
    private PlayerInput inputScript;

    [Header ("Settings")]
    [SerializeField] private float groundedRayInterval;
    [SerializeField] private float coyoteTime;
    [SerializeField] private float moveAcceleration;
    [SerializeField] private float moveDeaccelerationFactor;
    [SerializeField] private float moveMaxSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float groundedRayDist;

    private float moveDir;
    private Vector2 curVelocity;
    private float canJumpTimer = 0f;
    public bool IsGrounded { get; private set;}

    private void Awake() {
        body = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        inputScript = GetComponentInParent<PlayerInput>();

        StartCoroutine(GroundCheckCoroutine());
    }

    private void OnEnable() {
        // reset variables
        curVelocity = Vector2.zero;
    }
    
    private void Update() {
        canJumpTimer -= Time.deltaTime;
        MovementCalculations();
        LimitSpeed();
        MovePlayer();
    }

    /** Calculates x and y direction movement **/
    private void MovementCalculations() {

        moveDir = inputScript.MoveInput;

        // if no input, slow down
        if(moveDir == 0f && curVelocity.x != 0f) {
            curVelocity.x *= moveDeaccelerationFactor * Time.deltaTime;
            
            // set velocity to 0
            if(Mathf.Abs(curVelocity.x) < 0.2f)
                curVelocity.x = 0f;
        }
        // if input, speed up
        else
            curVelocity.x += moveDir * moveAcceleration * Time.deltaTime;

        if(inputScript.JumpInput && canJumpTimer > 0f)
            Jump();

    }

    /** Applies x direction movement calculations to rigidbody **/
    private void MovePlayer() {
        curVelocity.y = body.velocity.y;
        body.velocity = curVelocity;
    }

    /** Caps x velocity at moveSpeed **/
    private void LimitSpeed() {

        if(curVelocity.x > moveMaxSpeed)
            curVelocity.x = moveMaxSpeed;
        else if(curVelocity.x < -moveMaxSpeed)
            curVelocity.x = -moveMaxSpeed;

    }

    /** Applies jump force to rigidbody **/
    private void Jump() {
        canJumpTimer = -1f;
        body.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
    }

    /** Returns true if grounded **/
    private IEnumerator GroundCheckCoroutine() {

        IsGrounded = Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, 
            Vector2.down, groundedRayDist, floorLayer);

        if(IsGrounded == true) {
            canJumpTimer = coyoteTime;
        }

        yield return new WaitForSeconds(groundedRayInterval);
        StartCoroutine(GroundCheckCoroutine());
    }
}
