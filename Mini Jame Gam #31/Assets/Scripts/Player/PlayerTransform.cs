using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTransform : MonoBehaviour
{
    [Header ("References")]
    [SerializeField] private GameObject magicianPlayer;
    [SerializeField] private GameObject rabbitPlayer;
    [SerializeField] private GameObject magicianHat;
    [SerializeField] private GameObject smokePrefab;
    [SerializeField] private AudioClip magicSound;
    private PlayerMovement magicianMoveScript;
    private PlayerMovement rabbitMoveScript;
    private PlayerAnimations magicianAnimScript;
    private BoxCollider2D hatColl;
    private BoxCollider2D magicianColl;
    private PlayerInput inputScript;
    public enum State { Magician, Rabbit, None }
    private State playerState = State.Magician;

    [Header ("Settings")]
    [SerializeField] private float transformTime;

    private void Awake() {
        inputScript = GetComponent<PlayerInput>();

        magicianMoveScript = magicianPlayer.GetComponent<PlayerMovement>();
        rabbitMoveScript = rabbitPlayer.GetComponent<PlayerMovement>();
        magicianAnimScript = magicianPlayer.GetComponent<PlayerAnimations>();
        hatColl = magicianHat.GetComponent<BoxCollider2D>();
        magicianColl = magicianPlayer.GetComponent<BoxCollider2D>();
    }

    private void Update() {
        
        if(inputScript.TransformInput && playerState == State.Magician)
            ChangeState('R');
    }


    public void ChangeState(char stateChar) {

        switch(stateChar) {
            // change to magician
            case 'M':
                playerState = State.Magician;
                // enable movement
                magicianMoveScript.enabled = true;
                rabbitMoveScript.enabled = true;
                // only enable magician object
                magicianPlayer.SetActive(true);
                magicianHat.SetActive(false);
                rabbitPlayer.SetActive(false);
                break;

            // change to rabbit
            case 'R':
                playerState = State.Rabbit;
                magicianAnimScript.SetIsTransforming(true);
                // disable movement
                magicianMoveScript.enabled = false;
                rabbitMoveScript.enabled = false;
                SoundManager.current.PlaySFX(magicSound, 0.2f);
                Invoke("TransformToRabbit", transformTime);
                break;

            // disable input (for switching levels)
            case 'N':
                playerState = State.None;
                // disable movement
                magicianMoveScript.enabled = false;
                rabbitMoveScript.enabled = false;
                break;

            default:
                Debug.LogError("PlayerTransform > ChangeState, wrong argument given");
                break;
        }
    }

    private void TransformToRabbit() {

        // spawn smoke and stop transforming anim
        Instantiate(smokePrefab, magicianPlayer.transform.position, magicianPlayer.transform.rotation);
        magicianAnimScript.SetIsTransforming(false);

        // disable magician, enable hat and rabbit
        magicianPlayer.SetActive(false);
        magicianHat.SetActive(true);
        rabbitPlayer.SetActive(true);

        // spawn hat
        magicianHat.transform.position = magicianPlayer.transform.position + (Vector3.up * magicianColl.bounds.size.y);

        // set up rabbit
        rabbitPlayer.transform.position = magicianHat.transform.position + (Vector3.up * 0.5f * (hatColl.bounds.size.y + 0.2f));
    
        // enable movement
        magicianMoveScript.enabled = true;
        rabbitMoveScript.enabled = true;
    }
}
