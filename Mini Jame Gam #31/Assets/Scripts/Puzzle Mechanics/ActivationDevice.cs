using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivationDevice : MonoBehaviour
{
    [Header ("References")]
    [SerializeField] private bool isPlate;
    [SerializeField] private MovingPlatform platformScript;
    private PlayerInput inputScript;
    private Animator anim;
    private int isEnabledHash;
    private int itemsOnPlate;
    private float isNextToSwitchTimer = -1f;
    private bool isActivated = false;

    [Header ("Sounds")]
    [SerializeField] private AudioClip switchEnableSound;
    [SerializeField] private AudioClip switchDisableSound;
    [SerializeField] private AudioClip plateSound;

    private void Awake() {
        inputScript = GetComponent<PlayerInput>();
        anim = transform.GetChild(0).GetComponent<Animator>();
        isEnabledHash = Animator.StringToHash("isEnabled");
    }

    private void Start() {
        RestartEvent.current.onRestartEvent += ResetDevice;
    }

    private void OnDestroy() {
        RestartEvent.current.onRestartEvent -= ResetDevice;
    }

    /** Resets switch and platform, called when level is restarted **/
    public void ResetDevice() {
        isActivated = false;
        isNextToSwitchTimer = -1f;
        anim.SetBool(isEnabledHash, false);
        platformScript.ResetPlatform();
    }

    /** Enables moving platform for pressure plates and tracks if magician is next to switch **/
    private void OnTriggerEnter2D(Collider2D other) {
        // pressure plate
        if(isPlate && (other.gameObject.tag == "Magician" || other.gameObject.tag == "Hat")) {
            itemsOnPlate++;
            platformScript.ActivatePlatform();
            anim.SetBool(isEnabledHash, true);
            // play plate sound
            SoundManager.current.PlaySFX(plateSound, 0.3f);
        }
        // switch
        else if(!isPlate && other.gameObject.tag == "Magician") {
            isNextToSwitchTimer = 100f;
        }
    }

    /** Disables moving platform for pressure plates and tracks if magician is not next to switch**/
    private void OnTriggerExit2D(Collider2D other) {
        if(isPlate && (other.gameObject.tag == "Magician" || other.gameObject.tag == "Hat")) {
            itemsOnPlate--;

            if(itemsOnPlate <= 0) {
                platformScript.DeactivatePlatform();
                anim.SetBool(isEnabledHash, false);
                // play plate sound
                SoundManager.current.PlaySFX(plateSound, 0.3f);
            }
        }
        // switch
        else if(!isPlate && other.gameObject.tag == "Magician") {
            isNextToSwitchTimer = 0.2f;
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
                    anim.SetBool(isEnabledHash, false);
                    SoundManager.current.PlaySFX(switchDisableSound, 0.1f);
                }
                else {
                    anim.SetBool(isEnabledHash, true);
                    SoundManager.current.PlaySFX(switchEnableSound, 0.1f);
                }
            }
        }

    }
}
