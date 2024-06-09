using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    [SerializeField] private bool isMagician;
    [SerializeField] private float moveThreshold;
    private PlayerMovement movementScript;
    private Animator anim;
    private Rigidbody2D body;
    private int isMovingHash;
    private int isGroundedHash;
    private int isJumpingHash;
    private int isTransformingHash;
    float firstXPosition;
    float lastXPosition;
    bool isMoving;

    private void Awake() {
        movementScript = GetComponent<PlayerMovement>();
        anim = transform.GetChild(0).GetComponent<Animator>();
        body = GetComponent<Rigidbody2D>();

        if(anim == null)
            Debug.LogError("Make sure visuals are first child and have animator/sprite renderer componenets");

        // converts string to int
        isMovingHash = Animator.StringToHash("isMoving");
        isJumpingHash = Animator.StringToHash("isJumping");
        isGroundedHash = Animator.StringToHash("isGrounded");
        if(isMagician)
            isTransformingHash = Animator.StringToHash("isTransforming");

        firstXPosition = transform.position.x;
    }

    private void Update() {

        //anim.SetFloat(velocityHash, Mathf.Abs(body.velocity.x));
        anim.SetBool(isGroundedHash, movementScript.IsGrounded);
    }

    void FixedUpdate() {
        lastXPosition = transform.position.x;

        if(Mathf.Abs(lastXPosition - firstXPosition) > moveThreshold)
            isMoving = true;
        else
            isMoving = false;

        anim.SetBool(isMovingHash, isMoving);
        Debug.Log(isMoving);

        firstXPosition = lastXPosition;
    }

    public void SetIsTransforming(bool isTransforming) {

        if(isMagician)
            anim.SetBool(isTransformingHash, isTransforming);
    }



}
