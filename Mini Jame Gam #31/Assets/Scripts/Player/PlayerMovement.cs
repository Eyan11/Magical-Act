using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header ("References")]
    private int floorLayer;
    private int movingPlatformLayer;
    private PlayerAnimations animScript;
    private Rigidbody2D body;
    private BoxCollider2D coll;
    private PlayerInput inputScript;
    private Transform playerHolder;

    [Header ("Settings")]
    [SerializeField] private float groundedRayInterval;
    [SerializeField] private float coyoteTime;
    [SerializeField] private float jumpBufferTime;
    [SerializeField] private float groundedRayDist;
    [SerializeField] private float maxMoveSpeed;
    [SerializeField] private float exitPlatformDeacceleration;
    [SerializeField] private float moveAcceleration;
    [SerializeField] private float jumpSpeed;
    private Vector2 finalMoveSpeed = Vector2.zero;
    public float CurMoveVel { private set; get; }
    private float platformVel = 0f;
    private float moveDir;
    private float canJumpTimer;
    private float jumpInputTimer = 0f;
    public bool IsGrounded { get; private set;}
    private Vector3 playerScale = Vector3.one;
    private RaycastHit2D[] hits = new RaycastHit2D[20];
    private int numHit = 0;
    private bool allowInput = true;

    private void Awake() {
        body = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        inputScript = GetComponentInParent<PlayerInput>();
        animScript = GetComponent<PlayerAnimations>();
        playerHolder = transform.root;
        floorLayer = LayerMask.NameToLayer("Floor");
        movingPlatformLayer = LayerMask.NameToLayer("Moving Platform");
    }

    private void OnEnable() {
        EnableInput();
        playerScale = Vector3.one;
        canJumpTimer = -1f;
        StartCoroutine(GroundCheckCoroutine());
    }

    private void OnDisable() {
        // stop x velocity
        body.velocity = new Vector2(0f, body.velocity.y);
    }

    public void DisableInput() {
        allowInput = false;
        CurMoveVel = 0f;
    }

    public void EnableInput() {
        allowInput = true;
    }
    
    private void Update() {
        if(allowInput) {
            MoveCalculations();
            JumpCalculations();
        }
        MoveAndLimit();
    }


    /** Calculates x direction movement **/
    private void MoveCalculations() {
        // input
        moveDir = inputScript.MoveInput;

        // movement calculations
        if(moveDir == 0f)
            CurMoveVel = Mathf.Lerp(maxMoveSpeed * moveDir, 0f, moveAcceleration);
        else
            CurMoveVel = Mathf.Lerp(0f, maxMoveSpeed * moveDir, moveAcceleration);

        // make player face move input
        if(inputScript.MoveInput != 0f)
            playerScale.x = Mathf.Sign(inputScript.MoveInput);
        transform.localScale = playerScale;
    }


    /** Handles jump input and calling Jump() **/
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
    private void MoveAndLimit() {

        // apply velocity
        finalMoveSpeed.x = CurMoveVel + platformVel;
        finalMoveSpeed.y = body.velocity.y;
        body.velocity = finalMoveSpeed;

        // limit velocity
        if(body.velocity.x > maxMoveSpeed + platformVel)
            body.velocity = new Vector2(maxMoveSpeed + platformVel, body.velocity.y);
        else if(body.velocity.x < -maxMoveSpeed - platformVel)
            body.velocity = new Vector2(-maxMoveSpeed - platformVel, body.velocity.y);
    }


    /** Applies jump settings and jump force **/
    private void Jump() {
        canJumpTimer = -1f;
        jumpInputTimer = -1f;
        body.velocity = new Vector2(body.velocity.x, jumpSpeed);
    }


    /** Continuosly checks if grounded and if on moving platform **/
    private IEnumerator GroundCheckCoroutine() {
        
        // if box cast hits any collider
        numHit = coll.Cast(Vector2.down, hits, groundedRayDist);

        IsGrounded = false;

        for(int i = 0; i < numHit; i++) {

            // on moving platform
            if(hits[i].transform.gameObject.layer == movingPlatformLayer) {

                IsGrounded = true;
                platformVel = hits[i].transform.GetComponent<MovingPlatform>().GetHorizontalVelocity();
                break;
            }
            // on floor
            else if(hits[i].transform.gameObject.layer == floorLayer) {

                IsGrounded = true;

                // on hat
                if(hits[i].transform.gameObject.tag == "Hat")
                    platformVel = hits[i].transform.GetComponent<HatPlatformMovement>().GetHatToPlayerVel();
                else 
                    platformVel = 0f;
            }
        }

        if(!IsGrounded) {
            // reduce platform velocity
            platformVel += -Mathf.Sign(platformVel) * groundedRayInterval * exitPlatformDeacceleration;
            
            // set platform velocity to 0
            if (Mathf.Abs(platformVel) < 0.5)
                platformVel = 0f;
        }

        // coyote time
        if(IsGrounded && canJumpTimer < 0.1)
            canJumpTimer = coyoteTime;

        yield return new WaitForSeconds(groundedRayInterval);
        StartCoroutine(GroundCheckCoroutine());
    }
}
