using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HatPlatformMovement : MonoBehaviour
{
    [SerializeField] private float rayDist;
    [SerializeField] private float rayInterval;
    private int platformLayer;
    private BoxCollider2D coll;
    private Rigidbody2D body;
    private RaycastHit2D[] hits = new RaycastHit2D[20];
    private int numHit = 0;
    private Vector2 hatVel = Vector2.zero;

    
    private void Awake() {
        coll = GetComponent<BoxCollider2D>();
        body = GetComponent<Rigidbody2D>();
        platformLayer = LayerMask.NameToLayer("Moving Platform");
    }

    private void OnEnable() {
        body.velocity = Vector2.zero;
        hatVel = Vector2.zero;
        StartCoroutine(GroundCheckCoroutine());
    }

    public float GetHatToPlayerVel() {
        return hatVel.x * 0.7f;
    }

    /** Continuosly checks if grounded and if on moving platform **/
    private IEnumerator GroundCheckCoroutine() {
        
        // if box cast hits any collider
        numHit = coll.Cast(Vector2.down, hits, rayDist);

        for(int i = 0; i < numHit; i++) {

            if(hits[i].transform.gameObject.layer == platformLayer) {

                // get platform x velocity
                hatVel.x = hits[i].transform.GetComponent<MovingPlatform>().GetVelocity('H');
                Debug.Log("On Platform");
                break;
            }
            else
                hatVel.x = 0f;
        }

        // if in air, stop moving with platform
        if(numHit <= 0) {
            hatVel.x = 0f;
        }

        hatVel.y = body.velocity.y;
        body.velocity = hatVel;

        yield return new WaitForSeconds(rayInterval);
        StartCoroutine(GroundCheckCoroutine());
    }
}
