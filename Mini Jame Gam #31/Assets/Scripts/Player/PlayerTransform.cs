using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTransform : MonoBehaviour
{
    [Header ("References")]
    [SerializeField] private GameObject magicianPlayer;
    [SerializeField] private GameObject rabbitPlayer;
    [SerializeField] private GameObject magicianHat;
    private PlayerMovement magicianMoveScript;
    private PlayerMovement rabbitMoveScript;
    private BoxCollider2D hatColl;
    private BoxCollider2D magicianColl;
    private PlayerInput inputScript;
    public enum State { Magician, Rabbit, None }
    private State playerState = State.Magician;

    [Header ("Settings")]
    [SerializeField] private float transformDelay;

    private void Awake() {
        inputScript = GetComponent<PlayerInput>();

        magicianMoveScript = magicianPlayer.GetComponent<PlayerMovement>();
        rabbitMoveScript = rabbitPlayer.GetComponent<PlayerMovement>();
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
                // enable movement scripts
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
                // enable hat and rabbit objects
                magicianPlayer.SetActive(false);
                magicianHat.SetActive(true);
                rabbitPlayer.SetActive(true);
                TransformToRabbit();
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
        // spawn hat
        magicianHat.transform.position = magicianPlayer.transform.position + (Vector3.up * magicianColl.bounds.size.y);

        // set up rabbit
        rabbitPlayer.transform.position = magicianHat.transform.position + (Vector3.up * 0.5f * (hatColl.bounds.size.y + 0.2f));
    }
}
