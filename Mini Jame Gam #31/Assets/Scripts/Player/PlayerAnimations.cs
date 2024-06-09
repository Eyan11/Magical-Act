using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    [Header ("References")]
    [SerializeField] private bool isMagician;
    private PlayerMovement movementScript;
    private Animator anim;
    private Rigidbody2D body;
    private int velocityHash;
    private int isJumpingHash;
    private int isTransformingHash;

    private void Awake() {
        movementScript = GetComponent<PlayerMovement>();
        anim = GetComponent<Animator>();
        body = GetComponent<Rigidbody2D>();

        // converts string to int
        velocityHash = Animator.StringToHash("velocity");
        isJumpingHash = Animator.StringToHash("isJumping");
        if(isMagician)
            isTransformingHash = Animator.StringToHash("isTransforming");
    }

    private void Update() {

        anim.SetFloat(velocityHash, Mathf.Abs(body.velocity.x));
        anim.SetBool(isJumpingHash, !movementScript.IsGrounded);
    }

    public void SetIsTransforming(bool isTransforming) {

        if(isMagician)
            anim.SetBool(isTransformingHash, isTransforming);
    }

}
