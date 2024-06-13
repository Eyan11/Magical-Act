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
    [SerializeField] private float jumpBufferTime;
    [SerializeField] private float maxMoveSpeed;
    [SerializeField] private float moveForce;
    [SerializeField] private float jumpForce;
    [SerializeField] private float groundedRayDist;
    [SerializeField] private float groundedDrag;
    [SerializeField] private float airDrag;
    private const int MOVE_FORCE_MULT = 50;
    private float moveDir;
    private Vector2 curMoveForce;
    private float canJumpTimer;
    private float jumpInputTimer = 0f;
    public bool IsGrounded { get; private set;}
    private Vector3 playerScale = Vector3.one;

    private void Awake() {
        body = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        inputScript = GetComponentInParent<PlayerInput>();
        animScript = GetComponent<PlayerAnimations>();
    }

    private void OnEnable() {
        curMoveForce = Vector2.zero;
        playerScale = Vector3.one;
        canJumpTimer = -1f;
        StartCoroutine(GroundCheckCoroutine());
    }

    private void OnDisable() {
        // stop x velocity
        body.velocity = new Vector2(0f, body.velocity.y);
    }
    
    private void Update() {
        MoveCalculations();
        JumpCalculations();
        LimitSpeed();
    }

    private void FixedUpdate() {
        body.AddForce(curMoveForce * MOVE_FORCE_MULT, ForceMode2D.Force);
    }

    /** Calculates x and y direction movement **/
    private void MoveCalculations() {
        // input
        moveDir = inputScript.MoveInput;

        // move force
        if(moveDir == 0f)
            curMoveForce.x = 0f;
        else
            curMoveForce.x = moveDir * moveForce;

        // make player face move input
        if(inputScript.MoveInput != 0f)
            playerScale.x = Mathf.Sign(inputScript.MoveInput);
        transform.localScale = playerScale;
    }

    private void JumpCalculations() {
        canJumpTimer -= Time.deltaTime;
        jumpInputTimer -= Time.deltaTime;

        // jump input buffer
        if(inputScript.JumpInput)
            jumpInputTimer = jumpBufferTime;

        if(jumpInputTimer > 0f && canJumpTimer > 0f)
            Jump();
    }

    /** Caps x velocity at moveSpeed **/
    private void LimitSpeed() {

        if(body.velocity.x > maxMoveSpeed)
            body.velocity = new Vector2(maxMoveSpeed, body.velocity.y);
        else if(body.velocity.x < -maxMoveSpeed)
            body.velocity = new Vector2(-maxMoveSpeed, body.velocity.y);

        Debug.Log(body.velocity.x);
    }

    /** Applies jump speed to rigidbody **/
    private void Jump() {
        canJumpTimer = -1f;
        jumpInputTimer = -1f;
        body.drag = airDrag;
        body.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    /** Returns true if grounded **/
    private IEnumerator GroundCheckCoroutine() {

        IsGrounded = Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, 
            Vector2.down, groundedRayDist, floorLayer);

        if(IsGrounded == true && body.velocity.y < 0.1) {
            canJumpTimer = coyoteTime;
            body.drag = groundedDrag;
        }
        else
            body.drag = airDrag;

        yield return new WaitForSeconds(groundedRayInterval);
        StartCoroutine(GroundCheckCoroutine());
    }
}
