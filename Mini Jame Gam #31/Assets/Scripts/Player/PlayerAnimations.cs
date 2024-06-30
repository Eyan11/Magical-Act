using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    [SerializeField] private bool isMagician;
    private PlayerMovement movementScript;
    private Animator anim;
    private Rigidbody2D body;
    private int isMovingHash;
    private int isGroundedHash;
    private int isJumpingHash;
    private int isTransformingHash;

    private void Awake() {
        movementScript = GetComponent<PlayerMovement>();
        anim = transform.GetChild(0).GetComponent<Animator>();

        if(anim == null)
            Debug.LogError("Make sure visuals are first child and have animator/sprite renderer componenets");

        // converts string to int
        isMovingHash = Animator.StringToHash("isMoving");
        isJumpingHash = Animator.StringToHash("isJumping");
        isGroundedHash = Animator.StringToHash("isGrounded");
        if(isMagician)
            isTransformingHash = Animator.StringToHash("isTransforming");

    }

    private void Update() {
        anim.SetBool(isGroundedHash, movementScript.IsGrounded);
        anim.SetBool(isMovingHash, Mathf.Abs(movementScript.CurMoveVel) > 0.01);
    }

    public void SetIsTransforming(bool isTransforming) {
        if(isMagician)
            anim.SetBool(isTransformingHash, isTransforming);
    }



}
