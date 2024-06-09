using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header ("References")]
    [SerializeField] private LayerMask floorLayer;
    private PlayerAnimations animScript;
    private Rigidbody2D body;
    private BoxCollider2D coll;
    private PlayerInput inputScript;

    [Header ("Settings")]
    [SerializeField] private float groundedRayInterval;
    [SerializeField] private float coyoteTime;
    [SerializeField] private float moveAcceleration;
    [SerializeField] private float moveDeaccelerationFactor;
    [SerializeField] private float moveMaxSpeed;
    [SerializeField] private float jumpSpeed;
    [SerializeField] private float groundedRayDist;

    private float moveDir;
    private Vector2 curVelocity;
    private float canJumpTimer;
    public bool IsGrounded { get; private set;}
    private Vector3 playerScale = Vector3.one;

    private void Awake() {
        body = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        inputScript = GetComponentInParent<PlayerInput>();
        animScript = GetComponent<PlayerAnimations>();
    }

    private void OnEnable() {
        curVelocity = Vector2.zero;
        playerScale = Vector3.one;
        canJumpTimer = -1f;
        StartCoroutine(GroundCheckCoroutine());
    }

    private void OnDisable() {
        // stop x velocity
        body.velocity = new Vector2(0f, body.velocity.y);
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

    /** Applied movement calculations to rigidbody and rotates player **/
    private void MovePlayer() {
        curVelocity.y = body.velocity.y;
        body.velocity = curVelocity;

        // make player face move input
        if(inputScript.MoveInput != 0f)
            playerScale.x = Mathf.Sign(inputScript.MoveInput);
        transform.localScale = playerScale;
    }

    /** Caps x velocity at moveSpeed **/
    private void LimitSpeed() {

        if(curVelocity.x > moveMaxSpeed)
            curVelocity.x = moveMaxSpeed;
        else if(curVelocity.x < -moveMaxSpeed)
            curVelocity.x = -moveMaxSpeed;
    }

    /** Applies jump speed to rigidbody **/
    private void Jump() {
        canJumpTimer = -1f;
        body.velocity = new Vector2(body.velocity.x, jumpSpeed);
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
