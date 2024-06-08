using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    [Header ("References")]
    private PlayerMovement movementScript;
    private Animator anim;
    private Rigidbody2D body;
    private int velocityHash;
    private int isJumpingHash;

    private void Awake() {
        movementScript = GetComponent<PlayerMovement>();
        anim = GetComponent<Animator>();
        body = GetComponent<Rigidbody2D>();

        // converts string to int
        velocityHash = Animator.StringToHash("velocity");
        isJumpingHash = Animator.StringToHash("isJumping");
    }

    private void Update() {

        anim.SetFloat(velocityHash, Mathf.Abs(body.velocity.x));
        anim.SetBool(isJumpingHash, !movementScript.IsGrounded);
    }

}
