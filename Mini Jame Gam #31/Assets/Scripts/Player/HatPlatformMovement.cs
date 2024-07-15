using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HatPlatformMovement : MonoBehaviour
{
    [SerializeField] private float rayDist;
    [SerializeField] private float rayInterval;
    private const float HAT_VEL_MULT = 1.425f;
    private int platformLayer;
    private BoxCollider2D coll;
    private Rigidbody2D body;
    private RaycastHit2D[] hits = new RaycastHit2D[20];
    private int numHit = 0;
    private Vector2 platVel = Vector2.zero;

    
    private void Awake() {
        coll = GetComponent<BoxCollider2D>();
        body = GetComponent<Rigidbody2D>();
        platformLayer = LayerMask.NameToLayer("Moving Platform");
    }

    private void OnEnable() {
        body.velocity = Vector2.zero;
        platVel = Vector2.zero;
        StartCoroutine(GroundCheckCoroutine());
    }

    public float GetHatToPlayerVel() {
        return platVel.x;
    }

    /** Continuosly checks if grounded and if on moving platform **/
    private IEnumerator GroundCheckCoroutine() {
        
        // if box cast hits any collider
        numHit = coll.Cast(Vector2.down, hits, rayDist);

        for(int i = 0; i < numHit; i++) {

            if(hits[i].transform.gameObject.layer == platformLayer) {

                // get platform x velocity
                platVel.x = hits[i].transform.GetComponent<MovingPlatform>().GetHorizontalVelocity();
                break;
            }
            else
                platVel.x = 0f;
        }

        // if in air, stop moving with platform
        if(numHit <= 0) {
            platVel.x = 0f;
        }

        platVel.y = body.velocity.y;
        body.velocity = platVel * HAT_VEL_MULT;

        yield return new WaitForSeconds(rayInterval);
        StartCoroutine(GroundCheckCoroutine());
    }
}
