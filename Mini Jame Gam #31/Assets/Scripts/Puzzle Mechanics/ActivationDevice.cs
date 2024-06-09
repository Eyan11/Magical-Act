using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivationDevice : MonoBehaviour
{
    [Header ("References")]
    [SerializeField] private bool isPlate;
    [SerializeField] private MovingPlatform platformScript;
    private PlayerInput inputScript;
    private int itemsOnPlate;
    private float isNextToSwitchTimer = -1f;
    private bool isActivated = false;

    [Header ("Sounds")]
    [SerializeField] private AudioClip switchEnableSound;
    [SerializeField] private AudioClip switchDisableSound;
    [SerializeField] private AudioClip plateSound;

    private void Awake() {
        inputScript = GetComponent<PlayerInput>();
    }

    /** Enables moving platform for pressure plates **/
    private void OnTriggerEnter2D(Collider2D other) {
        if(isPlate && (other.gameObject.tag == "Magician" || other.gameObject.tag == "Hat")) {
            itemsOnPlate++;
            platformScript.ActivatePlatform();
            // play plate sound
            SoundManager.current.PlaySFX(plateSound, 0.3f);
        }
    }

    /** Disables moving platform for pressure plates **/
    private void OnTriggerExit2D(Collider2D other) {
        if(isPlate && (other.gameObject.tag == "Magician" || other.gameObject.tag == "Hat")) {
            itemsOnPlate--;

            if(itemsOnPlate <= 0) {
                platformScript.DeactivatePlatform();
                // play plate sound
                SoundManager.current.PlaySFX(plateSound, 0.3f);
            }
        }
    }

    /** Tracks if magician is next to switch **/
    private void OnTriggerStay2D(Collider2D other) {
        if(!isPlate && other.gameObject.tag == "Magician") {

            // timer used so input can be checked in Update()
            isNextToSwitchTimer = 0.5f;
        }
    }

    /** Toggles switch **/
    private void Update() {

        // only for switches
        if(!isPlate) {
            isNextToSwitchTimer -= Time.deltaTime;

            // toggle switch
            if(isNextToSwitchTimer > 0f && inputScript.InteractInput) {
                platformScript.TogglePlatformLoop();
                isActivated = !isActivated;

                // flip switch stick and play sound
                if(!isActivated) {
                    transform.localScale = Vector3.one;
                    SoundManager.current.PlaySFX(switchDisableSound, 0.1f);
                }
                else {
                    transform.localScale = new Vector3(-1f, 1f, 1f);
                    SoundManager.current.PlaySFX(switchEnableSound, 0.1f);
                }
            }
        }

    }
}
